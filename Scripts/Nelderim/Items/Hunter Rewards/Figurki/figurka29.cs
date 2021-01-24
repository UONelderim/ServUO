using System;

namespace Server.Items
{
	public class figurka29 : Item
	{
		[Constructable]
		public figurka29() : this( 1 )
		{
		}

		[Constructable]
		public figurka29( int amount ) : base( 0x25D8 )
		{
			Weight = 1.0;
			ItemID = 9688;
			Amount = amount;
			Name   = "Red Crystal";
			Hue = 0;
		}

		public figurka29( Serial serial ) : base( serial )
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