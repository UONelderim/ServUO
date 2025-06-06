#region References
using Server.ContextMenus;
using Server.Engines.PartySystem;
using Server.Engines.Quests;
using Server.Engines.Quests.Doom;
using Server.Guilds;
using Server.Misc;
using Server.Mobiles;
using Server.Network;
using System;
using System.Collections.Generic;
using System.Linq;
#endregion

namespace Server.Items
{
    public interface IDevourer
    {
        bool Devour(Corpse corpse);
    }

    [Flags]
    public enum CorpseFlag
    {
        None = 0x00000000,

        /// <summary>
        ///     Has this corpse been carved?
        /// </summary>
        Carved = 0x00000001,

        /// <summary>
        ///     If true, this corpse will not turn into bones
        /// </summary>
        NoBones = 0x00000002,

        /// <summary>
        ///     If true, the corpse has turned into bones
        /// </summary>
        IsBones = 0x00000004,

        /// <summary>
        ///     Has this corpse yet been visited by a taxidermist?
        /// </summary>
        VisitedByTaxidermist = 0x00000008,

        /// <summary>
        ///     Has this corpse yet been used to channel spiritual energy? (AOS Spirit Speak)
        /// </summary>
        Channeled = 0x00000010,

        /// <summary>
        ///     Was the owner criminal when he died?
        /// </summary>
        Criminal = 0x00000020,

        /// <summary>
        ///     Has this corpse been animated?
        /// </summary>
        Animated = 0x00000040,

        /// <summary>
        ///     Has this corpse been self looted?
        /// </summary>
        SelfLooted = 0x00000080,

        /// <summary>
        /// Does this corpse flag looters as criminal?
        /// </summary>
        LootCriminal = 0x00000100,

        /// <summary>
        ///     Was the owner a murderer when he died?
        /// </summary>
        Murderer = 0x00000200
    }

    public partial class Corpse : Container, ICarvable
    {
        private Mobile m_Owner; // Whos corpse is this?
        private Mobile m_Killer; // Who killed the owner?
        private CorpseFlag m_Flags; // @see CorpseFlag

        private List<Mobile> m_Looters; // Who's looted this corpse?

        private List<Item> m_EquipItems;
        // List of dropped equipment when the owner died. Ingame, these items display /on/ the corpse, not just inside

        private List<Mobile> m_Aggressors;
        // Anyone from this list will be able to loot this corpse; we attacked them, or they attacked us when we were freely attackable

        private List<Item> m_HasLooted;
        // Keeps a list of items that have been dragged from corpse. This prevents Loot Event Handler from triggering from the same item more than once.

        private string m_CorpseName;
        // Value of the CorpseNameAttribute attached to the owner when he died -or- null if the owner had no CorpseNameAttribute; use "the remains of ~name~"

        private IDevourer m_Devourer; // The creature that devoured this corpse

        // For notoriety:
        private AccessLevel m_AccessLevel; // Which AccessLevel the owner had when he died

        // For Forensics Evaluation
        public string m_Forensicist; // Name of the first PlayerMobile who used Forensic Evaluation on the corpse

        public static readonly TimeSpan MonsterLootRightSacrifice = TimeSpan.FromMinutes(2.0);

        public static readonly TimeSpan InstancedCorpseTime = TimeSpan.FromMinutes(3.0);

        [CommandProperty(AccessLevel.GameMaster)]
        public virtual bool InstancedCorpse => DateTime.UtcNow < TimeOfDeath + InstancedCorpseTime;

        private Dictionary<Item, InstancedItemInfo> m_InstancedItems;

        public bool HasAssignedInstancedLoot { get; private set; }

        private class InstancedItemInfo
        {
            private readonly Mobile m_Mobile;
            private readonly Item m_Item;

            public bool Perpetual { get; set; }

            public InstancedItemInfo(Item i, Mobile m)
            {
                m_Item = i;
                m_Mobile = m;
            }

            public bool IsOwner(Mobile m)
            {
                if (m_Item.LootType == LootType.Cursed) //Cursed Items are part of everyone's instanced corpse... (?)
                {
                    return true;
                }

                if (m == null)
                {
                    return false; //sanity
                }

                if (m_Mobile == m)
                {
                    return true;
                }

                Party myParty = Party.Get(m_Mobile);

                return myParty != null && myParty == Party.Get(m);
            }
        }

        public override bool IsChildVisibleTo(Mobile m, Item child)
        {
            if (!m.Player || m.IsStaff()) //Staff and creatures not subject to instancing.
            {
                return true;
            }

            if (m_InstancedItems != null && m_InstancedItems.TryGetValue(child, out InstancedItemInfo info) && (InstancedCorpse || info.Perpetual))
            {
                return info.IsOwner(m); //IsOwner checks Party stuff
            }

            return true;
        }

        public override void AddItem(Item item)
        {
            base.AddItem(item);

            if (InstancedCorpse && HasAssignedInstancedLoot)
            {
                if (item.GetBounce() != null)
                {
                    if (m_HasLooted == null)
                        m_HasLooted = new List<Item>();

                    m_HasLooted.Add(item);
                }

                AssignInstancedLoot(item);
            }
        }

        private void AssignInstancedLoot()
        {
            AssignInstancedLoot(Items);
        }

        public void AssignInstancedLoot(Item item)
        {
            AssignInstancedLoot(new[] { item });
        }

        private void AssignInstancedLoot(IEnumerable<Item> items)
        {
            if (m_Aggressors.Count == 0 || Items.Count == 0)
            {
                return;
            }

            if (m_InstancedItems == null)
            {
                m_InstancedItems = new Dictionary<Item, InstancedItemInfo>();
            }

            List<Item> stackables = new List<Item>();
            List<Item> unstackables = new List<Item>();

            foreach (Item item in items.Where(i => !m_InstancedItems.ContainsKey(i)))
            {
                if (item.LootType != LootType.Cursed) //Don't have curesd items take up someone's item spot.. (?)
                {
                    if (item.Stackable)
                    {
                        stackables.Add(item);
                    }
                    else
                    {
                        unstackables.Add(item);
                    }
                }
            }

            List<Mobile> attackers = new List<Mobile>(m_Aggressors);

            for (int i = 1; i < attackers.Count - 1; i++) //randomize
            {
                int rand = Utility.Random(i + 1);

                Mobile temp = attackers[rand];
                attackers[rand] = attackers[i];
                attackers[i] = temp;
            }

            //stackables first, for the remaining stackables, have those be randomly added after
            for (int i = 0; i < stackables.Count; i++)
            {
                Item item = stackables[i];

                if (item.Amount >= attackers.Count)
                {
                    int amountPerAttacker = item.Amount / attackers.Count;
                    int remainder = item.Amount % attackers.Count;

                    for (int j = 0; j < (remainder == 0 ? attackers.Count - 1 : attackers.Count); j++)
                    {
                        Item splitItem = Mobile.LiftItemDupe(item, item.Amount - amountPerAttacker);
                        //LiftItemDupe automagically adds it as a child item to the corpse

                        if (!m_InstancedItems.ContainsKey(splitItem))
                        {
                            m_InstancedItems.Add(splitItem, new InstancedItemInfo(splitItem, attackers[j]));
                        }
                        //What happens to the remaining portion?  TEMP FOR NOW UNTIL OSI VERIFICATION:  Treat as Single Item.
                    }

                    if (remainder == 0)
                    {
                        if (!m_InstancedItems.ContainsKey(item))
                        {
                            m_InstancedItems.Add(item, new InstancedItemInfo(item, attackers[attackers.Count - 1]));
                        }
                        //Add in the original item (which has an equal amount as the others) to the instance for the last attacker, cause it wasn't added above.
                    }
                    else
                    {
                        unstackables.Add(item);
                    }
                }
                else
                {
                    //What happens in this case?  TEMP FOR NOW UNTIL OSI VERIFICATION:  Treat as Single Item.
                    unstackables.Add(item);
                }
            }

            for (int i = 0; i < unstackables.Count; i++)
            {
                Mobile m = attackers[i % attackers.Count];
                Item item = unstackables[i];

                if (!m_InstancedItems.ContainsKey(item))
                {
                    m_InstancedItems.Add(item, new InstancedItemInfo(item, m));
                }
            }

            ColUtility.Free(stackables);
            ColUtility.Free(unstackables);
        }

        public void AddCarvedItem(Item carved, Mobile carver)
        {
            DropItem(carved);

            if (InstancedCorpse)
            {
                if (m_InstancedItems == null)
                {
                    m_InstancedItems = new Dictionary<Item, InstancedItemInfo>();
                }

                if (!m_InstancedItems.ContainsKey(carved))
                {
                    m_InstancedItems.Add(carved, new InstancedItemInfo(carved, carver));
                }
            }
        }

        public override bool IsDecoContainer => false;

        [CommandProperty(AccessLevel.GameMaster)]
        public DateTime TimeOfDeath { get; set; }

        public override bool DisplayWeight => false;

        public HairInfo Hair { get; }

        public FacialHairInfo FacialHair { get; }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool IsBones => GetFlag(CorpseFlag.IsBones);

        [CommandProperty(AccessLevel.GameMaster)]
        public bool Devoured => m_Devourer != null;

        [CommandProperty(AccessLevel.GameMaster)]
        public bool Carved { get => GetFlag(CorpseFlag.Carved); set => SetFlag(CorpseFlag.Carved, value);
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool VisitedByTaxidermist { get => GetFlag(CorpseFlag.VisitedByTaxidermist); set => SetFlag(CorpseFlag.VisitedByTaxidermist, value);
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool Channeled { get => GetFlag(CorpseFlag.Channeled); set => SetFlag(CorpseFlag.Channeled, value);
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool Animated { get => GetFlag(CorpseFlag.Animated); set => SetFlag(CorpseFlag.Animated, value);
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool SelfLooted { get => GetFlag(CorpseFlag.SelfLooted); set => SetFlag(CorpseFlag.SelfLooted, value);
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool LootCriminal { get => GetFlag(CorpseFlag.LootCriminal); set => SetFlag(CorpseFlag.LootCriminal, value);
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public AccessLevel AccessLevel => m_AccessLevel;

        public List<Mobile> Aggressors => m_Aggressors;

        public List<Mobile> Looters => m_Looters;

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile Killer => m_Killer;

        public List<Item> EquipItems => m_EquipItems;

        public List<Item> RestoreEquip { get; set; }

        public Guild Guild { get; }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Kills { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool Criminal { get => GetFlag(CorpseFlag.Criminal); set => SetFlag(CorpseFlag.Criminal, value);
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool Murderer { get => GetFlag(CorpseFlag.Murderer); set => SetFlag(CorpseFlag.Murderer, value);
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile Owner => m_Owner;

        #region Decay
        private static readonly string m_TimerID = "CorpseDecayTimer";
        private DateTime m_DecayTime;

        public void TurnToBones()
        {
            if (Deleted)
            {
                return;
            }

            ProcessDelta();
            SendRemovePacket();
            ItemID = Utility.Random(0xECA, 9); // bone graphic
            Hue = 0;
            ProcessDelta();

            SetFlag(CorpseFlag.NoBones, true);
            SetFlag(CorpseFlag.IsBones, true);

            var delay = Owner?.CorpseDecayTime ?? Mobile.DefaultCorpseDecay;
            m_DecayTime = DateTime.UtcNow + delay;

            if (!TimerRegistry.UpdateRegistry(m_TimerID, this, delay))
            {
                TimerRegistry.Register(m_TimerID, this, delay, TimerPriority.FiveSeconds, c => c.DoDecay());
            }
        }

        public void BeginDecay(TimeSpan delay)
        {
            m_DecayTime = DateTime.UtcNow + delay;

            TimerRegistry.Register(m_TimerID, this, delay, TimerPriority.FiveSeconds, c => c.DoDecay());
        }

        private void DoDecay()
        {
            if (!GetFlag(CorpseFlag.NoBones))
            {
                TurnToBones();
            }
            else
            {
                Delete();
            }
        }
        #endregion

        public static string GetCorpseName(Mobile m)
        {
            if (m is BaseCreature bc && bc.CorpseNameOverride != null)
            {
                return bc.CorpseNameOverride;
            }

            Type t = m.GetType();

            object[] attrs = t.GetCustomAttributes(typeof(CorpseNameAttribute), true);

            if (attrs.Length > 0 && attrs[0] is CorpseNameAttribute attr)
            {
                return attr.Name;
            }

            return null;
        }

        public static void Initialize()
        {
            Mobile.CreateCorpseHandler += Mobile_CreateCorpseHandler;
        }

        public static Container Mobile_CreateCorpseHandler(
            Mobile owner, HairInfo hair, FacialHairInfo facialhair, List<Item> initialContent, List<Item> equipItems)
        {
            var c = new Corpse(owner, hair, facialhair, equipItems);

            owner.Corpse = c;

            for (int i = 0; i < initialContent.Count; ++i)
            {
                Item item = initialContent[i];

                if (owner.Player && item.Parent == owner.Backpack)
                {
                    c.AddItem(item);
                }
                else
                {
                    c.DropItem(item);
                }

                if (owner.Player)
                {
                    c.SetRestoreInfo(item, item.Location);
                }
            }

            if (!owner.Player)
            {
                // c.AssignInstancedLoot();
                // c.HasAssignedInstancedLoot = true;
            }
            else
            {
                if (owner is PlayerMobile pm)
                {
                    c.RestoreEquip = pm.EquipSnapshot;
                }
            }

            Point3D loc = owner.Location;
            Map map = owner.Map;

            if (map == null || map == Map.Internal)
            {
                loc = owner.LogoutLocation;
                map = owner.LogoutMap;
            }

            c.MoveToWorld(loc, map);

            return c;
        }

        public override bool IsPublicContainer => false;

        public Corpse(Mobile owner, List<Item> equipItems)
            : this(owner, null, null, equipItems)
        { }

        public Corpse(Mobile owner, HairInfo hair, FacialHairInfo facialhair, List<Item> equipItems)
            : base(0x2006)
        {
            Movable = false;

            Stackable = true; // To supress console warnings, stackable must be true
            Amount = owner.Body; // Protocol defines that for itemid 0x2006, amount=body
            Stackable = false;

            Name = owner.Race == Race.DefaultRace ? owner.Name : owner.Race.GetName(Cases.Dopelniacz).ToLower();
            Hue = owner.Hue;

            Direction = owner.Direction;
            Light = (LightType)Direction;

            m_Owner = owner;

            m_CorpseName = GetCorpseName(owner);

            TimeOfDeath = DateTime.UtcNow;

            m_AccessLevel = owner.AccessLevel;
            Guild = owner.Guild as Guild;
            Kills = owner.Kills;

            SetFlag(CorpseFlag.Criminal, owner.Criminal);
            SetFlag(CorpseFlag.Murderer, owner.Murderer);

            Hair = hair;
            FacialHair = facialhair;

            // This corpse does not turn to bones if: the owner is not a player
            SetFlag(CorpseFlag.NoBones, !owner.Player);

            // Flagging looters as criminal can happen by default
            SetFlag(CorpseFlag.LootCriminal, true);

            m_Looters = new List<Mobile>();
            m_EquipItems = equipItems;

            m_Aggressors = new List<Mobile>(owner.Aggressors.Count + owner.Aggressed.Count);
            //bool addToAggressors = !( owner is BaseCreature );

            BaseCreature bc = owner as BaseCreature;

            TimeSpan lastTime = TimeSpan.MaxValue;

            for (int i = 0; i < owner.Aggressors.Count; ++i)
            {
                AggressorInfo info = owner.Aggressors[i];

                if (DateTime.UtcNow - info.LastCombatTime < lastTime)
                {
                    m_Killer = info.Attacker;
                    lastTime = DateTime.UtcNow - info.LastCombatTime;
                }

                if (bc == null && !info.CriminalAggression)
                {
                    m_Aggressors.Add(info.Attacker);
                }
            }

            for (int i = 0; i < owner.Aggressed.Count; ++i)
            {
                AggressorInfo info = owner.Aggressed[i];

                if (DateTime.UtcNow - info.LastCombatTime < lastTime)
                {
                    m_Killer = info.Defender;
                    lastTime = DateTime.UtcNow - info.LastCombatTime;
                }

                if (bc == null)
                {
                    m_Aggressors.Add(info.Defender);
                }
            }

            if (bc != null)
            {
                Mobile master = bc.GetMaster();

                if (master != null)
                {
                    m_Aggressors.Add(master);
                }

                List<DamageStore> rights = bc.GetLootingRights();
                for (int i = 0; i < rights.Count; ++i)
                {
                    DamageStore ds = rights[i];

                    if (ds.m_HasRight)
                    {
                        m_Aggressors.Add(ds.m_Mobile);
                        HasLootingRights.Add(ds.m_Mobile); //NELDERIM
                    }
                }
            }

            BeginDecay(owner.CorpseDecayTime);
            DevourCorpse();

            if (owner is PlayerMobile)
            {
                if (PlayerCorpses == null)
                    PlayerCorpses = new Dictionary<Corpse, int>();

                PlayerCorpses[this] = 0;
            }
        }

        public Corpse(Serial serial)
            : base(serial)
        { }

        public bool GetFlag(CorpseFlag flag)
        {
            return (m_Flags & flag) != 0;
        }

        public void SetFlag(CorpseFlag flag, bool on)
        {
            m_Flags = on ? m_Flags | flag : m_Flags & ~flag;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(12); // version

			if (RestoreEquip == null)
            {
                writer.Write(false);
            }
            else
            {
                writer.Write(true);
                writer.Write(RestoreEquip);
            }

            writer.Write((int)m_Flags);

            writer.WriteDeltaTime(TimeOfDeath);

            List<KeyValuePair<Item, Point3D>> list = m_RestoreTable == null ? null : new List<KeyValuePair<Item, Point3D>>(m_RestoreTable);
            int count = list?.Count ?? 0;

            writer.Write(count);

            for (int i = 0; i < count; ++i)
            {
                KeyValuePair<Item, Point3D> kvp = list[i];
                Item item = kvp.Key;
                Point3D loc = kvp.Value;

                writer.Write(item);

                if (item.Location == loc)
                {
                    writer.Write(false);
                }
                else
                {
                    writer.Write(true);
                    writer.Write(loc);
                }
            }

            bool decaying = m_DecayTime != DateTime.MinValue;
            writer.Write(decaying);

            if (decaying)
            {
                writer.WriteDeltaTime(m_DecayTime);
            }

            writer.Write(m_Looters);
            writer.Write(m_Killer);

            writer.Write(m_Aggressors);

            writer.Write(m_Owner);

            writer.Write(m_CorpseName);

            writer.Write((int)m_AccessLevel);
            writer.Write(Guild);
            writer.Write(Kills);

            writer.Write(m_EquipItems);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
				case 12:
                    {
                        if (reader.ReadBool())
                        {
                            RestoreEquip = reader.ReadStrongItemList();
                        }

                        goto case 11;
                    }
                case 11:
                    {
                        // Version 11, we move all bools to a CorpseFlag
                        m_Flags = (CorpseFlag)reader.ReadInt();
                        TimeOfDeath = reader.ReadDeltaTime();

                        int count = reader.ReadInt();

                        for (int i = 0; i < count; ++i)
                        {
                            Item item = reader.ReadItem();

                            if (reader.ReadBool())
                            {
                                SetRestoreInfo(item, reader.ReadPoint3D());
                            }
                            else if (item != null)
                            {
                                SetRestoreInfo(item, item.Location);
                            }
                        }

                        if (reader.ReadBool())
                        {
                            BeginDecay(reader.ReadDeltaTime() - DateTime.UtcNow);
                        }

                        m_Looters = reader.ReadStrongMobileList();
                        m_Killer = reader.ReadMobile();

                        m_Aggressors = reader.ReadStrongMobileList();
                        m_Owner = reader.ReadMobile();

                        m_CorpseName = reader.ReadString();

                        m_AccessLevel = (AccessLevel)reader.ReadInt();
                        reader.ReadInt(); // guild reserve
                        Kills = reader.ReadInt();

                        m_EquipItems = reader.ReadStrongItemList();
                        break;
                    }
            }

            if (m_Owner is PlayerMobile)
            {
                if (PlayerCorpses == null)
                    PlayerCorpses = new Dictionary<Corpse, int>();

                PlayerCorpses[this] = 0;
            }
        }

        public bool DevourCorpse()
        {
            if (Devoured || Deleted || m_Killer == null || m_Killer.Deleted || !m_Killer.Alive || !(m_Killer is IDevourer) ||
                m_Owner == null || m_Owner.Deleted)
            {
                return false;
            }

            m_Devourer = (IDevourer)m_Killer; // Set the devourer the killer
            return m_Devourer.Devour(this); // Devour the corpse if it hasn't
        }

        public override void SendInfoTo(NetState state, bool sendOplPacket)
        {
            base.SendInfoTo(state, sendOplPacket);

            if (((Body)Amount).IsHuman && ItemID == 0x2006)
            {
                state.Send(new CorpseContent(state.Mobile, this));

                state.Send(new CorpseEquip(state.Mobile, this));
            }
        }

        public bool IsCriminalAction(Mobile from)
        {
            if (from == m_Owner || from.AccessLevel >= AccessLevel.GameMaster)
            {
                return false;
            }

            if (!GetFlag(CorpseFlag.LootCriminal))
                return false;

            Party p = Party.Get(m_Owner);

            if (p != null && p.Contains(from))
            {
                PartyMemberInfo pmi = p[m_Owner];

                if (pmi != null && pmi.CanLoot)
                {
                    return false;
                }
            }

            return NotorietyHandlers.CorpseNotoriety(from, this) == Notoriety.Innocent;
        }

        public override bool CheckItemUse(Mobile from, Item item)
        {
            if (!base.CheckItemUse(from, item))
            {
                return false;
            }

            if (item != this)
            {
                return CanLoot(from);
            }

            return true;
        }

        public override bool CheckLift(Mobile from, Item item, ref LRReason reject)
        {
            if (!base.CheckLift(from, item, ref reject))
            {
                return false;
            }

            bool canLoot = CanLoot(from);

            if (m_HasLooted == null)
                m_HasLooted = new List<Item>();

            if (canLoot && !m_HasLooted.Contains(item))
            {
                m_HasLooted.Add(item);
                EventSink.InvokeCorpseLoot(new CorpseLootEventArgs(from, this, item));
            }

            return canLoot;
        }

        public override void OnItemUsed(Mobile from, Item item)
        {
            base.OnItemUsed(from, item);

            if (item is Food)
            {
                from.RevealingAction();
            }

            if (item != this && IsCriminalAction(from))
            {
                from.CriminalAction(true);
            }

            if (!m_Looters.Contains(from))
            {
                m_Looters.Add(from);
            }
        }

        public override void OnItemLifted(Mobile from, Item item)
        {
            base.OnItemLifted(from, item);

            if (item != this && from != m_Owner)
            {
                from.RevealingAction();
            }

            if (item != this && IsCriminalAction(from))
            {
                from.CriminalAction(true);
            }

            if (!m_Looters.Contains(from))
            {
                m_Looters.Add(from);
            }
        }

        private class OpenCorpseEntry : ContextMenuEntry
        {
            public OpenCorpseEntry()
                : base(6215, 2)
            { }

            public override void OnClick()
            {
                if (Owner.Target is Corpse corpse && Owner.From.CheckAlive())
                {
                    corpse.Open(Owner.From, false);
                }
            }
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);

            if (m_Owner == from && from.Alive)
            {
                list.Add(new OpenCorpseEntry());
            }
        }

        private Dictionary<Item, Point3D> m_RestoreTable;

        public bool GetRestoreInfo(Item item, ref Point3D loc)
        {
            if (m_RestoreTable == null || item == null)
            {
                return false;
            }

            return m_RestoreTable.TryGetValue(item, out loc);
        }

        public void SetRestoreInfo(Item item, Point3D loc)
        {
            if (item == null)
            {
                return;
            }

            if (m_RestoreTable == null)
            {
                m_RestoreTable = new Dictionary<Item, Point3D>();
            }

            m_RestoreTable[item] = loc;
        }

        public void ClearRestoreInfo(Item item)
        {
            if (m_RestoreTable == null || item == null)
            {
                return;
            }

            m_RestoreTable.Remove(item);

            if (m_RestoreTable.Count == 0)
            {
                m_RestoreTable = null;
            }
        }

        public bool CanLoot(Mobile from)
        {
            if (!IsCriminalAction(from))
            {
                return true;
            }

            Map map = Map;

            if (map == null || (map.Rules & MapRules.HarmfulRestrictions) != 0)
            {
                return false;
            }

            return true;
        }

        public bool CheckLoot(Mobile from)
        {
            if (!CanLoot(from))
            {
                if (m_Owner == null || !m_Owner.Player)
                {
                    from.SendLocalizedMessage(1005035); // You did not earn the right to loot this creature!
                }
                else
                {
                    from.SendLocalizedMessage(1010049); // You may not loot this corpse.
                }

                return false;
            }

            if (IsCriminalAction(from))
            {
                if (m_Owner == null || !m_Owner.Player)
                {
                    from.SendLocalizedMessage(1005036); // Looting this monster corpse will be a criminal act!
                }
                else
                {
                    from.SendLocalizedMessage(1005038); // Looting this corpse will be a criminal act!
                }
            }

            return true;
        }

        public virtual void Open(Mobile from, bool checkSelfLoot)
        {
            if (from.IsStaff() || from.InRange(GetWorldLocation(), 2))
            {
                #region Self Looting
                bool selfLoot = checkSelfLoot && from == m_Owner;

                if (selfLoot)
                {
                    SetFlag(CorpseFlag.SelfLooted, true);

                    List<Item> items = new List<Item>(Items);

                    bool gathered = false;

                    for (int k = 0; k < EquipItems.Count; ++k)
                    {
                        Item item2 = EquipItems[k];

                        if (!items.Contains(item2) && item2.IsChildOf(from.Backpack))
                        {
                            items.Add(item2);
                            gathered = true;
                        }
                    }

                    bool didntFit = false;

                    Container pack = from.Backpack;

                    for (int i = 0; !didntFit && i < items.Count; ++i)
                    {
                        Item item = items[i];
                        Point3D loc = item.Location;

                        if (item.Layer == Layer.Hair || item.Layer == Layer.FacialHair || !item.Movable)
                        {
                            continue;
                        }

                        if (from.FindItemOnLayer(Layer.OuterTorso) is DeathRobe robe)
                        {
                            robe.Delete();
                        }

                        if (m_EquipItems.Contains(item) && from.EquipItem(item))
                        {
                            gathered = true;
                        }
                        else if (m_RestoreTable != null && pack != null && pack.CheckHold(from, item, false, true, true) && m_RestoreTable.ContainsKey(item))
                        {
                            item.Location = loc;
                            pack.AddItem(item);
                            gathered = true;
                        }
                        else
                        {
                            didntFit = true;
                        }
                    }

                    if (gathered && !didntFit)
                    {
                        SetFlag(CorpseFlag.Carved, true);

                        if (ItemID == 0x2006)
                        {
                            ProcessDelta();
                            SendRemovePacket();
                            ItemID = Utility.Random(0xECA, 9); // bone graphic
                            Hue = 0;
                            ProcessDelta();
                        }

                        from.PlaySound(0x3E3);
                        from.SendLocalizedMessage(1062471); // You quickly gather all of your belongings.
                        items.Clear();
                        m_EquipItems.Clear();
                        return;
                    }

                    if (gathered)
                    {
                        from.SendLocalizedMessage(1062472); // You gather some of your belongings. The rest remain on the corpse.
                    }
                }
                #endregion

                if (!CheckLoot(from))
                {
                    return;
                }

                #region Quests
                if (from is PlayerMobile player)
                {
                    QuestSystem qs = player.Quest;

                    if (qs is TheSummoningQuest && qs.FindObjective(typeof(VanquishDaemonObjective)) is VanquishDaemonObjective obj && obj.Completed && obj.CorpseWithSkull == this)
                    {
                        GoldenSkull sk = new GoldenSkull();

                        if (player.PlaceInBackpack(sk))
                        {
                            obj.CorpseWithSkull = null;
                            qs.Complete();
                            player.SendLocalizedMessage(1050022); // For your valor in combating the devourer, you have been awarded a golden skull.
                        }
                        else
                        {
                            sk.Delete();
                            player.SendLocalizedMessage(1050023); // You find a golden skull, but your backpack is too full to carry it.
                        }
                    }
                }
                #endregion

                base.OnDoubleClick(from);
            }
            else
            {
                from.SendLocalizedMessage(500446); // That is too far away.
            }
        }

        public override void OnDoubleClick(Mobile from)
        {
            Open(from, true);

            if (m_Owner == from)
            {
                if (from.Corpse != null)
                    from.NetState.Send(new RemoveWaypoint(from.Corpse.Serial));
            }
        }

        public override bool CheckContentDisplay(Mobile from)
        {
            return false;
        }

        public override bool DisplaysContent => false;

        public override void AddNameProperty(ObjectPropertyList list)
        {
            if (ItemID == 0x2006) // Corpse form
            {
                if (m_CorpseName != null)
                {
                    list.Add(m_CorpseName);
                }
                else
                {
                    list.Add(1046414, Name); // the remains of ~1_NAME~
                }
            }
            else // Bone form
            {
                list.Add(1046414, Name); // the remains of ~1_NAME~
            }
        }

        public override void OnAosSingleClick(Mobile from)
        {
            int hue = Notoriety.GetHue(NotorietyHandlers.CorpseNotoriety(from, this));
            ObjectPropertyList opl = PropertyList;

            if (opl.Header > 0)
            {
                from.Send(new MessageLocalized(Serial, ItemID, MessageType.Label, hue, 3, opl.Header, Name, opl.HeaderArgs));
            }
        }

        public bool Carve(Mobile from, Item item)
        {
            if (IsCriminalAction(from) && Map != null && (Map.Rules & MapRules.HarmfulRestrictions) != 0)
            {
                if (m_Owner == null || !m_Owner.Player)
                {
                    from.SendLocalizedMessage(1005035); // You did not earn the right to loot this creature!
                }
                else
                {
                    from.SendLocalizedMessage(1010049); // You may not loot this corpse.
                }

                return false;
            }

            Mobile dead = m_Owner;

            if (GetFlag(CorpseFlag.Carved) || dead == null)
            {
                PrivateOverheadMessage(MessageType.Regular, 0x3B2, 500485, from.NetState); // You see nothing useful to carve from the corpse.
            }
            else if (dead is PlayerMobile && ((Body)Amount).IsHuman && ItemID == 0x2006)
            {
                new Blood(0x122D).MoveToWorld(Location, Map);

                new Torso().MoveToWorld(Location, Map);
                new LeftLeg().MoveToWorld(Location, Map);
                new LeftArm().MoveToWorld(Location, Map);
                new RightLeg().MoveToWorld(Location, Map);
                new RightArm().MoveToWorld(Location, Map);
                new Head(dead.Name).MoveToWorld(Location, Map);

                SetFlag(CorpseFlag.Carved, true);

                ProcessDelta();
                SendRemovePacket();
                ItemID = Utility.Random(0xECA, 9); // bone graphic
                Hue = 0;
                ProcessDelta();

                if (IsCriminalAction(from))
                {
                    from.CriminalAction(true);
                }
            }
            else if (dead is BaseCreature creature)
            {
                creature.OnCarve(from, this, item);
            }
            else
            {
                from.SendLocalizedMessage(500485); // You see nothing useful to carve from the corpse.
            }

            return true;
        }

        public override void Delete()
        {
            base.Delete();

            if (PlayerCorpses != null && PlayerCorpses.Remove(this))
            {
                if (PlayerCorpses.Count == 0)
                    PlayerCorpses = null;
            }

            if (m_HasLooted != null)
            {
                ColUtility.Free(m_HasLooted);
                m_HasLooted = null;
            }
        }

        public static Dictionary<Corpse, int> PlayerCorpses { get; set; }
    }
}
