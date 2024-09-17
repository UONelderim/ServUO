namespace Server.Items
{
	public class PrzysiegaTriamPergi : WoodlandBelt
	{
		public override int LabelNumber { get { return 1065741; } } // Przysiega Triam Pergi
		public override int InitMinHits => 255;
		public override int InitMaxHits => 255;

		[Constructable]
		public PrzysiegaTriamPergi()
		{
			Hue = 135;
			Attributes.Luck = 50;
			Attributes.LowerManaCost = 3;
		}

		public PrzysiegaTriamPergi(Serial serial)
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
