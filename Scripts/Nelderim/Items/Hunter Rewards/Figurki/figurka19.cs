using System;

namespace Server.Items
{
	public class figurka19 : Item
	{
		[Constructable]
		public figurka19() : this( 1 )
		{
		}

		[Constructable]
		public figurka19( int amount ) : base( 0x2D9B )
		{
			Weight = 1.0;
			ItemID = 11675;
			Amount = amount;
			Name   = "Parrot";
			Hue = 0;
		}

		public figurka19( Serial serial ) : base( serial )
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