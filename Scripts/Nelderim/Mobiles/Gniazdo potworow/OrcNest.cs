#region References

using System;

#endregion

namespace Server.Items
{
	public class OrcNest : MonsterNest
	{
		[Constructable]
		public OrcNest()
		{
			Name = "Ognisko orków";
			Hue = 0;
			MaxCount = 15;
			RangeHome = 20;
			RespawnTime = TimeSpan.FromSeconds(10.0);
			HitsMax = 1600;
			Hits = 1600;
			NestSpawnType = "Ork";
			ItemID = 10749;
			LootLevel = 1;
		}

		public override void AddLoot()
		{
			MonsterNestLoot loot = new MonsterNestLoot(6927, 0, this.LootLevel, "Porozwalane śmieci");
			loot.MoveToWorld(this.Location, this.Map);
		}

		public OrcNest(Serial serial) : base(serial)
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
