namespace Server.Items
{
	public class figurka06 : Item
	{
		[Constructable]
		public figurka06() : this(1)
		{
		}

		[Constructable]
		public figurka06(int amount) : base(0x25B9)
		{
			Weight = 1.0;
			ItemID = 9657;
			Amount = amount;
			Name = "Giant Scorpion";
			Hue = 0;
		}

		public figurka06(Serial serial) : base(serial)
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

			if (Hue == 0)
				Hue = 0;
		}
	}
}
