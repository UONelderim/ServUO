namespace Server.Items
{
	public class KrokWCieniu : ElvenBoots
	{
		public override int InitMinHits => 255;
		public override int InitMaxHits => 255;
		public override int BaseFireResistance { get { return 0; } }
		public override int BaseColdResistance { get { return 2; } }
		public override int BasePhysicalResistance { get { return 0; } }
		public override int BaseEnergyResistance { get { return 0; } }
		public override int BasePoisonResistance { get { return 0; } }

		[Constructable]
		public KrokWCieniu()
		{
			Hue = 1;
			Name = "Krok w cieniu";
			SkillBonuses.SetValues(0, SkillName.DetectHidden, 5.0);
			Attributes.BonusStr = -2;
			Attributes.BonusDex = 2;
		}

		public KrokWCieniu(Serial serial)
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
