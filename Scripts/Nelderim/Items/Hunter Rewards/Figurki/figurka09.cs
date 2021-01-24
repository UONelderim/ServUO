using System;

namespace Server.Items
{
	public class figurka09 : Item
	{
		[Constructable]
		public figurka09() : this( 1 )
		{
		}

		[Constructable]
		public figurka09( int amount ) : base( 0x261B )
		{
			Weight = 1.0;
			ItemID = 9755;
			Amount = amount;
			Name   = "Plague Beast";
			Hue = 0;
		}

		public figurka09( Serial serial ) : base( serial )
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