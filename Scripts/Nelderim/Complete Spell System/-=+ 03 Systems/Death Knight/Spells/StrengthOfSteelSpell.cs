using System;
using Server.Targeting;
using Server.Mobiles;

namespace Server.Spells.DeathKnight
{
	public class StrengthOfSteelSpell : DeathKnightSpell
	{
		private static SpellInfo m_Info = new(
			"Wytrzymalosc Stali",
			"Volac Fortitudo",
			212,
			9061
		);

		public override TimeSpan CastDelayBase => TimeSpan.FromSeconds(1);
		public override int RequiredTithing => 28;
		public override double RequiredSkill => 20.0;
		public override int RequiredMana => 20;

		public StrengthOfSteelSpell(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
		{
		}

		public override void OnCast()
		{
			Caster.Target = new InternalTarget(this);
		}

		public static int PlayerLevelMod(int value, Mobile m)
		{
			// THIS MULTIPLIES AGAINST THE RAW STAT TO GIVE THE RETURNING HIT POINTS, MANA, OR STAMINA
			// SO SETTING THIS TO 2.0 WOULD GIVE THE CHARACTER HITS POINTS EQUAL TO THEIR STRENGTH x 2
			// THIS ALSO AFFECTS BENEFICIAL SPELLS AND POTIONS THAT RESTORE HEALTH, STAMINA, AND MANA

			double mod = 1.0;
			if (m is PlayerMobile) { mod = 0.2; } // ONLY CHANGE THIS VALUE

			value = (int)(value * mod);
			if (value < 0) { value = 1; }

			return value;
		}

		public void Target(Mobile m)
		{
			if (!Caster.CanSee(m))
			{
				Caster.SendLocalizedMessage(500237); // Target can not be seen.
			}
			else if (CheckBSequence(m))
			{
				SpellHelper.Turn(Caster, m);

				int bonus = PlayerLevelMod((int)(GetKarmaPower(m) / 2), Caster);
				double timer = (GetKarmaPower(m) / 10);
				SpellHelper.AddStatBonus(Caster, m, StatType.Str, bonus, TimeSpan.FromMinutes(timer));

				m.PlaySound(0x1EB);
				m.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Waist);

				BuffInfo.AddBuff(Caster,
					new BuffInfo(BuffIcon.Strength, 1044122, 1044118, TimeSpan.FromMinutes(timer), Caster));
			}

			FinishSequence();
		}

		private class InternalTarget : Target
		{
			private StrengthOfSteelSpell m_Owner;

			public InternalTarget(StrengthOfSteelSpell owner) : base(12, false, TargetFlags.Beneficial)
			{
				m_Owner = owner;
			}

			protected override void OnTarget(Mobile from, object o)
			{
				if (o is Mobile)
				{
					m_Owner.Target((Mobile)o);
				}
			}

			protected override void OnTargetFinish(Mobile from)
			{
				m_Owner.FinishSequence();
			}
		}
	}
}
