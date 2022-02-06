using System;
using Server;
using Server.Network;

namespace Server.Items
{
	public class SnowyTree2021 : Item
	{
		[Constructable]
		public SnowyTree2021() : base( 0x2377 )
		{
			Weight = 1.0;
			LootType = LootType.Blessed;
			Label2 = "zima 2021";
		}

		public SnowyTree2021( Serial serial ) : base( serial )
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