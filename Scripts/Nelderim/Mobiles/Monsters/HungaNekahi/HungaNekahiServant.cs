using System;
using Server;
using Server.Misc;
using Server.Items;

namespace Server.Mobiles
{
	public class HungaNekahiServant : BaseCreature
	{

		[Constructable]
		public HungaNekahiServant() : base( AIType.AI_Melee, FightMode.Closest, 12, 1, 0.2, 0.4 )
		{

			SpeechHue = Utility.RandomDyedHue();
			
			Title = "- Nekahi Sluga";
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
			
			SetStr( 120, 140 );
			SetDex( 40, 60 );
			SetInt( 30, 50 );
			
			SetHits( 130, 170 );

			SetDamage( 8, 12 );


			SetDamageType( ResistanceType.Physical, 100 );
			
			SetResistance( ResistanceType.Physical, 25, 35 );
			SetResistance( ResistanceType.Fire, 30, 40 );
			SetResistance( ResistanceType.Cold, 30, 40 );
			SetResistance( ResistanceType.Poison, 30, 40 );
			SetResistance( ResistanceType.Energy, 45, 55 );


			SetSkill( SkillName.Anatomy, 100.0 );
			SetSkill( SkillName.Fencing, 50.0, 70.0 );
			SetSkill( SkillName.Swords, 50.0, 70.0 );
			SetSkill( SkillName.MagicResist, 50.0, 70.0 );
			SetSkill( SkillName.Macing, 110.0 );
			SetSkill( SkillName.Tactics, 100.0 );
			SetSkill( SkillName.Lumberjacking, 100.0 );

			Fame = 2000;
			Karma = -2000;

			VirtualArmor = 40;
			
			StrawHat Helm = new StrawHat ();
			Helm.Hue = 556; 
			Helm.Movable = false;
			EquipItem( Helm ); 
			
			Tunic chest = new Tunic ();
			chest.Hue = 556;
			chest.Movable = false;
			EquipItem ( chest );
			
			LongPants legs = new LongPants ();
			legs.Hue = 556;
			legs.Movable = false;
			EquipItem ( legs );

			Boots Boot = new Boots ();
			Boot.Hue = 33;
			AddItem ( Boot );
			
			BodySash sash = new BodySash();
			sash.Hue = 48;
			sash.Movable = false;
			EquipItem ( sash );

			Club sword = new Club ();
			sword.Hue = 589;
			sword.Movable = false;
			EquipItem ( sword );
			
		}
			
		public override void GenerateLoot()
		{
			AddLoot( LootPack.Meager );
		}

		public override bool AlwaysMurderer{ get{ return true; } }
		public override bool ShowFameTitle{ get{ return true; } }
		public override bool CanRummageCorpses{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Regular; } }


		public HungaNekahiServant( Serial serial ) : base( serial )
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
