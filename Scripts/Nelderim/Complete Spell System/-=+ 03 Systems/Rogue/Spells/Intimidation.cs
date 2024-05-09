#region References

using System;
using Server.Spells;
using Server.Targeting;

#endregion

namespace Server.ACC.CSS.Systems.Rogue
{
	public class RogueIntimidationSpell : RogueSpell
	{
		private static readonly SpellInfo m_Info = new SpellInfo(
			"Zastraszenie", " *wpatruje sie gniewnie w cel* ",
			//SpellCircle.Fourth,
			212,
			9041,
			Reagent.PowderOfTranslocation
		);

		public override SpellCircle Circle => SpellCircle.Fourth;

		public override double CastDelay => 3;

		public override double RequiredSkill => 80.0;

		public override int RequiredMana => 35;

		public RogueIntimidationSpell(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
		{
			if (this.Scroll != null)
				Scroll.Consume();
		}

		public override void OnCast()
		{
			Caster.Target = new InternalTarget(this);
		}

		public void Target(Mobile m)
		{
			if (!Caster.CanSee(m))
			{
				Caster.SendLocalizedMessage(500237); // Target can not be seen.
			}
			else if (CheckHSequence(m))
			{
				m.BoltEffect(0x480);

				SpellHelper.Turn(Caster, m);

				double damage = Caster.Skills[SkillName.Hiding].Value;

				SpellHelper.Damage(TimeSpan.Zero, m, Caster, damage, 0, 0, 0, 0, 40);
			}

			FinishSequence();
		}


		private class InternalTarget : Target
		{
			private readonly RogueIntimidationSpell m_Owner;

			public InternalTarget(RogueIntimidationSpell owner) : base(12, false, TargetFlags.Harmful)
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
