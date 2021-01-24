// 07.12.21 :: juri :: zmiana nazwy

using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class carpet6m : BaseAddon
	{
		public override BaseAddonDeed Deed
		{
			get
			{
				return new carpet6mDeed();
			}
		}

		[ Constructable ]
		public carpet6m()
		{
			AddonComponent ac = null;
			ac = new AddonComponent( 2799 );
			AddComponent( ac, -2, -2, 0 );
			ac = new AddonComponent( 2802 );
			AddComponent( ac, -2, -1, 0 );
			ac = new AddonComponent( 2802 );
			AddComponent( ac, -2, 0, 0 );
			ac = new AddonComponent( 2802 );
			AddComponent( ac, -2, 1, 0 );
			ac = new AddonComponent( 2800 );
			AddComponent( ac, -2, 2, 0 );
			
			ac = new AddonComponent( 2803 );
			AddComponent( ac, -1, -2, 0 );
			ac = new AddonComponent( 2797 );
			AddComponent( ac, -1, -1, 0 );
			ac = new AddonComponent( 2797 );
			AddComponent( ac, -1, 0, 0 );
			ac = new AddonComponent( 2797 );
			AddComponent( ac, -1, 1, 0 );
			ac = new AddonComponent( 2805 );			
			AddComponent( ac, -1, 2, 0 );
			
		  ac = new AddonComponent( 2803 );
			AddComponent( ac, 0, -2, 0 );
			ac = new AddonComponent( 2797 );
			AddComponent( ac, 0, -1, 0 );
			ac = new AddonComponent( 2797 );
			AddComponent( ac, 0, 0, 0 );
			ac = new AddonComponent( 2797 );
			AddComponent( ac, 0, 1, 0 );
			ac = new AddonComponent( 2805 );			
			AddComponent( ac, 0, 2, 0 );
			
      ac = new AddonComponent( 2803 );
			AddComponent( ac, 1, -2, 0 );
			ac = new AddonComponent( 2797 );
			AddComponent( ac, 1, -1, 0 );
			ac = new AddonComponent( 2797 );
			AddComponent( ac, 1, 0, 0 );
			ac = new AddonComponent( 2797 );
			AddComponent( ac, 1, 1, 0 );
			ac = new AddonComponent( 2805 );			
			AddComponent( ac, 1, 2, 0 );
			
			ac = new AddonComponent( 2801 );
			AddComponent( ac, 2, -2, 0 );
			ac = new AddonComponent( 2804 );
			AddComponent( ac, 2, -1, 0 );
			ac = new AddonComponent( 2804 );
			AddComponent( ac, 2, 0, 0 );
			ac = new AddonComponent( 2804 );
			AddComponent( ac, 2, 1, 0 );
			ac = new AddonComponent( 2798 );
			AddComponent( ac, 2, 2, 0 );
			
		}

		public carpet6m( Serial serial ) : base( serial )
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

	public class carpet6mDeed : BaseAddonDeed
	{
		public override BaseAddon Addon
		{
			get
			{
				return new carpet6m();
			}
		}

		[Constructable]
		public carpet6mDeed()
		{
			Name = "Sredni Szafirowy dywan";
		}

		public carpet6mDeed( Serial serial ) : base( serial )
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
