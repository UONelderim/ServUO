namespace Server.Items
{
	[Furniture]
	[Flipable(0xB32, 0xB33)]
	public class ThroneRC : ResouceCraftable
	{
		[Constructable]
		public ThroneRC() : base(0xB33)
		{
			Weight = 1.0;
		}

		public ThroneRC(Serial serial) : base(serial)
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

			if (Weight == 6.0)
				Weight = 1.0;
		}
	}

	[Furniture]
	[Flipable(0xB2E, 0xB2F, 0xB31, 0xB30)]
	public class WoodenThroneRC : ResouceCraftable
	{
		[Constructable]
		public WoodenThroneRC() : base(0xB2E)
		{
			Weight = 15.0;
		}

		public WoodenThroneRC(Serial serial) : base(serial)
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

			if (Weight == 6.0)
				Weight = 15.0;
		}
	}
}
