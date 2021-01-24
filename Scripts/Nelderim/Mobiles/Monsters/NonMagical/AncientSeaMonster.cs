using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "zwloki potwora morskiego" )]
	public class AncientSeaMonster : BaseCreature
	{
		public override double DifficultyScalar{ get{ return 1.10; } }
		[Constructable]
		public AncientSeaMonster() : base( AIType.AI_Melee, FightMode.Weakest, 12, 1, 0.2, 0.4 )
		{
			Name = "potwor morski";
			Body = 77; //95;
            Hue = 101;
			BaseSoundID = 447;

			SetStr( 275, 500 );
			SetDex( 100, 150 );
			SetInt( 20, 100 );

			SetHits( 250, 350 );

			SetDamage( 12, 24 );

			SetDamageType( ResistanceType.Physical, 70 );
			SetDamageType( ResistanceType.Energy, 30 );

			SetResistance( ResistanceType.Physical, 33, 57 );
			SetResistance( ResistanceType.Fire, 15, 55 );
			SetResistance( ResistanceType.Cold, 60, 70 );
			SetResistance( ResistanceType.Poison, 11, 33 );
			SetResistance( ResistanceType.Energy, 61, 70 );

			SetSkill( SkillName.MagicResist, 70.1, 90.0 );
			SetSkill( SkillName.Tactics, 77.1, 90.0 );
			SetSkill( SkillName.Wrestling, 70.1, 100.0 );

			Fame = 7500;
			Karma = -7500;

			VirtualArmor = 66;
			CanSwim = true;
			CantWalk = true;

			PackItem( new SulfurousAsh( 10 ) );
			PackItem( new BlackPearl( 20 ) );
			
			if( Utility.RandomDouble() < .20 )
				PackItem( new MessageInABottle() );

			PackItem( new SpecialFishingNet() );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Meager, 3 );
		}

		public override int Meat{ get{ return 2; } }
		public override int Hides{ get{ return 5; } }
		public override HideType HideType{ get{ return HideType.Barbed; } }
		public override int TreasureMapLevel{ get{ return 5; } }
		public override int Scales{ get{ return 9; } }
		public override ScaleType ScaleType{ get{ return ScaleType.Blue; } }

		public AncientSeaMonster( Serial serial ) : base( serial )
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
