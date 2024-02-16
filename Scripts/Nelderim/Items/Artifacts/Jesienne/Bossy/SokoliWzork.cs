namespace Server.Items
{
	public class SokoliWzork : RavenHelm
	{
		public override int InitMinHits => 60;
		public override int InitMaxHits => 60;

		public override int BasePhysicalResistance => 20;
		public override int BaseFireResistance => 10;
		public override int BaseColdResistance => 10;
		public override int BasePoisonResistance => 6;
		public override int BaseEnergyResistance => 4;

		[Constructable]
		public SokoliWzork()
		{
			Hue = 2673;
			Name = "Sokoli Wzrok";
			Attributes.BonusStam = 5;
			SkillBonuses.SetValues(0, SkillName.Archery, 10);
		}

		public SokoliWzork(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}
}
