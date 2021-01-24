using System;
using Server.Network;
using Server.Prompts;
using Server.Items;
using Server.Targeting;
using Server.Gumps;

namespace Server.Items
{
	public class KarmaDeed : Item 
	{
		[Constructable]
		public KarmaDeed() : base( 0x14F0 )
		{
			Weight = 1.0;
			Hue = 1111;
			Name = "Karma deed";
			LootType = LootType.Blessed;
		}

		public KarmaDeed( Serial serial ) : base( serial )
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
			LootType = LootType.Blessed;

			int version = reader.ReadInt();
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( !IsChildOf( from.Backpack ) ) 
			{
				 from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
			}
			else
			{
				from.SendGump( new KarmaDeedGump( (Mobile) from ) );
			}
		}	
	}
}