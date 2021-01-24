using System;

namespace Server.Items
{
	public class figurka30 : Item
	{
		[Constructable]
		public figurka30() : this( 1 )
		{
		}

		[Constructable]
		public figurka30( int amount ) : base( 0x25D9 )
		{
			Weight = 1.0;
			ItemID = 9689;
			Amount = amount;
			Name   = "Yellow Crystal";
			Hue = 0;
		}

		public figurka30( Serial serial ) : base( serial )
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