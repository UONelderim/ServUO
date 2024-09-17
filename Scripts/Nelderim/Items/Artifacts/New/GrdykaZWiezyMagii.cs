namespace Server.Items
{
	public class GrdykaZWiezyMagii : PlateMempo
	{
		public override int LabelNumber { get { return 1065777; } } // Grdyka Z Wiezy Magii
		public override int InitMinHits => 255;
		public override int InitMaxHits => 255;

		public override int BaseColdResistance { get { return 5; } }
		public override int BaseEnergyResistance { get { return 10; } }
		public override int BasePhysicalResistance { get { return 8; } }
		public override int BasePoisonResistance { get { return 12; } }
		public override int BaseFireResistance { get { return 7; } }

		[Constructable]
		public GrdykaZWiezyMagii()
		{
			Hue = 1151;
			ArmorAttributes.MageArmor = 1;
			Attributes.BonusMana = 8;
			Attributes.EnhancePotions = 25;
			Attributes.LowerManaCost = 5;
			Attributes.LowerRegCost = 15;
		}

		public GrdykaZWiezyMagii(Serial serial)
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
