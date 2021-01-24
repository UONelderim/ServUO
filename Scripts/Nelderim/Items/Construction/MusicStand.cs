using System;

namespace Server.Items
{
	[Furniture]
	[Flipable(0xEBB, 0xEBC)]
    public class TallMusicStandRC : ResouceCraftable
	{
		[Constructable]
		public TallMusicStandRC() : base(0xEBB)
		{
			Weight = 10.0;
		}

		public TallMusicStandRC(Serial serial) : base(serial)
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

			if ( Weight == 8.0 )
				Weight = 10.0;
		}
	}

	[Furniture]
	[Flipable(0xEB6,0xEB8)]
    public class ShortMusicStandRC : ResouceCraftable
	{
		[Constructable]
		public ShortMusicStandRC() : base(0xEB6)
		{
			Weight = 10.0;
		}

		public ShortMusicStandRC(Serial serial) : base(serial)
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
