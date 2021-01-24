using System;

namespace Server.Items
{
	public class figurka05 : Item
	{
		[Constructable]
		public figurka05() : this( 1 )
		{
		}

		[Constructable]
		public figurka05( int amount ) : base( 0x25B6 )
		{
			Weight = 1.0;
			ItemID = 9654;
			Amount = amount;
			Name   = "Pixie";
			Hue = 0;
		}

		public figurka05( Serial serial ) : base( serial )
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