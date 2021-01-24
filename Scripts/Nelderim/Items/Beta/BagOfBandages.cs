using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class BagOfBandages : Bag
	{
		[Constructable]
		public BagOfBandages() : this( 1000 )
		{
			Movable = true; 
			Hue = 1165; 
			Name = "worek z bandazami";
		}

		[Constructable]
		public BagOfBandages( int amount )
		{
			DropItem( new Bandage    ( 1000 ) );
		}

		public BagOfBandages( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}