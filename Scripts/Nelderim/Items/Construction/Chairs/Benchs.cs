using System;

namespace Server.Items
{
	[Furniture]
	[Flipable( 0xB2D, 0xB2C )]
    public class WoodenBenchRC : ResouceCraftable
	{
		[Constructable]
		public WoodenBenchRC() : base( 0xB2D )
		{
			Weight = 6;
		}

		public WoodenBenchRC(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int) 0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}
}
