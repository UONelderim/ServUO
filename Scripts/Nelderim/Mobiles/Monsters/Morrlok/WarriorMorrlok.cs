using System;
using Server;
using Server.Misc;
using Server.Items;

namespace Server.Mobiles
{
	public class WojownikMorrlok : BaseCreature
	{
		public override double DifficultyScalar{ get{ return 1.05; } }

		[Constructable]
		public WojownikMorrlok() : base( AIType.AI_Melee, FightMode.Closest, 12, 1, 0.2, 0.4 )
		{

			SpeechHue = Utility.RandomDyedHue();
			
			Title = "- wojownik";
			Hue = Utility.RandomSkinHue();

			if ( this.Female = Utility.RandomBool() )
			{
				Body = 0x191;
				Name = NameList.RandomName( "female" );
				AddItem( new Skirt( Utility.RandomNeutralHue() ) );
			}
			else
			{
				Body = 0x190;
				Name = NameList.RandomName( "male" );
				AddItem( new ShortPants( Utility.RandomNeutralHue() ) );
			}
			
			SetStr( 186, 260 );
			SetDex( 45, 65 );
			SetInt( 15, 40 );

			SetDamage( 12, 14 );


			SetDamageType( ResistanceType.Physical, 100 );
			
			SetResistance( ResistanceType.Physical, 35, 45 );
			SetResistance( ResistanceType.Fire, 25, 30 );
			SetResistance( ResistanceType.Cold, 25, 30 );
			SetResistance( ResistanceType.Poison, 10, 20 );
			SetResistance( ResistanceType.Energy, 10, 20 );


			SetSkill( SkillName.Anatomy, 125.0 );
			SetSkill( SkillName.Fencing, 46.0, 77.5 );
			SetSkill( SkillName.Macing, 35.0, 57.5 );
			SetSkill( SkillName.Poisoning, 60.0, 82.5 );
			SetSkill( SkillName.MagicResist, 83.5, 92.5 );
			SetSkill( SkillName.Swords, 125.0 );
			SetSkill( SkillName.Tactics, 125.0 );
			SetSkill( SkillName.Lumberjacking, 125.0 );

			Fame = 5000;
			Karma = -5000;

			VirtualArmor = 40;
			
			NorseHelm Helm = new NorseHelm ();
			Helm.Hue = 2106;
			Helm.Movable = false;
			EquipItem( Helm ); 
			
			ChainChest chest = new ChainChest ();
			chest.Hue = 2106;
			chest.Movable = false;
			EquipItem ( chest );
			
			PlateGorget Gorget = new PlateGorget ();
			Gorget.Hue = 2106;
			Gorget.Movable = false;
			EquipItem( Gorget );
			
			RingmailGloves Gloves = new RingmailGloves ();
			Gloves.Hue = 2106;
			Gloves.Movable = false;
			EquipItem( Gloves );
			
			RingmailLegs legs = new RingmailLegs ();
			legs.Hue = 2106;
			legs.Movable = false;
			EquipItem ( legs );


			Boots Boot = new Boots ();
			Boot.Hue = 1109;
			AddItem ( Boot );
			
			Cloak Cloa = new Cloak();
			Cloa.Hue = 1146;
			AddItem ( Cloa );
			
			Robe Rob = new Robe ();
			Rob.Hue = 1146;
			AddItem ( Rob );


			BladedStaff sword = new BladedStaff ();
			sword.Hue = 2106;
			sword.Movable = false;
			EquipItem ( sword );

            SetWeaponAbility( WeaponAbility.Dismount );
		}
			
		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich );
		}

		public override bool AlwaysMurderer{ get{ return true; } }
		public override bool ShowFameTitle{ get{ return true; } }
		public override bool CanRummageCorpses{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Regular; } }


		public WojownikMorrlok( Serial serial ) : base( serial )
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
