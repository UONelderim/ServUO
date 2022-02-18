namespace Server.Items
{
	public class PosokaPluskwy : Item
	{
		[Constructable]
		public PosokaPluskwy() : base(0x182C)
		{
			Name = "Posoka Pluskwy";
			Weight = 5.0;
			Hue = 1510;
			LootType = LootType.Cursed;
		}

		public PosokaPluskwy(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}
}
