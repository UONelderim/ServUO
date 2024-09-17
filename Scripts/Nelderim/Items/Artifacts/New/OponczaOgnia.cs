namespace Server.Items
{
	public class OponczaOgnia : Surcoat
	{
		public override int LabelNumber { get { return 1065743; } } // Oponcza Ognia
		public override int InitMinHits => 255;
		public override int InitMaxHits => 255;

		public override int BaseColdResistance { get { return -12; } }
		public override int BaseFireResistance { get { return 12; } }

		[Constructable]
		public OponczaOgnia()
		{
			Hue = 0x8F;
		}

		public OponczaOgnia(Serial serial)
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
