namespace Server.Items
{
	[FlipableAttribute(0x2B02, 0x2B03)]
	public class KolczanWStyluZachodnim : BaseQuiver
	{
		[Constructable]
		public KolczanWStyluZachodnim() : base(0x2B02)
		{
			WeightReduction = 50;
			LowerAmmoCost = 20;
			Capacity = 1000;
			Name = "kołczan w stylu zachodnim";
			Hue = 1567;
		}

		public KolczanWStyluZachodnim(Serial serial) : base(serial)
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
