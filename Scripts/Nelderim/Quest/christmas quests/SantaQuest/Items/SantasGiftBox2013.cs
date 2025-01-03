namespace Server.Items
{
	public class SantasGiftBox2013 : GiftBox
	{
		[Constructable]
		public SantasGiftBox2013()
		{
			Name = "Prezenty Od Pana";
			Hue = 33;


			DropItem(new SantasSleighSmallAddonDeed());
			DropItem(new SantasReindeer1());
			DropItem(new SantasReindeer2());
		}

		public SantasGiftBox2013(Serial serial)
			: base(serial)
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
