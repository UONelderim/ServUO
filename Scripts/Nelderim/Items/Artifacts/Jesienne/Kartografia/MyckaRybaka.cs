namespace Server.Items
{
	public class MyckaRybaka : SkullCap
	{
		public override int InitMinHits => 255;
		public override int InitMaxHits => 255;
		public override int BaseFireResistance { get { return 2; } }
		public override int BaseColdResistance { get { return 3; } }
		public override int BasePhysicalResistance { get { return 10; } }
		public override int BaseEnergyResistance { get { return 0; } }
		public override int BasePoisonResistance { get { return 10; } }

		[Constructable]
		public MyckaRybaka()
		{
			Hue = 1156;
			Name = "Mycka Rybaka";
			Attributes.BonusStr = 5;
			SkillBonuses.SetValues(0, SkillName.Fishing, 10.0);
		}

		public MyckaRybaka(Serial serial)
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
