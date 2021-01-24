using System;

namespace Server.Items
{
	public class figurka10 : Item
	{
		[Constructable]
		public figurka10() : this( 1 )
		{
		}

		[Constructable]
		public figurka10( int amount ) : base( 0x2621 )
		{
			Weight = 1.0;
			ItemID = 9761;
			Amount = amount;
			Name   = "Treefellow";
			Hue = 0;
		}

		public figurka10( Serial serial ) : base( serial )
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