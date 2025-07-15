using Server.ContextMenus;
using Server.Engines.PartySystem;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.Items
{
    public class TreasureMapChest : LockableContainer
    {
        private static readonly TimeSpan _DeleteTime = TimeSpan.FromHours(3);

        private List<Item> m_Lifted = new List<Item>();

        private ChestQuality _Quality;

        [Constructable]
        public TreasureMapChest(int level)
            : this(null, level, false)
        {
        }

        public TreasureMapChest(Mobile owner, int level, bool temporary)
            : base(0xE40)
        {
            Owner = owner;
            Level = level;
            DeleteTime = DateTime.UtcNow + _DeleteTime;

            Temporary = temporary;
            Guardians = new List<Mobile>();
            AncientGuardians = new List<Mobile>();

            TimerRegistry.Register("TreasureMapChest", this, _DeleteTime, chest => chest.Delete());
        }

        public TreasureMapChest(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber => 3000541;

        [CommandProperty(AccessLevel.GameMaster)]
        public int Level { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile Owner { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public DateTime DeleteTime { get; private set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public DateTime DigTime { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool Temporary { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool FirstOpenedByOwner { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public TreasureMap TreasureMap { get; set; }

        public Timer Timer { get; set; }

        public List<Mobile> Guardians { get; set; }

        public List<Mobile> AncientGuardians { get; set; }

        public ChestQuality ChestQuality
        {
            get { return _Quality; }
            set
            {
                if (_Quality != value)
                {
                    _Quality = value;

                    switch (_Quality)
                    {
                        case ChestQuality.Rusty: ItemID = 0xA306; break;
                        case ChestQuality.Standard: ItemID = 0xA304; break;
                        case ChestQuality.Gold: ItemID = 0xA308; break;
                    }
                }
            }
        }

        public bool FailedLockpick { get; set; }

        public override bool IsDecoContainer => false;
        
        public override bool CheckLocked(Mobile from)
        {
            if (from.AccessLevel > AccessLevel.Player)
            {
                return false;
            }

			if (CanOpen(from))
            {
                return base.CheckLocked(from);
            }

            return true;
        }

        public virtual bool CanOpen(Mobile from)
        {
            if (!Locked && TrapType != TrapType.None)
            {
                from.SendLocalizedMessage(1159008); // That appears to be trapped, using the remove trap skill would yield better results...
                return false;
            }
            if (AncientGuardians.Any(ag => ag.Alive))
            {
                from.SendLocalizedMessage(1046448); // You must first kill the guardians before you may open this chest.
                return false;
            }

            return !Locked;
        }

        public override bool CheckItemUse(Mobile from, Item item)
        {
            return CheckLoot(from, item != this) && base.CheckItemUse(from, item);
        }

        public override bool CheckLift(Mobile from, Item item, ref LRReason reject)
        {
            return CheckLoot(from, true) && base.CheckLift(from, item, ref reject);
        }

        public override void OnItemLifted(Mobile from, Item item)
        {
            bool notYetLifted = !m_Lifted.Contains(item);

            from.RevealingAction();

            if (notYetLifted)
            {
                m_Lifted.Add(item);

                if (0.1 >= Utility.RandomDouble()) // 10% chance to spawn a new monster
                {
                    BaseCreature spawn = TreasureMap.Spawn(Level, GetWorldLocation(), Map, from, false);

                    if (spawn != null)
                    {
                        spawn.Hue = 0;
                    }
                }
            }

            base.OnItemLifted(from, item);
        }

        public void SpawnAncientGuardian(Mobile from)
        {
            ExecuteTrap(from);

            if (!AncientGuardians.Any(g => g != null && g.Alive))
            {
                BaseCreature spawn = TreasureMap.Spawn(Level, GetWorldLocation(), Map, from, false);

                if (spawn != null)
                {
                    spawn.NoLootOnDeath = true;

                    spawn.Name = "Starozytny Potwor";
                    spawn.Title = "(Straznik)";
                    spawn.Tamable = false;

                    spawn.Hue = 1960;

                    for (int i = 0; i < spawn.Skills.Length; i++)
                    {
                        Skill skill = spawn.Skills[i];

                        if (skill.Base > 0.0)
                           skill.Base = skill.Base;
                    }

                    AncientGuardians.Add(spawn);
                }
            }
        }

        public override bool CheckHold(Mobile m, Item item, bool message, bool checkItems, bool checkWeight, int plusItems, int plusWeight)
        {
            if (m.AccessLevel < AccessLevel.GameMaster)
            {
                m.SendLocalizedMessage(1048122, "", 0x8A5); // The chest refuses to be filled with treasure again.
                return false;
            }

            return base.CheckHold(m, item, message, checkItems, checkWeight, plusItems, plusWeight);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(4); // version

            writer.Write(FailedLockpick);
            writer.Write((int)_Quality);
            writer.Write(DigTime);
            writer.Write(AncientGuardians, true);

            writer.Write(FirstOpenedByOwner);
            writer.Write(TreasureMap);

            writer.Write(Guardians, true);
            writer.Write(Temporary);

            writer.Write(Owner);

            writer.Write(Level);
            writer.WriteDeltaTime(DeleteTime);
            writer.Write(m_Lifted, true);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 4:
                    FailedLockpick = reader.ReadBool();
                    _Quality = (ChestQuality)reader.ReadInt();
                    DigTime = reader.ReadDateTime();
                    AncientGuardians = reader.ReadStrongMobileList();
                    goto case 3;
                case 3:
                    FirstOpenedByOwner = reader.ReadBool();
                    TreasureMap = reader.ReadItem() as TreasureMap;
                    goto case 2;
                case 2:
                    {
                        Guardians = reader.ReadStrongMobileList();
                        Temporary = reader.ReadBool();

                        goto case 1;
                    }
                case 1:
                    {
                        Owner = reader.ReadMobile();

                        goto case 0;
                    }
                case 0:
                    {
                        Level = reader.ReadInt();
                        DeleteTime = reader.ReadDeltaTime();
                        m_Lifted = reader.ReadStrongItemList();

                        if (version < 2)
                            Guardians = new List<Mobile>();
                        if (version < 4)
	                        AncientGuardians = new List<Mobile>();

                        break;
                    }
            }

            if (!Temporary && DeleteTime > DateTime.UtcNow)
            {
                TimerRegistry.Register("TreasureMapChest", this, DeleteTime - DateTime.UtcNow, chest => chest.Delete());
            }
            else
            {
                Delete();
            }
        }

        public override void OnAfterDelete()
        {
            if (Timer != null)
                Timer.Stop();

            Timer = null;

            base.OnAfterDelete();
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);

            if (from.Alive)
                list.Add(new RemoveEntry(from, this));
        }

        public override void LockPick(Mobile from)
        {
            base.LockPick(from);

            if (Map != null && (FailedLockpick || 0.05 >= Utility.RandomDouble()))
            {
                Grubber grubber = new Grubber();
                grubber.MoveToWorld(Map.GetSpawnPosition(Location, 1), Map);

                Item item = null;

                if (Items.Count > 0)
                {
                    do
                    {
                        item = Items[Utility.Random(Items.Count)];
                    }
                    while (item == null || item.LootType == LootType.Blessed);
                }

                grubber.PackItem(item);
                grubber.PrivateOverheadMessage(MessageType.Regular, 33, 1159062, from.NetState); // *A grubber appears and ganks a piece of your loot!*
            }
        }

        public override void DisplayTo(Mobile to)
        {
            base.DisplayTo(to);

            if (!FirstOpenedByOwner && to == Owner)
            {
                if (TreasureMap != null)
                {
                    TreasureMap.OnChestOpened((PlayerMobile)to, this);
                }

                FirstOpenedByOwner = true;
            }
        }

        public override bool ExecuteTrap(Mobile from)
        {
            if (TrapType != TrapType.None)
            {
                int damage;

                if (TrapLevel > 0)
                    damage = Utility.RandomMinMax(10, 30) * TrapLevel;
                else
                    damage = TrapPower;

                AOS.Damage(from, damage, 0, 100, 0, 0, 0);

                // Your skin blisters from the heat!
                from.LocalOverheadMessage(MessageType.Regular, 0x2A, 503000);

                Effects.SendLocationEffect(from.Location, from.Map, 0x36BD, 15, 10);
                Effects.PlaySound(from.Location, from.Map, 0x307);

                return true;
            }
            return base.ExecuteTrap(from);
        }

        public void BeginRemove(Mobile from)
        {
            if (!from.Alive)
                return;

            from.CloseGump(typeof(RemoveGump));
            from.SendGump(new RemoveGump(from, this));
        }

        public void EndRemove(Mobile from)
        {
            if (Deleted || from != Owner || !from.InRange(GetWorldLocation(), 3))
                return;

            from.SendLocalizedMessage(1048124, "", 0x8A5); // The old, rusted chest crumbles when you hit it.
            Delete();
        }

        private bool CheckLoot(Mobile m, bool criminalAction)
        {
            if (Temporary)
                return false;

            if (m.AccessLevel >= AccessLevel.GameMaster || Owner == null || m == Owner)
                return true;

            Party p = Party.Get(Owner);

            if (p != null && p.Contains(m))
                return true;

            Map map = Map;

            if (map != null && (map.Rules & MapRules.HarmfulRestrictions) == 0)
            {
                if (criminalAction)
                    m.CriminalAction(true);
                else
                    m.SendLocalizedMessage(1010630); // Taking someone else's treasure is a criminal offense!

                return true;
            }

            m.SendLocalizedMessage(1010631); // You did not discover this chest!
            return false;
        }

        private class RemoveGump : Gump
        {
            private readonly Mobile m_From;
            private readonly TreasureMapChest m_Chest;
            public RemoveGump(Mobile from, TreasureMapChest chest)
                : base(15, 15)
            {
                m_From = from;
                m_Chest = chest;

                Closable = false;
                Disposable = false;

                AddPage(0);

                AddBackground(30, 0, 240, 240, 2620);

                AddHtmlLocalized(45, 15, 200, 80, 1048125, 0xFFFFFF, false, false); // When this treasure chest is removed, any items still inside of it will be lost.
                AddHtmlLocalized(45, 95, 200, 60, 1048126, 0xFFFFFF, false, false); // Are you certain you're ready to remove this chest?

                AddButton(40, 153, 4005, 4007, 1, GumpButtonType.Reply, 0);
                AddHtmlLocalized(75, 155, 180, 40, 1048127, 0xFFFFFF, false, false); // Remove the Treasure Chest

                AddButton(40, 195, 4005, 4007, 2, GumpButtonType.Reply, 0);
                AddHtmlLocalized(75, 197, 180, 35, 1006045, 0xFFFFFF, false, false); // Cancel
            }

            public override void OnResponse(NetState sender, RelayInfo info)
            {
                if (info.ButtonID == 1)
                    m_Chest.EndRemove(m_From);
            }
        }

        private class RemoveEntry : ContextMenuEntry
        {
            private readonly Mobile m_From;
            private readonly TreasureMapChest m_Chest;
            public RemoveEntry(Mobile from, TreasureMapChest chest)
                : base(6149, 3)
            {
                m_From = from;
                m_Chest = chest;

                Enabled = (from == chest.Owner);
            }

            public override void OnClick()
            {
                if (m_Chest.Deleted || m_From != m_Chest.Owner || !m_From.CheckAlive())
                    return;

                m_Chest.BeginRemove(m_From);
            }
        }
    }
}
