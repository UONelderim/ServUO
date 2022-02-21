#region References

using Server.Engines.Craft;

#endregion

namespace Server.Items
{
	public class BowFletchingPowderOfTemperament : SpecializedPowderOfTemperament
	{
		public override CraftSystem CraftSystem => DefBowFletching.CraftSystem;

		[Constructable]
		public BowFletchingPowderOfTemperament() : this(5)
		{
		}

		[Constructable]
		public BowFletchingPowderOfTemperament(int uses) : base(uses)
		{
		}

		public BowFletchingPowderOfTemperament(Serial serial) : base(serial)
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
