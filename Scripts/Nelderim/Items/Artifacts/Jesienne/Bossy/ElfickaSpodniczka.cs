namespace Server.Items
{
	public class ElfickaSpodniczka : LeafTonlet
	{
		public override int InitMinHits => 255;
		public override int InitMaxHits => 255;

		public override int BasePhysicalResistance { get { return -15; } }
		public override int BaseFireResistance { get { return 10; } }
		public override int BaseColdResistance { get { return -10; } }
		public override int BasePoisonResistance { get { return 30; } }
		public override int BaseEnergyResistance { get { return 8; } }

		[Constructable]
		public ElfickaSpodniczka()
		{
			Hue = 2882;
			Name = "Elficka Spódniczka";
			Attributes.LowerManaCost = 10;
			SkillBonuses.SetValues(0, SkillName.Spellweaving, 5);
		}

		public ElfickaSpodniczka(Serial serial)
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
