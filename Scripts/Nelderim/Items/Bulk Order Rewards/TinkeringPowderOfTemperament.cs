#region References

using Server.Engines.Craft;

#endregion

namespace Server.Items
{
	public class TinkeringPowderOfTemperament : SpecializedPowderOfTemperament
	{
		public override CraftSystem CraftSystem => DefTinkering.CraftSystem;

		[Constructable]
		public TinkeringPowderOfTemperament() : this(1)
		{
		}

		[Constructable]
		public TinkeringPowderOfTemperament(int uses) : base(uses)
		{
			Name = "Proszek wzmocnienia wyrobow majstra";
			Hue = 0x455;
		}

		public TinkeringPowderOfTemperament(Serial serial) : base(serial)
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
