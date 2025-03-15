using Server.Network;
using Server.Spells;
using Server.Targeting;
using System;
using System.Linq;

namespace Server.Items
{
    public partial class IceHorn : Item
    {
        [Constructable]
        public IceHorn()
            : base(0xFC7)
        {
            Hue = 2170;
            Weight = 1.0;
        }

        public IceHorn(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber => 3070047;

        public override void OnDoubleClick(Mobile from)
        {
            if (CheckUse(from))
            {
                from.SendLocalizedMessage(3070048); // Select an area to freeze.
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

            if (!from.CanBeginAction(typeof(IceHorn)))
            {
                from.SendLocalizedMessage(1049615); // You must take a moment to catch your breath.
                return false;
            }

            if (from.Backpack == null || from.Backpack.GetAmount(typeof(BlackPearl)) < 4)
            {
                from.SendLocalizedMessage(3070049); // You do not have enough black pearl.
                return false;
            }

            return true;
        }

        private class InternalTarget : Target
        {
            private readonly IceHorn m_Horn;
            public InternalTarget(IceHorn horn)
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

            from.BeginAction(typeof(IceHorn));
            Timer.DelayCall(TimeSpan.FromSeconds(6.0), new TimerStateCallback(EndAction), from);

            int music = from.Skills[SkillName.Musicianship].Fixed;
            int sucChance = 500 + (music - 775) * 2;
            double dSucChance = sucChance / 1000.0;

            if (!from.CheckSkill(SkillName.Musicianship, dSucChance))
            {
                from.SendLocalizedMessage(1049618); // The horn emits a pathetic squeak.
                from.PlaySound(0x18A);
                return;
            }

            from.Backpack.ConsumeUpTo(typeof(BlackPearl), 4);

            from.PlaySound(0x15E);
            Effects.SendPacket(from, from.Map, new HuedEffect(EffectType.Moving, from.Serial, Serial.Zero, 0x36D4, from.Location, loc, 5, 0, false, true, 1151, 0));

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
                    SpellHelper.Damage(TimeSpan.Zero, m, from, toDeal, 0, 0, 0, 100, 0);

                    
                    m.FixedParticles(0x374A, 10, 15, 5038, 1181, 2, EffectLayer.Head);
                    m.PlaySound(0x213);
                }
            }

            if (--UsesRemaining <= 0)
            {
                from.SendLocalizedMessage(1049619); // The ice horn crumbles in your hands.
                Delete();
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.WriteEncodedInt(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadEncodedInt();
        }

        private static void EndAction(object state)
        {
            Mobile m = (Mobile)state;
            m.EndAction(typeof(IceHorn));
            m.SendLocalizedMessage(1049621); // You catch your breath.
        }
    }
}
