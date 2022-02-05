using Server.Engines.Craft;

namespace Server.Items
{
	public class CarpentryPowderOfTemperament : SpecializedPowderOfTemperament
	{
		public override CraftSystem CraftSystem => DefCarpentry.CraftSystem;

		[Constructable]
		public CarpentryPowderOfTemperament() : this(5)
		{
		}

		[Constructable]
		public CarpentryPowderOfTemperament(int uses) : base(uses)
		{
		}

		public CarpentryPowderOfTemperament(Serial serial) : base(serial)
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
