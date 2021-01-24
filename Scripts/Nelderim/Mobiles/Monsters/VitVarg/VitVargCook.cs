using System;
using Server;
using Server.Misc;
using Server.Items;

namespace Server.Mobiles
{
	public class VitVargCook : BaseCreature
	{

		[Constructable]
		public VitVargCook() : base( AIType.AI_Melee, FightMode.Closest, 12, 1, 0.15, 0.35 )
		{

			SpeechHue = Utility.RandomDyedHue();
			
			Title = "- Varg Kucharz";
			Hue = Utility.RandomSkinHue();

			if ( this.Female = Utility.RandomBool() )
			{
				Body = 0x191;
				Name = NameList.RandomName( "female" );
				AddItem( new FullApron( Utility.RandomNeutralHue() ) );
			}
			else
			{
				Body = 0x190;
				Name = NameList.RandomName( "male" );
				AddItem( new FullApron( Utility.RandomNeutralHue() ) );
			}
			
			SetStr( 120, 180 );
			SetDex( 100, 140 );
			SetInt( 40, 60 );

			SetDamage( 10, 12 );


			SetDamageType( ResistanceType.Physical, 100 );
			
			SetResistance( ResistanceType.Physical, 35, 50 );
			SetResistance( ResistanceType.Fire, 25, 40 );
			SetResistance( ResistanceType.Cold, 55, 60 );
			SetResistance( ResistanceType.Poison, 35, 45 );
			SetResistance( ResistanceType.Energy, 35, 45 );


			SetSkill( SkillName.Anatomy, 125.0 );
			SetSkill( SkillName.Fencing, 120.0, 140.0 );
			SetSkill( SkillName.MagicResist, 83.5, 92.5 );
			SetSkill( SkillName.Swords, 120.0 );
			SetSkill( SkillName.Tactics, 100.0 );

			Fame = 5000;
			Karma = -5000;

			VirtualArmor = 40;
			
			Bonnet Helm = new Bonnet ();
			Helm.Hue = 856;
			Helm.Movable = false;
			EquipItem( Helm ); 
			
			FancyShirt chest = new FancyShirt ();
			chest.Hue = 856;
			chest.Movable = false;
			EquipItem ( chest );		
	
			
			LeatherLegs Legs = new LeatherLegs ();
			Legs.Hue = 2106;
			Legs.Movable = false;
			EquipItem( Legs );

			Boots Boot = new Boots ();
			Boot.Hue = 1109;
			AddItem ( Boot );
			
			Cloak Cloa = new Cloak();
			Cloa.Hue = 288;
			Cloa.Movable = false;
			AddItem ( Cloa );
			
			BodySash sash = new BodySash();
			sash.Hue = 288;
			sash.Movable = false;
			EquipItem ( sash );

			Cleaver sword = new Cleaver ();
			sword.Hue = 781;
			sword.Movable = false;
			EquipItem ( sword );

            SetWeaponAbility( WeaponAbility.BleedAttack );
		}

        public override double WeaponAbilityChance => 0.2;

        private DateTime m_NextBomb;
		private int m_Thrown;

		public override void OnActionCombat()
		{
			Mobile combatant = Combatant as Mobile;

			if ( combatant == null || combatant.Deleted || combatant.Map != Map || !InRange( combatant, 12 ) || !CanBeHarmful( combatant ) || !InLOS( combatant ) )
				return;

			if ( DateTime.Now >= m_NextBomb )
			{
				ThrowBomb( combatant );

				m_Thrown++;

				if ( 0.75 >= Utility.RandomDouble() && (m_Thrown % 2) == 1 ) // 75% chance to quickly throw another bomb
					m_NextBomb = DateTime.Now + TimeSpan.FromSeconds( 3.0 );
				else
					m_NextBomb = DateTime.Now + TimeSpan.FromSeconds( 5.0 + (10.0 * Utility.RandomDouble()) ); // 5-15 seconds
			}
		}

		public void ThrowBomb( Mobile m )
		{
			DoHarmful( m );

			this.MovingParticles( m, 0x1C19, 1, 0, false, true, 0, 0, 9502, 6014, 0x11D, EffectLayer.Waist, 0 );

			new InternalTimer( m, this ).Start();
		}

		private class InternalTimer : Timer
		{
			private Mobile m_Mobile, m_From;

			public InternalTimer( Mobile m, Mobile from ) : base( TimeSpan.FromSeconds( 1.0 ) )
			{
				m_Mobile = m;
				m_From = from;
				Priority = TimerPriority.TwoFiftyMS;
			}

			protected override void OnTick()
			{
				m_Mobile.PlaySound( 0x11D );
				AOS.Damage( m_Mobile, m_From, Utility.RandomMinMax( 10, 20 ), 0, 100, 0, 0, 0 );
			}
		}


		
		public override void GenerateLoot()
		{
			AddLoot( LootPack.Average );
		}

		public override bool AlwaysMurderer{ get{ return true; } }
		public override bool ShowFameTitle{ get{ return true; } }
		public override bool CanRummageCorpses{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Greater; } }
		
		public VitVargCook( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}
