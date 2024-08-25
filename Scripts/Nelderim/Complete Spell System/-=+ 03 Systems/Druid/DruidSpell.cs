#region References

using System;
using Server.Mobiles;
using Server.Spells;

#endregion

namespace Server.ACC.CSS.Systems.Druid
{
	public abstract class DruidSpell : CSpell
	{
		public DruidSpell(Mobile caster, Item scroll, SpellInfo info)
			: base(caster, scroll, info)
		{
		}

		public abstract SpellCircle Circle { get; }
		public override bool ClearHandsOnCast => false;
		public override SkillName CastSkill => SkillName.Herbalism;
		public override SkillName DamageSkill => SkillName.Magery;
		public override TimeSpan CastDelayBase => TimeSpan.FromSeconds(3 * CastDelaySecondsPerTick);
		public override int CastRecoveryBase => 4;

		public override void GetCastSkills(out double min, out double max)
		{
			min = RequiredSkill;
			max = RequiredSkill + 30.0;
		}

		public override int GetMana()
		{
			return RequiredMana;
		}

		public override TimeSpan GetCastDelay()
		{
			return TimeSpan.FromSeconds(CastDelay);
		}
		
		public override bool CheckFizzle()
		{
			if (!base.CheckFizzle())
				return false;
			
			if (Caster is PlayerMobile && !((PlayerMobile)Caster).Nature)
			{
				Caster.SendLocalizedMessage(3060182); // Aby korzystac z tych zaklec, musisz wykonac odpowiednie zadanie..
				return false;
			}


			return true;
		}

		public virtual bool CheckResisted(Mobile target)
		{
			double n = GetResistPercent(target);

			n /= 100.0;

			if (n <= 0.0)
				return false;

			if (n >= 1.0)
				return true;

			int maxSkill = (1 + (int)Circle) * 10;
			maxSkill += (1 + ((int)Circle / 6)) * 25;

			if (target.Skills[SkillName.MagicResist].Value < maxSkill)
				target.CheckSkill(SkillName.MagicResist, 0.0, 120.0);

			return (n >= Utility.RandomDouble());
		}

		public virtual double GetResistPercent(Mobile target)
		{
			return GetResistPercentForCircle(target, Circle);
		}

		public virtual double GetResistPercentForCircle(Mobile target, SpellCircle circle)
		{
			double firstPercent = target.Skills[SkillName.MagicResist].Value / 5.0;
			double secondPercent = target.Skills[SkillName.MagicResist].Value -
			                       (((Caster.Skills[CastSkill].Value - 20.0) / 5.0) + (1 + (int)circle) * 5.0);

			return (firstPercent > secondPercent ? firstPercent : secondPercent) / 2.0; 
		}
	}
}
