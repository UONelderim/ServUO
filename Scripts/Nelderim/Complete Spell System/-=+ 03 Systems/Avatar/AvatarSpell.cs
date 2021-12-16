using System;
using Server;
using Server.Spells;
using Server.Network;
using Server.Mobiles;

namespace Server.ACC.CSS.Systems.Avatar
{
    public abstract class AvatarSpell : CSpell
    {
        public override SkillName CastSkill { get { return SkillName.Meditation; } }
        public override SkillName DamageSkill { get { return SkillName.Anatomy; } }

        public abstract SpellCircle Circle { get; }

        public override bool ClearHandsOnCast { get { return false; } }

        public override TimeSpan CastDelayBase { get { return TimeSpan.FromSeconds(3 * CastDelaySecondsPerTick); } }

        public override int CastRecoveryBase { get { return 7; } }
        public override int CastRecoveryFastScalar { get { return 1; } }
        public override int CastRecoveryPerSecond { get { return 4; } }
        public override int CastRecoveryMinimum { get { return 0; } }

        public AvatarSpell(Mobile caster, Item scroll, SpellInfo info)
            : base(caster, scroll, info)
        {
        }

        public override bool CheckCast()
        {
            if (!base.CheckCast())
                return false;

            if (Caster.Skills[SkillName.Meditation].Value < RequiredSkill)
            {
                Caster.SendLocalizedMessage(1060172, RequiredSkill.ToString("F1")); // You must have at least ~1_SKILL_REQUIREMENT~ Chivalry to use this ability,
                return false;
            }
            else if (Caster.TithingPoints < RequiredTithing)
            {
                Caster.SendLocalizedMessage(1060173, RequiredTithing.ToString()); // You must have at least ~1_TITHE_REQUIREMENT~ Tithing Points to use this ability,
                return false;
            }
            else if (Caster.Mana < ScaleMana(RequiredMana))
            {
                Caster.SendLocalizedMessage(1060174, RequiredMana.ToString()); // You must have at least ~1_MANA_REQUIREMENT~ Mana to use this ability.
                return false;
            }

            return true;
        }

        public override bool CheckFizzle()
        {
            int requiredTithing = this.RequiredTithing;

            if (AosAttributes.GetValue(Caster, AosAttribute.LowerRegCost) > Utility.Random(100))
                requiredTithing = 0;

            int mana = ScaleMana(RequiredMana);

            if (Caster.Skills[SkillName.Meditation].Value < RequiredSkill)
            {
                Caster.SendLocalizedMessage(1060172, RequiredSkill.ToString("F1")); // You must have at least ~1_SKILL_REQUIREMENT~ Chivalry to use this ability,
                return false;
            }
            else if (Caster.TithingPoints < requiredTithing)
            {
                Caster.SendLocalizedMessage(1060173, RequiredTithing.ToString()); // You must have at least ~1_TITHE_REQUIREMENT~ Tithing Points to use this ability,
                return false;
            }
            else if (Caster.Mana < mana)
            {
                Caster.SendLocalizedMessage(1060174, RequiredMana.ToString()); // You must have at least ~1_MANA_REQUIREMENT~ Mana to use this ability.
                return false;
            }

            Caster.TithingPoints -= requiredTithing;

            if (!base.CheckFizzle())
                return false;

            Caster.Mana -= mana;

            return true;
        }

        public override void DoFizzle()
        {
            Caster.PlaySound(0x1D6);
            Caster.NextSpellTime = DateTime.Now;
        }

        public override void DoHurtFizzle()
        {
            Caster.PlaySound(0x1D6);
        }

        public override void OnDisturb(DisturbType type, bool message)
        {
            base.OnDisturb(type, message);

            if (message)
                Caster.PlaySound(0x1D6);
        }

        public override void OnBeginCast()
        {
            base.OnBeginCast();

            SendCastEffect();
        }

        public virtual void SendCastEffect()
        {
            Caster.FixedEffect(0x37C4, 10, 42, 4, 3);
        }

        public override void GetCastSkills(out double min, out double max)
        {
            min = RequiredSkill;
            max = RequiredSkill + 50.0;
        }

        public override int GetMana()
        {
            return 0;
        }

        public int ComputePowerValue(int div)
        {
            return ComputePowerValue(Caster, div);
        }

        public static int ComputePowerValue(Mobile from, int div)
        {
            if (from == null)
                return 0;

            int v = (int)Math.Sqrt(from.Karma + 20000 + (from.Skills.Begging.Fixed * 10));

            return v / div;
        }
    }
}