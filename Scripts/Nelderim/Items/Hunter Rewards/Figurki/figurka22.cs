using System;

namespace Server.Items
{
	public class figurka22 : Item
	{
		[Constructable]
		public figurka22() : this( 1 )
		{
		}

		[Constructable]
		public figurka22( int amount ) : base( 0x20DE )
		{
			Weight = 1.0;
			ItemID = 8414;
			Amount = amount;
			Name   = "Lizard Man";
			Hue = 0;
		}

		public figurka22( Serial serial ) : base( serial )
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