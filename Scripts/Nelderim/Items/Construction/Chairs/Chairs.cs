using System;

namespace Server.Items
{
	[Furniture]
	[Flipable( 0xB4F, 0xB4E, 0xB50, 0xB51 )]
    public class FancyWoodenChairCushionRC : ResouceCraftable
	{
		[Constructable]
		public FancyWoodenChairCushionRC() : base(0xB4F)
		{
			Weight = 20.0;
		}

        public FancyWoodenChairCushionRC(Serial serial)
            : base(serial)
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
				Weight = 20.0;
		}
	}

	[Furniture]
	[Flipable( 0xB53, 0xB52, 0xB54, 0xB55 )]
    public class WoodenChairCushionRC : ResouceCraftable
	{
		[Constructable]
		public WoodenChairCushionRC() : base(0xB53)
		{
			Weight = 20.0;
		}

		public WoodenChairCushionRC(Serial serial) : base(serial)
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
				Weight = 20.0;
		}
	}

	[Furniture]
	[Flipable( 0xB57, 0xB56, 0xB59, 0xB58 )]
    public class WoodenChairRC : ResouceCraftable
	{
		[Constructable]
		public WoodenChairRC() : base(0xB57)
		{
			Weight = 20.0;
		}

		public WoodenChairRC(Serial serial) : base(serial)
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
				Weight = 20.0;
		}
	}

	[Furniture]
	[Flipable( 0xB5B, 0xB5A, 0xB5C, 0xB5D )]
    public class BambooChairRC : ResouceCraftable
	{
		[Constructable]
		public BambooChairRC() : base(0xB5B)
		{
			Weight = 20.0;
		}

		public BambooChairRC(Serial serial) : base(serial)
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
				Weight = 20.0;
		}
	}

	[DynamicFliping]
	[Flipable( 0x2DE3, 0x2DE4, 0x2DE5, 0x2DE6 )]
    public class OrnateElvenChairRC : ResouceCraftable
	{
		[Constructable]
		public OrnateElvenChairRC() : base( 0x2DE3 )
		{
			Weight = 1.0;
		}

		public OrnateElvenChairRC( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();
		}
	}

	[DynamicFliping]
	[Flipable( 0x2DEB, 0x2DEC, 0x2DED, 0x2DEE )]
    public class BigElvenChairRC : ResouceCraftable
	{
		[Constructable]
		public BigElvenChairRC() : base( 0x2DEB )
		{
		}

		public BigElvenChairRC( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();
		}
	}

	[DynamicFliping]
	[Flipable( 0x2DF5, 0x2DF6 )]
    public class ElvenReadingChairRC : ResouceCraftable
	{
		[Constructable]
		public ElvenReadingChairRC() : base( 0x2DF5 )
		{
		}

        public ElvenReadingChairRC(Serial serial)
            : base(serial)
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();
		}
	}
}
