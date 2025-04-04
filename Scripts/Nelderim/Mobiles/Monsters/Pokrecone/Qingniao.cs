using System;
using System.Collections.Generic;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("resztki qingniao")]
    public class Qingniao : BaseCreature
    {
        [Constructable]
        public Qingniao() : base(AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            Name = "qingniao";
            Body = 6;
            Hue = 88;

            SetStr(77, 95);
            SetDex(51, 69);
            SetInt(53, 70);

            SetHits(150, 200);

            SetDamage(1);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 40);
            SetResistance(ResistanceType.Fire, 25);
            SetResistance(ResistanceType.Cold, 30);
            SetResistance(ResistanceType.Poison, 30);
            SetResistance(ResistanceType.Energy, 30);

            SetSkill(SkillName.MagicResist, 55.0, 65.0);
            SetSkill(SkillName.Tactics, 45.6, 54.4);
            SetSkill(SkillName.Wrestling, 50.7, 59.6);

            switch (Utility.Random(6))
            {
                case 0:
                    PackItem(new Apple());
                    break;
                case 1:
                    PackItem(new Banana());
                    break;
                case 2:
                    PackItem(new Grapes());
                    break;
                case 3:
                    PackItem(new Lemon());
                    break;
                case 4:
                    PackItem(new Lime());
                    break;
                case 5:
                    PackItem(new Pear());
                    break;
            }
        }

        public override int GetIdleSound()
        {
            return 0x07D;
        }

        public override int GetAngerSound()
        {
            return 0x07E;
        }

        public override int GetAttackSound()
        {
            return 0x07F;
        }

        public override int GetHurtSound()
        {
            return 0x080;
        }

        public override int GetDeathSound()
        {
            return 0x081;
        }

        public override MeatType MeatType => MeatType.Bird;
        public override int Meat => 1;
        public override int Feathers => 25;

        public override void OnThink()
        {
            base.OnThink();

            if(Combatant is Mobile m)
            {
	            Peace(m);
	            Undress(m);
	            Suppress(m);
	            Provoke(m);
            }
        }

        #region Peace

        private DateTime m_NextPeace;

        public void Peace(Mobile target)
        {
            if (target == null || Deleted || !Alive || m_NextPeace > DateTime.Now || 0.1 < Utility.RandomDouble())
                return;

            PlayerMobile p = target as PlayerMobile;

            if (p != null && !p.Peaced && !p.Hidden && CanBeHarmful(p))
            {
                p.PeacedUntil = DateTime.UtcNow + TimeSpan.FromMinutes(1);
                p.SendLocalizedMessage(500616); // You hear lovely music, and forget to continue battling!
                p.FixedParticles(0x376A, 1, 32, 0x15BD, EffectLayer.Waist);
                p.Combatant = null;

                PlaySound(0x58D);
            }

            m_NextPeace = DateTime.Now + TimeSpan.FromSeconds(10);
        }

        #endregion

        #region Suppress

        private static Dictionary<Mobile, Timer> m_Suppressed = new ();
        private DateTime m_NextSuppress;

        public void Suppress(Mobile target)
        {
            if (target == null || m_Suppressed.ContainsKey(target) || Deleted || !Alive ||
                m_NextSuppress > DateTime.Now || 0.1 < Utility.RandomDouble())
                return;

            TimeSpan delay = TimeSpan.FromSeconds(Utility.RandomMinMax(20, 80));

            if (!target.Hidden && CanBeHarmful(target))
            {
                target.SendLocalizedMessage(1072061); // You hear jarring music, suppressing your strength.

                for (int i = 0; i < target.Skills.Length; i++)
                {
                    Skill s = target.Skills[i];

                    target.AddSkillMod(new TimedSkillMod(s.SkillName, true, s.Base * -0.28, delay));
                }

                int count = (int)Math.Round(delay.TotalSeconds / 1.25);
                Timer timer = new AnimateTimer(target, count);
                m_Suppressed.Add(target, timer);
                timer.Start();

                PlaySound(0x58C);
            }

            m_NextSuppress = DateTime.Now + TimeSpan.FromSeconds(10);
        }

        public static void SuppressRemove(Mobile target)
        {
            if (target != null && m_Suppressed.ContainsKey(target))
            {
                Timer timer = m_Suppressed[target];

                if (timer != null || timer.Running)
                    timer.Stop();

                m_Suppressed.Remove(target);
            }
        }

        private class AnimateTimer : Timer
        {
            private Mobile m_Owner;
            private int m_Count;

            public AnimateTimer(Mobile owner, int count) : base(TimeSpan.Zero, TimeSpan.FromSeconds(1.25))
            {
                m_Owner = owner;
                m_Count = count;
            }

            protected override void OnTick()
            {
                if (m_Owner.Deleted || !m_Owner.Alive || m_Count-- < 0)
                {
                    SuppressRemove(m_Owner);
                }
                else
                    m_Owner.FixedParticles(0x376A, 1, 32, 0x15BD, EffectLayer.Waist);
            }
        }

        #endregion

        #region Undress

        private DateTime m_NextUndress;

        public void Undress(Mobile target)
        {
            if (target == null || Deleted || !Alive || m_NextUndress > DateTime.Now || 0.005 < Utility.RandomDouble())
                return;

            if (target.Player && target.Female && !target.Hidden && CanBeHarmful(target))
            {
                UndressItem(target, Layer.OuterTorso);
                UndressItem(target, Layer.InnerTorso);
                UndressItem(target, Layer.MiddleTorso);
                UndressItem(target, Layer.Pants);
                UndressItem(target, Layer.Shirt);

                target.SendMessage("The qingniao's music makes your blood race. Your clothing is too confining.");
            }

            m_NextUndress = DateTime.Now + TimeSpan.FromMinutes(1);
        }

        public void UndressItem(Mobile m, Layer layer)
        {
            Item item = m.FindItemOnLayer(layer);

            if (item != null && item.Movable)
                m.PlaceInBackpack(item);
        }

        #endregion

        #region Provoke

        private DateTime m_NextProvoke;

        public void Provoke(Mobile target)
        {
            if (target == null || Deleted || !Alive || m_NextProvoke > DateTime.Now || 0.05 < Utility.RandomDouble())
                return;

            IPooledEnumerable eable = GetMobilesInRange(RangePerception);
            foreach (Mobile m in eable)
            {
                if (m is BaseCreature)
                {
                    BaseCreature c = (BaseCreature)m;

                    if (c == this || c == target || c.Unprovokable || c.IsParagon || c.BardProvoked ||
                        c.AccessLevel != AccessLevel.Player || !c.CanBeHarmful(target))
                        continue;

                    c.Provoke(this, target, true);

                    if (target.Player)
                        target.SendLocalizedMessage(1072062); // You hear angry music, and start to fight.

                    PlaySound(0x58A);
                    break;
                }
            }
            eable.Free();

            m_NextProvoke = DateTime.Now + TimeSpan.FromSeconds(10);
        }

        #endregion
        
        public Qingniao(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
