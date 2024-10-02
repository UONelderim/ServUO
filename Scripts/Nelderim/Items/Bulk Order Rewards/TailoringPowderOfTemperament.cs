#region References

using Server.Engines.Craft;

#endregion

namespace Server.Items
{
	public class TailoringPowderOfTemperament : SpecializedPowderOfTemperament
	{
		public override CraftSystem CraftSystem => DefTailoring.CraftSystem;

		[Constructable]
		public TailoringPowderOfTemperament() : this(1)
		{
		}

		[Constructable]
		public TailoringPowderOfTemperament(int uses) : base(uses)
		{
			Name = "Proszek wzmocnienia wyrobow krawieckich";
			Hue = 0x483;
		}

		public TailoringPowderOfTemperament(Serial serial) : base(serial)
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
