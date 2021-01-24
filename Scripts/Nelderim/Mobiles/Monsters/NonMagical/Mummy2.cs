using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "resztki mumii" )]
	public class Mummy2 : BaseCreature
	{
		public override double DifficultyScalar{ get{ return 1.02; } }
		public override bool BleedImmune { get { return true; } }
		
		[Constructable]
		public Mummy2() : base( AIType.AI_Melee, FightMode.Closest, 8, 1, 0.4, 0.8 )
		{
			Name = "mumia";
			Body = 154;
			BaseSoundID = 471;
			Hue = 61;

			SetStr( 446, 470 );
			SetDex( 91, 120 );
			SetInt( 26, 40 );

			SetHits( 308, 322 );

			SetDamage( 23, 29 );

			SetDamageType( ResistanceType.Physical, 40 );
			SetDamageType( ResistanceType.Cold, 60 );

			SetResistance( ResistanceType.Physical, 45, 55 );
			SetResistance( ResistanceType.Fire, 10, 20 );
			SetResistance( ResistanceType.Cold, 50, 60 );
			SetResistance( ResistanceType.Poison, 120, 130 );
			SetResistance( ResistanceType.Energy, 20, 30 );

			SetSkill( SkillName.MagicResist, 15.1, 40.0 );
			SetSkill( SkillName.Tactics, 35.1, 50.0 );
			SetSkill( SkillName.Wrestling, 35.1, 50.0 );

			Fame = 4000;
			Karma = -4000;

			VirtualArmor = 50;

			PackItem( new Garlic( 5 ) );
			PackItem( new Bandage( 10 ) );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich );
			AddLoot( LootPack.Potions );
		}

		public override Poison PoisonImmune{ get{ return Poison.Deadly; } }

		public Mummy2( Serial serial ) : base( serial )
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
