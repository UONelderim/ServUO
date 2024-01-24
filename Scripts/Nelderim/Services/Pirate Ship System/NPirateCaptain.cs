using System;
using System.Collections;
using System.Collections.Generic;
using Server.Items;
using Server.Multis;

namespace Server.Mobiles
{
    [CorpseName("zwloki kapitana piratow")]
    public class NPirateCaptain : BaseCreature
    {
        private NPirateShip m_PirateShip;
        private DateTime nextAbilityTime;

        public override bool InitialInnocent => true;


        [Constructable]
        public NPirateCaptain() : base(AIType.AI_Archer, FightMode.Closest, 15, 1, 0.2, 0.4)
        {
            Hue = Race.Human.RandomSkinHue();

            if (Female == Utility.RandomBool())
            {
                Body = 0x191;
            }
            else
            {
                Body = 0x190;
            }

            Title = "- kapitan piratow";

            SpeechHue = Utility.RandomDyedHue();

            AddItem(new ThighBoots());
            AddItem(new TricorneHat(Utility.RandomRedHue()));

            SetStr(105, 155);
            SetDex(181, 195);
            SetInt(61, 75);
            SetHits(80, 100);

            SetDamage(15, 20);

            SetDamageType(ResistanceType.Physical, 100, 100);
            SetDamageType(ResistanceType.Fire, 25, 50);
            SetDamageType(ResistanceType.Cold, 25, 50);
            SetDamageType(ResistanceType.Energy, 25, 50);
            SetDamageType(ResistanceType.Poison, 25, 50);

            SetResistance(ResistanceType.Physical, 100, 100);
            SetResistance(ResistanceType.Fire, 25, 50);
            SetResistance(ResistanceType.Cold, 25, 50);
            SetResistance(ResistanceType.Energy, 25, 50);
            SetResistance(ResistanceType.Poison, 25, 50);

            SetSkill(SkillName.Fencing, 86.0, 97.5);
            SetSkill(SkillName.Macing, 85.0, 87.5);
            SetSkill(SkillName.MagicResist, 55.0, 67.5);
            SetSkill(SkillName.Swords, 85.0, 87.5);
            SetSkill(SkillName.Tactics, 85.0, 87.5);
            SetSkill(SkillName.Wrestling, 35.0, 37.5);
            SetSkill(SkillName.Archery, 85.0, 87.5);

            CanSwim = true;
            CantWalk = true;

            Fame = 5000;
            Karma = -5000;
            VirtualArmor = 66;

            switch (Utility.Random(1))
            {
                case 0:
                    AddItem(new LongPants(Utility.RandomRedHue()));
                    break;
                case 1:
                    AddItem(new ShortPants(Utility.RandomRedHue()));
                    break;
            }

            switch (Utility.Random(3))
            {
                case 0:
                    AddItem(new FancyShirt(Utility.RandomRedHue()));
                    break;
                case 1:
                    AddItem(new Shirt(Utility.RandomRedHue()));
                    break;
                case 2:
                    AddItem(new Doublet(Utility.RandomRedHue()));
                    break;
            }

            switch (Utility.Random(5))
            {
                case 0:
                    AddItem(new Bow());
                    break;
                case 1:
                    AddItem(new CompositeBow());
                    break;
                case 2:
                    AddItem(new Crossbow());
                    break;
                case 3:
                    AddItem(new RepeatingCrossbow());
                    break;
                case 4:
                    AddItem(new HeavyCrossbow());
                    break;
            }
        }

        protected override void OnCreate()
        {
	        base.OnCreate();
	        
            RaceGenerator.Init(this);
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
        }

        public override bool IsScaredOfScaryThings => false;

        public override bool AlwaysMurderer => true;

        public override Poison PoisonImmune => Poison.Regular;

        public override bool AutoDispel => true;

        public override bool CanRummageCorpses => true;

        public override bool AllureImmune => true;


        public void Emote()
        {
            switch (Utility.Random(85))
            {
                case 1:
                    PlaySound(Female ? 785 : 1056);
                    Say("*kaszle!*");
                    break;
                case 2:
                    PlaySound(Female ? 818 : 1092);
                    Say("*pociaga nosem*");
                    break;
            }
        }

        public override bool PlayerRangeSensitive => false;

        private bool boatspawn;
        private DateTime m_NextPickup;
        private Mobile m_Mobile;
        private BaseBoat m_enemyboat;
        private ArrayList list;
        private Direction enemydirection;

        private static readonly String[] _Texts =
        {
	        "Szykuj sie na spotkanie z piratem brachu!", "Madrze to atakowac pirata, glupcze?",
	        "Glebia morza cie pochlonie!", "Czas umierac zawszony kundlu!", "Szykuj sie na zemste mych kamratow!"
        };

        public override void OnThink()
        {
            if (DateTime.Now >= nextAbilityTime && Combatant == null)
            {
                nextAbilityTime = DateTime.Now + TimeSpan.FromSeconds(Utility.RandomMinMax(4, 6));
                Say(Utility.RandomList(_Texts));
                Emote();
            }


            if (boatspawn == false)
            {
                Map map = Map;
                if (map == null)
                    return;
                Z = -5;
                m_PirateShip = new NPirateShip();

                Point3D loc = new Point3D(X, Y - 1, Z);
                Point3D loccrew = new Point3D(X, Y - 1, Z + 1);

                m_PirateShip.MoveToWorld(loc, map);
                boatspawn = true;

                for (int i = 0; i < 5; ++i)
                {
                    NPirateCrew m_PirateCrew = new NPirateCrew();
                    m_PirateCrew.MoveToWorld(loccrew, map);
                }
            }

            base.OnThink();
            if (DateTime.Now < m_NextPickup)
                return;

            if (m_PirateShip == null)
            {
                return;
            }

            m_NextPickup = DateTime.Now + TimeSpan.FromSeconds(Utility.RandomMinMax(1, 2));

            enemydirection = Direction.North;
            var eable = GetItemsInRange(100);
            foreach (Item enemy in eable)
            {
                if (enemy is BaseBoat && enemy != m_PirateShip && !(enemy is NPirateShip))
                {
                    List<Mobile> targets = new List<Mobile>();
                    IPooledEnumerable eable2 = enemy.GetMobilesInRange(16);

                    foreach (Mobile m in eable2)
                    {
                        if (m is PlayerMobile)
                            targets.Add(m);
                    }

                    eable2.Free();
                    if (targets.Count > 0)
                    {
                        m_enemyboat = enemy as BaseBoat;
                        enemydirection = GetDirectionTo(m_enemyboat);
                        break;
                    }
                }
            }
            eable.Free();

            if (m_enemyboat == null)
            {
                return;
            }

            if (m_PirateShip != null && m_enemyboat != null)
            {
                if (m_PirateShip != null && (enemydirection == Direction.North) &&
                    m_PirateShip.Facing != Direction.North)
                {
                    m_PirateShip.Facing = Direction.North;
                }
                else if (m_PirateShip != null && (enemydirection == Direction.South) &&
                         m_PirateShip.Facing != Direction.South)
                {
                    m_PirateShip.Facing = Direction.South;
                }
                else if (m_PirateShip != null &&
                         (enemydirection == Direction.East || enemydirection == Direction.Right ||
                          enemydirection == Direction.Down) && m_PirateShip.Facing != Direction.East)
                {
                    m_PirateShip.Facing = Direction.East;
                }
                else if (m_PirateShip != null &&
                         (enemydirection == Direction.West || enemydirection == Direction.Left ||
                          enemydirection == Direction.Up) && m_PirateShip.Facing != Direction.West)
                {
                    m_PirateShip.Facing = Direction.West;
                }

                m_PirateShip.StartMove(Direction.North, true);

                if (m_PirateShip != null && InRange(m_enemyboat, 10) && m_PirateShip.IsMoving)
                {
                    m_PirateShip.StopMove(false);
                }
            }
            else
            {
                if (m_PirateShip != null && m_PirateShip.IsMoving)
                {
                    m_PirateShip.StopMove(false);
                }
            }
        }

        public override void OnDelete()
        {
            if (m_PirateShip != null)
            {
                new SinkTimer(m_PirateShip).Start();
            }
            base.OnDelete();
        }

        public override void OnDamagedBySpell(Mobile caster)
        {
            if (caster == this)
                return;

            SpawnPirate(caster);
        }

        public void SpawnPirate(Mobile target)
        {
            Map map = target.Map;

            if (map == null)
                return;

            int pirates = 0;

            var eable = GetMobilesInRange(10);
            foreach (Mobile m in eable)
            {
                if (m is NPirateCrew)
                    ++pirates;
            }
            eable.Free();

            if (pirates < 10 && Utility.RandomDouble() <= 0.25)
            {
                BaseCreature PirateCrew = new NPirateCrew();

                Point3D loc = target.Location;
                bool validLocation = false;

                for (int j = 0; !validLocation && j < 10; ++j)
                {
                    int x = target.X + Utility.Random(3) - 1;
                    int y = target.Y + Utility.Random(3) - 1;
                    int z = map.GetAverageZ(x, y);

                    if (validLocation = map.CanFit(x, y, Z, 16, false, false))
                        loc = new Point3D(x, y, Z);
                    else if (validLocation = map.CanFit(x, y, z, 16, false, false))
                        loc = new Point3D(x, y, z);
                }

                PirateCrew.MoveToWorld(loc, map);

                PirateCrew.Combatant = target;
            }
        }

        public override bool OnBeforeDeath()
        {
            if (m_PirateShip != null)
            {
                new SinkTimer(m_PirateShip).Start();
            }

            return true;
        }

        private class SinkTimer : Timer
        {
            private BaseBoat m_Boat;
            private int m_Count;

            public SinkTimer(BaseBoat boat) : base(TimeSpan.FromSeconds(1.0), TimeSpan.FromSeconds(4.0))
            {
                m_Boat = boat;

                Priority = TimerPriority.TwoFiftyMS;
            }

            protected override void OnTick()
            {
                if (m_Count == 4)
                {
                    List<Mobile> targets = new List<Mobile>();
                    IPooledEnumerable eable = m_Boat.GetMobilesInRange(16);

                    foreach (Mobile m in eable)
                    {
                        if (m is NPirateCrew)
                            targets.Add(m);
                    }

                    eable.Free();
                    if (targets.Count > 0)
                    {
                        for (int i = 0; i < targets.Count; ++i)
                        {
                            Mobile m = targets[i];
                            m.Kill();
                        }
                    }
                }

                if (m_Count >= 15)
                {
                    m_Boat.Delete();
                    Stop();
                }
                else
                {
                    if (m_Count < 5)
                    {
                        m_Boat.Location = new Point3D(m_Boat.X, m_Boat.Y, m_Boat.Z - 1);

                        if (m_Boat.TillerMan is Mobile tillerMan && m_Count < 5)
                            tillerMan.Say(1007168 + m_Count);
                    }
                    else
                    {
                        m_Boat.Location = new Point3D(m_Boat.X, m_Boat.Y, m_Boat.Z - 3);
                    }

                    ++m_Count;
                }
            }
        }

        public NPirateCaptain(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((Item)m_PirateShip);
            writer.Write((bool)boatspawn);
            writer.Write((int)1);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            m_PirateShip = reader.ReadItem() as NPirateShip;
            boatspawn = reader.ReadBool();
            int version = reader.ReadInt();

            if(version == 0)
				reader.ReadBool();
        }
    }
}
