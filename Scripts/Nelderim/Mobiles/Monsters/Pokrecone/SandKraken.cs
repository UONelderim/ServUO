using System;
using System.Collections;
using System.Collections.Generic;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("zwloki piaskowego krakena")]
    public class SandKraken : BaseCreature
    {
        [Constructable]
        public SandKraken() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.175, 0.350)
        {
            Name = "piaskowy kraken";
            Body = 77;
            Hue = 0x648;

            SetStr(756, 780);
            SetDex(226, 245);
            SetInt(26, 40);

            SetHits(1208, 1936);
            SetMana(0);

            SetDamage(19, 33);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 50);
            SetResistance(ResistanceType.Fire, 50);
            SetResistance(ResistanceType.Cold, -50);
            SetResistance(ResistanceType.Poison, 0);
            SetResistance(ResistanceType.Energy, 100);

            SetSkill(SkillName.MagicResist, 15.1, 20.0);
            SetSkill(SkillName.Tactics, 85.1, 89.7);
            SetSkill(SkillName.Wrestling, 85.1, 91.5);

            Fame = 12000;
            Karma = -12000;

            Tamable = true;
            ControlSlots = 4;
            MinTameSkill = 76.7;

            CanSwim = true;
            CantWalk = false;

            PackGold(779, 1086);
            PackItem(new SulfurousAsh(Utility.RandomMinMax(15, 25)));
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Gems, 5);
        }

        public override int GetIdleSound()
        {
            return 353;
        }

        public override int GetAngerSound()
        {
            return 649;
        }

        public override int GetAttackSound()
        {
            return 651;
        }

        public override int GetHurtSound()
        {
            return 652;
        }

        public override int GetDeathSound()
        {
            return 653;
        }

        public override FoodType FavoriteFood
        {
            get { return FoodType.Meat | FoodType.Fish; }
        }

        public void SandAttack(Mobile m)
        {
            DoHarmful(m);

            m.FixedParticles(0x36B0, 10, 25, 9540, 2413, 0, EffectLayer.Waist);

            new InternalTimer(m, this).Start();
        }

        private class InternalTimer : Timer
        {
            private Mobile m_Mobile, m_From;

            public InternalTimer(Mobile m, Mobile from) : base(TimeSpan.FromSeconds(1.0))
            {
                m_Mobile = m;
                m_From = from;
                Priority = TimerPriority.TwoFiftyMS;
            }

            protected override void OnTick()
            {
                m_Mobile.PlaySound(0x4CF);
                AOS.Damage(m_Mobile, m_From, Utility.RandomMinMax(1, 30), 90, 10, 0, 0, 0);
            }
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (Map == null)
                return;
            
            BaseCreature bc = defender as BaseCreature;
            if (bc == null || bc.BardProvoked)
                return;

            if (0.4 > Utility.RandomDouble())
            {
                Mobile target = bc;
                Mobile master = bc.GetMaster();

                if (master != null && InRange(master, 18))
                    target = master;

                Animate(10, 4, 1, true, false, 0);

                List<Mobile> targets = new List<Mobile>();

                IPooledEnumerable eable = target.GetMobilesInRange(8);
                foreach (Mobile m in eable)
                {
                    if (m == this || !CanBeHarmful(m))
                        continue;

                    if (m.Player && m.Alive)
                        targets.Add(m);
                    else
                    {
                        BaseCreature bc2 = m as BaseCreature;
                        if (bc2 != null && (bc2.Controlled || bc2.Summoned || bc2.Team != Team))
                            targets.Add(m);
                    }
                }
                eable.Free();

                foreach (var m in targets)
                {
                    DoHarmful(m);
                    AOS.Damage(m, this, Utility.RandomMinMax(15, 30), true, 0, 0, 100, 0, 0);
                    m.FixedParticles(0x3709, 1, 10, 0x1F78, 0x849, 0, (EffectLayer)255);
                }
            }
        }
        
        public SandKraken(Serial serial) : base(serial)
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
    }
}