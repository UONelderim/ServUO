namespace Server.Items
{
	public class WisiorMagowBialejWiezy : GoldNecklace
	{
		public override int InitMinHits => 255;
		public override int InitMaxHits => 255;

		[Constructable]
		public WisiorMagowBialejWiezy()
		{
			Hue = 1153;
			Name = "Wisior Magow Bialej Wiezy";
			Attributes.RegenMana = 1;
			Attributes.LowerManaCost = 5;
			Attributes.LowerRegCost = 15;
			LootType = LootType.Cursed;
			Resistances.Cold = 10;
			Resistances.Fire = 10;
			Resistances.Poison = 10;
			Resistances.Energy = 10;
		}

		public WisiorMagowBialejWiezy(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.WriteEncodedInt(0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadEncodedInt();
		}
	}
}
