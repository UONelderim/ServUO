using System;

namespace Server.Items
{
	public class figurka13 : Item
	{
		[Constructable]
		public figurka13() : this( 1 )
		{
		}

		[Constructable]
		public figurka13( int amount ) : base( 0x2764 )
		{
			Weight = 1.0;
			ItemID = 10084;
			Amount = amount;
			Name   = "Crane";
			Hue = 0;
		}

		public figurka13( Serial serial ) : base( serial )
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