using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "zwloki sluzu" )]
	public class BarakSlime : BaseCreature
	{
		[Constructable]
		public BarakSlime() : base( AIType.AI_Melee, FightMode.Closest, 12, 1, 0.2, 0.4 )
		{
			Name = "sluz";
			Body = 51;
			BaseSoundID = 456;

			Hue = Utility.RandomSlimeHue();

			SetStr( 22, 34 );
			SetDex( 16, 21 );
			SetInt( 16, 20 );

			SetHits( 15, 19 );

			SetDamage( 1, 5 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 5, 10 );
			SetResistance( ResistanceType.Poison, 10, 20 );

			SetSkill( SkillName.Poisoning, 30.1, 50.0 );
			SetSkill( SkillName.MagicResist, 15.1, 20.0 );
			SetSkill( SkillName.Tactics, 19.3, 34.0 );
			SetSkill( SkillName.Wrestling, 19.3, 34.0 );

			Fame = 300;
			Karma = -300;

			VirtualArmor = 8;

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 23.1;
		}

		/*public override void GenerateLoot()
		{
			AddLoot( LootPack.Poor );
		}*/

		public override Poison PoisonImmune{ get{ return Poison.Lesser; } }
		public override Poison HitPoison{ get{ return Poison.Lesser; } }
		public override FoodType FavoriteFood{ get{ return FoodType.Meat | FoodType.Fish | FoodType.FruitsAndVegies | FoodType.GrainsAndHay | FoodType.Eggs; } }

		public BarakSlime( Serial serial ) : base( serial )
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
