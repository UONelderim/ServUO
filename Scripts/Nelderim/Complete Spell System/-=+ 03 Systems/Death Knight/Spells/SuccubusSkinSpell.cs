using System;
using System.Collections;
using Server.Targeting;
using Server.Network;
using Server.Mobiles;

namespace Server.Spells.DeathKnight
{
	public class SuccubusSkinSpell : DeathKnightSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Skora Sukkuba", "Erinyes Carnem",
				236,
				9011
			);

		private static Hashtable m_Table = new Hashtable();
        public override TimeSpan CastDelayBase { get { return TimeSpan.FromSeconds(3); } }
		public override int RequiredTithing{ get{ return 49; } }
        public override int RequiredMana { get { return 32; } }
		public override double RequiredSkill{ get{ return 68.0; } }

		public SuccubusSkinSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
		}

		public static bool HasEffect( Mobile m )
		{
			return ( m_Table[m] != null );
		}

		public static void RemoveEffect( Mobile m )
		{
			Timer t = (Timer)m_Table[m];

			if ( t != null )
			{
				t.Stop();
				m_Table.Remove( m );
			} 
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

			if ( m_Table.Contains( m ) )
			{
				Caster.LocalOverheadMessage( MessageType.Regular, 0x481, false, "Ten cel juz korzysta z tego efektu." );
			}

			else if ( CheckBSequence( m, false ) && CheckFizzle() )
			{
				SpellHelper.Turn( Caster, m );

				Timer t = new InternalTimer( m, Caster );
				t.Start();
				m_Table[m] = t;
				m.PlaySound( 0x202 );
				m.FixedParticles( 0x3779, 1, 46, 9502, 5, 3, EffectLayer.Waist );
				m.SendMessage( "Twa skora zmienia sie, powodujac zasklepianie sie ran." );

				double timer = GetKarmaPower( Caster );

				BuffInfo.AddBuff ( m, new BuffInfo ( BuffIcon.CorpseSkin, 1044123, 1044118, TimeSpan.FromSeconds ( timer ), m ) );
			}

			FinishSequence();
		}

		private class InternalTarget : Target
		{
			private SuccubusSkinSpell m_Owner;

			public InternalTarget( SuccubusSkinSpell owner ) : base( 12, false, TargetFlags.Beneficial )
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


		private class InternalTimer : Timer
		{
			private Mobile dest, source;
			private DateTime NextTick;
			private DateTime Expire;

			public InternalTimer( Mobile m, Mobile from ) : base( TimeSpan.FromSeconds( 0.1 ), TimeSpan.FromSeconds( 0.1 ) )
			{
				dest = m;
				source = from;
				Priority = TimerPriority.FiftyMS;
				double timer = GetKarmaPower( from );
				Expire = DateTime.Now + TimeSpan.FromSeconds( timer );
			}

			protected override void OnTick()
			{
				if ( !dest.CheckAlive() )
				{
					Stop();
					m_Table.Remove( dest );
				}

				if ( DateTime.Now < NextTick )
					return;

				if ( DateTime.Now >= NextTick )
				{
					double heal = PlayerLevelMod( Utility.RandomMinMax( 5, 10 ), dest );
					dest.Heal( (int)heal );
					dest.FixedParticles( 0x3779, 1, 46, 9502, 5, 3, EffectLayer.Waist );
					NextTick = DateTime.Now + TimeSpan.FromSeconds( 4 );
				}

				if ( DateTime.Now >= Expire )
				{
					Stop();
					if ( m_Table.Contains( dest ) )
						m_Table.Remove( dest );
				}
			}
		}
	}
}