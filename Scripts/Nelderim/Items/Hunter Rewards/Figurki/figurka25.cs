using System;

namespace Server.Items
{
	public class figurka25 : Item
	{
		[Constructable]
		public figurka25() : this( 1 )
		{
		}

		[Constructable]
		public figurka25( int amount ) : base( 0x20F3 )
		{
			Weight = 1.0;
			ItemID = 8435;
			Amount = amount;
			Name   = "Fire Elemental";
			Hue = 0;
		}

		public figurka25( Serial serial ) : base( serial )
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