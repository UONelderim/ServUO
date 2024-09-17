namespace Server.Items
{
	public class OponczaEnergii : Surcoat
	{
		public override int LabelNumber { get { return 1065745; } } // Oponcza Energii
		public override int InitMinHits => 255;
		public override int InitMaxHits => 255;

		public override int BaseEnergyResistance { get { return 12; } }
		public override int BasePoisonResistance { get { return -12; } }

		[Constructable]
		public OponczaEnergii()
		{
			Hue = 0x62B;
		}

		public OponczaEnergii(Serial serial)
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
