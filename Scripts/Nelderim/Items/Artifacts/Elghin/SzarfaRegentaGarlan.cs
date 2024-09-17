namespace Server.Items
{
	public class SzarfaRegentaGarlan : BodySash
	{
		public override int InitMinHits => 255;
		public override int InitMaxHits => 255;

		public override int BaseEnergyResistance { get { return -20; } }

		[Constructable]
		public SzarfaRegentaGarlan()
		{
			Hue = 2812;
			Name = "Szarfa Regenta Garlan";
			Attributes.BonusMana = 5;
			Attributes.BonusStam = 5;
			LootType = LootType.Cursed;
		}

		public SzarfaRegentaGarlan(Serial serial)
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
