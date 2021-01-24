using System;

namespace Server.Items
{
	public class figurka01 : Item
	{
		[Constructable]
		public figurka01() : this( 1 )
		{
		}

		[Constructable]
		public figurka01( int amount ) : base( 0x2587 )
		{
			Weight = 1.0;
			ItemID = 9607;
			Amount = amount;
			Name   = "Ice Fiend Daemon";
			Hue = 0;
		}

		public figurka01( Serial serial ) : base( serial )
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