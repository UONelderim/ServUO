namespace Server.Items
{
	public class RycerzeWojny : BronzeShield
	{
		public override int LabelNumber { get { return 1065816; } } // Rycerze Wojny
		public override int InitMinHits => 255;
		public override int InitMaxHits => 255;

		public override int BasePhysicalResistance { get { return 6; } }
		public override int BaseFireResistance { get { return 4; } }
		public override int BaseColdResistance { get { return 4; } }
		public override int BasePoisonResistance { get { return 4; } }
		public override int BaseEnergyResistance { get { return 4; } }

		[Constructable]
		public RycerzeWojny()
		{
			Hue = 0x620;
			Attributes.DefendChance = 10;
			Attributes.AttackChance = 10;
			Attributes.ReflectPhysical = 40;
		}

		public RycerzeWojny(Serial serial) : base(serial)
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
