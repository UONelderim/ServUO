using Server.Engines.PartySystem;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using System;
using System.Collections.Generic;

namespace Server.Items
{
    [Flipable(17615, 17616)]
    public class LobsterTrap : Container, ITelekinesisable, IBaitable
    {
        public static readonly int TrapID = Utility.RandomMinMax(17615, 17616);
        public static readonly int BuoyID = 17611;
        public static readonly int MaxCatch = 5;

        private Type m_BaitType;
        private bool m_EnhancedBait;
        private int m_BaitUses;
        private bool m_InUse;
        private Timer m_Timer;
        private Mobile m_Owner;
        private int m_Bobs;

        [CommandProperty(AccessLevel.GameMaster)]
        public Type BaitType
        {
            get { return m_BaitType; }
            set
            {
                m_BaitType = value;

                if (m_BaitType == null)
                {
                    m_EnhancedBait = false;
                    m_BaitUses = 0;
                }

                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool EnhancedBait { get { return m_EnhancedBait; } set { m_EnhancedBait = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int BaitUses { get { return m_BaitUses; } set { m_BaitUses = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool InUse { get { return m_InUse; } set { m_InUse = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile Owner { get { return m_Owner; } set { m_Owner = value; InvalidateProperties(); } }

        public override int LabelNumber => 1096487;
        public override bool DisplaysContent => false;

        [Constructable]
        public LobsterTrap() : base(17615)
        {
        }

        public void OnTelekinesis(Mobile from)
        {
            if (m_InUse && ItemID == BuoyID && CanUseTrap(from))
                EndTimer(from);
        }

        public override bool OnDragDropInto(Mobile from, Item item, Point3D p)
        {
            return false;
        }

        public override bool OnDragDrop(Mobile from, Item dropped)
        {
            return false;
        }

        public override bool TryDropItem(Mobile from, Item dropped, bool sendFullMessage)
        {
            return false;
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            if (ItemID == TrapID)
            {
                if (Items.Count == 0)
                    list.Add(1116389); //empty lobster trap
                else if (Items.Count >= MaxCatch)
                    list.Add(1149599); //full lobster trap
                else
                    list.Add(1096487); //lobster trap
            }
            else
            {
                if (m_Owner == null)
                    list.Add(1096487); //lobster trap
                else
                    list.Add(1116390, m_Owner.Name);
            }
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            if (m_BaitType != null)
            {
                object label = FishInfo.GetFishLabel(m_BaitType);
                if (label is int)
                    list.Add(1116468, string.Format("#{0}", (int)label)); //baited to attract: ~1_val~
                else if (label is string)
                    list.Add(1116468, (string)label);

                list.Add(1116466, m_BaitUses.ToString());
            }
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (IsChildOf(from.Backpack))
            {
                if (ItemID == BuoyID)
                    ItemID = TrapID;

                if (Items.Count > 0)
                {
                    DumpContents(from);
                    from.SendMessage("Wysypujesz zawartosc pulapki na homary do swojego plecaka.");
                }
                else
                {
                    from.SendLocalizedMessage(500974); //What water do you want to fish in?
                    from.BeginTarget(-1, true, TargetFlags.None, OnTarget);
                }
            }
            else if (ItemID == BuoyID)
            {
                if (RootParent != null)
                    ItemID = TrapID;

                InvalidateProperties();

                if (CanUseTrap(from))
                    EndTimer(from);

                InvalidateProperties();
            }
            else
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
            }
        }

        private void DumpContents(Mobile from)
        {
            Container pack = from.Backpack;

            foreach (Item item in new List<Item>(Items))
            {
                if (item == null)
                    continue;

                if (!pack.TryDropItem(from, item, false))
                    item.MoveToWorld(from.Location, from.Map);

                from.SendLocalizedMessage(1116386, string.Format("#{0}", item.LabelNumber));
            }
        }

        public void OnTarget(Mobile from, object targeted)
        {
            if (Deleted || m_InUse)
                return;

            IPoint3D pnt = (IPoint3D)targeted;
            Map map = from.Map;

            if (map == null || map == Map.Internal)
                return;

            int x = pnt.X; int y = pnt.Y; int z = pnt.Z;

            if (!from.InRange(pnt, 6))
            {
                from.SendLocalizedMessage(1116388); // The trap is too cumbersome to deploy that far away.
            }
            else if (!IsValidTile(targeted, map))
                from.SendLocalizedMessage(3070068); //Tutaj nie postawisz pulapki
            else if (!IsValidLocation(x, y, z, map))
                from.SendLocalizedMessage(1116393); //The location is too close to another trap.
            else
            {
                m_Owner = from;
                ItemID = BuoyID;
                InvalidateProperties();
                Movable = false;
                MoveToWorld(new Point3D(x, y, z), map);
                m_Bobs = 0;
                m_InUse = true;
                StartTimer();

                Effects.PlaySound(this, map, Utility.Random(0x025, 3));
                Effects.SendMovingEffect(from, this, TrapID, 7, 0, false, false);
            }
        }

        public void StartTimer()
        {
            if (m_Timer != null)
                m_Timer.Stop();

            m_Timer = Timer.DelayCall(TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1), OnTick);
        }

        public void EndTimer(Mobile from)
        {
            if (m_Timer != null)
            {
                m_Timer.Stop();
                m_Timer = null;
            }

            if (from == null) from = m_Owner;
            if (from == null)
                return;

            Movable = true;
            ItemID = TrapID;
            InvalidateProperties();
            m_InUse = false;

            if (RootParent == null)
            {
                if (from.Backpack == null || !from.Backpack.TryDropItem(from, this, false))
                    MoveToWorld(from.Location, from.Map);
            }
        }

        public void OnTick()
        {
            m_Bobs++;

            PublicOverheadMessage(MessageType.Regular, 0, 1116364); //**bob**

            if (m_Owner != null && (!SpecialFishingNet.ValidateDeepWater(Map, X, Y) || m_Owner.Skills[SkillName.Fishing].Base >= 75.0))
            {
                m_Owner.CheckSkill(SkillName.Fishing, 0, m_Owner.Skills[SkillName.Fishing].Cap);
            }

            if (!m_InUse)
            {
                EndTimer(null);
                return;
            }

            if (m_Bobs * 5 > Utility.Random(100))
            {
                OnTrapLost();
                return;
            }

            bool rare = true;
            double bump = m_Bobs / 100.0;

            Type type = FishInfo.GetSpecialItem(m_Owner, this, Location, bump, this is LavaLobsterTrap);

            if (type != null)
            {
                Item item = Loot.Construct(type);
                DropItem(item);

                if (item is RareCrabAndLobster && rare)
                {
                    RareCrabAndLobster fish = (RareCrabAndLobster)item;

                    fish.Fisher = m_Owner;
                    fish.DateCaught = DateTime.UtcNow;
                    fish.Weight = Utility.RandomMinMax(10, 200);
                    fish.Stackable = false;
                }

                if (m_Owner != null)
                    m_Owner.SendMessage("Wyglada na to, ze cos zlapales!");

                CheckBait();
            }
            else if (Utility.RandomBool())
            {
                Item item;

                if (Utility.RandomBool())
                    item = new Crab();
                else
                    item = new Lobster();

                if (m_Owner != null)
                    m_Owner.SendMessage("Wyglada na to, ze cos zlapales!");

                DropItem(item);
                CheckBait();
            }
        }

        private void CheckBait()
        {
            if (m_BaitType != null)
            {
                BaitUses--;

                if (m_BaitUses == 0)
                {
                    BaitType = null;
                    EnhancedBait = false;

                    if (m_Owner != null)
                        m_Owner.SendMessage("Wykorzystales cala przynete.");
                }
            }
        }

        public void OnTrapLost()
        {
            if (m_Timer != null)
                m_Timer.Stop();

            Effects.PlaySound(this, Map, Utility.Random(0x025, 3));

            IPooledEnumerable eable = GetMobilesInRange(12);
            foreach (Mobile mob in eable)
            {
                if (mob is PlayerMobile && m_Owner != null)
                    mob.SendLocalizedMessage(1116385, m_Owner.Name); //~1_NAME~'s trap bouy is pulled beneath the waves.
            }
            eable.Free();

            Delete();
        }

        private bool CanUseTrap(Mobile from)
        {
            if (m_Owner == null || RootParent != null)
                return false;

            if (!from.InRange(Location, 6))
            {
                from.SendLocalizedMessage(500295); //You are too far away to do that.
                return false;
            }

            //is owner, or in same guild
            if (m_Owner == from || (from.Guild != null && from.Guild == m_Owner.Guild))
                return true;

            //partied
            if (Party.Get(from) == Party.Get(m_Owner))
                return true;

            //fel rules
            if (from.Map != null && from.Map.Rules == MapRules.FeluccaRules)
            {
                from.CriminalAction(true);
                from.SendLocalizedMessage(1149823); //The owner of the lobster trap notices you committing a criminal act!

                if (m_Owner != null)
                    m_Owner.SendMessage("Widzisz jak ktos dobiera sie do Twojej pulapki na homary");

                return true;
            }

            from.SendLocalizedMessage(1116391); //You realize that the trap isn't yours so you leave it alone.
            return false;
        }

        public virtual bool IsValidTile(object targeted, Map map)
        {
            int tileID = 0;

            if (targeted is LandTarget)
            {
                LandTarget obj = (LandTarget)targeted;
                tileID = obj.TileID;
            }

            else if (targeted is StaticTarget)
            {
                StaticTarget obj = (StaticTarget)targeted;
                tileID = obj.ItemID;
            }

            for (int i = 0; i < UseableTiles.Length; i++)
            {
                if (UseableTiles[i] == tileID)
                    return true;
            }
            return false;
        }

        public bool IsValidLocation(int x, int y, int z, Map map)
        {
            IPooledEnumerable eable = map.GetItemsInRange(new Point3D(x, y, z), 1);

            foreach (Item item in eable)
            {
                if (item is LobsterTrap)
                {
                    eable.Free();
                    return false;
                }
            }
            eable.Free();
            return true;
        }

        public virtual int[] UseableTiles => m_WaterTiles;
        private readonly int[] m_WaterTiles = new int[]
        {
            //Deep Water
            0x00AA, 0x00A9,
            0x00A8, 0x00AB,
            0x0136, 0x0137,
            //Shallow Water
            0x5797, 0x579C,
            0x746E, 0x7485,
            0x7490, 0x74AB,
            0x74B5, 0x75D5,
            //Static tiles
            0x1797, 0x1798,
            0x1799, 0x179A,
            0x179B, 0x179C,

        };

        public LobsterTrap(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);

            int index = FishInfo.GetIndexFromType(m_BaitType);
            writer.Write(index);
            writer.Write(m_Bobs);
            writer.Write(m_InUse);
            writer.Write(m_Owner);
            writer.Write(m_BaitUses);
            writer.Write(m_EnhancedBait);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            int index = reader.ReadInt();
            m_BaitType = FishInfo.GetTypeFromIndex(index);

            m_Bobs = reader.ReadInt();
            m_InUse = reader.ReadBool();
            m_Owner = reader.ReadMobile();
            m_BaitUses = reader.ReadInt();
            m_EnhancedBait = reader.ReadBool();

            if (m_BaitType != null && m_BaitUses <= 0)
                BaitType = null;

            if (m_InUse)
                StartTimer();
        }
    }
}
