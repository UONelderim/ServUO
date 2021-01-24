using System;
using Server.Engines.Craft;

namespace Server.Items
{
	[Furniture]
    public class StoolRC : ResouceCraftable
	{
		[Constructable]
		public StoolRC() : base( 0xA2A )
		{
			Weight = 10.0;
		}

		public StoolRC(Serial serial) : base(serial)
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

			if ( Weight == 6.0 )
				Weight = 10.0;
		}
	}

	[Furniture]
    public class FootStoolRC : ResouceCraftable
    {
		[Constructable]
		public FootStoolRC() : base( 0xB5E )
		{
			Weight = 6.0;
		}

		public FootStoolRC(Serial serial) : base(serial)
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

			if ( Weight == 6.0 )
				Weight = 10.0;
		}
	}
}
