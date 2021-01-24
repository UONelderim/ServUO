using System;

namespace Server.Items
{
	public class figurka18 : Item
	{
		[Constructable]
		public figurka18() : this( 1 )
		{
		}

		[Constructable]
		public figurka18( int amount ) : base( 0x2D96 )
		{
			Weight = 1.0;
			ItemID = 11670;
			Amount = amount;
			Name   = "Cu Sidhe";
			Hue = 0;
		}

		public figurka18( Serial serial ) : base( serial )
		{
		}

		

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			if ( Hue == 0 )
				Hue = 0;
		}
	}
}