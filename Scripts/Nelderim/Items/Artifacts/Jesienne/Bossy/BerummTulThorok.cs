namespace Server.Items
{
	public class BerummTulThorok : PlateGorget
	{
		public override int InitMinHits => 255;
		public override int InitMaxHits => 255;
		public override int BasePhysicalResistance { get { return 35; } }
		public override int BaseFireResistance { get { return 15; } }
		public override int BaseColdResistance { get { return 5; } }
		public override int BasePoisonResistance { get { return 1; } }
		public override int BaseEnergyResistance { get { return 10; } }

		[Constructable]
		public BerummTulThorok()
		{
			Hue = 2872;
			Name = "berumm tul Thorok";
			Attributes.BonusHits = 10;
			Attributes.DefendChance = 15;
			Attributes.CastRecovery = -2;
		}

		public BerummTulThorok(Serial serial)
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
