using System;

namespace Server.Items
{
	public class figurka26 : Item
	{
		[Constructable]
		public figurka26() : this( 1 )
		{
		}

		[Constructable]
		public figurka26( int amount ) : base( 0x25DC )
		{
			Weight = 1.0;
			ItemID = 9692;
			Amount = amount;
			Name   = "White Crystal";
			Hue = 0;
		}

		public figurka26( Serial serial ) : base( serial )
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