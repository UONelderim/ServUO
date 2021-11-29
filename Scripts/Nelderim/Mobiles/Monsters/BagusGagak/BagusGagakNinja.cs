
using System;
using Server;
using Server.Misc;
using Server.Items;

namespace Server.Mobiles
{
	public class BagusGagakNinja : BaseCreature
	{
		[Constructable]
		public BagusGagakNinja() : base( AIType.AI_Melee, FightMode.Closest, 12, 1, 0.15, 0.3 )
		{

			SpeechHue = Utility.RandomDyedHue();
			
			Title = "- Gagak Skrytobojca";
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
			
			SetStr( 120, 140 );
			SetDex( 120, 160 );
			SetInt( 40, 60 );

			SetDamage( 10, 14 );


			SetDamageType( ResistanceType.Physical, 100 );
			
			SetResistance( ResistanceType.Physical, 35, 45 );
			SetResistance( ResistanceType.Fire, 30, 40 );
			SetResistance( ResistanceType.Cold, 30, 40 );
			SetResistance( ResistanceType.Poison, 50, 55 );
			SetResistance( ResistanceType.Energy, 40, 45 );


			SetSkill( SkillName.Anatomy, 120.0 );
			SetSkill( SkillName.Fencing, 110.0, 120.0 );
			SetSkill( SkillName.Poisoning, 90.0, 100.0 );
			SetSkill( SkillName.MagicResist, 83.5, 92.5 );
			SetSkill( SkillName.Swords, 120.0 );
			SetSkill( SkillName.Tactics, 100.0 );

			Fame = 5000;
			Karma = -5000;

			VirtualArmor = 40;

            SetWeaponAbility( WeaponAbility.ArmorPierce );

            LeatherNinjaHood Helm = new LeatherNinjaHood ();
			Helm.Hue = 2101;
			Helm.Movable = false;
			EquipItem( Helm ); 
			
			LeatherMempo Gorget = new LeatherMempo ();
			Gorget.Hue = 2101;
			Gorget.Movable = false;
			EquipItem( Gorget );
			
			LeatherNinjaJacket chest = new LeatherNinjaJacket ();
			chest.Hue = 2101;
			chest.Movable = false;
			EquipItem ( chest );		
	
			LeatherNinjaMitts Gloves = new LeatherNinjaMitts ();
			Gloves.Hue = 2101;
			Gloves.Movable = false;
			EquipItem( Gloves );
			
			LeatherNinjaPants legs = new LeatherNinjaPants ();
			legs.Hue = 2101;
			legs.Movable = false;
			EquipItem ( legs );

			Sandals Boot = new Sandals ();
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

			Sai sword = new Sai ();
			sword.Hue = 424;
			sword.Movable = false;
			EquipItem ( sword );
		}

        public override double WeaponAbilityChance => 0.3;

        public override void GenerateLoot()
		{
			AddLoot( LootPack.Average );
		}

		public override bool AlwaysMurderer{ get{ return true; } }
		public override bool ShowFameTitle{ get{ return true; } }
		public override bool CanRummageCorpses{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Greater; } }
		
		public override Poison HitPoison{ get{ return Poison.Greater; } }
		public override double HitPoisonChance{ get{ return 0.3; } }


		public BagusGagakNinja( Serial serial ) : base( serial )
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
