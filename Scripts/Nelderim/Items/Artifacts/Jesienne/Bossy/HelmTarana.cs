namespace Server.Items
{
	public class HelmTarana : DecorativePlateKabuto
	{
		public override int InitMinHits => 255;
		public override int InitMaxHits => 255;

		public override int BasePhysicalResistance { get { return 35; } }
		public override int BaseFireResistance { get { return 30; } }
		public override int BaseColdResistance { get { return 3; } }
		public override int BasePoisonResistance { get { return -30; } }
		public override int BaseEnergyResistance { get { return 7; } }

		[Constructable]
		public HelmTarana()
		{
			Hue = 1563;
			Name = "Helm Tarana";
			Attributes.BonusMana = 10;
			Attributes.RegenMana = 1;
			SkillBonuses.SetValues(0, SkillName.Parry, 5);
		}

		public HelmTarana(Serial serial)
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
