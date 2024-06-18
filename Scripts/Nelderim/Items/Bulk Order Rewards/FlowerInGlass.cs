namespace Server.Items
{
	[Furniture]

	public class FlowerInGlassA : Item
	{
		[Constructable]
		public FlowerInGlassA()
			: base(0xA8E8)
		{
			Weight = 20.0;
		}

		public FlowerInGlassA(Serial serial)
			: base(serial)
		{
		}

		public override int LabelNumber => 3060071;
		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}
}
