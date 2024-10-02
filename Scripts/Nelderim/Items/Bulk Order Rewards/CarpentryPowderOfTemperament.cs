#region References

using Server.Engines.Craft;

#endregion

namespace Server.Items
{
	public class CarpentryPowderOfTemperament : SpecializedPowderOfTemperament
	{
		public override CraftSystem CraftSystem => DefCarpentry.CraftSystem;

		[Constructable]
		public CarpentryPowderOfTemperament() : this(1)
		{
		}

		[Constructable]
		public CarpentryPowderOfTemperament(int uses) : base(uses)
		{
			Name = "Proszek wzmocnienia wyrobow stolarskich";
			Hue = 0x5E8;
		}

		public CarpentryPowderOfTemperament(Serial serial) : base(serial)
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
