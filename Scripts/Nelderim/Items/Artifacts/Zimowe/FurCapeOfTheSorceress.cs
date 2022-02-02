namespace Server.Items
{
	public class FurCapeOfTheSorceress : FurCape
	{
		public override int InitMinHits { get { return 60; } }
		public override int InitMaxHits { get { return 60; } }


		[Constructable]
		public FurCapeOfTheSorceress()
		{
			Name = "Futrzany Plaszcz Czarodziejki";
			Hue = 1266;
			Attributes.BonusInt = -5;
			Attributes.BonusMana = 10;
			Attributes.LowerManaCost = 5;
			Attributes.LowerRegCost = 10;
		}

		public FurCapeOfTheSorceress(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}
}
