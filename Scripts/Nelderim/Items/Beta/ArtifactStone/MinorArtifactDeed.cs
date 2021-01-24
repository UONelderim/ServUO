//Written By WeedGod of WeedGods Workshop
using System;
using Server;
using Server.Gumps;
using Server.Network;

namespace Server.Items
{

	public class MinorArtifactDeed : Item
	{

		[Constructable]
		public MinorArtifactDeed() : this( null )
		{
		}

		[Constructable]
		public MinorArtifactDeed ( string name ) : base ( 0x14F0 )
		{
			Name = "kamien mniejszych artefaktow";
			LootType = LootType.Blessed;
			Hue = 1173;
		}

		public MinorArtifactDeed ( Serial serial ) : base ( serial )
		{
		}

      		public override void OnDoubleClick( Mobile from ) 
      		{
			if ( !IsChildOf( from.Backpack ) )
			{
				from.SendLocalizedMessage( 1042001 );
			}
			else
			{ 
				from.SendGump( new MinorArtifactGump( from, this ) ); 
			}
		}

		public override void Serialize ( GenericWriter writer)
		{
			base.Serialize ( writer );

			writer.Write ( (int) 0);
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize ( reader );

			int version = reader.ReadInt();
		}
	}
}
