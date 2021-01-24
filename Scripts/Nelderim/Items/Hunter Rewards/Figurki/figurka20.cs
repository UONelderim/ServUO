using System;

namespace Server.Items
{
	public class figurka20 : Item
	{
		[Constructable]
		public figurka20() : this( 1 )
		{
		}

		[Constructable]
		public figurka20( int amount ) : base( 0x0ED )
		{
			Weight = 1.0;
			ItemID = 8429;
			Amount = amount;
			Name   = "Air Elemental";
			Hue = 0;
		}

		public figurka20( Serial serial ) : base( serial )
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