namespace Server.Items
{
	[FlipableAttribute(0xA5DC)]
	public class ZapomnianaPiesn : Item
	{
		[Constructable]
		public ZapomnianaPiesn() : this(0)
		{
		}

		[Constructable]
		public ZapomnianaPiesn(int hue) : base(5358)
		{
			Weight = 2.0;
			Hue = 1151;
			Name = "Zapomniana Piesn";
			Label1 = "*przedmiot zadania Zaklec Barda*";
		}

		public ZapomnianaPiesn(Serial serial) : base(serial)
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
