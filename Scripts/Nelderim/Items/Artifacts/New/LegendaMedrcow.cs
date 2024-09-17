namespace Server.Items
{
	public class LegendaMedrcow : StuddedChest
	{
		public override int LabelNumber { get { return 1065788; } } // Legenda Medrcow
		public override int InitMinHits => 255;
		public override int InitMaxHits => 255;

		public override int BasePhysicalResistance { get { return 3; } }
		public override int BaseColdResistance { get { return 9; } }
		public override int BaseFireResistance { get { return 9; } }
		public override int BaseEnergyResistance { get { return 15; } }
		public override int BasePoisonResistance { get { return 10; } }

		[Constructable]
		public LegendaMedrcow()
		{
			Hue = 1797;
			Attributes.BonusStr = 5;
			Attributes.BonusInt = 7;
			Attributes.BonusDex = 5;
			Attributes.Luck = 205;
			ArmorAttributes.MageArmor = 1;
		}

		public LegendaMedrcow(Serial serial)
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
