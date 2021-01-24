using System;
using System.Collections;
using Server;
using Server.Multis;
using Server.Network;

namespace Server.Items
{

	[Furniture]
	[Flipable( 0x2815, 0x2816 )]
    public class TallCabinetRC : ResouceCraftableBaseContainer
	{
		[Constructable]
		public TallCabinetRC() : base( 0x2815 )
		{
			Weight = 1.0;
		}

		public TallCabinetRC( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}

	[Furniture]
	[Flipable( 0x2817, 0x2818 )]
    public class ShortCabinetRC : ResouceCraftableBaseContainer
	{
		[Constructable]
		public ShortCabinetRC() : base( 0x2817 )
		{
			Weight = 1.0;
		}

		public ShortCabinetRC( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}


	[Furniture]
	[Flipable( 0x2857, 0x2858 )]
    public class RedArmoireRC : ResouceCraftableBaseContainer
	{
		[Constructable]
		public RedArmoireRC() : base( 0x2857 )
		{
			Weight = 1.0;
		}

		public RedArmoireRC( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}

	[Furniture]
	[Flipable( 0x285D, 0x285E )]
    public class CherryArmoireRC : ResouceCraftableBaseContainer
	{
		[Constructable]
		public CherryArmoireRC() : base( 0x285D )
		{
			Weight = 1.0;
		}

		public CherryArmoireRC( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}

	[Furniture]
	[Flipable( 0x285B, 0x285C )]
    public class MapleArmoireRC : ResouceCraftableBaseContainer
	{
		[Constructable]
		public MapleArmoireRC() : base( 0x285B )
		{
			Weight = 1.0;
		}

		public MapleArmoireRC( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}

	[Furniture]
	[Flipable( 0x2859, 0x285A )]
    public class ElegantArmoireRC : ResouceCraftableBaseContainer
	{
		[Constructable]
		public ElegantArmoireRC() : base( 0x2859 )
		{
			Weight = 1.0;
		}

		public ElegantArmoireRC( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}

	[Furniture]
	[Flipable( 0xa9d, 0xa9e )]
    public class EmptyBookcaseRC : ResouceCraftableBaseContainer
	{
		[Constructable]
		public EmptyBookcaseRC() : base( 0xA9D )
		{
		}

		public EmptyBookcaseRC( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();

			if ( version == 0 && Weight == 1.0 )
				Weight = -1;
		}
	}

	[Furniture]
	[Flipable( 0xa2c, 0xa34 )]
    public class DrawerRC : ResouceCraftableBaseContainer
	{
		[Constructable]
		public DrawerRC() : base( 0xA2C )
		{
			Weight = 1.0;
		}

		public DrawerRC( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}

	[Furniture]
	[Flipable( 0xa30, 0xa38 )]
    public class FancyDrawerRC : ResouceCraftableBaseContainer
	{
		[Constructable]
		public FancyDrawerRC() : base( 0xA30 )
		{
			Weight = 1.0;
		}

		public FancyDrawerRC( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}

	[Furniture]
	[Flipable( 0xa4f, 0xa53 )]
    public class ArmoireRC : ResouceCraftableBaseContainer
	{
		[Constructable]
		public ArmoireRC() : base( 0xA4F )
		{
			Weight = 1.0;
		}

		public override void DisplayTo( Mobile m )
		{
			if ( DynamicFurniture.Open( this, m ) )
				base.DisplayTo( m );
		}

		public ArmoireRC( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();

			DynamicFurniture.Close( this );
		}
	}

	[Furniture]
	[Flipable( 0xa4d, 0xa51 )]
    public class FancyArmoireRC : ResouceCraftableBaseContainer
	{
		[Constructable]
		public FancyArmoireRC() : base( 0xA4D )
		{
			Weight = 1.0;
		}

		public override void DisplayTo( Mobile m )
		{
			if ( DynamicFurniture.Open( this, m ) )
				base.DisplayTo( m );
		}

		public FancyArmoireRC( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();

			DynamicFurniture.Close( this );
		}
	}

}