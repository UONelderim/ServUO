using System;
using Server;
using Server.Misc;
using Server.Items;

namespace Server.Mobiles
{
	public class BagusGagakLumberjack : BaseCreature
	{

		[Constructable]
		public BagusGagakLumberjack() : base( AIType.AI_Melee, FightMode.Closest, 12, 1, 0.2, 0.4 )
		{

			SpeechHue = Utility.RandomDyedHue();
			
			Title = "- Gagak Drwal";
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
			
			SetStr( 140, 160 );
			SetDex( 30, 50 );
			SetInt( 30, 50 );
			
			SetHits( 100, 120 );

			SetDamage( 10, 14 );


			SetDamageType( ResistanceType.Physical, 100 );
			
			SetResistance( ResistanceType.Physical, 25, 35 );
			SetResistance( ResistanceType.Fire, 30, 40 );
			SetResistance( ResistanceType.Cold, 30, 40 );
			SetResistance( ResistanceType.Poison, 30, 40 );
			SetResistance( ResistanceType.Energy, 45, 55 );


			SetSkill( SkillName.Anatomy, 100.0 );
			SetSkill( SkillName.Fencing, 50.0, 70.0 );
			SetSkill( SkillName.Swords, 90.0, 100.0 );
			SetSkill( SkillName.MagicResist, 60.0, 80.0 );
			SetSkill( SkillName.Tactics, 100.0 );
			SetSkill( SkillName.Lumberjacking, 100.0 );

			Fame = 2000;
			Karma = -2000;

			VirtualArmor = 40;
			
			FeatheredHat Helm = new FeatheredHat ();
			Helm.Hue = 2101; 
			Helm.Movable = false;
			EquipItem( Helm ); 
			
			LeatherChest chest = new LeatherChest ();
			chest.Hue = 2101;
			chest.Movable = false;
			EquipItem ( chest );
			
			LeatherLegs legs = new LeatherLegs ();
			legs.Hue = 2101;
			legs.Movable = false;
			EquipItem ( legs );

			Boots Boot = new Boots ();
			Boot.Hue = 262;
			AddItem ( Boot );
			
			BodySash sash = new BodySash();
			sash.Hue = 262;
			sash.Movable = false;
			EquipItem ( sash );

			Hatchet axe = new Hatchet ();
			axe.Hue = 424;
			axe.Movable = false;
			EquipItem ( axe );
			
		}
			
		public override void GenerateLoot()
		{
			AddLoot( LootPack.Meager );
		}

		public override bool AlwaysMurderer{ get{ return true; } }
		public override bool ShowFameTitle{ get{ return true; } }
		public override bool CanRummageCorpses{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Regular; } }


		public BagusGagakLumberjack( Serial serial ) : base( serial )
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
