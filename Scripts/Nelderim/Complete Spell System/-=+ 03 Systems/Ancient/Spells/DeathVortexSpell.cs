using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;

namespace Server.ACC.CSS.Systems.Ancient
{
    public class AncientDeathVortexSpell : AncientSpell
    {
        public override double CastDelay { get { return 2.5; } }
        public override double RequiredSkill { get { return 80.0; } }
        public override int RequiredMana { get { return 50; } }
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Wir Śmierci", "Vas Corp Hur",
                                                        260,
                                                        9032,
                                                        false,
                                                        Reagent.Bloodmoss,
                                                        Reagent.SulfurousAsh,
                                                        Reagent.MandrakeRoot,
                                                        Reagent.Nightshade
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Eighth; }
        }

        public AncientDeathVortexSpell(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info)
        {
                                if (this.Scroll != null)
                        Scroll.Consume();
        }

      public override bool CheckCast()
		{
			if ( !base.CheckCast() )
				return false;

			if ( (Caster.Followers + (Core.SE ? 2 : 1)) > Caster.FollowersMax )
			{
				Caster.SendLocalizedMessage( 1049645 ); // You have too many followers to summon that creature.
				return false;
			}

			return true;
		}

		public override void OnCast()
		{
			Caster.Target = new InternalTarget( this );
		}


public void Target( IPoint3D p )
		{
			Map map = Caster.Map;

			SpellHelper.GetSurfaceTop( ref p );

			if ( map == null || !map.CanSpawnMobile( p.X, p.Y, p.Z ) )
			{
				Caster.SendLocalizedMessage( 501942 ); // That location is blocked.
			}
			else if ( SpellHelper.CheckTown( p, Caster ) && CheckSequence() )
			{
				TimeSpan duration;

				if ( Core.AOS )
					duration = TimeSpan.FromSeconds( 90.0 );
				else
					duration = TimeSpan.FromSeconds( Utility.Random( 80, 40 ) );

				BaseCreature.Summon( new DeathVortex(), false, Caster, new Point3D( p ), 0x212, duration );
			}

			FinishSequence();
		}

        private class InternalTarget : Target
        {
            private AncientDeathVortexSpell m_Owner;

            public InternalTarget(AncientDeathVortexSpell owner)
                : base(12, true, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is IPoint3D)
                    m_Owner.Target((IPoint3D)o);
            }

            protected override void OnTargetOutOfLOS(Mobile from, object o)
            {
                from.SendLocalizedMessage(501943); // Target cannot be seen. Try again.
                from.Target = new InternalTarget(m_Owner);
                from.Target.BeginTimeout(from, TimeoutTime - DateTime.Now);
                m_Owner = null;
            }

            protected override void OnTargetFinish(Mobile from)
            {
                if (m_Owner != null)
                    m_Owner.FinishSequence();
            }
        }
    }
}
