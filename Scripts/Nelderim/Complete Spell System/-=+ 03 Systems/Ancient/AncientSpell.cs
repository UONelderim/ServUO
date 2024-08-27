#region References

using System;
using Server.Mobiles;
using Server.Spells;

#endregion

namespace Server.ACC.CSS.Systems.Ancient
{
	public abstract class AncientSpell : CSpell
	{
		public AncientSpell(Mobile caster, Item scroll, SpellInfo info) : base(caster, scroll, info)
		{
		}

		public abstract SpellCircle Circle { get; }

		public override TimeSpan CastDelayBase { get { return TimeSpan.FromSeconds(3 * CastDelaySecondsPerTick); } }
		public override SkillName CastSkill { get { return SkillName.Magery; } }
		public override SkillName DamageSkill { get { return SkillName.EvalInt; } }

		public override bool ClearHandsOnCast { get { return true; } }

		public override void GetCastSkills(out double min, out double max)
		{
			min = RequiredSkill;
			max = RequiredSkill;
		}
		
		public override bool CheckFizzle()
		{
			if (!base.CheckFizzle())
				return false;
			
			if (Caster is PlayerMobile && !((PlayerMobile)Caster).Ancient)
			{
				Caster.SendLocalizedMessage(3060182); // Aby korzystac z tych zaklec, musisz wykonac odpowiednie zadanie..
				return false;
			}


			return true;
		}

		public override TimeSpan GetCastDelay()
		{
			return TimeSpan.FromSeconds(CastDelay);
		}
	}
}
