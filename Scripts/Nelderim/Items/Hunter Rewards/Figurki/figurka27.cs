using System;

namespace Server.Items
{
	public class figurka27 : Item
	{
		[Constructable]
		public figurka27() : this( 1 )
		{
		}

		[Constructable]
		public figurka27( int amount ) : base( 0x25D6 )
		{
			Weight = 1.0;
			ItemID = 9686;
			Amount = amount;
			Name   = "Green Crystal";
			Hue = 0;
		}

		public figurka27( Serial serial ) : base( serial )
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