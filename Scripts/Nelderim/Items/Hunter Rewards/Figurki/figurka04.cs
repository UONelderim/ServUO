using System;

namespace Server.Items
{
	public class figurka04 : Item
	{
		[Constructable]
		public figurka04() : this( 1 )
		{
		}

		[Constructable]
		public figurka04( int amount ) : base( 0x259F )
		{
			Weight = 1.0;
			ItemID = 9631;
			Amount = amount;
			Name   = "Imp";
			Hue = 0;
		}

		public figurka04( Serial serial ) : base( serial )
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