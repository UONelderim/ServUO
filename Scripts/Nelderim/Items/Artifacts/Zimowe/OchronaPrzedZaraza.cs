namespace Server.Items
{
	public class OchronaPrzedZaraza : StuddedMempo
	{
		public override int InitMinHits => 255;
		public override int InitMaxHits => 255;
		public override int BaseFireResistance { get { return 12; } }
		public override int BaseColdResistance { get { return 13; } }
		public override int BasePhysicalResistance { get { return 10; } }
		public override int BaseEnergyResistance { get { return 20; } }
		public override int BasePoisonResistance { get { return 10; } }

		[Constructable]
		public OchronaPrzedZaraza()
		{
			Hue = 1150;
			Attributes.CastSpeed = -1;
			Name = "Ochrona Przed Zaraza";
			Attributes.RegenMana = 5;
		}

		public OchronaPrzedZaraza(Serial serial)
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
