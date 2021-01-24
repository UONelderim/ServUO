using System;

namespace Server.Items
{
	public class figurka21 : Item
	{
		[Constructable]
		public figurka21() : this( 1 )
		{
		}

		[Constructable]
		public figurka21( int amount ) : base( 0x20D7 )
		{
			Weight = 1.0;
			ItemID = 8407;
			Amount = amount;
			Name   = "Earth Elemental";
			Hue = 0;
		}

		public figurka21( Serial serial ) : base( serial )
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