using System;

namespace Server.Items
{
	public class figurka17 : Item
	{
		[Constructable]
		public figurka17() : this( 1 )
		{
		}

		[Constructable]
		public figurka17( int amount ) : base( 0x20D6 )
		{
			Weight = 1.0;
			ItemID = 8406;
			Amount = amount;
			Name   = "Dragon";
			Hue = 0;
		}

		public figurka17( Serial serial ) : base( serial )
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