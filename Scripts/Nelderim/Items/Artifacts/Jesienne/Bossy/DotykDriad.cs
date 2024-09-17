namespace Server.Items
{
	public class DotykDriad : HideGloves
	{
		public override int InitMinHits => 255;
		public override int InitMaxHits => 255;

		public override int BasePhysicalResistance { get { return -10; } }
		public override int BaseFireResistance { get { return 6; } }
		public override int BaseColdResistance { get { return 35; } }
		public override int BasePoisonResistance { get { return 11; } }
		public override int BaseEnergyResistance { get { return 15; } }

		[Constructable]
		public DotykDriad()
		{
			Hue = 1167;
			Name = "Dotyk Driad";
			Attributes.RegenHits = 2;
			Attributes.BonusHits = 5;
			Attributes.ReflectPhysical = 40;
			SkillBonuses.SetValues(0, SkillName.Anatomy, 5);
			SkillBonuses.SetValues(0, SkillName.Healing, 5);
		}

		public DotykDriad(Serial serial)
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
