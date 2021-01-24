using System;

namespace Server.Items
{
	public class figurka08 : Item
	{
		[Constructable]
		public figurka08() : this( 1 )
		{
		}

		[Constructable]
		public figurka08( int amount ) : base( 0x2620 )
		{
			Weight = 1.0;
			ItemID = 9760;
			Amount = amount;
			Name   = "Crystal Elemental";
			Hue = 0;
		}

		public figurka08( Serial serial ) : base( serial )
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