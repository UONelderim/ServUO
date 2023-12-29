using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class Tyton : Item
	{

		[Constructable]
		public Tyton() : this( 1 )
		{
		}

		[Constructable]
		public Tyton( int amount ) : base( 0x1789 )
		{
			Name = "tyton";
			Weight = 0.1;
			Hue = 2129;
			Stackable = true;
			Amount = amount;

		}

		public Tyton( Serial serial ) : base( serial )
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