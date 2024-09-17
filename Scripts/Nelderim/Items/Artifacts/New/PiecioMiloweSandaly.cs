namespace Server.Items
{
	public class PiecioMiloweSandaly : Sandals
	{
		public override int LabelNumber { get { return 1065740; } } // Pieciomilowe Sandaly
		public override int InitMinHits => 255;
		public override int InitMaxHits => 255;

		public override int BasePhysicalResistance { get { return 2; } }

		[Constructable]
		public PiecioMiloweSandaly()
		{
			Hue = 0x1F0;
			SkillBonuses.SetValues(0, SkillName.Begging, 10.0);
			Attributes.BonusDex = 5;
		}

		public PiecioMiloweSandaly(Serial serial)
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
