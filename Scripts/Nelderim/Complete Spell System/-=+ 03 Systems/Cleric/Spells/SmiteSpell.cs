#region References

using System;
using Server.Spells;
using Server.Targeting;

#endregion

namespace Server.ACC.CSS.Systems.Cleric
{
	public class ClericSmiteSpell : ClericSpell
	{
		private static readonly SpellInfo m_Info = new SpellInfo(
			"Smagnięcie", "Ferio",
			//SpellCircle.Eighth,
			212,
			9041
		);

		public override SpellCircle Circle => SpellCircle.Eighth;

		public override int RequiredTithing => 60;
		public override double RequiredSkill => 80.0;

		public override int RequiredMana => 35;

		public ClericSmiteSpell(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
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

				var baseDamage = Caster.Skills[SkillName.Healing].Value + (Caster.Skills[SkillName.Anatomy].Value) / 10;
				var damage = baseDamage * ClericDivineFocusSpell.GetScalar(Caster);

				SpellHelper.Damage(TimeSpan.Zero, m, Caster, damage, 0, 0, 0, 0, 40);
			}

			FinishSequence();
		}


		private class InternalTarget : Target
		{
			private readonly ClericSmiteSpell m_Owner;

			public InternalTarget(ClericSmiteSpell owner) : base(12, false, TargetFlags.Harmful)
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
