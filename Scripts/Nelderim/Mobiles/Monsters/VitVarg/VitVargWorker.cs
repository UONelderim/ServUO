using System;
using Server;
using Server.Misc;
using Server.Items;

namespace Server.Mobiles
{
	public class VitVargWorker : BaseCreature
	{
		[Constructable]
		public VitVargWorker() : base( AIType.AI_Melee, FightMode.Closest, 12, 1, 0.15, 0.3 )
		{

			SpeechHue = Utility.RandomDyedHue();
			
			Title = "- Varg Robotnik";
			Hue = Utility.RandomSkinHue();

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
				AddItem( new Kilt( Utility.RandomNeutralHue() ) );
			}
			
			SetStr( 50, 100 );
			SetDex( 50, 80 );
			SetInt( 30, 50 );

			SetDamage( 8, 12 );


			SetDamageType( ResistanceType.Physical, 100 );
			
			SetResistance( ResistanceType.Physical, 30, 35 );
			SetResistance( ResistanceType.Fire, 25, 30 );
			SetResistance( ResistanceType.Cold, 50, 55 );
			SetResistance( ResistanceType.Poison, 30, 35 );
			SetResistance( ResistanceType.Energy, 30, 35 );


			SetSkill( SkillName.Anatomy, 80.0 );
			SetSkill( SkillName.Fencing, 80.0, 100.0 );
			SetSkill( SkillName.Tactics, 100.0 );

			Fame = 2000;
			Karma = -2000;

			VirtualArmor = 30;
			
			FloppyHat Helm = new FloppyHat ();
			Helm.Hue = 856;
			Helm.Movable = false;
			EquipItem( Helm ); 
			
			Shirt chest = new Shirt ();
			chest.Hue = 856;
			chest.Movable = false;
			EquipItem ( chest );	

			LongPants legs = new LongPants ();
			legs.Hue = 856;
			legs.Movable = false;
			EquipItem ( legs );			
	
			Boots Boot = new Boots ();
			Boot.Hue = 1109;
			AddItem ( Boot );
			
			BodySash sash = new BodySash();
			sash.Hue = 288;
			sash.Movable = false;
			EquipItem ( sash );

			Pitchfork sword = new Pitchfork ();
			sword.Hue = 781;
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
		public override Poison PoisonImmune{ get{ return Poison.Lesser; } }
		
		public VitVargWorker( Serial serial ) : base( serial )
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
