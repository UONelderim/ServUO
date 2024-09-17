namespace Server.Items
{
	public class SmoczaPrzywara : DragonLegs
	{
		public override int InitMinHits => 255;
		public override int InitMaxHits => 255;

		public override int BaseColdResistance { get { return -15; } }
		public override int BaseEnergyResistance { get { return 11; } }
		public override int BasePhysicalResistance { get { return 5; } }
		public override int BasePoisonResistance { get { return 4; } }
		public override int BaseFireResistance { get { return 25; } }

		[Constructable]
		public SmoczaPrzywara()
		{
			Hue = 1161;
			Name = "Smocza Przywara";
			Attributes.RegenHits = 2;
			Attributes.RegenMana = 2;
			SkillBonuses.SetValues(0, SkillName.Macing, 5.0);
			SkillBonuses.SetValues(0, SkillName.MagicResist, 5.0);
		}

		public SmoczaPrzywara(Serial serial)
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
