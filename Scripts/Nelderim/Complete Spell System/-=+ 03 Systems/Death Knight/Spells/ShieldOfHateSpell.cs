using System;
using System.Collections;
using Server.Network;
using Server.Targeting;

namespace Server.Spells.DeathKnight
{
	public class ShieldOfHateSpell : DeathKnightSpell
	{
		private static SpellInfo m_Info = new(
				"Tarcza Nienawisci", "Bael Odi",
				236,
				9011
			);

		public override TimeSpan CastDelayBase => TimeSpan.FromSeconds( 1 );
		public override int RequiredTithing => 77;
		public override double RequiredSkill => 60.0;
		public override int RequiredMana => 48;

		public ShieldOfHateSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
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

			if ( m_Table.Contains( m ) )
			{
				Caster.LocalOverheadMessage( MessageType.Regular, 0x481, false, "That target is under the effect of that spell already." );
			}

			if ( CheckBSequence( m ) /*&& CheckFizzle()*/ )
			{
				SpellHelper.Turn( Caster, m );

				ExpireTimer timer = (ExpireTimer)m_Table[m];

				if ( timer != null )
					timer.DoExpire();
				else
					m.SendMessage( "Twoja odpronosc fizyczna wrazasta." );
            		m.PlaySound( 0x5C0 );
					m.FixedParticles( 0x376A, 1, 29, 9961, 1152, 0, EffectLayer.Waist ); 

				TimeSpan duration = TimeSpan.FromSeconds( GetKarmaPower( Caster ) );

				ResistanceMod[] mods = new ResistanceMod[4]
					{
						new( ResistanceType.Fire, -10 ),
						new( ResistanceType.Poison, -20 ),
						new( ResistanceType.Cold, -10 ),
						new( ResistanceType.Physical, +80 )
					};

				timer = new ExpireTimer( m, mods, duration );
				timer.Start();

				BuffInfo.AddBuff ( m, new BuffInfo ( BuffIcon.ReactiveArmor, 1044120, 1044118, duration, m ) );

				m_Table[m] = timer;

				for ( int i = 0; i < mods.Length; ++i )
					m.AddResistanceMod( mods[i] );
			}

			FinishSequence();
		}

		private static Hashtable m_Table = new();

		public static bool RemoveCurse( Mobile m )
		{
			ExpireTimer t = (ExpireTimer)m_Table[m];

			if ( t == null )
				return false;

			m.SendMessage( "Tarcza rozplywa sie w powietrzu..." );
			m.PlaySound( 488 );
			t.DoExpire();
			return true;
		}

		public static bool UnderEffect( Mobile m )
		{
			return m_Table.Contains( m );
		}

		private class ExpireTimer : Timer
		{
			private Mobile m_Mobile;
			private ResistanceMod[] m_Mods;

			public ExpireTimer( Mobile m, ResistanceMod[] mods, TimeSpan delay ) : base( delay )
			{
				m_Mobile = m;
				m_Mods = mods;
			}

			public void DoExpire()
			{
				for ( int i = 0; i < m_Mods.Length; ++i )
					m_Mobile.RemoveResistanceMod( m_Mods[i] );

				Stop();
				BuffInfo.RemoveBuff( m_Mobile, BuffIcon.AttuneWeapon );
				m_Table.Remove( m_Mobile );
			}

			protected override void OnTick()
			{
				m_Mobile.SendMessage( "Tarcza rozprasza sie..." );
				m_Mobile.PlaySound( 488 );
				DoExpire();
			}
		}

		private class InternalTarget : Target
		{
			private ShieldOfHateSpell m_Owner;

			public InternalTarget( ShieldOfHateSpell owner ) : base( 12, false, TargetFlags.Beneficial )
			{
				m_Owner = owner;
			}

			protected override void OnTarget( Mobile from, object o )
			{
				if ( o is Mobile )
					m_Owner.Target( (Mobile) o );
			}

			protected override void OnTargetFinish( Mobile from )
			{
				m_Owner.FinishSequence();
			}
		}
	}
}
