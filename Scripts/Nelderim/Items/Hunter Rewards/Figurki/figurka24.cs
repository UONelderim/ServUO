using System;

namespace Server.Items
{
	public class figurka24 : Item
	{
		[Constructable]
		public figurka24() : this( 1 )
		{
		}

		[Constructable]
		public figurka24( int amount ) : base( 0x210B )
		{
			Weight = 1.0;
			ItemID = 8459;
			Amount = amount;
			Name   = "Water Elemental";
			Hue = 0;
		}

		public figurka24( Serial serial ) : base( serial )
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