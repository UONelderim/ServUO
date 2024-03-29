namespace Server.Items
{
	public class OdbijajacyStrzaly : LeatherGorget
	{
		public override int LabelNumber { get { return 1065811; } } // Odbijajacy Strzaly
		public override int InitMinHits { get { return 60; } }
		public override int InitMaxHits { get { return 60; } }

		public override int BasePhysicalResistance { get { return 3; } }
		public override int BaseFireResistance { get { return 9; } }
		public override int BaseColdResistance { get { return 12; } }
		public override int BasePoisonResistance { get { return 13; } }
		public override int BaseEnergyResistance { get { return 8; } }

		[Constructable]
		public OdbijajacyStrzaly()
		{
			Hue = 0x951;
			Attributes.BonusInt = 5;
			Attributes.LowerManaCost = 6;
			Attributes.ReflectPhysical = 30;
		}

		public OdbijajacyStrzaly(Serial serial) : base(serial)
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
