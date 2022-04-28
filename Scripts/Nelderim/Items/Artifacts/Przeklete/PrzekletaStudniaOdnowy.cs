namespace Server.Items
{
	public class PrzekletaStudniaOdnowy : GoldRing
	{
		public override int InitMinHits { get { return 45; } }
		public override int InitMaxHits { get { return 45; } }

		[Constructable]
		public PrzekletaStudniaOdnowy()
		{
			Name = "Przeklęta Studnia Odnowy";
			Hue = 1180;
			LootType = LootType.Cursed;
			Attributes.RegenMana = 10;
			Attributes.RegenHits = 6;
			Attributes.RegenStam = 6;
		}

		public PrzekletaStudniaOdnowy(Serial serial) : base(serial)
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
