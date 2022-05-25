using System;
using System.Collections;
using Server.Targeting;
using Server.Network;
using Server.Mobiles;

namespace Server.Spells.DeathKnight
{
	public class DemonicTouchSpell : DeathKnightSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Dotyk Demona", "Raum Curare",
				224,
				9061
			);

		public override TimeSpan CastDelayBase { get { return TimeSpan.FromSeconds( 1 ); } }
		public override int RequiredTithing{ get{ return 21; } }
		public override double RequiredSkill{ get{ return 15.0; } }
		public override int RequiredMana{ get{ return 16; } }

		public DemonicTouchSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
		}
		public static int PlayerLevelMod( int value, Mobile m )
		{
			// THIS MULTIPLIES AGAINST THE RAW STAT TO GIVE THE RETURNING HIT POINTS, MANA, OR STAMINA
			// SO SETTING THIS TO 2.0 WOULD GIVE THE CHARACTER HITS POINTS EQUAL TO THEIR STRENGTH x 2
			// THIS ALSO AFFECTS BENEFICIAL SPELLS AND POTIONS THAT RESTORE HEALTH, STAMINA, AND MANA

			double mod = 1.0;
				if ( m is PlayerMobile ){ mod = 1.25; } // ONLY CHANGE THIS VALUE

			value = (int)( value * mod );
				if ( value < 0 ){ value = 1; }

			return value;
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
			else if ( CheckBSequence( m, false ) && CheckFizzle() )
			{
				SpellHelper.Turn( Caster, m );

				m.PlaySound( 0x202 );
				m.FixedParticles( 0x376A, 1, 62, 0x480, 3, 3, EffectLayer.Waist );
				m.FixedParticles( 0x37C4, 1, 46, 0x481, 5, 3, EffectLayer.Waist );

				double toHeal = GetKarmaPower( Caster ) / 2;
				int heal = PlayerLevelMod( (int)toHeal, Caster );
				m.Heal( heal );
			}

			FinishSequence();
		}

		private class InternalTarget : Target
		{
			private DemonicTouchSpell m_Owner;

			public InternalTarget( DemonicTouchSpell owner ) : base( 12, false, TargetFlags.Beneficial )
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