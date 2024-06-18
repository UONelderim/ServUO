namespace Server.Items
{
	[Furniture]
	public class FlowerInGlassB : Item
	{
		[Constructable]
		public FlowerInGlassB()
			: base(0xA8EB)
		{
			Weight = 20.0;
		}

		public FlowerInGlassB(Serial serial)
			: base(serial)
		{
		}

		public override int LabelNumber => 3060072;
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
