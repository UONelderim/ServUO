using System;

namespace Server.Items
{
	public class figurka02 : Item
	{
		[Constructable]
		public figurka02() : this( 1 )
		{
		}

		[Constructable]
		public figurka02( int amount ) : base( 0x2589 )
		{
			Weight = 1.0;
			ItemID = 9609;
			Amount = amount;
			Name   = "Ethereal Warriors";
			Hue = 0;
		}

		public figurka02( Serial serial ) : base( serial )
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