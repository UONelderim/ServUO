using System;
using Server;
using Server.Misc;
using Server.Items;

namespace Server.Mobiles
{
	public class HungaNekahiPirate : BaseCreature
	{

		[Constructable]
		public HungaNekahiPirate() : base( AIType.AI_Melee, FightMode.Closest, 12, 1, 0.2, 0.4 )
		{

			SpeechHue = Utility.RandomDyedHue();
			
			Title = "- Nekahi Pirat";
			Hue = Utility.RandomSkinHue();

			if ( this.Female = Utility.RandomBool() )
			{
				Body = 0x191;
				Name = NameList.RandomName( "female" );
			}
			else
			{
				Body = 0x190;
				Name = NameList.RandomName( "male" );
			}
			
			SetStr( 140, 180 );
			SetDex( 120, 140 );
			SetInt( 50, 65 );
			
			SetHits( 200, 220 );

			SetDamage( 12, 17 );


			SetDamageType( ResistanceType.Physical, 70 );
			SetDamageType( ResistanceType.Energy, 30 );
			
			SetResistance( ResistanceType.Physical, 35, 45 );
			SetResistance( ResistanceType.Fire, 35, 45 );
			SetResistance( ResistanceType.Cold, 35, 45 );
			SetResistance( ResistanceType.Poison, 35, 45 );
			SetResistance( ResistanceType.Energy, 40, 50 );


			SetSkill( SkillName.Anatomy, 100.0 );
			SetSkill( SkillName.Swords, 80.0, 100.0 );
			SetSkill( SkillName.MagicResist, 80.0, 90.0 );
			SetSkill( SkillName.Fencing, 60.0, 90.0);
			SetSkill( SkillName.Tactics, 100.0 );

			Fame = 5000;
			Karma = -5000;

			VirtualArmor = 38;
			
			FeatheredHat Helm = new FeatheredHat ();
			Helm.Hue = 556; 
			Helm.Movable = false;
			EquipItem( Helm ); 
			
			Doublet chest = new Doublet ();
			chest.Hue = 556;
			chest.Movable = false;
			EquipItem ( chest );
			
			LeatherGloves Gloves = new LeatherGloves ();
			Gloves.Hue = 556;
			Gloves.Movable = false;
			EquipItem( Gloves );
			
			LeatherLegs legs = new LeatherLegs ();
			legs.Hue = 556;
			legs.Movable = false;
			EquipItem ( legs );

			Boots Boot = new Boots ();
			Boot.Hue = 33;
			AddItem ( Boot );
			
			Cloak Cloa = new Cloak();
			Cloa.Hue = 33;
			Cloa.Movable = false;
			AddItem ( Cloa );
			
			BodySash sash = new BodySash();
			sash.Hue = 33;
			sash.Movable = false;
			EquipItem ( sash );

			Cutlass sword = new Cutlass ();
			sword.Hue = 589;
			sword.Movable = false;
			EquipItem ( sword );
			
			Buckler shield = new Buckler ();
			shield.Hue = 589;
			shield.Movable = false;
			EquipItem ( shield );

            SetWeaponAbility( WeaponAbility.BleedAttack );
		}

        public override double WeaponAbilityChance => 0.3;

        public override void GenerateLoot()
		{
			AddLoot( LootPack.Average );
		}

		public override bool AlwaysMurderer{ get{ return true; } }
		public override bool ShowFameTitle{ get{ return true; } }
		public override bool CanRummageCorpses{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Regular; } }


		public HungaNekahiPirate( Serial serial ) : base( serial )
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
