namespace Server.Items
{
	public class KrwaweNieszczescie : ChainLegs
	{
		public override int InitMinHits { get { return 60; } }
		public override int InitMaxHits { get { return 60; } }

		public override int BasePhysicalResistance { get { return 18; } }
		public override int BaseFireResistance { get { return 6; } }
		public override int BaseColdResistance { get { return -8; } }
		public override int BasePoisonResistance { get { return 9; } }
		public override int BaseEnergyResistance { get { return 19; } }

		[Constructable]
		public KrwaweNieszczescie()
		{
			Hue = 38;
			Name = "Krwawe Nieszczescie";
			Attributes.BonusMana = 3;
			Attributes.RegenHits = 4;
			Attributes.RegenMana = 2;
		}

		public KrwaweNieszczescie(Serial serial) : base(serial)
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
