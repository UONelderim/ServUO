using System;

namespace Server.Items
{

	[Furniture]
    public class ElegantLowTableRC : ResouceCraftable
	{
		[Constructable]
		public ElegantLowTableRC() : base(0x2819)
		{
			Weight = 1.0;
		}

		public ElegantLowTableRC(Serial serial) : base(serial)
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

	[Furniture]
    public class PlainLowTableRC : ResouceCraftable
	{
		[Constructable]
		public PlainLowTableRC() : base(0x281A)
		{
			Weight = 1.0;
		}

		public PlainLowTableRC(Serial serial) : base(serial)
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

	[Furniture]
	[Flipable(0xB90,0xB7D)]
    public class LargeTableRC : ResouceCraftable
	{
		[Constructable]
		public LargeTableRC() : base(0xB90)
		{
			Weight = 1.0;
		}

		public LargeTableRC(Serial serial) : base(serial)
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

	[Furniture]
	[Flipable(0xB35,0xB34)]
    public class NightstandRC : ResouceCraftable
	{
		[Constructable]
		public NightstandRC() : base(0xB35)
		{
			Weight = 1.0;
		}

		public NightstandRC(Serial serial) : base(serial)
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

	[Furniture]
	[Flipable(0xB8F,0xB7C)]
    public class YewWoodTableRC : ResouceCraftable
	{
		[Constructable]
		public YewWoodTableRC() : base(0xB8F)
		{
			Weight = 1.0;
		}

		public YewWoodTableRC(Serial serial) : base(serial)
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
