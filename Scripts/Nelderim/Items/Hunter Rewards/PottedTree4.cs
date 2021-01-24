using System;
using Server.Items;
using Server.Network;

namespace Server.Items
{
	public class PottedTree4 : Item
	{
		[Constructable]
		public PottedTree4() : base( 0x2377 )
		{
			Weight = 100;
		}

		public PottedTree4( Serial serial ) : base( serial )
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