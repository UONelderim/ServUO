namespace Server.Items
{
	[Furniture]
	public class FlowerInGlassC : Item
	{
		[Constructable]
		public FlowerInGlassC()
			: base(0xA8EC)
		{
			Weight = 20.0;
		}

		public FlowerInGlassC(Serial serial)
			: base(serial)
		{
		}

		public override int LabelNumber => 3060073;
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
