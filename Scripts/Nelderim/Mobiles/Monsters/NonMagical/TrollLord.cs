using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "zwloki lorda trolli" )]
	public class TrollLord : BaseCreature
	{
		[Constructable]
		public TrollLord () : base( AIType.AI_Melee, FightMode.Weakest, 12, 1, 0.2, 0.4 )
		{			
			Name = "lord trolli";
			Body = Utility.RandomList( 53, 54 );
			BaseSoundID = 461;
			Hue = 2306;
			Kills = 20;

			SetStr( 476, 605 );
			SetDex( 126, 165 );
			SetInt( 46, 70 );

			SetHits( 406, 623 );

			SetDamage( 15, 22 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 55, 65 );
			SetResistance( ResistanceType.Fire, 35, 55 );
			SetResistance( ResistanceType.Cold, 35, 55 );
			SetResistance( ResistanceType.Poison, 35, 55 );
			SetResistance( ResistanceType.Energy, 35, 55 );

			SetSkill( SkillName.MagicResist, 75.1, 90.0 );
			SetSkill( SkillName.Tactics, 80.1, 90.0 );
			SetSkill( SkillName.Wrestling, 80.1, 96.0 );

			Fame = 3500;
			Karma = -3500;

			VirtualArmor = 60;

		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich );
			AddLoot( LootPack.Average  );
			AddLoot( LootPack.Meager  );
		}

        public override double AttackMasterChance { get { return 0.4; } }
		public override int TreasureMapLevel{ get{ return 1; } }
		public override int Meat{ get{ return 2; } }
		public override int Hides{ get{ return 8; } }

		public TrollLord( Serial serial ) : base( serial )
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
