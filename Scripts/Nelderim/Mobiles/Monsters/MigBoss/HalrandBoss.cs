using Server.Engines.CannedEvil;
using Server.Items;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Server.Mobiles
{
    public class HalrandBoss : BaseCreature
    {
	    private static readonly ArrayList m_Instances = new ArrayList();
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
        private readonly int m_MaxAbilityInterval = 15; //seconds
        private readonly int m_MinAbilityInterval = 10; //seconds
        private DateTime m_NextAbilityTime;

        private bool m_TrueForm;
        private bool m_IsSpawned;
       // private Item m_GateItem;
        private List<GreaterArcaneDaemon> m_ArcaneDaemon;
        public override bool TaintedLifeAura => true;
        public override bool TeleportsPets => true;

        Dictionary<Mobile, int> m_DamageEntries;
        [Constructable]
        public HalrandBoss()
            : base(AIType.AI_Paladin, FightMode.Weakest, 18, 1, 0.2, 0.4)
        {
            Name = "Duch Halranda Wulfrosta";
            BodyValue = 400;
            Hue = 2904; // zmienić na ducha wraz z ciuchami? 
            Female = false;

            SetStr(900, 1000);
            SetDex(125, 135);
            SetInt(1000, 1200);

            Kills = 2000;

            Fame = 22500;
            Karma = -22500;
            
            SpiritOfTheTotem Helm = new SpiritOfTheTotem();
            Helm.Hue = 1560;
            Helm.LootType = LootType.Blessed;
            Helm.Name = "wilcza maska";
            AddItem(Helm);
            PlateDo Chest = new PlateDo();
            Chest.Hue = 1560;
            Chest.LootType = LootType.Blessed;
            AddItem(Chest);
            JackalsCollar Gorget = new JackalsCollar();
            Gorget.Hue = 1560;
            Gorget.LootType = LootType.Blessed;
            AddItem(Gorget);
            PlateGloves Gloves = new PlateGloves();
            Gloves.Hue = 1560;
            Gloves.LootType = LootType.Blessed;
            AddItem(Gloves);
            StuddedArms Arms = new StuddedArms();
            Arms.Hue = 1560;
            Arms.LootType = LootType.Blessed;
            AddItem(Arms);
            PlateHaidate Legs = new PlateHaidate();
            Legs.Hue = 1560;
            Legs.LootType = LootType.Blessed;
            AddItem(Legs);


            HoodedShroudOfShadows Robe = new HoodedShroudOfShadows();
            Robe.Hue = 1187;
            Robe.LootType = LootType.Blessed;
            AddItem(Robe);

            ZyronicClaw Axe = new ZyronicClaw();
            Axe.LootType = LootType.Blessed;
            AddItem(Axe);

           


            Item hair = new Item(Utility.RandomList(0x203C));
            hair.Hue = Race.RandomHairHue();
            hair.Layer = Layer.Hair;
            hair.Movable = false;
            AddItem(hair);

            SetDamageType(ResistanceType.Cold, 50);
            SetDamageType(ResistanceType.Energy, 50);

            SetResistance(ResistanceType.Physical, 55, 65);
            SetResistance(ResistanceType.Fire, 70, 100);
            SetResistance(ResistanceType.Cold, 90, 100);
            SetResistance(ResistanceType.Poison, 70, 80);
            SetResistance(ResistanceType.Energy, 90, 100);

            SetSkill(SkillName.Wrestling, 90.1, 100.0);
            SetSkill(SkillName.Swords, 100.1, 120.0);
            SetSkill(SkillName.Tactics, 90.2, 110.0);
            SetSkill(SkillName.MagicResist, 120.2, 160.0);
            SetSkill(SkillName.Magery, 120.0);
            SetSkill(SkillName.EvalInt, 120.0);
            SetSkill(SkillName.Meditation, 120.0);
            SetSkill(SkillName.Necromancy, 120.0);
            SetSkill(SkillName.DetectHidden, 120.0);
            SetSkill(SkillName.Tracking, 120.0);
            SetSkill(SkillName.SpiritSpeak, 90.0);
            SetSkill(SkillName.Chivalry, 120.0);
            SetSkill(SkillName.Meditation, 120.0);
            

            m_ArcaneDaemon = new List<GreaterArcaneDaemon>();
        }

        public HalrandBoss(Serial serial)
            : base(serial)
        {
        }

        public static ArrayList Instances => m_Instances;

        public static bool CanSpawn => (m_Instances.Count == 0);
        public Type[] UniqueList => new[] { typeof(AcidProofRobe) }; //zmienić w przyszłości

        public Type[] SharedList => new[] { typeof(TheRobeOfBritanniaAri) }; //zmienić w przyszłości

        public Type[] DecorativeList => new[] { typeof(EvilIdolSkull), typeof(SkullPole) }; //zmienić w przyszłości
        public override bool AutoDispel => true;

        public override bool Unprovokable => true;

        public override Poison PoisonImmune => Poison.Lethal;

        [CommandProperty(AccessLevel.GameMaster)]
        public override int HitsMax => m_TrueForm ? 65000 : 30000;

        [CommandProperty(AccessLevel.GameMaster)]
        public override int ManaMax => 5000;

        public override bool DisallowAllMoves => m_TrueForm;

        public override bool TeleportsTo => true;

    /*  public static Harrower Spawn(Point3D platLoc, Map platMap)
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

         //  harrower.m_GateItem = new HarrowerGate(harrower, platLoc, platMap, entry.m_Entrance, Map.Felucca);

           return harrower;
       }*/

        public override void GenerateLoot()
        {
            AddLoot(LootPack.SuperBoss, 2);
            AddLoot(LootPack.Meager);
        }

        public void Morph()
        {
            if (m_TrueForm)
                return;

            m_TrueForm = true;

            Name = "Prawdziwa forma Halranda Wulfrosta";
            BodyValue = 400; 
            Female = false;
            Hue = 0x497;

            Hits = HitsMax;
            Stam = StamMax;
            Mana = ManaMax;
            
            SpiritOfTheTotem Helm = new SpiritOfTheTotem();
            Helm.Hue = 1560;
            Helm.LootType = LootType.Blessed;
            Helm.Name = "wilcza maska";
            AddItem(Helm);
            PlateDo Chest = new PlateDo();
            Chest.Hue = 1560;
            Chest.LootType = LootType.Blessed;
            AddItem(Chest);
            JackalsCollar Gorget = new JackalsCollar();
            Gorget.Hue = 1560;
            Gorget.LootType = LootType.Blessed;
            AddItem(Gorget);
            PlateGloves Gloves = new PlateGloves();
            Gloves.Hue = 1560;
            Gloves.LootType = LootType.Blessed;
            AddItem(Gloves);
            StuddedArms Arms = new StuddedArms();
            Arms.Hue = 1560;
            Arms.LootType = LootType.Blessed;
            AddItem(Arms);
            PlateHaidate Legs = new PlateHaidate();
            Legs.Hue = 1560;
            Legs.LootType = LootType.Blessed;
            AddItem(Legs);


            HoodedShroudOfShadows Robe = new HoodedShroudOfShadows();
            Robe.Hue = 1185;
            Robe.LootType = LootType.Blessed;
            AddItem(Robe);

            ZyronicClaw Axe = new ZyronicClaw();
            Axe.LootType = LootType.Blessed;
            AddItem(Axe);

           


            Item hair = new Item(Utility.RandomList(0x203C));
            hair.Hue = Race.RandomHairHue();
            hair.Layer = Layer.Hair;
            hair.Movable = false;
            AddItem(hair);

            ProcessDelta();

            Say("JAK ŚMIECIE WY ŚMIECIE!? JA ZBUDOWAŁEM TEN ŚWIAT!!"); 

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

                    GreaterArcaneDaemon spawn = new GreaterArcaneDaemon()
                    {
                        Team = Team
                    };

                    spawn.MoveToWorld(new Point3D(x, y, z), map);

                    m_ArcaneDaemon.Add(spawn);
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
         //   writer.Write(m_GateItem);
           // writer.WriteMobileList(m_ArcaneDaemon);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_IsSpawned = reader.ReadBool();
            m_TrueForm = reader.ReadBool();
          //  m_GateItem = reader.ReadItem();
           m_ArcaneDaemon = reader.ReadStrongMobileList<GreaterArcaneDaemon>();

            if (m_IsSpawned)
                m_Instances.Add(this);
        }
/*
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

            for (int i = 0; i < ChampionSystem.StatScrollAmount; ++i)
            {
                Mobile m = toGive[i % toGive.Count];

                m.SendLocalizedMessage(1049524); // You have received a scroll of power!
                m.AddToBackpack(new StatCapScroll(m_StatCap + RandomStatScrollLevel()));

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
                            //prot.AddToBackpack(new StatCapScroll(m_StatCap + RandomStatScrollLevel()));*
                        }
                    }
                }
            }
        }*/
/*
        private static int RandomStatScrollLevel()
        {
            double random = Utility.RandomDouble();

            if (0.1 >= random)
                return 25;
            else if (0.25 >= random)
                return 20;
            else if (0.45 >= random)
                return 15;
            else if (0.70 >= random)
                return 10;
            return 5;
        }*/

	    public override void OnThink()
	    {
		    if (DateTime.Now >= m_NextAbilityTime)
		    {
			    ThrowSnowball();
			    m_NextAbilityTime = DateTime.Now +
			                        TimeSpan.FromSeconds(Utility.RandomMinMax(m_MinAbilityInterval,
				                        m_MaxAbilityInterval));
		    }

		    base.OnThink();
	    }

	    private void ThrowSnowball()
	    {
		    var targets = new HashSet<Mobile>();
		    foreach (var aggressorInfo in Aggressors) targets.Add(aggressorInfo.Attacker);
		    foreach (var aggressorInfo in Aggressed) targets.Add(aggressorInfo.Defender);

		    if (targets.Count > 0)
		    {
			    Say("Lap To Gowno!"); 
			    foreach (var target in targets)
				    if (Utility.RandomDouble() > 0.8)
				    {
					    PlaySound(0x145);
					    Animate(9, 1, 1, true, false, 0);

					    
					    Effects.SendMovingEffect(this, target, 0x913, 7, 0, false, true, 1161, 0);
					    AOS.Damage(target, Utility.RandomMinMax(10, 18), 0, 0, 0, 100, 0);
				    }
		    }
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
                   // GivePowerScrolls();

                    GoldShower.DoForHarrower(Location, Map);

                    m_DamageEntries = new Dictionary<Mobile, int>();

                    for (int i = 0; i < m_ArcaneDaemon.Count; ++i)
                    {
                        Mobile m = m_ArcaneDaemon[i];

                        if (!m.Deleted)
                            m.Kill();

                        RegisterDamageTo(m);
                    }

                    m_ArcaneDaemon.Clear();

                    RegisterDamageTo(this);
                    AwardArtifact(GetArtifact());

                   // if (m_GateItem != null)
                    //    m_GateItem.Delete();
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

        public void AwardArtifact(Item artifact)
        {
            if (artifact == null)
                return;

            int totalDamage = 0;

            Dictionary<Mobile, int> validEntries = new Dictionary<Mobile, int>();

            foreach (KeyValuePair<Mobile, int> kvp in m_DamageEntries)
            {
                if (IsEligible(kvp.Key, artifact))
                {
                    validEntries.Add(kvp.Key, kvp.Value);
                    totalDamage += kvp.Value;
                }
            }

            int randomDamage = Utility.RandomMinMax(1, totalDamage);

            totalDamage = 0;

            foreach (KeyValuePair<Mobile, int> kvp in validEntries)
            {
                totalDamage += kvp.Value;

                if (totalDamage >= randomDamage)
                {
                    GiveArtifact(kvp.Key, artifact);
                    return;
                }
            }

            artifact.Delete();
        }

        public void GiveArtifact(Mobile to, Item artifact)
        {
            if (to == null || artifact == null)
                return;

            to.PlaySound(0x5B4);

            Container pack = to.Backpack;

            if (pack == null || !pack.TryDropItem(to, artifact, false))
                artifact.Delete();
            else
                to.SendLocalizedMessage(1062317); // For your valor in combating the fallen beast, a special artifact has been bestowed on you.
        }

        public bool IsEligible(Mobile m, Item Artifact)
        {
            return m.Player && m.Alive && m.InRange(Location, 32) && m.Backpack != null && m.Backpack.CheckHold(m, Artifact, false);
        }

        public Item GetArtifact()
        {
            double random = Utility.RandomDouble();
            if (0.05 >= random)
                return CreateArtifact(UniqueList); //place holder
            else if (0.15 >= random)
                return CreateArtifact(DecorativeList);			//place holder
            else if (0.30 >= random)
                return CreateArtifact(SharedList);		//place holder
            return null;
        }

        public Item CreateArtifact(Type[] list)
        {
            if (list.Length == 0)
                return null;

            int random = Utility.Random(list.Length);

            Type type = list[random];

            return Loot.Construct(type);
        }
        private class SpawnEntry
        {
            public readonly Point3D m_Location;
          //  public readonly Point3D m_Entrance;
            public SpawnEntry(Point3D loc, Point3D ent)
            {
                m_Location = loc;
           //     m_Entrance = ent;
            }
        }
    }
}
