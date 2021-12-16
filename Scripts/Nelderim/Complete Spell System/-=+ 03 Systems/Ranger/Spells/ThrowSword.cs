using System;
using Server.Targeting;
using Server.Network;
using Server.Mobiles;
using Server.Items;
using Server.Spells;

namespace Server.ACC.CSS.Systems.Ranger
{
	public class RangerThrowSwordSpell : RangerSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
		                                                "Rzut mieczem", "*wyciaga sztylet zza pazuchy i ciska nim w cel",
		                                                //SpellCircle.Fourth,
		                                                212,
		                                                9041,
		                                                Reagent.Nightshade,
		                                                CReagent.SpringWater,
		                                                Reagent.Bloodmoss
		                                               );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

		public override double CastDelay{ get{ return 3.0; } }
		public override int RequiredMana{ get{ return 25; } }
		public override double RequiredSkill{ get{ return 50; } }

		public RangerThrowSwordSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
		}

		public override bool CheckCast()
		{
			if ( !base.CheckCast( ) )
				return false;

			if ( GetSword() == null )
			{
				Caster.SendMessage( "Czy jestes pewien, ze, trzymasz jakis miecz?" );
				return false;
			}

			return true;
		}

		public override void OnCast()
		{
			Caster.Target = new InternalTarget( this );
		}

		public override bool DelayedDamage{ get{ return true; } }

		public void Target( Mobile m )
		{
			if ( !Caster.CanSee( m ) )
			{
				Caster.SendLocalizedMessage( 500237 ); // Target can not be seen.
			}
			else if ( CheckHSequence( m ) && CheckFizzle() )
			{
				if ( GetSword() != null )
				{
					Item sword = GetSword();
					BaseWeapon bw = (BaseWeapon)sword;

					int min = bw.MinDamage + 5;
					int max = bw.MaxDamage + 20;

					int phys = bw.AosElementDamages.Physical;
					int cold = bw.AosElementDamages.Cold;
					int fire = bw.AosElementDamages.Fire;
					int engy = bw.AosElementDamages.Energy;
					int pois = bw.AosElementDamages.Poison;

					if ( ( phys + cold + fire + engy + pois ) < 1 ){ phys = 100; }

					int damage = Utility.RandomMinMax( min, max );

					Effects.SendMovingEffect( Caster, m, sword.ItemID, 30, 10, false, false, sword.Hue-1, 0 );

					Caster.PlaySound( 0x5D2 );

				Caster.SendMessage( "" + min + "_" + max + "_" + damage + "_" + phys + "_" + fire + "_" + cold + "_" + pois + "_" + engy + "" );

					// Deal the damage
					AOS.Damage( m, Caster, damage, phys, fire, cold, pois, engy );
				}
			}

			FinishSequence();
		}

		public Item GetSword()
		{
			if ( Caster.FindItemOnLayer( Layer.OneHanded ) != null )
			{
				Item oneHand = Caster.FindItemOnLayer( Layer.OneHanded );
				if ( oneHand is BaseSword ){ return oneHand; }
			}

			if ( Caster.FindItemOnLayer( Layer.TwoHanded ) != null )
			{
				Item twoHand = Caster.FindItemOnLayer( Layer.TwoHanded );
				if ( twoHand is BaseSword ){ return twoHand; }
			}

			return null;
		}

		private class InternalTarget : Target
		{
			private RangerThrowSwordSpell m_Owner;

			public InternalTarget( RangerThrowSwordSpell owner ) : base( Core.ML ? 10 : 12, false, TargetFlags.Harmful )
			{
				m_Owner = owner;
			}

			protected override void OnTarget( Mobile from, object o )
			{
				if ( o is Mobile )
					m_Owner.Target( (Mobile)o );
			}

			protected override void OnTargetFinish( Mobile from )
			{
				m_Owner.FinishSequence();
			}
		}
	}
}
