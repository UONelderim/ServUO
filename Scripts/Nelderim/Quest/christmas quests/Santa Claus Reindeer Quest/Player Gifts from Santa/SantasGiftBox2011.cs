namespace Server.Items
{
	[Flipable(0x232A, 0x232B)]
	public class SantasGiftBox2011 : GiftBox
	{
		public int offset;

		[Constructable]
		public SantasGiftBox2011()
		{
			Name = "Prezent od Pana ";

			DropItem(new HolidayBell2011());
			DropItem(new ChristmasCandle2011());
			DropItem(new SantasChairAddonDeed());
			DropItem(new SantasCoal());
			DropItem(new SantasElfBoots());
			DropItem(new Snowpile2011());
			DropItem(new SantasTimepiece());
			DropItem(new RadioactiveXMasTreeAddonDeed());

			offset = Utility.Random(0, 10);

			switch (Utility.Random(9))
			{
				case 0:
					DropItem(new HolidayBell2011());
					break;
				case 1:
					DropItem(new SantasChairAddonDeed());
					break;
				case 2:
					DropItem(new ChristmasCandle2011());
					break;
				case 3:
					DropItem(new SantasCoal());
					break;
				case 4:
					DropItem(new SantasElfBoots());
					break;
				case 5:
					DropItem(new WhiteSnowflake());
					break;
				case 6:
					DropItem(new Snowpile2011());
					break;
				case 7:
					DropItem(new SantasTimepiece());
					break;
				case 8:
					DropItem(new RadioactiveXMasTreeAddonDeed());
					break;
			}
		}

		public SantasGiftBox2011(Serial serial) : base(serial)
		{
		}

		public override void GetProperties(ObjectPropertyList list)
		{
			base.GetProperties(list);

			list.Add(1007149 + offset);
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0);

			writer.Write(offset);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			offset = reader.ReadInt();
		}
	}
}
