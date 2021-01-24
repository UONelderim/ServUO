using System;

namespace Server.Items
{
	public class figurka03 : Item
	{
		[Constructable]
		public figurka03() : this( 1 )
		{
		}

		[Constructable]
		public figurka03( int amount ) : base( 0x2590 )
		{
			Weight = 1.0;
			ItemID = 9616;
			Amount = amount;
			Name   = "Genie Efereet";
			Hue = 0;
		}

		public figurka03( Serial serial ) : base( serial )
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