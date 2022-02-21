#region References

using System;

#endregion

namespace Server.Items
{
	public class RatNest : MonsterNest
	{
		[Constructable]
		public RatNest()
		{
			Name = "Gniazdo szczurów";
			Hue = 0;
			MaxCount = 25;
			RangeHome = 20;
			RespawnTime = TimeSpan.FromSeconds(2.0);
			HitsMax = 1600;
			Hits = 1600;
			NestSpawnType = "Ogromny Szczur";
			ItemID = 7090;
			LootLevel = 1;
		}

		public override void AddLoot()
		{
			MonsterNestLoot loot = new MonsterNestLoot(7100, 0, this.LootLevel, "Zniszczone gniazdo");
			loot.MoveToWorld(this.Location, this.Map);
		}

		public RatNest(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}
}
