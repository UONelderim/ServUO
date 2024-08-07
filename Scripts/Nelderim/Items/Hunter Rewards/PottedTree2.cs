namespace Server.Items
{
	public class PottedTree2 : Item
	{
		[Constructable]
		public PottedTree2() : base(0x2376)
		{
			Weight = 100;
		}

		public PottedTree2(Serial serial) : base(serial)
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
