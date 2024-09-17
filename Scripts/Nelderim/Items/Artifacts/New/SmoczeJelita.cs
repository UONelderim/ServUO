namespace Server.Items
{
	public class SmoczeJelita : LeatherLegs
	{
		public override int InitMinHits => 255;
		public override int InitMaxHits => 255;

		public override int BaseColdResistance { get { return 8; } }
		public override int BaseEnergyResistance { get { return 14; } }
		public override int BasePhysicalResistance { get { return 7; } }
		public override int BasePoisonResistance { get { return 6; } }
		public override int BaseFireResistance { get { return 15; } }

		[Constructable]
		public SmoczeJelita()
		{
			Name = "Nogawice Ze Smoczej Skóry";
			Hue = 1000;
			Attributes.Luck = 150;
			Attributes.RegenMana = 2;
			Attributes.LowerRegCost = 15;
			Attributes.BonusMana = 5;
		}

		public SmoczeJelita(Serial serial)
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
