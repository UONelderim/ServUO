namespace Server.Items
{
	[FlipableAttribute(0x2B02, 0x2B03)]
	public class KolczanWStyluPolnocnym : BaseQuiver
	{
		[Constructable]
		public KolczanWStyluPolnocnym() : base(0x2B02)
		{
			WeightReduction = 40;
			LowerAmmoCost = 15;
			Capacity = 800;
			Name = "kołczan w stylu północnym";
			Hue = 1150;
		}

		public KolczanWStyluPolnocnym(Serial serial) : base(serial)
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
