using System;
using Server.Targeting;

namespace Server.Spells.DeathKnight
{
	public class StrikeSpell : DeathKnightSpell
	{
		private static SpellInfo m_Info = new(
				"Uderzenie", "Naberius Impetus",
				230,
				9022
			);

		public override TimeSpan CastDelayBase => TimeSpan.FromSeconds( 3 );
		public override int RequiredTithing => 140;
		public override double RequiredSkill => 80.0;
		public override int RequiredMana => 30;

		public StrikeSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
		}

		public override void OnCast()
		{
			Caster.Target = new InternalTarget( this );
		}

		public void Target( Mobile m )
		{
			if ( !Caster.CanSee( m ) )
			{
				Caster.SendLocalizedMessage( 500237 ); // Target can not be seen.
			}
			else if ( CheckHSequence( m ) /*&& CheckFizzle()*/ )
			{
				m.FixedParticles( 0x36BD, 20, 10, 5044, EffectLayer.Head );
				m.PlaySound( 0x307 );

				SpellHelper.Turn( Caster, m );

				double damage = GetKarmaPower( Caster ) / 2;

				SpellHelper.Damage( TimeSpan.Zero, m, Caster, damage, 0, 0, 0, 0, 100 );
			}

			FinishSequence();
		}


		private class InternalTarget : Target
		{
			private StrikeSpell m_Owner;

			public InternalTarget( StrikeSpell owner ) : base( 12, false, TargetFlags.Harmful )
			{
				m_Owner = owner;
			}

			protected override void OnTarget( Mobile from, object o )
			{
				if ( o is Mobile )
				{
					m_Owner.Target( (Mobile)o );
				}
			}

			protected override void OnTargetFinish( Mobile from )
			{
				m_Owner.FinishSequence();
			}
		}
	}
}
