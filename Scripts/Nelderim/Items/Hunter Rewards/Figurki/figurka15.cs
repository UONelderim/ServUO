using System;

namespace Server.Items
{
	public class figurka15 : Item
	{
		[Constructable]
		public figurka15() : this( 1 )
		{
		}

		[Constructable]
		public figurka15( int amount ) : base( 0x2767 )
		{
			Weight = 1.0;
			ItemID = 10087;
			Amount = amount;
			Name   = "Fan Dancer";
			Hue = 0;
		}

		public figurka15( Serial serial ) : base( serial )
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