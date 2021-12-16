using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Targeting;
using Server.Network;
using Server.Mobiles;
using Server.Items;
using Server.Spells;
using Server.Spells.Seventh;
using Server.Gumps;

namespace Server.ACC.CSS.Systems.Rogue
{
    public class RogueIntimidationSpell : RogueSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Zastraszenie", " *wpatruje sie gniewnie w cel* ",
            //SpellCircle.Fourth,
                                                        212,
                                                        9041,
														Reagent.PowderOfTranslocation
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public override double CastDelay { get { return 3; } }

		public override double RequiredSkill{ get{ return 80.0; } }

		public override int RequiredMana{ get{ return 35; } }

       public RogueIntimidationSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
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
			else if ( CheckHSequence( m ) )
			{
				m.BoltEffect( 0x480 );

				SpellHelper.Turn( Caster, m );

				double damage = Caster.Skills[SkillName.Hiding].Value;

				if ( Core.AOS )
				{
					SpellHelper.Damage( TimeSpan.Zero, m, Caster, damage, 0, 0, 0, 0, 40 );
				}
				else
				{
					SpellHelper.Damage( TimeSpan.Zero, m, Caster, damage );
				}
			}

			FinishSequence();
		}


		private class InternalTarget : Target
		{
			private RogueIntimidationSpell m_Owner;

			public InternalTarget( RogueIntimidationSpell owner ) : base( 12, false, TargetFlags.Harmful )
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
