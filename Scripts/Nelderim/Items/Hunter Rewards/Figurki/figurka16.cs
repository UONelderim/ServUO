using System;

namespace Server.Items
{
	public class figurka16 : Item
	{
		[Constructable]
		public figurka16() : this( 1 )
		{
		}

		[Constructable]
		public figurka16( int amount ) : base( 0x276C )
		{
			Weight = 1.0;
			ItemID = 10092;
			Amount = amount;
			Name   = "Lady Of The Snow";
			Hue = 0;
		}

		public figurka16( Serial serial ) : base( serial )
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