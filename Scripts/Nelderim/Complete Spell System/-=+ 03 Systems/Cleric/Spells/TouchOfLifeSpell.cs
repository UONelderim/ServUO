using System;
using System.Collections;
using Server.Targeting;
using Server.Network;
using Server.Mobiles;
using Server.Spells;

namespace Server.ACC.CSS.Systems.Cleric
{
	public class ClericTouchOfLifeSpell : ClericSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
		                                                "Dotyk Życia", "Tactus Vitalis",
		                                                //SpellCircle.Third,
		                                                212,
		                                                9041
		                                               );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Sixth; }
        }

		public override int RequiredTithing{ get{ return 10; } }
		public override double RequiredSkill{ get{ return 30.0; } }

		public override int RequiredMana{ get{ return 20; } }

		public ClericTouchOfLifeSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
			                    if (this.Scroll != null)
                        Scroll.Consume();
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
			else if ( CheckBSequence( m, false ) )
			{
				SpellHelper.Turn( Caster, m );

				m.PlaySound( 0x202 );
				m.FixedParticles( 0x376A, 1, 62, 0x480, 3, 3, EffectLayer.Waist );
				m.FixedParticles( 0x3779, 1, 46, 0x481, 5, 3, EffectLayer.Waist );

				double toHeal = Caster.Skills[SkillName.Healing].Value * 0.1 + Caster.Skills[SkillName.Anatomy].Value/50;

				toHeal *= ClericDivineFocusSpell.GetScalar( Caster );

				m.Heal( (int)toHeal );
			}

			FinishSequence();
		}

		private class InternalTarget : Target
		{
			private ClericTouchOfLifeSpell m_Owner;

			public InternalTarget( ClericTouchOfLifeSpell owner ) : base( 12, false, TargetFlags.Beneficial )
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
