using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Items
{
	public class SkeletalKnightNest : MonsterNest
	{
		[Constructable]
		public SkeletalKnightNest() : base()
		{
			Name = "Kupka kości";
			Hue = 0;
			MaxCount = 6;
			RespawnTime = TimeSpan.FromSeconds( 30.0 );
			HitsMax = 1600;
			Hits = 1600;
			NestSpawnType = "Kościany rycerz";
			ItemID = 3793;
			LootLevel = 1;
		}

		public override void AddLoot()
		{
			MonsterNestLoot loot = new MonsterNestLoot( 6927, 0, this.LootLevel, "Rozwalone kości" );
			loot.MoveToWorld( this.Location, this.Map );
		}

		public SkeletalKnightNest( Serial serial ) : base( serial )
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