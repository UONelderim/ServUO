namespace Server.Items
{
	public class SzataHutum : FurCape
	{
		public override int LabelNumber { get { return 1065794; } } // Szata Hutum
		public override int InitMinHits => 255;
		public override int InitMaxHits => 255;

		[Constructable]
		public SzataHutum()
		{
			Hue = 1266;
			Attributes.BonusMana = 5;
			Attributes.LowerManaCost = 3;
		}

		public SzataHutum(Serial serial)
			: base(serial)
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
