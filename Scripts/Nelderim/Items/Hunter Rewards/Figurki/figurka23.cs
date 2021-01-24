using System;

namespace Server.Items
{
	public class figurka23 : Item
	{
		[Constructable]
		public figurka23() : this( 1 )
		{
		}

		[Constructable]
		public figurka23( int amount ) : base( 0x20E0 )
		{
			Weight = 1.0;
			ItemID = 8416;
			Amount = amount;
			Name   = "Orc";
			Hue = 0;
		}

		public figurka23( Serial serial ) : base( serial )
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