namespace Server.Items
{
	public class SpecialQuiver : ElvenQuiver
	{
		public override int LabelNumber => 1032657;
		public override bool CanFortify => false;

		public SpecialQuiver()
		{
			Hue = 0x4E7; // zmienic
		}

		public SpecialQuiver(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.WriteEncodedInt(0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadEncodedInt();
		}
	}
}
