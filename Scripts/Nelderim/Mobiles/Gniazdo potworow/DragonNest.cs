#region References

using System;

#endregion

namespace Server.Items
{
	public class DragonNest : MonsterNest
	{
		[Constructable]
		public DragonNest()
		{
			Name = "Gniazdo smocze";
			Hue = 32;
			MaxCount = 7;
			RespawnTime = TimeSpan.FromSeconds(45.0);
			HitsMax = 2600;
			Hits = 2600;
			NestSpawnType = "Smok";
			LootLevel = 7;
			RangeHome = 15;
		}

		public override void AddLoot()
		{
			MonsterNestLoot loot = new MonsterNestLoot(4971, 32, this.LootLevel, "Smocze jaja");
			loot.MoveToWorld(this.Location, this.Map);
		}

		public DragonNest(Serial serial) : base(serial)
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
