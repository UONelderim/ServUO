namespace Server.Items
{
	public class KoszulaZPajeczychNici : Shirt
	{
		public override int InitMinHits => 255;
		public override int InitMaxHits => 255;

		public override int BaseColdResistance { get { return 1; } }

		[Constructable]
		public KoszulaZPajeczychNici()
		{
			Hue = 2250;
			Name = "Koszula z pajeczych nici";
			Attributes.BonusHits = 1;
			LootType = LootType.Cursed;
		}

		public KoszulaZPajeczychNici(Serial serial)
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
