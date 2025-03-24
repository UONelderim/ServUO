using Server.Engines.CannedEvil;
using Server.Items;
using Server.Services.Virtues;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Server.Mobiles
{
    public class Harrower : BaseCreature
    {
        private static readonly int m_StatCap = Config.Get("PlayerCaps.TotalStatCap", 225);

        private static readonly SpawnEntry[] m_Entries =
        {
	        new(new Point3D(5526, 589, 5), new Point3D(5525, 681, 5))
        };
        private static readonly List<Harrower> m_Instances = [];
        private static readonly double[] m_Offsets =
        {
            Math.Cos(000.0 / 180.0 * Math.PI), Math.Sin(000.0 / 180.0 * Math.PI),
            Math.Cos(040.0 / 180.0 * Math.PI), Math.Sin(040.0 / 180.0 * Math.PI),
            Math.Cos(080.0 / 180.0 * Math.PI), Math.Sin(080.0 / 180.0 * Math.PI),
            Math.Cos(120.0 / 180.0 * Math.PI), Math.Sin(120.0 / 180.0 * Math.PI),
            Math.Cos(160.0 / 180.0 * Math.PI), Math.Sin(160.0 / 180.0 * Math.PI),
            Math.Cos(200.0 / 180.0 * Math.PI), Math.Sin(200.0 / 180.0 * Math.PI),
            Math.Cos(240.0 / 180.0 * Math.PI), Math.Sin(240.0 / 180.0 * Math.PI),
            Math.Cos(280.0 / 180.0 * Math.PI), Math.Sin(280.0 / 180.0 * Math.PI),
            Math.Cos(320.0 / 180.0 * Math.PI), Math.Sin(320.0 / 180.0 * Math.PI),
        };

        private const int PowerScrollAmount = 6;

        private bool m_TrueForm;
        private bool m_IsSpawned;
        private Item m_GateItem;
        private List<HarrowerTentacles> m_Tentacles;

        Dictionary<Mobile, int> m_DamageEntries;
        [Constructable]
        public Harrower()
            : base(AIType.AI_NecroMage, FightMode.Closest, 18, 1, 0.2, 0.4)
        {
            Name = "Przedwieczny";
            BodyValue = 146;

            SetStr(900, 1000);
            SetDex(125, 135);
            SetInt(1000, 1200);

            Fame = 22500;
            Karma = -22500;

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Energy, 50);

            SetResistance(ResistanceType.Physical, 55, 65);
            SetResistance(ResistanceType.Fire, 60, 80);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 60, 80);
            SetResistance(ResistanceType.Energy, 60, 80);

            SetSkill(SkillName.Wrestling, 90.1, 100.0);
            SetSkill(SkillName.Tactics, 90.2, 110.0);
            SetSkill(SkillName.MagicResist, 120.2, 160.0);
            SetSkill(SkillName.Magery, 120.0);
            SetSkill(SkillName.EvalInt, 120.0);
            SetSkill(SkillName.Meditation, 120.0);

            m_Tentacles = new List<HarrowerTentacles>();
        }

        public Harrower(Serial serial)
            : base(serial)
        {
        }

        public static bool CanSpawn => (m_Instances.Count == 0);

        public Type[] UniqueList => new[] { typeof(AcidProofRobe) };

        public Type[] SharedList => new[] { typeof(TheRobeOfBritanniaAri) };

        public Type[] DecorativeList => new[] { typeof(EvilIdolSkull), typeof(SkullPole) };

        public override bool AutoDispel => true;

        public override bool Unprovokable => true;

        public override Poison PoisonImmune => Poison.Lethal;

        [CommandProperty(AccessLevel.GameMaster)]
        public override int HitsMax => m_TrueForm ? 65000 : 30000;

        [CommandProperty(AccessLevel.GameMaster)]
        public override int ManaMax => 5000;

        public override bool DisallowAllMoves => m_TrueForm;

        public override bool TeleportsTo => true;

        public static Harrower Spawn(Point3D platLoc, Map platMap)
        {
            if (m_Instances.Count > 0)
                return null;

            SpawnEntry entry = m_Entries[Utility.Random(m_Entries.Length)];

            Harrower harrower = new Harrower
            {
                m_IsSpawned = true
            };

            m_Instances.Add(harrower);

            harrower.MoveToWorld(entry.m_Location, Map.Felucca);

            harrower.m_GateItem = new HarrowerGate(harrower, platLoc, platMap, entry.m_Entrance, Map.Felucca);

            return harrower;
        }

        public override void GenerateLoot()
        {
	        AddLoot(LootPack.LootItem<ParoxysmusSwampDragonStatuette>(15));
        }

        public void Morph()
        {
            if (m_TrueForm)
                return;

            m_TrueForm = true;

            Name = "Prawdziwa forma Przedwiecznego";
            BodyValue = 780;
            Hue = 0x497;

            Hits = HitsMax;
            Stam = StamMax;
            Mana = ManaMax;

            ProcessDelta();

            Say(1049499); // Behold my true form!

            Map map = Map;

            if (map != null)
            {
                for (int i = 0; i < m_Offsets.Length; i += 2)
                {
                    double rx = m_Offsets[i];
                    double ry = m_Offsets[i + 1];

                    int dist = 0;
                    bool ok = false;
                    int x = 0, y = 0, z = 0;

                    while (!ok && dist < 10)
                    {
                        int rdist = 10 + dist;

                        x = X + (int)(rx * rdist);
                        y = Y + (int)(ry * rdist);
                        z = map.GetAverageZ(x, y);

                        if (!(ok = map.CanFit(x, y, Z, 16, false, false)))
                            ok = map.CanFit(x, y, z, 16, false, false);

                        if (dist >= 0)
                            dist = -(dist + 1);
                        else
                            dist = -(dist - 1);
                    }

                    if (!ok)
                        continue;

                    HarrowerTentacles spawn = new HarrowerTentacles(this)
                    {
                        Team = Team
                    };

                    spawn.MoveToWorld(new Point3D(x, y, z), map);

                    m_Tentacles.Add(spawn);
                }
            }
        }

        public override void OnAfterDelete()
        {
            m_Instances.Remove(this);

            base.OnAfterDelete();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(1); // version

            writer.Write(m_IsSpawned);
            writer.Write(m_TrueForm);
            writer.Write(m_GateItem);
            writer.WriteMobileList(m_Tentacles);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_IsSpawned = reader.ReadBool();
            m_TrueForm = reader.ReadBool();
            m_GateItem = reader.ReadItem();
            m_Tentacles = reader.ReadStrongMobileList<HarrowerTentacles>();

            if (m_IsSpawned)
                m_Instances.Add(this);
        }

        public void GivePowerScrolls()
        {
            List<Mobile> toGive = new List<Mobile>();
            List<DamageStore> rights = GetLootingRights();

            for (int i = rights.Count - 1; i >= 0; --i)
            {
                DamageStore ds = rights[i];

                if (ds.m_HasRight)
                    toGive.Add(ds.m_Mobile);
            }

            if (toGive.Count == 0)
                return;

            // Randomize
            for (int i = 0; i < toGive.Count; ++i)
            {
                int rand = Utility.Random(toGive.Count);
                Mobile hold = toGive[i];
                toGive[i] = toGive[rand];
                toGive[rand] = hold;
            }

            for (int i = 0; i < PowerScrollAmount; ++i)
            {
                Mobile m = toGive[i % toGive.Count];

                m.SendLocalizedMessage(1049524); // You have received a scroll of power!
                m.AddToBackpack(RandomPowerScroll(i == 0));

                if (m is PlayerMobile)
                {
                    PlayerMobile pm = (PlayerMobile)m;

                    for (int j = 0; j < pm.JusticeProtectors.Count; ++j)
                    {
                        Mobile prot = pm.JusticeProtectors[j];

                        if (prot.Map != m.Map || prot.Murderer || prot.Criminal || !JusticeVirtue.CheckMapRegion(m, prot))
                            continue;

                        int chance = 0;

                        switch (VirtueHelper.GetLevel(prot, VirtueName.Justice))
                        {
                            case VirtueLevel.Seeker:
                                chance = 60;
                                break;
                            case VirtueLevel.Follower:
                                chance = 80;
                                break;
                            case VirtueLevel.Knight:
                                chance = 100;
                                break;
                        }

                        if (chance > Utility.Random(100))
                        {
                            prot.SendLocalizedMessage(1049368); // You have been rewarded for your dedication to Justice!
                            prot.AddToBackpack(RandomPowerScroll());
                        }
                    }
                }
            }
        }

        //First is always 120
        private static PowerScroll RandomPowerScroll(bool first = false)
        {
	        int level;
	        double random = Utility.RandomDouble();

	        if (0.05 >= random || first)
		        level = 20;
	        else if (0.3 >= random)
		        level = 15;
	        else
		        level = 10;

	        return PowerScroll.CreateRandomNoCraft(level, level);
        }

        public override bool OnBeforeDeath()
        {
            if (m_TrueForm)
            {
                List<DamageStore> rights = GetLootingRights();

                for (int i = rights.Count - 1; i >= 0; --i)
                {
                    DamageStore ds = rights[i];

                    if (ds.m_HasRight && ds.m_Mobile is PlayerMobile)
                        PlayerMobile.ChampionTitleInfo.AwardHarrowerTitle((PlayerMobile)ds.m_Mobile);
                }

                if (!NoKillAwards)
                {
                    GivePowerScrolls();

                    GoldShower.DoForHarrower(Location, Map);

                    m_DamageEntries = new Dictionary<Mobile, int>();

                    for (int i = 0; i < m_Tentacles.Count; ++i)
                    {
                        Mobile m = m_Tentacles[i];

                        if (!m.Deleted)
                            m.Kill();

                        RegisterDamageTo(m);
                    }

                    m_Tentacles.Clear();

                    RegisterDamageTo(this);
                    
                    //Two extra chances for artifact
                    if (Utility.RandomDouble() < 0.03) 
	                    ArtifactHelper.DistributeArtifacts(this);
                    if (Utility.RandomDouble() < 0.01) 
	                    ArtifactHelper.DistributeArtifacts(this);

                    if (m_GateItem != null)
                        m_GateItem.Delete();
                }

                return base.OnBeforeDeath();
            }
            else
            {
                Morph();
                return false;
            }
        }

        public virtual void RegisterDamageTo(Mobile m)
        {
            if (m == null)
                return;

            foreach (DamageEntry de in m.DamageEntries)
            {
                Mobile damager = de.Damager;

                Mobile master = damager.GetDamageMaster(m);

                if (master != null)
                    damager = master;

                RegisterDamage(damager, de.DamageGiven);
            }
        }

        public void RegisterDamage(Mobile from, int amount)
        {
            if (from == null || !from.Player)
                return;

            if (m_DamageEntries.ContainsKey(from))
                m_DamageEntries[from] += amount;
            else
                m_DamageEntries.Add(from, amount);

            from.SendMessage(string.Format("Total Damage: {0}", m_DamageEntries[from]));
        }
        
        private class SpawnEntry
        {
            public readonly Point3D m_Location;
            public readonly Point3D m_Entrance;
            public SpawnEntry(Point3D loc, Point3D ent)
            {
                m_Location = loc;
                m_Entrance = ent;
            }
        }
    }
}
