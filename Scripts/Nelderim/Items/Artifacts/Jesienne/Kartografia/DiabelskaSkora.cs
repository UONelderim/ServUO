namespace Server.Items
{
	public class DiabelskaSkora : LeatherMempo
	{
		public override int InitMinHits => 255;
		public override int InitMaxHits => 255;
		public override int BaseFireResistance { get { return 0; } }
		public override int BaseColdResistance { get { return 20; } }
		public override int BasePhysicalResistance { get { return 10; } }
		public override int BaseEnergyResistance { get { return 5; } }
		public override int BasePoisonResistance { get { return 13; } }

		[Constructable]
		public DiabelskaSkora()
		{
			Hue = 743;
			Name = "Diabelska Skora";
			Attributes.RegenHits = 2;
			Attributes.Luck = 145;
			SkillBonuses.SetValues(0, SkillName.Archery, 10.0);
		}

		public DiabelskaSkora(Serial serial)
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
