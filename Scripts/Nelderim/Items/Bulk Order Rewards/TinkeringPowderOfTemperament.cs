using Server.Engines.Craft;

namespace Server.Items
{
	public class TinkeringPowderOfTemperament : SpecializedPowderOfTemperament
	{
		public override CraftSystem CraftSystem => DefTinkering.CraftSystem;

		[Constructable]
		public TinkeringPowderOfTemperament() : this(5)
		{
		}

		[Constructable]
		public TinkeringPowderOfTemperament(int uses) : base(uses)
		{
		}

		public TinkeringPowderOfTemperament(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}
}
