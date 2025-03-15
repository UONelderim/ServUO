using Server.Network;
using Server.Spells;
using Server.Targeting;
using System;
using System.Linq;

namespace Server.Items
{
    public partial class EnergyHorn : Item
    {
        [Constructable]
        public EnergyHorn()
            : base(0xFC7)
        {
            Hue = 1092; // Niebieski kolor dla efektu energii
            Weight = 1.0;
        }

        public EnergyHorn(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber => 3070053; // Energy Horn

        public override void OnDoubleClick(Mobile from)
        {
            if (CheckUse(from))
            {
                from.SendLocalizedMessage(3070054); // Select an area to unleash energy.
                from.Target = new InternalTarget(this);
            }
        }

        protected bool CheckUse(Mobile from)
        {
            if (!IsAccessibleTo(from))
                return false;

            if (from.Map != Map || !from.InRange(GetWorldLocation(), 2))
            {
                from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1019045); // I can't reach that.
                return false;
            }

            if (!from.CanBeginAction(typeof(FireHorn)) || !from.CanBeginAction(typeof(IceHorn)) || !from.CanBeginAction(typeof(PoisonHorn)) || !from.CanBeginAction(typeof(EnergyHorn)))
            {
                from.SendLocalizedMessage(1049615); // You must take a moment to catch your breath.
                return false;
            }

            if (from.Backpack == null || from.Backpack.GetAmount(typeof(GraveDust)) < 4)
            {
                from.SendLocalizedMessage(3070055); // You do not have enough Grave Dust.
                return false;
            }

            return true;
        }

        private class InternalTarget : Target
        {
            private readonly EnergyHorn m_Horn;
            public InternalTarget(EnergyHorn horn)
                : base(3, true, TargetFlags.Harmful)
            {
                m_Horn = horn;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (m_Horn.Deleted)
                    return;

                IPoint3D loc = targeted is Item ? ((Item)targeted).GetWorldLocation() : targeted as IPoint3D;
                m_Horn.Use(from, loc);
            }
        }

        public void Use(Mobile from, IPoint3D loc)
        {
            if (!CheckUse(from))
                return;

            from.BeginAction(typeof(FireHorn));
            from.BeginAction(typeof(IceHorn));
            from.BeginAction(typeof(PoisonHorn));
            from.BeginAction(typeof(EnergyHorn));
            Timer.DelayCall(TimeSpan.FromSeconds(6.0), () => {
                from.EndAction(typeof(FireHorn));
                from.EndAction(typeof(IceHorn));
                from.EndAction(typeof(PoisonHorn));
                from.EndAction(typeof(EnergyHorn));
            });

            int music = from.Skills[SkillName.Musicianship].Fixed;
            int sucChance = 500 + (music - 775) * 2;
            double dSucChance = sucChance / 1000.0;

            if (!from.CheckSkill(SkillName.Musicianship, dSucChance))
            {
                from.SendLocalizedMessage(1049618); // The horn emits a pathetic squeak.
                from.PlaySound(0x18A);
                return;
            }

            from.Backpack.ConsumeUpTo(typeof(MandrakeRoot), 4);

            from.PlaySound(0x20A); // Dźwięk Energy Bolt
            Effects.SendPacket(from, from.Map, new HuedEffect(EffectType.Moving, from.Serial, Serial.Zero, 0x379F, from.Location, loc, 5, 0, false, true, 1092, 4043));

            var targets = SpellHelper.AcquireIndirectTargets(from, loc, from.Map, 2).OfType<Mobile>().ToList();
            int count = targets.Count;
            bool playerVsPlayer = targets.Any(t => t.Player);

            if (count > 0)
            {
                int prov = from.Skills[SkillName.Provocation].Fixed;
                int disc = from.Skills[SkillName.Discordance].Fixed;
                int peace = from.Skills[SkillName.Peacemaking].Fixed;

                int minDamage, maxDamage;

                int musicScaled = music + Math.Max(0, music - 900) * 2;
                int provScaled = prov + Math.Max(0, prov - 900) * 2;
                int discScaled = disc + Math.Max(0, disc - 900) * 2;
                int peaceScaled = peace + Math.Max(0, peace - 900) * 2;

                int weightAvg = (musicScaled + provScaled * 3 + discScaled * 3 + peaceScaled) / 80;

                int avgDamage;
                if (playerVsPlayer)
	                avgDamage = weightAvg / 3;
                else
	                avgDamage = weightAvg / 2;

                minDamage = (avgDamage * 9) / 10;
                maxDamage = (avgDamage * 10) / 9;

                double damage = Utility.RandomMinMax(minDamage, maxDamage);

                if (count > 1)
                    damage = (damage * 2) / count;

                foreach (Mobile m in targets)
                {
                    double toDeal = damage;

                    from.DoHarmful(m);
                    SpellHelper.Damage(TimeSpan.Zero, m, from, toDeal, 0, 0, 0, 0, 100); // 100% obrażeń od energii
                    
                    Effects.SendBoltEffect(m, true, 0, false);
                    m.PlaySound(0x29);
                }
            }
            
            if (--UsesRemaining <= 0)
            {
                from.SendLocalizedMessage(1049619); // The energy horn crumbles in your hands.
                Delete();
            }
        }
    }
}
