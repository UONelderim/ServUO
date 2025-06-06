using Server.Commands;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Multis;
using Server.Engines.RisingTide;

using System;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class BountyQuestSpawner : Item
    {
        public static void Configure()
        {
            m_ActiveZones = new Dictionary<SpawnZone, List<BaseShipCaptain>>();

            foreach (int i in Enum.GetValues(typeof(SpawnZone)))
                m_ActiveZones.Add((SpawnZone)i, new List<BaseShipCaptain>());
        }

        public static void GenerateShipSpawner()
        {
            if (m_Instance == null)
            {
                m_Instance = new BountyQuestSpawner();
                m_Instance.MoveToWorld(new Point3D(4558, 2347, 0), Map.Felucca);
            }
        }

        private static readonly int[] GoldRange =
        {
            1000, 10000
        };

        private static BountyQuestSpawner m_Instance;
        public static BountyQuestSpawner Instance => m_Instance;

        private static readonly Dictionary<Mobile, int> m_Bounties = new Dictionary<Mobile, int>();
        public static Dictionary<Mobile, int> Bounties => m_Bounties;

        private static readonly Dictionary<SpawnZone, SpawnDefinition> m_Zones = new Dictionary<SpawnZone, SpawnDefinition>();
        public static Dictionary<SpawnZone, SpawnDefinition> Zones => m_Zones;

        private Timer m_Timer;
        public Timer Timer => m_Timer;

        private static Dictionary<SpawnZone, List<BaseShipCaptain>> m_ActiveZones;

        private int m_MaxTram;
        private int m_MaxFel;
        private int m_MaxTokuno;
        private TimeSpan m_SpawnTime;
        private bool m_Active;

        [CommandProperty(AccessLevel.GameMaster)]
        public int aFelMoonglowCount => m_ActiveZones[SpawnZone.FelMoonglow].Count;

        [CommandProperty(AccessLevel.GameMaster)]
        public int aTramMoonglowCount => m_ActiveZones[SpawnZone.TramMoonglow].Count;

        [CommandProperty(AccessLevel.GameMaster)]
        public int aTramJhelomCount => m_ActiveZones[SpawnZone.TramJhelom].Count;

        [CommandProperty(AccessLevel.GameMaster)]
        public int aFelJhelomCount => m_ActiveZones[SpawnZone.FelJhelom].Count;

        [CommandProperty(AccessLevel.GameMaster)]
        public int aTokunoPirateCount => m_ActiveZones[SpawnZone.TokunoPirate].Count;

        [CommandProperty(AccessLevel.GameMaster)]
        public int aTramMerch1Count => m_ActiveZones[SpawnZone.TramMerch1].Count;

        [CommandProperty(AccessLevel.GameMaster)]
        public int aFelMerch1Count => m_ActiveZones[SpawnZone.FelMerch1].Count;

        [CommandProperty(AccessLevel.GameMaster)]
        public int aTramMerch2Count => m_ActiveZones[SpawnZone.TramMerch2].Count;

        [CommandProperty(AccessLevel.GameMaster)]
        public int aFelMerch2Count => m_ActiveZones[SpawnZone.FelMerch2].Count;

        [CommandProperty(AccessLevel.GameMaster)]
        public int aTokunoMerchCount => m_ActiveZones[SpawnZone.TokunoMerch].Count;

        [CommandProperty(AccessLevel.GameMaster)]
        public int MaxTram { get { return m_MaxTram; } set { m_MaxTram = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int MaxFel { get { return m_MaxFel; } set { m_MaxFel = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int MaxTokuno { get { return m_MaxTokuno; } set { m_MaxTokuno = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan SpawnTime
        {
            get { return m_SpawnTime; }
            set
            {
                m_SpawnTime = value;

                if (m_Timer != null)
                    m_Timer.Stop();

                m_Timer = Timer.DelayCall(m_SpawnTime, m_SpawnTime, OnTick);

            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int bMerchantCount
        {
            get
            {
                int count = 0;

                foreach (List<BaseShipCaptain> list in m_ActiveZones.Values)
                {
                    foreach (BaseShipCaptain capt in list)
                    {
                        if (capt is MerchantCaptain)
                            count++;
                    }
                }

                return count;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int bPirateCount => m_Bounties.Count;

        [CommandProperty(AccessLevel.GameMaster)]
        public bool Active
        {
            get { return m_Active; }
            set
            {
                if (m_Active != value)
                {
                    if (value)
                    {
                        if (m_Timer != null)
                        {
                            m_Timer.Stop();
                            m_Timer = null;
                        }

                        m_Timer = Timer.DelayCall(m_SpawnTime, m_SpawnTime, OnTick);
                    }
                    else
                    {
                        if (m_Timer != null)
                            m_Timer.Stop();

                        m_Timer = null;

                        RemoveSpawns();
                    }
                }

                m_Active = value;
            }
        }

        [Constructable]
        public BountyQuestSpawner() : base(0xED4)
        {
            if (m_Instance != null && !m_Instance.Deleted)
            {
                Active = false;
                Delete();
            }

            Name = "Pirate/Merchant Spawner";
            Visible = false;
            Movable = false;
            m_SpawnTime = TimeSpan.FromMinutes(15);

            m_MaxTram = 1;
            m_MaxFel = 1;
            m_MaxTokuno = 1;

            Active = true;
            m_Instance = this;
        }

        public override void Delete()
        {
            base.Delete();

            Active = false;
        }

        public static void AddZone(SpawnDefinition def)
        {
            if (!m_Zones.ContainsKey(def.Zone))
                m_Zones.Add(def.Zone, def);
        }

        public void HandleDeath(BaseShipCaptain captain)
        {
            if (captain is PirateCaptain)
                RemoveBounty(captain);

            else if (captain is MerchantCaptain)
                RemoveMerchant(captain);
        }

        public void RemoveBounty(BaseShipCaptain pirate)
        {
            SpawnZone zone = pirate.Zone;

            if (m_ActiveZones[zone].Contains(pirate))
                m_ActiveZones[zone].Remove(pirate);

            if (m_Bounties.ContainsKey(pirate))
                m_Bounties.Remove(pirate);
        }

        public void RemoveMerchant(BaseShipCaptain merchant)
        {
            SpawnZone zone = merchant.Zone;

            if (m_ActiveZones[zone].Contains(merchant))
                m_ActiveZones[zone].Remove(merchant);
        }

        public void OnTick()
        {
            if (m_Active)
                SpawnRandom();
        }

        public void SpawnRandom()
        {
            foreach (int i in Enum.GetValues(typeof(SpawnZone)))
            {
                SpawnZone zone = (SpawnZone)i;

                switch (zone)
                {
                    case SpawnZone.FelMoonglow:
                        if (m_ActiveZones[zone].Count < m_MaxFel)
                            SpawnPirateAndGalleon(zone, Map.Felucca);
                        break;
                    case SpawnZone.FelMerch1:
                        if (m_ActiveZones[zone].Count < m_MaxFel)
                            SpawnMerchantAndGalleon(zone, Map.Felucca);
                        break;
                    case SpawnZone.FelMerch2:
                        if (m_ActiveZones[zone].Count < m_MaxFel)
                            SpawnMerchantAndGalleon(zone, Map.Felucca);
                        break;
                }
            }
        }

        public void RemoveSpawns()
        {
            List<BaseShipCaptain> ToRemove = new List<BaseShipCaptain>();

            foreach (List<BaseShipCaptain> list in m_ActiveZones.Values)
            {
                foreach (BaseShipCaptain capt in list)
                    ToRemove.Add(capt);
            }

            foreach (BaseShipCaptain cap in ToRemove)
                cap.TryDecayGalleon(cap.Galleon);
        }

        public void SpawnPirateAndGalleon(SpawnZone zone, Map map)
        {
            SpawnDefinition def = m_Zones[zone];

            if (map != Map.Internal && map != null)
            {
                Rectangle2D rec = def.SpawnRegion;
                OrcishGalleon gal = new OrcishGalleon(Direction.North);
                PirateCaptain pirate = new PirateCaptain(gal)
                {
                    Zone = zone
                };
                gal.Owner = pirate;
                Point3D p = Point3D.Zero;
                bool spawned = false;
                for (int i = 0; i < 25; i++)
                {
                    int x = Utility.Random(rec.X, rec.Width);
                    int y = Utility.Random(rec.Y, rec.Height);
                    p = new Point3D(x, y, -5);

                    if (gal.CanFit(p, map, gal.ItemID))
                    {
                        spawned = true;
                        break;
                    }
                }

                if (!spawned)
                {
                    gal.Delete();
                    pirate.Delete();
                    return;
                }

                int gold = Utility.RandomMinMax(GoldRange[0], GoldRange[1]);
                gal.MoveToWorld(p, map);
                gal.AutoAddCannons(pirate);
                pirate.MoveToWorld(new Point3D(gal.X, gal.Y - 1, gal.ZSurface), map);

                int crewCount = Utility.RandomMinMax(3, 5);

                for (int i = 0; i < crewCount; i++)
                {
                    Mobile crew = new PirateCrew();

                    if (i == 0)
                        crew.Title = "- kapitan orkow";

                    pirate.AddToCrew(crew);
                    crew.MoveToWorld(new Point3D(gal.X + Utility.RandomList(-1, 1), gal.Y + Utility.RandomList(-1, 0, 1), gal.ZSurface), map);
                }

                Point2D[] course = def.GetRandomWaypoints();
                gal.BoatCourse = new BoatCourse(gal, new List<Point2D>(def.GetRandomWaypoints()));

                gal.NextNavPoint = 0;
                gal.StartCourse(false, false);

                FillHold(gal);

                m_Bounties.Add(pirate, gold);
                m_ActiveZones[zone].Add(pirate);
            }
        }

        public void SpawnMerchantAndGalleon(SpawnZone zone, Map map)
        {
            SpawnDefinition def = m_Zones[zone];

            if (map != Map.Internal && map != null)
            {
                Rectangle2D rec = def.SpawnRegion;
                bool garg = Utility.RandomBool();
                BaseGalleon gal;
                Point3D p = Point3D.Zero;
                bool spawned = false;

                if (garg)
                    gal = new GargishGalleon(Direction.North);
                else
                    gal = new TokunoGalleon(Direction.North);

                MerchantCaptain captain = new MerchantCaptain(gal);

                for (int i = 0; i < 25; i++)
                {
                    int x = Utility.Random(rec.X, rec.Width);
                    int y = Utility.Random(rec.Y, rec.Height);
                    p = new Point3D(x, y, -5);

                    if (gal.CanFit(p, map, gal.ItemID))
                    {
                        spawned = true;
                        break;
                    }
                }

                if (!spawned)
                {
                    gal.Delete();
                    captain.Delete();
                    return;
                }

                gal.Owner = captain;
                captain.Zone = zone;
                gal.MoveToWorld(p, map);
                gal.AutoAddCannons(captain);
                captain.MoveToWorld(new Point3D(gal.X, gal.Y - 1, gal.ZSurface), map);

                int crewCount = Utility.RandomMinMax(3, 5);

                for (int i = 0; i < crewCount; i++)
                {
                    Mobile crew = new MerchantCrew();
                    captain.AddToCrew(crew);
                    crew.MoveToWorld(new Point3D(gal.X + Utility.RandomList(-1, 1), gal.Y + Utility.RandomList(-1, 0, 1), gal.ZSurface), map);
                }

                Point2D[] course = def.GetRandomWaypoints();
                gal.BoatCourse = new BoatCourse(gal, new List<Point2D>(def.GetRandomWaypoints()));

                gal.NextNavPoint = 0;
                gal.StartCourse(false, false);

                FillHold(gal);

                m_ActiveZones[zone].Add(captain);
            }
        }

        public static void ResetNavPoints(BaseBoat boat)
        {
            boat.NextNavPoint = 0;
            boat.StartCourse(false, false);
        }

        public static void FillHold(BaseGalleon galleon)
        {
            if (galleon == null)
                return;

            Container hold = galleon.GalleonHold;

            if (hold != null)
            {
                int cnt = Utility.RandomMinMax(7, 14);

                for (int i = 0; i < cnt; i++)
                {
                    Item item = RunicReforging.GenerateRandomItem(galleon);

                    if (item != null)
                        hold.DropItem(item);
                }

                hold.DropItem(new Ramrod());
                hold.DropItem(new Cannonball(Utility.RandomMinMax(7, 10)));              
                hold.DropItem(new Grapeshot(Utility.RandomMinMax(7, 10)));
                hold.DropItem(new PowderCharge(Utility.RandomMinMax(7, 10)));
                hold.DropItem(new FuseCord(Utility.RandomMinMax(7, 10)));

                if (.10 >= Utility.RandomDouble())
                    hold.DropItem(new SmugglersCache());

                if (.10 >= Utility.RandomDouble())
                {
                    FishSteak steaks = new FishSteak(5);
                    switch (Utility.Random(5))
                    {
                        case 0:
                            steaks.Name = "dorsz w przyprawach";
                            steaks.Hue = 1759;
                            break;
                        case 1:
                            steaks.Name = "suszony tunczyk";
                            steaks.Hue = 2108;
                            break;
                        case 2:
                            steaks.Name = "solona makrela";
                            steaks.Hue = 1864;
                            break;
                        case 3:
                            steaks.Name = "solony sledz";
                            steaks.Hue = 2302;
                            break;
                        case 4:
                            steaks.Name = "dorsz w przyprawach";
                            steaks.Hue = 1637;
                            break;
                    }

                    hold.DropItem(steaks);
                }

                hold.DropItem(new Gold(Utility.RandomMinMax(5000, 25000)));

                if (0.50 > Utility.RandomDouble())
                {
                    switch (Utility.Random(4))
                    {
                        case 0:
                        case 1:
                        case 2: hold.DropItem(new IronWire(Utility.RandomMinMax(1, 5))); break;
                        case 3:
                        case 4:
                        case 5: hold.DropItem(new CopperWire(Utility.RandomMinMax(1, 5))); break;
                        case 6:
                        case 7: hold.DropItem(new SilverWire(Utility.RandomMinMax(1, 5))); break;
                        case 8: hold.DropItem(new GoldWire(Utility.RandomMinMax(1, 5))); break;
                    }
                }

                switch (Utility.Random(8))
                {
                    case 0:
                        if (Utility.RandomBool())
                            hold.DropItem(new IronOre(Utility.RandomMinMax(40, 50)));
                        else
                            hold.DropItem(new IronIngot(Utility.RandomMinMax(40, 50)));
                        break;
                    case 1:
                        if (Utility.RandomBool())
                            hold.DropItem(new DullCopperOre(Utility.RandomMinMax(40, 50)));
                        else
                            hold.DropItem(new DullCopperIngot(Utility.RandomMinMax(40, 50)));
                        break;
                    case 2:
                        if (Utility.RandomBool())
                            hold.DropItem(new ShadowIronOre(Utility.RandomMinMax(40, 50)));
                        else
                            hold.DropItem(new ShadowIronIngot(Utility.RandomMinMax(40, 50)));
                        break;
                    case 3:
                        if (Utility.RandomBool())
                            hold.DropItem(new CopperOre(Utility.RandomMinMax(40, 50)));
                        else
                            hold.DropItem(new CopperIngot(Utility.RandomMinMax(40, 50)));
                        break;
                    case 4:
                        if (Utility.RandomBool())
                            hold.DropItem(new BronzeOre(Utility.RandomMinMax(40, 50)));
                        else
                            hold.DropItem(new BronzeIngot(Utility.RandomMinMax(40, 50)));
                        break;
                    case 5:
                        if (Utility.RandomBool())
                            hold.DropItem(new AgapiteOre(Utility.RandomMinMax(40, 50)));
                        else
                            hold.DropItem(new AgapiteIngot(Utility.RandomMinMax(40, 50)));
                        break;
                    case 6:
                        if (Utility.RandomBool())
                            hold.DropItem(new VeriteOre(Utility.RandomMinMax(40, 50)));
                        else
                            hold.DropItem(new VeriteIngot(Utility.RandomMinMax(40, 50)));
                        break;
                    case 7:
                        if (Utility.RandomBool())
                            hold.DropItem(new ValoriteOre(Utility.RandomMinMax(40, 50)));
                        else
                            hold.DropItem(new ValoriteIngot(Utility.RandomMinMax(40, 50)));
                        break;
                }

                switch (Utility.Random(5))
                {
                    case 0:
                        if (Utility.RandomBool())
                            hold.DropItem(new Board(Utility.RandomMinMax(40, 50)));
                        else
                            hold.DropItem(new Log(Utility.RandomMinMax(40, 50)));
                        break;
                    case 1:
                        if (Utility.RandomBool())
                            hold.DropItem(new OakBoard(Utility.RandomMinMax(40, 50)));
                        else
                            hold.DropItem(new OakLog(Utility.RandomMinMax(40, 50)));
                        break;
                    case 2:
                        if (Utility.RandomBool())
                            hold.DropItem(new AshBoard(Utility.RandomMinMax(40, 50)));
                        else
                            hold.DropItem(new AshLog(Utility.RandomMinMax(40, 50)));
                        break;
                    case 3:
                        if (Utility.RandomBool())
                            hold.DropItem(new YewBoard(Utility.RandomMinMax(40, 50)));
                        else
                            hold.DropItem(new YewLog(Utility.RandomMinMax(40, 50)));
                        break;
                    case 4:
                        if (Utility.RandomBool())
                            hold.DropItem(new BloodwoodBoard(Utility.RandomMinMax(40, 50)));
                        else
                            hold.DropItem(new BloodwoodLog(Utility.RandomMinMax(40, 50)));
                        break;
                }

                switch (Utility.Random(4))
                {
                    case 0:
                        if (Utility.RandomBool())
                            hold.DropItem(new Leather(Utility.RandomMinMax(40, 50)));
                        else
                            hold.DropItem(new Hides(Utility.RandomMinMax(40, 50)));
                        break;
                    case 1:
                        if (Utility.RandomBool())
                            hold.DropItem(new SpinedLeather(Utility.RandomMinMax(40, 50)));
                        else
                            hold.DropItem(new SpinedHides(Utility.RandomMinMax(40, 50)));
                        break;
                    case 2:
                        if (Utility.RandomBool())
                            hold.DropItem(new HornedLeather(Utility.RandomMinMax(40, 50)));
                        else
                            hold.DropItem(new HornedHides(Utility.RandomMinMax(40, 50)));
                        break;
                    case 3:
                        if (Utility.RandomBool())
                            hold.DropItem(new BarbedLeather(Utility.RandomMinMax(40, 50)));
                        else
                            hold.DropItem(new BarbedHides(Utility.RandomMinMax(40, 50)));
                        break;
                }

                switch (Utility.Random(4))
                {
                    case 0: hold.DropItem(new HeavyCannonball(Utility.RandomMinMax(5, 10))); break;
                    case 1: hold.DropItem(new LightCannonball(Utility.RandomMinMax(5, 10))); break;
                    case 2: hold.DropItem(new HeavyGrapeshot(Utility.RandomMinMax(5, 10))); break;
                    case 3: hold.DropItem(new LightGrapeshot(Utility.RandomMinMax(5, 10))); break;
                }


                //Rares
                if (0.8 > Utility.RandomDouble())
                {
                    Item deed;

                    if (Utility.RandomBool())
                    {
                        deed = new CarronadeDeed();
                    }
                    else
                    {
                        deed = new CulverinDeed();
                    }

                    hold.DropItem(deed);
                }

                if (0.025 > Utility.RandomDouble())
                {
                    if (Utility.RandomBool())
                        hold.DropItem(new WhiteClothDyeTub());
                    else
                        hold.DropItem(PermanentBoatPaint.DropRandom());
                }

                RefinementComponent.Roll(hold, 3, 0.25);

                if (RisingTideEvent.Instance.Running)
                {
                   hold.DropItem(new MaritimeCargo());
                   hold.DropItem(new MaritimeCargo());
                   
                    if (galleon is OrcishGalleon)
                    {
                        hold.DropItem(new MaritimeCargo());
                        
                        if (Utility.RandomBool())
                        { 
	                        hold.DropItem(new MaritimeCargo());
                        }
                    }
                }
            }
        }

        public bool IsObjective(Mobile from)
        {
            return m_Bounties.ContainsKey(from);
        }

        public BountyQuestSpawner(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(1);

            writer.Write(m_MaxFel);
            writer.Write(m_MaxTram);
            writer.Write(m_MaxTokuno);
            writer.Write(m_SpawnTime);
            writer.Write(m_Active);

            writer.Write(m_ActiveZones.Count);
            foreach (KeyValuePair<SpawnZone, List<BaseShipCaptain>> kvp in m_ActiveZones)
            {
                writer.Write((int)kvp.Key);
                writer.Write(kvp.Value.Count);
                foreach (BaseShipCaptain capt in kvp.Value)
                    writer.Write(capt);
            }

            writer.Write(m_Bounties.Count);
            foreach (KeyValuePair<Mobile, int> kvp in m_Bounties)
            {
                writer.Write(kvp.Key);
                writer.Write(kvp.Value);
            }
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_MaxFel = reader.ReadInt();
            m_MaxTram = reader.ReadInt();
            m_MaxTokuno = reader.ReadInt();
            m_SpawnTime = reader.ReadTimeSpan();
            m_Active = reader.ReadBool();

            int count = reader.ReadInt();
            for (int i = 0; i < count; i++)
            {
                SpawnZone zone = (SpawnZone)reader.ReadInt();
                int c = reader.ReadInt();
                for (int j = 0; j < c; j++)
                {
                    BaseShipCaptain capt = reader.ReadMobile() as BaseShipCaptain;
                    if (capt != null && !capt.Deleted && capt.Alive)
                    {
                        m_ActiveZones[zone].Add(capt);
                    }
                }
            }

            count = reader.ReadInt();
            for (int i = 0; i < count; i++)
            {
                Mobile mob = reader.ReadMobile();
                int amt = reader.ReadInt();

                if (mob != null && !mob.Deleted)
                    m_Bounties.Add(mob, amt);
            }

            if (version == 0)
            {
                m_MaxTram = 1;
                m_MaxFel = 1;
                m_MaxTokuno = 1;

                RemoveSpawns();
            }

            m_Instance = this;

            if (m_Active)
                m_Timer = Timer.DelayCall(m_SpawnTime, m_SpawnTime, OnTick);
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from.AccessLevel > AccessLevel.Player)
                from.SendGump(new PropertiesGump(from, this));
        }

        #region Command
        public static void GetRoutes_OnCommand(CommandEventArgs e)
        {
            MapItem mapitem;

            for (int i = 0; i < SpawnDefinition.PirateTramFelCoursesJhelom.Length; i++)
            {
                mapitem = new MapItem();
                mapitem.SetDisplay(5, 5, 5120 - 32, 4096 - 10, 400, 400);

                for (int j = 0; j < SpawnDefinition.PirateTramFelCoursesJhelom[i].Length; j++)
                {
                    Point2D pnt = SpawnDefinition.PirateTramFelCoursesJhelom[i][j];
                    mapitem.AddWorldPin(pnt.X, pnt.Y);
                }

                mapitem.Name = string.Format("Piraci - Nelderim {0}", i + 1);
                e.Mobile.AddToBackpack(mapitem);
            }

            for (int i = 0; i < SpawnDefinition.PirateTramFelCoursesMoonglow.Length; i++)
            {
                mapitem = new MapItem();
                mapitem.SetDisplay(5, 5, 5120 - 32, 4096 - 10, 400, 400);

                for (int j = 0; j < SpawnDefinition.PirateTramFelCoursesJhelom[i].Length; j++)
                {
                    Point2D pnt = SpawnDefinition.PirateTramFelCoursesJhelom[i][j];
                    mapitem.AddWorldPin(pnt.X, pnt.Y);
                }

                mapitem.Name = string.Format("Piraci - Nelderim {0}", i + 1);
                e.Mobile.AddToBackpack(mapitem);
            }

            for (int i = 0; i < SpawnDefinition.PirateTokunoCourses.Length; i++)
            {
                mapitem = new MapItem();
                mapitem.SetDisplay(5, 5, 1448 - 32, 1448 - 10, 400, 400);

                for (int j = 0; j < SpawnDefinition.PirateTokunoCourses[i].Length; j++)
                {
                    Point2D pnt = SpawnDefinition.PirateTokunoCourses[i][j];
                    mapitem.AddWorldPin(pnt.X, pnt.Y);
                }

                mapitem.Name = string.Format("Pirate - tokuno {0}", i + 1);
                e.Mobile.AddToBackpack(mapitem);
            }

            for (int i = 0; i < SpawnDefinition.MerchantTokunoCourses.Length; i++)
            {
                mapitem = new MapItem();
                mapitem.SetDisplay(5, 5, 1448 - 32, 1448 - 10, 400, 400);

                for (int j = 0; j < SpawnDefinition.PirateTokunoCourses[i].Length; j++)
                {
                    Point2D pnt = SpawnDefinition.PirateTokunoCourses[i][j];
                    mapitem.AddWorldPin(pnt.X, pnt.Y);
                }

                mapitem.Name = string.Format("Merchant - tokuno {0}", i + 1);
                e.Mobile.AddToBackpack(mapitem);
            }
            for (int i = 0; i < SpawnDefinition.MerchantTramFelCourses1.Length; i++)
            {
                mapitem = new MapItem();
                mapitem.SetDisplay(5, 5, 5120 - 32, 4096 - 10, 400, 400);

                for (int j = 0; j < SpawnDefinition.MerchantTramFelCourses1[i].Length; j++)
                {
                    Point2D pnt = SpawnDefinition.MerchantTramFelCourses1[i][j];
                    mapitem.AddWorldPin(pnt.X, pnt.Y);
                }

                mapitem.Name = string.Format("Handlarze - Nelderim(a) {0}", i + 1);
                e.Mobile.AddToBackpack(mapitem);
            }
            for (int i = 0; i < SpawnDefinition.MerchantTramFelCourses2.Length; i++)
            {
                mapitem = new MapItem();
                mapitem.SetDisplay(5, 5, 5120 - 32, 4096 - 10, 400, 400);

                for (int j = 0; j < SpawnDefinition.MerchantTramFelCourses2[i].Length; j++)
                {
                    Point2D pnt = SpawnDefinition.MerchantTramFelCourses2[i][j];
                    mapitem.AddWorldPin(pnt.X, pnt.Y);
                }

                mapitem.Name = string.Format("Handlarze - Nelderim(b) {0}", i + 1);
                e.Mobile.AddToBackpack(mapitem);
            }

        }
        #endregion
    }

    public enum SpawnZone
    {
        //Pirate
        TramJhelom,
        FelJhelom,
        TramMoonglow,
        FelMoonglow,
        TokunoPirate,

        //merchants
        TramMerch1,
        TramMerch2,
        FelMerch1,
        FelMerch2,
        TokunoMerch
    }

    public class SpawnDefinition
    {
        private Rectangle2D m_SpawnRegion;
        private readonly Point2D[][] m_Waypoints;
        private readonly SpawnZone m_Zone;
        private readonly Map m_Map;

        public Rectangle2D SpawnRegion => m_SpawnRegion;
        public Point2D[][] Waypoints => m_Waypoints;
        public SpawnZone Zone => m_Zone;
        public Map Map => m_Map;

        public SpawnDefinition(Rectangle2D spawnreg, Point2D[][] waypoints, SpawnZone type, Map map)
        {
            m_SpawnRegion = spawnreg;
            m_Waypoints = waypoints;
            m_Zone = type;
            m_Map = map;

            BountyQuestSpawner.AddZone(this);
        }

        public Point2D[] GetRandomWaypoints()
        {
            return m_Waypoints[Utility.Random(m_Waypoints.Length)];
        }

        //Defines the definitions.
        public static void Configure()
        {
            new SpawnDefinition(new Rectangle2D(1500, 3600, 180, 400), m_PirateTramFelCoursesJhelom, SpawnZone.FelJhelom, Map.Felucca);
            new SpawnDefinition(new Rectangle2D(4570, 630, 400, 100), m_PirateTramFelCoursesMoonglow, SpawnZone.FelMoonglow, Map.Felucca);

            new SpawnDefinition(new Rectangle2D(1780, 1650, 300, 200), m_MerchantTramFelCourses1, SpawnZone.FelMerch1, Map.Felucca);
            new SpawnDefinition(new Rectangle2D(4400, 2924, 100, 200), m_MerchantTramFelCourses2, SpawnZone.FelMerch2, Map.Felucca);
        }

        public static Point2D[][] PirateTramFelCoursesJhelom => m_PirateTramFelCoursesJhelom;
        private static readonly Point2D[][] m_PirateTramFelCoursesJhelom =
        {
            new Point2D[]{ new Point2D(1598, 3861), new Point2D(1520, 3470), new Point2D(1418, 3314), new Point2D(1159, 3277), new Point2D(1320, 3508), new Point2D(1527, 3584) },
            new Point2D[]{ new Point2D(2190, 3667), new Point2D(2023, 4016), new Point2D(1795, 3855), new Point2D(1613, 3887) },
            new Point2D[]{ new Point2D(2135, 4070), new Point2D(2802, 4070), new Point2D(2620, 3761), new Point2D(1725, 3794), },
            new Point2D[]{ new Point2D(2154, 3775), new Point2D(2378, 3652), new Point2D(2388, 3812), new Point2D(1696, 3797), },
            new Point2D[]{ new Point2D(1599, 3933), new Point2D(1299, 3953), new Point2D(971, 3799), new Point2D(813, 3326), new Point2D(1247, 3296), new Point2D(1655, 3890) },
            new Point2D[]{ new Point2D(1694, 3735), new Point2D(1607, 3402), new Point2D(1808, 3966), },
            new Point2D[]{ new Point2D(1068, 3616), new Point2D(1491, 3751), new Point2D(2150, 3727), new Point2D(1691, 3916), },
        };

        public static Point2D[][] PirateTramFelCoursesMoonglow => m_PirateTramFelCoursesMoonglow;
        private static readonly Point2D[][] m_PirateTramFelCoursesMoonglow =
        {
            new Point2D[]{ new Point2D(4415, 792), new Point2D(3509, 835), new Point2D(4073, 1809), new Point2D(4799, 1670), new Point2D(4861, 1061), new Point2D(4533, 589) },
            new Point2D[]{ new Point2D(4265, 145), new Point2D(5015, 153), new Point2D(5001, 669), new Point2D(4950, 720), new Point2D(4573, 663) },
            new Point2D[]{ new Point2D(5739, 3749), new Point2D(5408, 3688), new Point2D(5683, 3461), new Point2D(5793, 3733), new Point2D(5708, 3593) }, //Podmrok_C
            new Point2D[]{ new Point2D(5043, 155), new Point2D(4447, 231), new Point2D(4531, 609) },
        };

        public static Point2D[][] PirateTokunoCourses => m_PirateTokunoCourses;
        private static readonly Point2D[][] m_PirateTokunoCourses =
        {
            new Point2D[]{ new Point2D(1324, 1178), new Point2D(1358, 1334), new Point2D(1032, 1358), new Point2D(1070, 1240) },
            new Point2D[]{ new Point2D(1370, 1074), new Point2D(1422, 962), new Point2D(1416, 620), new Point2D(1422, 1310) },
            new Point2D[]{ new Point2D(1032, 1104), new Point2D(982, 1078), new Point2D(942, 914), new Point2D(942, 1086), new Point2D(982, 1078), new Point2D(1134, 1202) },
            new Point2D[]{ new Point2D(1320, 1378), new Point2D(1050, 1204), new Point2D(1356, 1088), new Point2D(1244, 1300) },
        };

        public static Point2D[][] MerchantTokunoCourses => m_MerchantTokunoCourses;
        private static readonly Point2D[][] m_MerchantTokunoCourses =
        {
            new Point2D[]{ new Point2D(460, 1408), new Point2D(878, 1408), new Point2D(500, 1408) },
            new Point2D[]{ new Point2D(460, 1408), new Point2D(460, 768), new Point2D(460, 1350) },
        };

        public static Point2D[][] MerchantTramFelCourses1 => m_MerchantTramFelCourses1;
        private static readonly Point2D[][] m_MerchantTramFelCourses1 =
        {
            new Point2D[]{ new Point2D(2290, 1479), new Point2D(2420, 1400), new Point2D(3936, 1866), new Point2D(3244, 2245), new Point2D(1843, 1947) },
            new Point2D[]{ new Point2D(2649, 1680), new Point2D(2296, 2910), new Point2D(3226, 2902), new Point2D(2884, 1765), new Point2D(2711, 1564) },
        };

        public static Point2D[][] MerchantTramFelCourses2 => m_MerchantTramFelCourses2;
        private static readonly Point2D[][] m_MerchantTramFelCourses2 =
        {
            new Point2D[]{ new Point2D(4129, 2367), new Point2D(4129, 1891), new Point2D(4773, 1891), new Point2D(4773, 2639), new Point2D(4129, 2639), new Point2D(4129, 2351) },
            new Point2D[]{ new Point2D(4013, 2382), new Point2D(4000, 3515), new Point2D(3141, 3515), new Point2D(3141, 3043), new Point2D(4093, 3043), new Point2D(4093, 2371) },
        };
    }
}
