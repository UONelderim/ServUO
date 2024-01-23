using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("zwloki anchimayen")]
    public class Anchimayen : BaseCreature
    {
        [Constructable]
        public Anchimayen() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.175, 0.350)
        {
            Name = "anchimayen";
            Body = 748;
            BaseSoundID = 0x482;
            Hue = 1281;

            SetStr(78, 98);
            SetDex(80, 94);
            SetInt(38, 60);

            SetHits(92, 120);
            SetMana(0);

            SetDamage(1, 4);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 28);
            SetResistance(ResistanceType.Fire, 100);
            SetResistance(ResistanceType.Cold, -50);
            SetResistance(ResistanceType.Poison, 5);
            SetResistance(ResistanceType.Energy, -25);

            SetSkill(SkillName.MagicResist, 46.6, 58.7);
            SetSkill(SkillName.Tactics, 45.4, 59.7);
            SetSkill(SkillName.Wrestling, 49.4, 58.1);

            Fame = 1500;
            Karma = -1500;

            PackGold(80, 125);

            if (Utility.RandomDouble() < 0.10)
                PackItem(new MagicUnTrapScroll());

            PackItem(Loot.RandomWeapon());
            PackItem(new Bone());

            if (0.03 > Utility.RandomDouble())
                PackItem(Loot.RandomGem());
        }

        public override bool BleedImmune
        {
            get { return true; }
        }

        public override Poison PoisonImmune
        {
            get { return Poison.Regular; }
        }

        private DateTime m_NextRadiation;

        protected override bool OnMove(Direction d)
        {
            ApplyRadiation();
            return base.OnMove(d);
        }

        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            ApplyRadiation();
            base.OnMovement(m, oldLocation);
        }
        
        private void ApplyRadiation()
        {
            if (IsDeadBondedPet) return;
            if (m_NextRadiation > DateTime.Now) return;
            
            IPooledEnumerable eable = GetMobilesInRange(2);
            foreach (Mobile m in eable)
            {
                new RadiationTimer(this, m).Start();
            }
            eable.Free();
            m_NextRadiation = DateTime.Now.AddSeconds(Utility.Random(10));
        }

        public Anchimayen(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
        
        private class RadiationTimer : Timer
        {
            private readonly Anchimayen m_From;
            private readonly Mobile m_Mobile;

            public RadiationTimer(Anchimayen from, Mobile mobile)
                : base(TimeSpan.Zero, TimeSpan.FromSeconds(1))
            {
                m_From = from;
                m_Mobile = mobile;
            }

            protected override void OnTick()
            {
                if (m_From.Deleted || !m_From.Alive || m_From == m_Mobile ||
                    m_Mobile.AccessLevel != AccessLevel.Player ||
                    !Utility.InRange(m_From.Location, m_Mobile.Location, 2) ||
                    !Spells.SpellHelper.ValidIndirectTarget(m_Mobile, m_From) || 
                    !m_From.CanBeHarmful(m_Mobile, false, false))
                {
                    Stop();
                    return;
                }
                
                AOS.Damage(m_Mobile, m_From, Utility.Random(5, 8), 0, 100, 0, 0, 0, true);
                m_Mobile.RevealingAction();
                m_From.DoHarmful(m_Mobile);
                m_Mobile.PlaySound(0x208);
                m_Mobile.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.LeftFoot);
            }
        }
    }
}