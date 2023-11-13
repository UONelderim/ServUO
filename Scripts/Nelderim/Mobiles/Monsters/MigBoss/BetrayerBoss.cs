using System;
using Server;
using Server.Misc;
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

			Fame = 15000;
			Karma = -15000;

			VirtualArmor = 65;
			SpeechHue = Utility.RandomDyedHue();

			PackItem( new PowerCrystal() );
			//PackReg( 5 );
			//PackReg( 5 );

			if ( 0.02 > Utility.RandomDouble() )
				PackItem( new BlackthornWelcomeBook() );

			m_NextAbilityTime = DateTime.Now + TimeSpan.FromSeconds( Utility.RandomMinMax( 5, 30 ) );

			SetWeaponAbility(WeaponAbility.Bladeweave);
			SetWeaponAbility(WeaponAbility.DefenseMastery);
			SetWeaponAbility(WeaponAbility.Feint);
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

		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich );
			AddLoot( LootPack.Rich );
			AddLoot( LootPack.Gems, 1 );
		}
		
		//public override void AddWeaponAbilities()
  //      {
  //          WeaponAbilities.Add( WeaponAbility.Bladeweave, 0.4 );
  //          WeaponAbilities.Add( WeaponAbility.DefenseMastery, 0.222 );
  //          WeaponAbilities.Add( WeaponAbility.Feint, 0.222 );
  //      }

		public override bool AlwaysMurderer{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override int Meat{ get{ return 1; } }
		public override bool AutoDispel{ get{ return false; } }
		public override double DifficultyScalar { get { return 1.40; } }
		public override bool BardImmune{ get{ return false; } }
        public override double AttackMasterChance { get { return 0.15; } }
        public override double SwitchTargetChance { get { return 0.15; } }
		public override int TreasureMapLevel{ get{ return 5; } }

		public override void OnGaveMeleeAttack( Mobile defender )
		{
			base.OnGaveMeleeAttack( defender );

			if ( !m_Stunning && 0.3 > Utility.RandomDouble() )
			{
				m_Stunning = true;

				defender.Animate( 21, 6, 1, true, false, 0 );
				this.PlaySound( 0xEE );
				defender.LocalOverheadMessage( MessageType.Regular, 0x3B2, false, "You have been stunned by a colossal blow!" );

				BaseWeapon weapon = this.Weapon as BaseWeapon;
				if ( weapon != null )
					weapon.OnHit( this, defender );

				if ( defender.Alive )
				{
					defender.Frozen = true;
					Timer.DelayCall( TimeSpan.FromSeconds( 5.0 ), new TimerStateCallback( Recover_Callback ), defender );
				}
			}
		}

		private void Recover_Callback( object state )
		{
			Mobile defender = state as Mobile;

			if ( defender != null )
			{
				defender.Frozen = false;
				defender.Combatant = null;
				defender.LocalOverheadMessage( MessageType.Regular, 0x3B2, false, "You recover your senses." );
			}

			m_Stunning = false;
		}

		private DateTime m_NextAbilityTime;

		public override void OnActionCombat()
		{
			IDamageable combatant = Combatant;

			if ( DateTime.Now < m_NextAbilityTime || combatant == null || combatant.Deleted || combatant.Map != Map || !InRange( combatant, 3 ) || !CanBeHarmful( combatant ) || !InLOS( combatant ) )
				return;

			m_NextAbilityTime = DateTime.Now + TimeSpan.FromSeconds( Utility.RandomMinMax( 5, 30 ) );

			if ( Utility.RandomBool() )
			{
				this.FixedParticles( 0x376A, 9, 32, 0x2539, EffectLayer.LeftHand );
				this.PlaySound( 0x1DE );

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
