// 06.05.18 :: Migalart :: utworzenie

using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "zwloki zapomnianego" )]
	public class NZapomniany : BaseCreature
	{            
		[Constructable]
		public NZapomniany() : base( AIType.AI_Melee, FightMode.Closest, 9, 1, 0.2, 0.4 )
		{			
			Name = "zapomniany";
			Body = 267;
			BaseSoundID = 367;

			SetStr( 336, 365 );
			SetDex( 56, 75 );
			SetInt( 31, 55 );

			SetHits( 282, 299 );

			SetDamage( 8, 21 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 35, 40 );
			SetResistance( ResistanceType.Fire, 15, 25 );
			SetResistance( ResistanceType.Cold, 40, 50 );
			SetResistance( ResistanceType.Poison, 15, 25 );
			SetResistance( ResistanceType.Energy, 15, 25 );

			SetSkill( SkillName.MagicResist, 40.1, 55.0 );
			SetSkill( SkillName.Tactics, 50.1, 70.0 );
			SetSkill( SkillName.Wrestling, 50.1, 60.0 );

			Fame = 4000;
			Karma = -4000;

			VirtualArmor = 48;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Meager );
			AddLoot( LootPack.Average );
		}
        
        public override double AttackMasterChance { get { return 0.15; } }
		public override bool CanRummageCorpses{ get{ return true; } }
		public override int TreasureMapLevel{ get{ return 1; } }
		public override int Meat{ get{ return 4; } }

		public NZapomniany( Serial serial ) : base( serial )
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
