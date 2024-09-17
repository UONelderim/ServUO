namespace Server.Items
{
	public class RekawyJingu : PlateArms
	{
		public override int LabelNumber { get { return 1065854; } } // Rekawy Jingu
		public override int InitMinHits => 255;
		public override int InitMaxHits => 255;

		public override int BasePhysicalResistance { get { return 10; } }
		public override int BaseFireResistance { get { return 15; } }
		public override int BaseColdResistance { get { return 7; } }
		public override int BasePoisonResistance { get { return 15; } }
		public override int BaseEnergyResistance { get { return 8; } }

		[Constructable]
		public RekawyJingu()
		{
			Hue = 1165;
			Attributes.BonusHits = 5;
			Attributes.LowerManaCost = 5;
			ArmorAttributes.MageArmor = 1;

			SkillBonuses.SetValues(0, SkillName.MagicResist, 10);
		}

		public RekawyJingu(Serial serial)
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
