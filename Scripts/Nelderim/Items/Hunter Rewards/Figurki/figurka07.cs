using System;

namespace Server.Items
{
	public class figurka07 : Item
	{
		[Constructable]
		public figurka07() : this( 1 )
		{
		}

		[Constructable]
		public figurka07( int amount ) : base( 0x25CF )
		{
			Weight = 1.0;
			ItemID = 9679;
			Amount = amount;
			Name   = "Wolf";
			Hue = 0;
		}

		public figurka07( Serial serial ) : base( serial )
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