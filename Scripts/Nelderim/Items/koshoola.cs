using System;

namespace Server.Items
{

		
	[FlipableAttribute( 0x1458 )]
	public class koshoola : BaseShirt
	{
		[Constructable]
		public koshoola() : this( 0 )
		{
		}

		[Constructable]
		public koshoola( int hue ) : base( 0x1458, hue )
		{
			Weight = 2.0;
			Name = "zdobiona koszula";
		}

		public koshoola( Serial serial ) : base( serial )
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

			if ( Weight == 2.0 )
				Weight = 1.0;
		}
	}
	}