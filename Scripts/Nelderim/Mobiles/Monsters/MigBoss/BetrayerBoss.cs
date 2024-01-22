using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
	[CorpseName( "zwloki zdrajcy pana" )]
	public class BetrayerBoss : BaseCreature
	{
		private bool m_Stunning;

		[Constructable]
		public BetrayerBoss() : base( AIType.AI_Mage, FightMode.Closest, 12, 1, 0.2, 0.4 )
		{
			Name = "zdrajca Pana";
			Body = 767;
			Hue = 2412;


			SetStr( 1500, 1600 );
			SetDex( 120, 130 );
			SetInt( 600, 800 );

			SetHits( 22000 );

			SetDamage( 18, 25 );

			SetDamageType( ResistanceType.Poison, 20 );
			SetDamageType( ResistanceType.Cold, 80 );
			SetDamageType( ResistanceType.Physical, 0 );

			SetResistance( ResistanceType.Physical, 80 );
			SetResistance( ResistanceType.Fire, 65 );
			SetResistance( ResistanceType.Cold, 70 );
			SetResistance( ResistanceType.Poison, 50 );
			SetResistance( ResistanceType.Energy, 30 );

			SetSkill( SkillName.EvalInt, 110.1, 120.2 );
			SetSkill( SkillName.Magery, 155.1, 160.0 );
			SetSkill( SkillName.MagicResist, 110.1, 120.0 );
			SetSkill( SkillName.Tactics, 110.1, 120.0 );
			SetSkill( SkillName.Wrestling, 70.1, 80.0 );
			SetSkill( SkillName.DetectHidden, 90.1, 120.5 );
			SetSkill( SkillName.Bushido, 90.1, 120.5 );
            SetSkill( SkillName.Focus, 20.2, 30.0 );

			SpeechHue = Utility.RandomDyedHue();

			PackItem( new PowerCrystal() );

			if ( 0.02 > Utility.RandomDouble() )
				PackItem( new BlackthornWelcomeBook() );

			_NextAbilityTime = DateTime.Now + TimeSpan.FromSeconds( Utility.RandomMinMax( 5, 30 ) );
		}
		
		public override void OnDeath( Container c )
		{
			base.OnDeath( c );

			ArtifactHelper.ArtifactDistribution(this);
		}

		public override int GetDeathSound()
		{
			return 0x423;
		}

		public override int GetAttackSound()
		{
			return 0x23B;
		}

		public override int GetHurtSound()
		{
			return 0x140;
		}

		public override bool AlwaysMurderer => true;
		public override Poison PoisonImmune => Poison.Lethal;
		public override int Meat => 1;
		public override bool AutoDispel => false;
		public override double DifficultyScalar => 1.40;
		public override bool BardImmune => false;
		public override double AttackMasterChance => 0.15;
		public override double SwitchTargetChance => 0.15;
		public override int TreasureMapLevel => 5;
		public override double WeaponAbilityChance => 0.8;

		public override void OnGaveMeleeAttack( Mobile defender )
		{
			base.OnGaveMeleeAttack( defender );

			if ( !m_Stunning && 0.3 > Utility.RandomDouble() )
			{
				m_Stunning = true;

				defender.Animate( 21, 6, 1, true, false, 0 );
				PlaySound( 0xEE );
				defender.LocalOverheadMessage( MessageType.Regular, 0x3B2, false, "You have been stunned by a colossal blow!" );

				if ( Weapon is BaseWeapon weapon )
					weapon.OnHit( this, defender );

				if ( defender.Alive )
				{
					defender.Frozen = true;
					Timer.DelayCall( TimeSpan.FromSeconds( 5.0 ), () => Recover_Callback(defender));
				}
			}
		}

		private void Recover_Callback( Mobile defender )
		{
			defender.Frozen = false;
			defender.Combatant = null;
			defender.LocalOverheadMessage( MessageType.Regular, 0x3B2, false, "You recover your senses." );

			m_Stunning = false;
		}

		private DateTime _NextAbilityTime;

		public override void OnActionCombat()
		{
			if ( DateTime.Now < _NextAbilityTime || Combatant == null || Combatant.Deleted || Combatant.Map != Map || !InRange( Combatant, 3 ) || !CanBeHarmful( Combatant ) || !InLOS( Combatant ) )
				return;

			_NextAbilityTime = DateTime.Now + TimeSpan.FromSeconds( Utility.RandomMinMax( 5, 30 ) );

			if ( Utility.RandomBool() )
			{
				FixedParticles( 0x376A, 9, 32, 0x2539, EffectLayer.LeftHand );
				PlaySound( 0x1DE );

				IPooledEnumerable eable = GetMobilesInRange( 2 );
				foreach ( Mobile m in eable )
				{
					if ( m != this && IsEnemy( m ) )
					{
						m.ApplyPoison( this, Poison.Deadly );
					}
				}
				eable.Free();
			}
		}

		public BetrayerBoss( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}
