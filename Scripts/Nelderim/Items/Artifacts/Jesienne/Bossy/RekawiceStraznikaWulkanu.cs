namespace Server.Items
{
	public class RekawiceStraznikaWulkanu : DragonGloves
	{
		public override int InitMinHits => 255;
		public override int InitMaxHits => 255;

		public override int BasePhysicalResistance { get { return 2; } }
		public override int BaseFireResistance { get { return 10; } }
		public override int BaseColdResistance { get { return 2; } }
		public override int BasePoisonResistance { get { return 2; } }
		public override int BaseEnergyResistance { get { return 2; } }

		[Constructable]
		public RekawiceStraznikaWulkanu()
		{
			Hue = 2189;
			Name = "Rekawice Straznika Wulkanu";
			Attributes.BonusInt = 5;
			Attributes.BonusStr = 10;
			Attributes.BonusDex = -10;
			Attributes.LowerManaCost = 5;
			Attributes.CastRecovery = 1;
			SkillBonuses.SetValues(0, SkillName.Fencing, 10);
		}

		public RekawiceStraznikaWulkanu(Serial serial)
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
