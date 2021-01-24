// 07.12.21 :: juri :: zmiana nazwy

using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class carpet5m : BaseAddon
	{
		public override BaseAddonDeed Deed
		{
			get
			{
				return new carpet5mDeed();
			}
		}

		[ Constructable ]
		public carpet5m()
		{
			AddonComponent ac = null;
			ac = new AddonComponent( 2788 );
			AddComponent( ac, -2, -2, 0 );
			ac = new AddonComponent( 2791 );
			AddComponent( ac, -2, -1, 0 );
			ac = new AddonComponent( 2791 );
			AddComponent( ac, -2, 0, 0 );
			ac = new AddonComponent( 2791 );
			AddComponent( ac, -2, 1, 0 );
			ac = new AddonComponent( 2789 );
			AddComponent( ac, -2, 2, 0 );
			
			ac = new AddonComponent( 2792 );
			AddComponent( ac, -1, -2, 0 );
			ac = new AddonComponent( 2795 );
			AddComponent( ac, -1, -1, 0 );
			ac = new AddonComponent( 2795 );
			AddComponent( ac, -1, 0, 0 );
			ac = new AddonComponent( 2795 );
			AddComponent( ac, -1, 1, 0 );
			ac = new AddonComponent( 2794 );			
			AddComponent( ac, -1, 2, 0 );
			
		  ac = new AddonComponent( 2792 );
			AddComponent( ac, 0, -2, 0 );
			ac = new AddonComponent( 2795 );
			AddComponent( ac, 0, -1, 0 );
			ac = new AddonComponent( 2795 );
			AddComponent( ac, 0, 0, 0 );
			ac = new AddonComponent( 2795 );
			AddComponent( ac, 0, 1, 0 );
			ac = new AddonComponent( 2794 );			
			AddComponent( ac, 0, 2, 0 );
			
      ac = new AddonComponent( 2792 );
			AddComponent( ac, 1, -2, 0 );
			ac = new AddonComponent( 2795 );
			AddComponent( ac, 1, -1, 0 );
			ac = new AddonComponent( 2795 );
			AddComponent( ac, 1, 0, 0 );
			ac = new AddonComponent( 2795 );
			AddComponent( ac, 1, 1, 0 );
			ac = new AddonComponent( 2794 );			
			AddComponent( ac, 1, 2, 0 );
			
			ac = new AddonComponent( 2790 );
			AddComponent( ac, 2, -2, 0 );
			ac = new AddonComponent( 2793 );
			AddComponent( ac, 2, -1, 0 );
			ac = new AddonComponent( 2793 );
			AddComponent( ac, 2, 0, 0 );
			ac = new AddonComponent( 2793 );
			AddComponent( ac, 2, 1, 0 );
			ac = new AddonComponent( 2787 );
			AddComponent( ac, 2, 2, 0 );
			
		}

		public carpet5m( Serial serial ) : base( serial )
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

	public class carpet5mDeed : BaseAddonDeed
	{
		public override BaseAddon Addon
		{
			get
			{
				return new carpet5m();
			}
		}

		[Constructable]
		public carpet5mDeed()
		{
			Name = "Sredni Rubinowy dywan";
		}

		public carpet5mDeed( Serial serial ) : base( serial )
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
