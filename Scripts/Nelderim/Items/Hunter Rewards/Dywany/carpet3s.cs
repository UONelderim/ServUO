// 07.12.21 :: juri :: zmiana nazwy

using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class carpet3s : BaseAddon
	{
		public override BaseAddonDeed Deed
		{
			get
			{
				return new carpet3sDeed();
			}
		}

		[ Constructable ]
		public carpet3s()
		{
			AddonComponent ac = null;
			ac = new AddonComponent( 2771 );
			AddComponent( ac, -1, -1, 0 );
			ac = new AddonComponent( 2774 );
			AddComponent( ac, -1, 0, 0 );
			ac = new AddonComponent( 2772 );
			AddComponent( ac, -1, 1, 0 );
			
			ac = new AddonComponent( 2775 );
			AddComponent( ac, 0, -1, 0 );
			ac = new AddonComponent( 2769 );
			AddComponent( ac, 0, 0, 0 );
			ac = new AddonComponent( 2777 );
			AddComponent( ac, 0, 1, 0 );
			
			ac = new AddonComponent( 2773 );
			AddComponent( ac, 1, -1, 0 );
			ac = new AddonComponent( 2776 );
			AddComponent( ac, 1, 0, 0 );
			ac = new AddonComponent( 2770 );
			AddComponent( ac, 1, 1, 0 );
			
		}

		public carpet3s( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( 0 ); // Version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}

	public class carpet3sDeed : BaseAddonDeed
	{
		public override BaseAddon Addon
		{
			get
			{
				return new carpet3s();
			}
		}

		[Constructable]
		public carpet3sDeed()
		{
			Name = "Maly Zlocisty dywan";
		}

		public carpet3sDeed( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( 0 ); // Version
		}

		public override void	Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}
