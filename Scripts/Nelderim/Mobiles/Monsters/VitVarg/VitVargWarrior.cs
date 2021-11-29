using System;
using Server;
using Server.Misc;
using Server.Items;

namespace Server.Mobiles
{
	public class VitVargWarrior : BaseCreature
	{

		[Constructable]
		public VitVargWarrior() : base( AIType.AI_Melee, FightMode.Closest, 12, 1, 0.2, 0.4 )
		{

			SpeechHue = Utility.RandomDyedHue();
			
			Title = "- Varg Wojownik";
			Hue = Race.RandomSkinHue();

			if ( this.Female = Utility.RandomBool() )
			{
				Body = 0x191;
				Name = NameList.RandomName( "female" );
				AddItem( new Kilt( Utility.RandomNeutralHue() ) );
			}
			else
			{
				Body = 0x190;
				Name = NameList.RandomName( "male" );
				AddItem( new ShortPants( Utility.RandomNeutralHue() ) );
			}
			
			SetStr( 250, 300 );
			SetDex( 60, 80 );
			SetInt( 40, 60 );
			
			SetHits( 290, 340 );

			SetDamage( 14, 16 );


			SetDamageType( ResistanceType.Physical, 60 );
			SetDamageType( ResistanceType.Cold, 40 );
			
			SetResistance( ResistanceType.Physical, 40, 50 );
			SetResistance( ResistanceType.Fire, 30, 45 );
			SetResistance( ResistanceType.Cold, 55, 65 );
			SetResistance( ResistanceType.Poison, 40, 55 );
			SetResistance( ResistanceType.Energy, 40, 55 );


			SetSkill( SkillName.Anatomy, 130.0 );
			SetSkill( SkillName.Fencing, 46.0, 77.5 );
			SetSkill( SkillName.Macing, 35.0, 57.5 );
			SetSkill( SkillName.MagicResist, 85.0, 95.0 );
			SetSkill( SkillName.Swords, 130.0 );
			SetSkill( SkillName.Tactics, 130.0 );
			SetSkill( SkillName.Lumberjacking, 130.0 );

			Fame = 5000;
			Karma = -5000;

			VirtualArmor = 45;
			
			LeatherCap Helm = new LeatherCap ();
			Helm.Hue = 856; 
			Helm.Movable = false;
			EquipItem( Helm ); 
			
			StuddedChest chest = new StuddedChest ();
			chest.Hue = 856;
			chest.Movable = false;
			EquipItem ( chest );
			
			StuddedGorget Gorget = new StuddedGorget ();
			Gorget.Hue = 856;
			Gorget.Movable = false;
			EquipItem( Gorget );
			
			StuddedGloves Gloves = new StuddedGloves ();
			Gloves.Hue = 856;
			Gloves.Movable = false;
			EquipItem( Gloves );
			
			StuddedLegs legs = new StuddedLegs ();
			legs.Hue = 856;
			legs.Movable = false;
			EquipItem ( legs );

			Boots Boot = new Boots ();
			Boot.Hue = 856;
			AddItem ( Boot );
			
			Cloak Cloa = new Cloak();
			Cloa.Hue = 288;
			Cloa.Movable = false;
			AddItem ( Cloa );
			
			BodySash sash = new BodySash();
			sash.Hue = 288;
			sash.Movable = false;
			EquipItem ( sash );

			VikingSword sword = new VikingSword ();
			sword.Hue = 781;
			sword.Movable = false;
			EquipItem ( sword );
			
			BronzeShield Shield = new BronzeShield ();
			Shield.Hue = 781;
			Shield.Movable = false;
			EquipItem ( Shield );

            SetWeaponAbility( WeaponAbility.CrushingBlow );
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


		public VitVargWarrior( Serial serial ) : base( serial )
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
