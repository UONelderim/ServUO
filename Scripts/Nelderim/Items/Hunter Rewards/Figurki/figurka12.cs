using System;

namespace Server.Items
{
	public class figurka12 : Item
	{
		[Constructable]
		public figurka12() : this( 1 )
		{
		}

		[Constructable]
		public figurka12( int amount ) : base( 0x2763 )
		{
			Weight = 1.0;
			ItemID = 10083;
			Amount = amount;
			Name   = "Bake-Kitsune";
			Hue = 0;
		}

		public figurka12( Serial serial ) : base( serial )
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