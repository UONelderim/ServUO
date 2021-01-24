using System;
using Server;
using Server.Misc;
using Server.Items;

namespace Server.Mobiles
{
	public class HungaNekahiWarrior : BaseCreature
	{

		[Constructable]
		public HungaNekahiWarrior() : base( AIType.AI_Melee, FightMode.Closest, 12, 1, 0.2, 0.4 )
		{

			SpeechHue = Utility.RandomDyedHue();
			
			Title = "- Nekahi Wojownik";
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
			
			SetStr( 250, 300 );
			SetDex( 60, 80 );
			SetInt( 40, 60 );
			
			SetHits( 290, 340 );

			SetDamage( 14, 16 );


			SetDamageType( ResistanceType.Physical, 60 );
			SetDamageType( ResistanceType.Cold, 40 );
			
			SetResistance( ResistanceType.Physical, 35, 45 );
			SetResistance( ResistanceType.Fire, 30, 40 );
			SetResistance( ResistanceType.Cold, 30, 40 );
			SetResistance( ResistanceType.Poison, 35, 50 );
			SetResistance( ResistanceType.Energy, 45, 60 );


			SetSkill( SkillName.Anatomy, 100.0 );
			SetSkill( SkillName.Healing, 85.0 );
			SetSkill( SkillName.Swords, 80.0, 90.0 );
			SetSkill( SkillName.Macing, 35.0, 57.5 );
			SetSkill( SkillName.MagicResist, 85.0, 95.0 );
			SetSkill( SkillName.Fencing, 120.0 );
			SetSkill( SkillName.Tactics, 100.0 );
			SetSkill( SkillName.Lumberjacking, 100.0 );

			Fame = 5000;
			Karma = -5000;

			VirtualArmor = 45;
			
			Helmet Helm = new Helmet ();
			Helm.Hue = 556; 
			Helm.Movable = false;
			EquipItem( Helm ); 
			
			RingmailChest chest = new RingmailChest ();
			chest.Hue = 556;
			chest.Movable = false;
			EquipItem ( chest );
			
			PlateGorget Gorget = new PlateGorget ();
			Gorget.Hue = 556;
			Gorget.Movable = false;
			EquipItem( Gorget );
			
			StuddedArms Arms = new StuddedArms ();
			Arms.Hue = 556;
			Arms.Movable = false;
			EquipItem( Arms );
			
			RingmailGloves Gloves = new RingmailGloves ();
			Gloves.Hue = 556;
			Gloves.Movable = false;
			EquipItem( Gloves );
			
			RingmailLegs legs = new RingmailLegs ();
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

			WarFork sword = new WarFork ();
			sword.Hue = 589;
			sword.Movable = false;
			EquipItem ( sword );
			
			WoodenKiteShield Shield = new WoodenKiteShield ();
			Shield.Hue = 589;
			Shield.Movable = false;
			EquipItem ( Shield );

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


		public HungaNekahiWarrior( Serial serial ) : base( serial )
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
