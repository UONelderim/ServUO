namespace Server.Items
{
	public class KoszulaZPajeczychNici : Shirt
	{
		public override int InitMinHits { get { return 60; } }
		public override int InitMaxHits { get { return 60; } }

		public override int BaseColdResistance { get { return 1; } }

		[Constructable]
		public KoszulaZPajeczychNici()
		{
			Hue = 2523;
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
