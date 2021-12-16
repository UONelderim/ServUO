using System;
using System.Collections;
using Server.Targeting;
using Server.Network;
using Server.Mobiles;
using Server.Spells;

namespace Server.ACC.CSS.Systems.Cleric
{
	public class ClericSacredBoonSpell : ClericSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
		                                                "Święty znak", "Vir Consolatio",
		                                                //SpellCircle.Fourth,
		                                                212,
		                                                9041
		                                               );
        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

		private static Hashtable m_Table = new Hashtable();

		public override int RequiredTithing{ get{ return 15; } }
		public override double RequiredSkill{ get{ return 25.0; } }

		public override int RequiredMana{ get{ return 20; } }

		public ClericSacredBoonSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
			                    if (this.Scroll != null)
                        Scroll.Consume();
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
				Caster.LocalOverheadMessage( MessageType.Regular, 0x481, false, "Ten cel już korzysta z tego efektu." );
			}
			else if ( m.Poisoned || Server.Items.MortalStrike.IsWounded( m ) )
			{
				Caster.LocalOverheadMessage( MessageType.Regular, 0x3B2, (Caster == m) ? 1005000 : 1010398 );
			}
			else if ( m.Hits >= m.HitsMax )
			{
				Caster.SendLocalizedMessage( 500955 ); // "Jego stan zdrowia jest idealny!"
			}
			else if ( m is BaseCreature && ((BaseCreature)m).IsAnimatedDead )
			{
				Caster.SendLocalizedMessage( 1061654 ); // "Ta istota nie jest zywa, nie mozesz jej leczyc."
			}
			else if ( m.IsDeadBondedPet )
			{
				Caster.SendLocalizedMessage( 1060177 ); // "Nie mozesz wyleczyc martwego stworzenia."
			}
				else if ( m.Poisoned || Server.Items.MortalStrike.IsWounded( m ) )
			{
				Caster.LocalOverheadMessage( MessageType.Regular, 0x3B2, (Caster == m) ? 1005000 : 1010398 );
			}
			else if ( CheckBSequence( m, false ) )
			{
				SpellHelper.Turn( Caster, m );

				Timer t = new InternalTimer( m, Caster );
				t.Start();
				m_Table[m] = t;
				m.PlaySound( 0x202 );
				m.FixedParticles( 0x376A, 1, 62, 9923, 3, 3, EffectLayer.Waist );
				m.FixedParticles( 0x3779, 1, 46, 9502, 5, 3, EffectLayer.Waist );
				m.SendMessage( "Magia otaczająca was leczy wasze rany." );
			}

			FinishSequence();
		}

		private class InternalTarget : Target
		{
			private ClericSacredBoonSpell m_Owner;

			public InternalTarget( ClericSacredBoonSpell owner ) : base( 12, false, TargetFlags.Beneficial )
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
				Expire = DateTime.Now + TimeSpan.FromSeconds( 30.0 );
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
					double heal = Utility.RandomMinMax( 6, 9 ) + source.Skills[SkillName.Anatomy].Value / 50.0;
					heal *= ClericDivineFocusSpell.GetScalar( source );

					dest.Heal( (int)heal );

					dest.PlaySound( 0x202 );
					dest.FixedParticles( 0x376A, 1, 62, 9923, 3, 3, EffectLayer.Waist );
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
