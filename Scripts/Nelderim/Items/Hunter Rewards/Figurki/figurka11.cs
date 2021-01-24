using System;

namespace Server.Items
{
	public class figurka11 : Item
	{
		[Constructable]
		public figurka11() : this( 1 )
		{
		}

		[Constructable]
		public figurka11( int amount ) : base( 0x2766 )
		{
			Weight = 1.0;
			ItemID = 10086;
			Amount = amount;
			Name   = "Denkou Yajuu";
			Hue = 0;
		}

		public figurka11( Serial serial ) : base( serial )
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