namespace Server.Items
{
	public class RekawiceGornikaZOrod : LeatherGloves
	{
		public override int InitMinHits => 255;
		public override int InitMaxHits => 255;
		public override int BaseFireResistance { get { return 20; } }
		public override int BaseColdResistance { get { return 3; } }
		public override int BasePhysicalResistance { get { return 10; } }
		public override int BaseEnergyResistance { get { return 20; } }
		public override int BasePoisonResistance { get { return 10; } }

		[Constructable]
		public RekawiceGornikaZOrod()
		{
			Hue = 1156;
			Name = "Rekawice Gornika z Orod";
			SkillBonuses.SetValues(0, SkillName.Mining, 40.0);
		}

		public RekawiceGornikaZOrod(Serial serial)
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
