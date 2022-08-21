using System;
using System.Reflection;
using Server.Targeting;
using Server.Network;
using Server.Mobiles;
using Server.Items;
using Server.Spells;
using Server.Spells.Ninjitsu;
using Server.Spells.Necromancy;
using System.Collections.Generic;
using Server.Items;

namespace Server.ACC.CSS.Systems.Ancient
{
    public class AncientCloneSpell : AncientSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Klonowanie", "In Quas Xen",
            //SpellCircle.Sixth,
                                                        230,
                                                        9022,
                                                        Reagent.SulfurousAsh,
                                                        Reagent.SpidersSilk,
                                                        Reagent.Bloodmoss,
                                                        Reagent.Ginseng,
                                                        Reagent.Nightshade,
                                                        Reagent.MandrakeRoot
                                                       );
        public override SpellCircle Circle
        {
            get { return SpellCircle.Sixth; }
        }
        public override double RequiredSkill { get { return 71.1; } }
		public override double CastDelay { get { return 2.5; } }
        public override int RequiredMana { get { return 33; } }
        public AncientCloneSpell(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info)
        {
        }

        public override bool CheckCast()
		{
			if ( Caster.Mounted )
			{
				Caster.SendLocalizedMessage( 1063132 ); // You cannot use this ability while mounted.
				return false;
			}
			else if ( (Caster.Followers + 1) > Caster.FollowersMax )
			{
				Caster.SendLocalizedMessage( 1063133 ); // You cannot summon a mirror image because you have too many followers.
				return false;
			}
			else if( TransformationSpellHelper.UnderTransformation( Caster, typeof( HorrificBeastSpell ) ) )
			{
				Caster.SendLocalizedMessage( 1061091 ); // You cannot cast that spell in this form.
				return false;
			}

			return base.CheckCast();
		}

		public override bool CheckDisturb( DisturbType type, bool firstCircle, bool resistable )
		{
			return false;
		}

		public override void OnBeginCast()
		{
			base.OnBeginCast();

			Caster.SendLocalizedMessage( 1063134 ); // You begin to summon a mirror image of yourself.
		}

		public override void OnCast()
		{
			if ( Caster.Mounted )
			{
				Caster.SendLocalizedMessage( 1063132 ); // You cannot use this ability while mounted.
			}
			else if ( (Caster.Followers + 1) > Caster.FollowersMax )
			{
				Caster.SendLocalizedMessage( 1063133 ); // You cannot summon a mirror image because you have too many followers.
			}
			else if( TransformationSpellHelper.UnderTransformation( Caster, typeof( HorrificBeastSpell ) ) )
			{
				Caster.SendLocalizedMessage( 1061091 ); // You cannot cast that spell in this form.
			}
			else if ( CheckSequence() )
			{
				Caster.FixedParticles( 0x376A, 1, 14, 0x13B5, EffectLayer.Waist );
				Caster.PlaySound( 0x511 );

				new Clone( Caster ).MoveToWorld( Caster.Location, Caster.Map );
			}

			FinishSequence();
		}
	}
}