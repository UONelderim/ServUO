using System;
using Server.Spells;

namespace Server.ACC.CSS.Systems.Druid
{
	public class DruidHollowReedSpell : DruidSpell
	{
		private static SpellInfo m_Info = new(
			"Siła Natury",
			"En Crur Aeta Sec En Ess ",
			203,
			9061,
			false,
			Reagent.Bloodmoss,
			Reagent.MandrakeRoot,
			Reagent.Nightshade
		);

		public override SpellCircle Circle => SpellCircle.Second;
		public override double CastDelay => 1.0;
		public override double RequiredSkill => 30.0;
		public override int RequiredMana => 30;

		public DruidHollowReedSpell(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
		{
			Scroll?.Consume();
		}

		public override void OnCast()
		{
			if (!Caster.CanBeginAction(typeof(DruidHollowReedSpell)))
			{
				Caster.SendLocalizedMessage(1005559);
			}

			else if (CheckSequence())
			{
				var modValue = (int)((Caster.Skills[CastSkill].Value + Caster.Skills[DamageSkill].Value) / 12);
				var duration = TimeSpan.FromMilliseconds(15);
				Caster.AddStatMod(new StatMod(StatType.All, "[Druid] Hollow Reed", modValue, duration));
			}
		}
	}
}
