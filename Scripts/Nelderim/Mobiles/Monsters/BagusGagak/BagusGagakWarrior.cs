using System;
using Server;
using Server.Misc;
using Server.Items;

namespace Server.Mobiles
{
	public class BagusGagakWarrior : BaseCreature
	{
		[Constructable]
		public BagusGagakWarrior() : base( AIType.AI_Melee, FightMode.Closest, 12, 1, 0.2, 0.4 )
		{

			SpeechHue = Utility.RandomDyedHue();
			
			Title = "- Gagak Wojownik";
			Hue = Race.RandomSkinHue();

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
			
			SetStr( 200, 240 );
			SetDex( 120, 140 );
			SetInt( 45, 60 );
			
			SetHits( 240, 280 );

			SetDamage( 11, 15 );


			SetDamageType( ResistanceType.Physical, 80 );
			SetDamageType( ResistanceType.Poison, 20 );
			
			SetResistance( ResistanceType.Physical, 35, 50 );
			SetResistance( ResistanceType.Fire, 40, 45 );
			SetResistance( ResistanceType.Cold, 40, 45 );
			SetResistance( ResistanceType.Poison, 50, 55 );
			SetResistance( ResistanceType.Energy, 40, 45 );


			SetSkill( SkillName.Anatomy, 100.0 );
			SetSkill( SkillName.Fencing, 50.0, 70.0 );
			SetSkill( SkillName.Macing, 50.0, 70.0 );
			SetSkill( SkillName.MagicResist, 90.0, 100.0 );
			SetSkill( SkillName.Swords, 100.0, 120.0 );
			SetSkill( SkillName.Tactics, 110.0 );

			Fame = 5000;
			Karma = -5000;

			VirtualArmor = 45;

            SetWeaponAbility( WeaponAbility.ArmorIgnore );

            LeatherCap Helm = new LeatherCap ();
			Helm.Hue = 2101; 
			Helm.Movable = false;
			EquipItem( Helm ); 
			
			StuddedChest chest = new StuddedChest ();
			chest.Hue = 2101;
			chest.Movable = false;
			EquipItem ( chest );
			
			StuddedGorget Gorget = new StuddedGorget ();
			Gorget.Hue = 2101;
			Gorget.Movable = false;
			EquipItem( Gorget );
			
			StuddedGloves Gloves = new StuddedGloves ();
			Gloves.Hue = 2101;
			Gloves.Movable = false;
			EquipItem( Gloves );
			
			StuddedLegs legs = new StuddedLegs ();
			legs.Hue = 2101;
			legs.Movable = false;
			EquipItem ( legs );

			Boots Boot = new Boots ();
			Boot.Hue = 262;
			AddItem ( Boot );
			
			Cloak Cloa = new Cloak();
			Cloa.Hue = 262;
			Cloa.Movable = false;
			AddItem ( Cloa );
			
			BodySash sash = new BodySash();
			sash.Hue = 262;
			sash.Movable = false;
			EquipItem ( sash );

			Katana sword = new Katana ();
			sword.Hue = 424;
			sword.Movable = false;
			EquipItem ( sword );
			
			MetalShield Shield = new MetalShield ();
			Shield.Hue = 424;
			Shield.Movable = false;
			EquipItem ( Shield );
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


		public BagusGagakWarrior( Serial serial ) : base( serial )
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
