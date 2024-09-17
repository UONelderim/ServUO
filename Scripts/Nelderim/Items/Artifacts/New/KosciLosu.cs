namespace Server.Items
{
	public class KosciLosu : BoneArms
	{
		public override int LabelNumber { get { return 1065796; } } // Kosci Losu
		public override int InitMinHits => 255;
		public override int InitMaxHits => 255;

		[Constructable]
		public KosciLosu()
		{
			Hue = 1165;
			Attributes.AttackChance = 5;
			Attributes.BonusDex = 5;
			Attributes.DefendChance = 5;
			Attributes.EnhancePotions = 10;
			Attributes.NightSight = 1;
		}

		public KosciLosu(Serial serial)
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
