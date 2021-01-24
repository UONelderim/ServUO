using System;
using System.Collections;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;
using Server.Spells;

namespace Server.ACC.CSS.Systems.Bard
{
	public class BardMagesBalladSpell : BardSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
		                                                "Pieśń Do Magów", "Mentus",
		                                                //SpellCircle.First,
		                                                212,9041
		                                               );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

		public override double CastDelay{ get{ return 3; } }
		public override double RequiredSkill{ get{ return 85; } }
		public override int RequiredMana{ get{ return 30; } }

		public BardMagesBalladSpell( Mobile caster, Item scroll) : base( caster, scroll, m_Info )
		{
			                    if (this.Scroll != null)
                        Scroll.Consume();
		}

		public override void OnCast()
		{
			if( CheckSequence() )
			{
				ArrayList targets = new ArrayList();

				foreach ( Mobile m in Caster.GetMobilesInRange( 3 ) )
				{
					if ( Caster.CanBeBeneficial( m, false, true ) && !(m is Golem) )
						targets.Add( m );
				}

				for ( int i = 0; i < targets.Count; ++i )
				{
					Mobile m = (Mobile)targets[i];

					TimeSpan duration = TimeSpan.FromSeconds( Caster.Skills[SkillName.Provocation].Value * 0.1 );
					int rounds = (int)( Caster.Skills[SkillName.Musicianship].Value * .16 );

					new ExpireTimer( m, 0, rounds, TimeSpan.FromSeconds( 2 ) ).Start();

					m.FixedParticles( 0x376A, 9, 32, 5030, 0x256, 3, EffectLayer.Waist );
					m.PlaySound( 0x1F2 );
				}
			}

			FinishSequence();
		}

		private class ExpireTimer : Timer
		{
			private Mobile m_Mobile;
			private int m_Round;
			private int m_Totalrounds;

			public ExpireTimer( Mobile m, int round, int totalrounds, TimeSpan delay ) : base( delay )
			{
				m_Mobile = m;
				m_Round = round;
				m_Totalrounds = totalrounds;
			}

			protected override void OnTick()
			{
				if ( m_Mobile != null )
				{

					m_Mobile.Mana += 10;

					if ( m_Round >= m_Totalrounds )
					{
						m_Mobile.SendMessage( "Efekt pieśni wygasa" );
					}
					else
					{
						m_Round += 1;
						new ExpireTimer( m_Mobile, m_Round, m_Totalrounds, TimeSpan.FromSeconds( 2 ) ).Start();
					}
				}
			}
		}
	}
}
