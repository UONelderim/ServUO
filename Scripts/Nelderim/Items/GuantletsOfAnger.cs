namespace Server.Items
{
	public class GuantletsOfAnger : PlateGloves //TODO Do wyjebania
	{
		public override int LabelNumber { get { return 1065833; } } // Rekawice Gniewu
		public override int InitMinHits { get { return 60; } }
		public override int InitMaxHits { get { return 60; } }

		public override int BasePhysicalResistance { get { return 14; } }
		public override int BaseFireResistance { get { return 4; } }
		public override int BaseColdResistance { get { return 15; } }
		public override int BasePoisonResistance { get { return 10; } }
		public override int BaseEnergyResistance { get { return 9; } }


		[Constructable]
		public GuantletsOfAnger()
		{
			Hue = 0x29b;

			Attributes.BonusHits = 8;
			Attributes.RegenHits = 2;
			Attributes.DefendChance = 10;
		}

		public GuantletsOfAnger(Serial serial) : base(serial)
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
