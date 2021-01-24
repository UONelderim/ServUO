/////////////////////
//Crafted By Broze///
/////////////////////
using System;
using Server;
using Server.Gumps;
using Server.Network;

namespace Server.Items
{

	public class ArtifactDeed : Item
	{

		[Constructable]
		public ArtifactDeed() : this( null )
		{
		}

		[Constructable]
		public ArtifactDeed ( string name ) : base ( 0x14F0 )
		{
			Name = "zwoj artefaktu";
			LootType = LootType.Blessed;
			Hue = 1161;
		}

		public ArtifactDeed ( Serial serial ) : base ( serial )
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
				from.SendGump( new ArtifactGump( from, this ) ); 
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