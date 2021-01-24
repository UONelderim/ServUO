using System;

namespace Server.Items
{
	public class figurka14 : Item
	{
		[Constructable]
		public figurka14() : this( 1 )
		{
		}

		[Constructable]
		public figurka14( int amount ) : base( 0x281C )
		{
			Weight = 1.0;
			ItemID = 10268;
			Amount = amount;
			Name   = "Fire Beetle";
			Hue = 0;
		}

		public figurka14( Serial serial ) : base( serial )
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