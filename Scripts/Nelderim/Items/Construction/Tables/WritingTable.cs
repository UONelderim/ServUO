using System;

namespace Server.Items
{
	[Furniture]
	[Flipable(0xB4A,0xB49, 0xB4B, 0xB4C)]
    public class WritingTableRC : ResouceCraftable
	{
		[Constructable]
		public WritingTableRC() : base(0xB4A)
		{
			Weight = 1.0;
		}

		public WritingTableRC(Serial serial) : base(serial)
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

			if ( Weight == 4.0 )
				Weight = 1.0;
		}
	}
}
