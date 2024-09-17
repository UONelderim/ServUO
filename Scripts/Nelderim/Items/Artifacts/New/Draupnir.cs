namespace Server.Items
{
	public class Draupnir : GoldRing
	{
		public override int LabelNumber { get { return 1065780; } } // Draupnir
		public override int InitMinHits => 255;
		public override int InitMaxHits => 255;

		[Constructable]
		public Draupnir()
		{
			Hue = 1170;
			Attributes.LowerRegCost = 10;
			Attributes.LowerManaCost = 8;
			Attributes.CastRecovery = 3;
			Attributes.BonusInt = 8;
		}

		public Draupnir(Serial serial)
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
