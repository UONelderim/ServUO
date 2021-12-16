using System;
using System.Collections;
using Server.Targeting;
using Server.Network;
using Server.Mobiles;
using Server.Items;
using Server.Spells;

namespace Server.ACC.CSS.Systems.Ranger
{
	public class RangerTrialByFireSpell : RangerSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
		                                                "Magiczne ziola", "*wyciaga woreczek magiczych ziol i zaczyna je rzuc",
		                                                //SpellCircle.Third,
		                                                212,
		                                                9041,
														Reagent.Bloodmoss
		                                               );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; }
        }


		public override double RequiredSkill{ get{ return 85.0; } }
		public override int RequiredMana{ get{ return 19; } }

		public static void Initialize()
		{
			RangerPlayerEvent.HitByWeapon += new RangerPlayerEvent.OnWeaponHit( InternalCallback );
		}

		public RangerTrialByFireSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
			                    if (this.Scroll != null)
                        Scroll.Consume();
		}

		public override bool CheckCast()
		{
			if ( !base.CheckCast() )
				return false;

			if ( !Caster.CanBeginAction( typeof(RangerTrialByFireSpell) ))
			{
				Caster.SendLocalizedMessage( 501775 ); // This spell is already in effect
				return false;
			}

			return true;
		}

		public override void OnCast()
		{
			if ( CheckSequence() )
			{
				Caster.SendMessage( "Twe ciało zamarza." );
				Caster.BeginAction( typeof( RangerTrialByFireSpell ) );

				Caster.FixedParticles( 0x3709, 10, 30, 5052, 0x480, 0, EffectLayer.LeftFoot );
				Caster.PlaySound( 0x208 );

				DateTime Expire = DateTime.Now + TimeSpan.FromMinutes( Caster.Skills[SkillName.Tactics].Value / 50.0 );
				new InternalTimer( Caster, Expire ).Start();

			}
			FinishSequence();
		}

		private static void InternalCallback( Mobile attacker, Mobile defender, int damage, WeaponAbility a )
		{
			if ( !defender.CanBeginAction( typeof( RangerTrialByFireSpell ) ) && Utility.RandomBool() )
			{
				defender.DoHarmful( attacker );

				double scale = 1.0;

				scale += defender.Skills[SkillName.Tactics].Value * 0.001;

				if ( defender.Player )
				{
					scale += defender.Int * 0.001;
					scale += AosAttributes.GetValue( defender, AosAttribute.WeaponDamage ) * 0.01;
				}

				int baseDamage = 6 + (int)(defender.Skills[SkillName.Tactics].Value / 5.0);

				double firedmg = Utility.RandomMinMax( baseDamage, baseDamage + 3 );

				firedmg *= scale;

				SpellHelper.Damage( TimeSpan.Zero, attacker, defender, firedmg, 0, 0, 100, 0, 0 );

				attacker.FixedParticles( 0x3709, 10, 30, 5052, 0x480, 0, EffectLayer.LeftFoot );
				attacker.PlaySound( 0x208 );
			}
		}

		private class InternalTimer : Timer
		{
			private Mobile Source;
			private DateTime Expire;

			public InternalTimer( Mobile from, DateTime end ) : base( TimeSpan.Zero, TimeSpan.FromSeconds( 0.1 ) )
			{
				Source = from;
				Expire = end;
			}

			protected override void OnTick()
			{
				if ( DateTime.Now >= Expire || !Source.CheckAlive() )
				{
					Source.EndAction( typeof( RangerTrialByFireSpell ) );
					Stop();
					Source.SendMessage( "Moc liści wygasa." );
				}
			}
		}
	}
}
