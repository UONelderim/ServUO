namespace Server.Items
{
	public class WiernyPrzysiedze : PlateGorget
	{
		public override int LabelNumber { get { return 1065762; } } // Wierny Przysiedze
		public override int InitMinHits => 255;
		public override int InitMaxHits => 255;

		public override int BaseColdResistance { get { return 8; } }
		public override int BaseEnergyResistance { get { return 8; } }
		public override int BasePhysicalResistance { get { return 10; } }
		public override int BasePoisonResistance { get { return 13; } }
		public override int BaseFireResistance { get { return 10; } }

		[Constructable]
		public WiernyPrzysiedze()
		{
			Hue = 1388;
			ArmorAttributes.MageArmor = 1;
			Attributes.AttackChance = 10;
			Attributes.BonusHits = 10;
			Attributes.LowerManaCost = 5;
		}

		public WiernyPrzysiedze(Serial serial)
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
