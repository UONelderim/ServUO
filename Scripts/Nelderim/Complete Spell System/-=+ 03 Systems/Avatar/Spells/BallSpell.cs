#region References

using Server.Spells;
using Server.Targeting;

#endregion

namespace Server.ACC.CSS.Systems.Avatar
{
	public class AvatarBallSpell : AvatarSpell
	{
		private static readonly SpellInfo m_Info = new SpellInfo(
			"Kula Sniezna", "In Vas Frost",
			//SpellCircle.First,
			233,
			9012
		);

		public override SpellCircle Circle => SpellCircle.First;

		public override double CastDelay => 1.5;
		public override int RequiredTithing => 100;
		public override double RequiredSkill => 70.0;
		public override int RequiredMana => 45;

		public AvatarBallSpell(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
		{
			if (this.Scroll != null)
				Scroll.Consume();
		}

		public override void OnCast()
		{
			Caster.Target = new InternalTarget(this);
		}

		public override bool DelayedDamage => true;

		public void Target(Mobile m)
		{
			if (!Caster.CanSee(m))
			{
				Caster.SendLocalizedMessage(500237); // Target can not be seen.
			}
			else if (CheckHSequence(m))
			{
				Mobile source = Caster;

				SpellHelper.Turn(source, m);

				//	SpellHelper.CheckReflect( CirclePower, ref source, ref m );

				double damage = ((Caster.Skills[CastSkill].Value - m.Skills[SkillName.Anatomy].Value) / 10) + 30;
				if (damage > 50) { damage = 50.0; }

				if (damage < 10) { damage = 10.0; }

				m.FixedParticles(0x11B6, 0, 9502, 4019, 1153, 0, EffectLayer.Head);

				source.PlaySound(0x650);

				SpellHelper.Damage(this, m, damage, 0, 0, 100, 0, 0);
				//Server.Misc.Research.ConsumeScroll( Caster, true, spellIndex, false );
			}

			FinishSequence();
		}

		private class InternalTarget : Target
		{
			private readonly AvatarBallSpell m_Owner;

			public InternalTarget(AvatarBallSpell owner) : base(10, false, TargetFlags.Harmful)
			{
				m_Owner = owner;
			}

			protected override void OnTarget(Mobile from, object o)
			{
				if (o is Mobile)
					m_Owner.Target((Mobile)o);
			}

			protected override void OnTargetFinish(Mobile from)
			{
				m_Owner.FinishSequence();
			}
		}
	}
}
