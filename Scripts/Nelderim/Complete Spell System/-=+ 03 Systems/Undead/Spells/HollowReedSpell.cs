using System;
using Server.Targeting;
using Server.Network;
using Server.Spells;

namespace Server.ACC.CSS.Systems.Undead
{
	public class UndeadHollowReedSpell : UndeadSpell
	{
		private static SpellInfo m_Info = new (
			"Hedonizm",
			"En Nargh Aeta Sec En Ess ",
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

		public UndeadHollowReedSpell(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
		{
		}

		public override void OnCast()
		{
			if (!Caster.CanBeginAction(typeof(UndeadHollowReedSpell)))
			{
				Caster.SendLocalizedMessage(1005559);
			}

			else if (CheckSequence())
			{
				var modValue = (int)((Caster.Skills[CastSkill].Value + Caster.Skills[DamageSkill].Value) / 12);
				var duration = TimeSpan.FromSeconds(((6 * Caster.Skills[DamageSkill].Fixed) / 50) + 1);
				Caster.AddStatMod(new StatMod(StatType.All, "[Undead] Hollow Reed", modValue, duration));
			}
		}
	}
}
