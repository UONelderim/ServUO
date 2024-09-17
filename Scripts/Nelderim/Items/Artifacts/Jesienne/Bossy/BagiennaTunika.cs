namespace Server.Items
{
	public class BagiennaTunika : RingmailChest
	{
		public override int InitMinHits => 255;
		public override int InitMaxHits => 255;

		public override int BasePhysicalResistance { get { return 10; } }
		public override int BaseFireResistance { get { return 10; } }
		public override int BaseColdResistance { get { return 0; } }
		public override int BasePoisonResistance { get { return 20; } }
		public override int BaseEnergyResistance { get { return 0; } }

		[Constructable]
		public BagiennaTunika()
		{
			Hue = 2187;
			Name = "Bagienna Tunika";
			Attributes.BonusStam = 5;
			Attributes.WeaponSpeed = 10;
			Attributes.LowerManaCost = 8;
			SkillBonuses.SetValues(0, SkillName.Chivalry, 10);
		}

		public BagiennaTunika(Serial serial)
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
