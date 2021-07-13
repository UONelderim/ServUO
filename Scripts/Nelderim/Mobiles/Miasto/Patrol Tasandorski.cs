using System;
using Server;
using Server.Misc;
using Server.Items;

namespace Server.Mobiles
{
	public class PatrolTasandorski : BaseCreature
	{
        public override void AddWeaponAbilities()
        {
            WeaponAbilities.Add( WeaponAbility.ArmorIgnore, 0.3 );
        }

		[Constructable]
		public PatrolTasandorski() : base( AIType.AI_Melee, FightMode.Criminal, 12, 1, 0.2, 0.4 )
		{

			SpeechHue = Utility.RandomDyedHue();
			
			Title = "- Patrol Tasandorski";
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
			
			SetStr( 160, 200 );
			SetDex( 100, 120 );
			SetInt( 50, 65 );
			
			SetHits( 200, 240 );

			SetDamage( 14, 18 );


			SetDamageType( ResistanceType.Physical, 60 );
			SetDamageType( ResistanceType.Fire, 40 );
			
			SetResistance( ResistanceType.Physical, 35, 45 );
			SetResistance( ResistanceType.Fire, 55, 65 );
			SetResistance( ResistanceType.Cold, 40, 45 );
			SetResistance( ResistanceType.Poison, 35, 50 );
			SetResistance( ResistanceType.Energy, 35, 50 );


			SetSkill( SkillName.Anatomy, 100.0 );
			SetSkill( SkillName.Swords, 50.0, 70.0 );
			SetSkill( SkillName.MagicResist, 80.0, 90.0 );
			SetSkill( SkillName.Fencing, 90.0, 110.0);
			SetSkill( SkillName.Tactics, 100.0 );

			Fame = 5000;
			Karma = -5000;

			VirtualArmor = 40;
			
			NorseHelm Helm = new NorseHelm ();
			Helm.Hue = 0; 
			Helm.Movable = false;
			EquipItem( Helm ); 
			
			PlateChest chest = new PlateChest ();
			chest.Hue = 0;
			chest.Movable = false;
			EquipItem ( chest );
			
			PlateGorget Gorget = new PlateGorget ();
			Gorget.Hue = 0;
			Gorget.Movable = false;
			EquipItem( Gorget );
			
			PlateGloves Gloves = new PlateGloves  ();
			Gloves.Hue = 0;
			Gloves.Movable = false;
			EquipItem( Gloves );
			
			PlateLegs legs = new PlateLegs ();
			legs.Hue = 0;
			legs.Movable = false;
			EquipItem ( legs );

            PlateArms arms = new PlateArms ();
			arms.Hue = 0;
			arms.Movable = false;
			EquipItem ( arms );

			Boots Boot = new Boots ();
			Boot.Hue = 2894;
			AddItem ( Boot );
			
			Cloak Cloa = new Cloak();
			Cloa.Hue = 2894;
			Cloa.Movable = false;
			AddItem ( Cloa );
			
			BodySash sash = new BodySash();
			sash.Hue = 2894;
			sash.Movable = false;
			EquipItem ( sash );

			Spear sword = new Spear ();
			sword.Hue = 0;
			sword.Movable = false;
			EquipItem ( sword );
			
		}
			
		public override void GenerateLoot()
		{
			AddLoot( LootPack.Average );
		}

		
		public override bool ShowFameTitle{ get{ return true; } }
		public override bool CanRummageCorpses{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Regular; } }


		public PatrolTasandorski( Serial serial ) : base( serial )
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
