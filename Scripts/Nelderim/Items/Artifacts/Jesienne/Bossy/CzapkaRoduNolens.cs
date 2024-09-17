namespace Server.Items
{
	public class CzapkaRoduNolens : Bonnet
	{
		public override int InitMinHits => 255;
		public override int InitMaxHits => 255;

		public override int BasePhysicalResistance { get { return -20; } }
		public override int BaseFireResistance { get { return 25; } }
		public override int BaseColdResistance { get { return 35; } }
		public override int BasePoisonResistance { get { return 0; } }
		public override int BaseEnergyResistance { get { return 24; } }

		[Constructable]
		public CzapkaRoduNolens()
		{
			Hue = 1621;
			Name = "Czapka Rodu Nolens";
			Attributes.RegenHits = 3;
			Attributes.RegenStam = 3;
			Attributes.RegenMana = 3;
			Attributes.BonusMana = 5;
			Attributes.LowerRegCost = 15;
			SkillBonuses.SetValues(0, SkillName.Alchemy, 10);
		}

		public CzapkaRoduNolens(Serial serial)
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
