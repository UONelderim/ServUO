// 07.12.21 :: juri :: zmiana nazwy

using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class carpet5s : BaseAddon
	{
		public override BaseAddonDeed Deed
		{
			get
			{
				return new carpet5sDeed();
			}
		}

		[ Constructable ]
		public carpet5s()
		{
			AddonComponent ac = null;
			ac = new AddonComponent( 2788 );
			AddComponent( ac, -1, -1, 0 );
			ac = new AddonComponent( 2791 );
			AddComponent( ac, -1, 0, 0 );
			ac = new AddonComponent( 2789 );
			AddComponent( ac, -1, 1, 0 );
			
			ac = new AddonComponent( 2792 );
			AddComponent( ac, 0, -1, 0 );
			ac = new AddonComponent( 2795 );
			AddComponent( ac, 0, 0, 0 );
			ac = new AddonComponent( 2794 );
			AddComponent( ac, 0, 1, 0 );
			
			ac = new AddonComponent( 2790 );
			AddComponent( ac, 1, -1, 0 );
			ac = new AddonComponent( 2793 );
			AddComponent( ac, 1, 0, 0 );
			ac = new AddonComponent( 2787 );
			AddComponent( ac, 1, 1, 0 );
			
		}

		public carpet5s( Serial serial ) : base( serial )
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

	public class carpet5sDeed : BaseAddonDeed
	{
		public override BaseAddon Addon
		{
			get
			{
				return new carpet5s();
			}
		}

		[Constructable]
		public carpet5sDeed()
		{
			Name = "Maly Rubinowy dywan";
		}

		public carpet5sDeed( Serial serial ) : base( serial )
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
