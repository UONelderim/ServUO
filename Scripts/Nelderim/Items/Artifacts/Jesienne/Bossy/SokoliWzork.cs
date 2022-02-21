namespace Server.Items
{
	public class SokoliWzork : RavenHelm
	{
		public override int InitMinHits { get { return 60; } }
		public override int InitMaxHits { get { return 60; } }

		public override int BasePhysicalResistance { get { return 20; } }
		public override int BaseFireResistance { get { return 10; } }
		public override int BaseColdResistance { get { return 10; } }
		public override int BasePoisonResistance { get { return 6; } }
		public override int BaseEnergyResistance { get { return 4; } }

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
			writer.Write(0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}
}
