using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "zwloki lorda ettinow" )]
	public class EttinLord : BaseCreature
	{
		[Constructable]
		public EttinLord() : base( AIType.AI_Melee, FightMode.Closest, 12, 1, 0.2, 0.4 )
		{
			Name = "lord ettinow";
			Body = 18;
			BaseSoundID = 367;
			Hue = 1709;
			Kills = 20;

			SetStr( 436, 465 );
			SetDex( 86, 95 );
			SetInt( 31, 55 );

			SetHits( 282, 299 );

			SetDamage( 14, 21 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 55, 60 );
			SetResistance( ResistanceType.Fire, 25, 35 );
			SetResistance( ResistanceType.Cold, 40, 50 );
			SetResistance( ResistanceType.Poison, 25, 35 );
			SetResistance( ResistanceType.Energy, 25, 35 );

			SetSkill( SkillName.MagicResist, 50.1, 65.0 );
			SetSkill( SkillName.Tactics, 70.1, 90.0 );
			SetSkill( SkillName.Wrestling, 70.1, 90.0 );

			Fame = 3000;
			Karma = -3000;

			VirtualArmor = 48;

			PackItem( new FertileDirt( Utility.RandomMinMax( 1, 3 ) ) );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Average  );
			AddLoot( LootPack.Meager  );
            AddLoot( LootPack.Potions );
		}


        public override double AttackMasterChance { get { return 0.3; } }
		public override bool CanRummageCorpses{ get{ return true; } }
		public override int TreasureMapLevel{ get{ return 1; } }
		public override int Meat{ get{ return 4; } }

		public EttinLord( Serial serial ) : base( serial )
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
