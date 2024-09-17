namespace Server.Items
{
	public class OponczaMrozu : Surcoat
	{
		public override int LabelNumber { get { return 1065742; } } // Oponcza Mrozu
		public override int InitMinHits => 255;
		public override int InitMaxHits => 255;

		public override int BaseColdResistance { get { return 12; } }
		public override int BaseFireResistance { get { return -12; } }

		[Constructable]
		public OponczaMrozu()
		{
			Hue = 0x1EF;
		}

		public OponczaMrozu(Serial serial)
			: base(serial)
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
