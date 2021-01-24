using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class carpet1m : BaseAddon
	{
		public override BaseAddonDeed Deed
		{
			get
			{
				return new carpet1mDeed();
			}
		}

		[ Constructable ]
		public carpet1m()
		{
			AddonComponent ac = null;
			ac = new AddonComponent( 2755 );
			AddComponent( ac, -2, -2, 0 );
			ac = new AddonComponent( 2806 );
			AddComponent( ac, -2, -1, 0 );
			ac = new AddonComponent( 2806 );
			AddComponent( ac, -2, 0, 0 );
			ac = new AddonComponent( 2806 );
			AddComponent( ac, -2, 1, 0 );
			ac = new AddonComponent( 2756 );
			AddComponent( ac, -2, 2, 0 );
			
			ac = new AddonComponent( 2807 );
			AddComponent( ac, -1, -2, 0 );
			ac = new AddonComponent( 2750 );
			AddComponent( ac, -1, -1, 0 );
			ac = new AddonComponent( 2750 );
			AddComponent( ac, -1, 0, 0 );
			ac = new AddonComponent( 2750 );
			AddComponent( ac, -1, 1, 0 );
			ac = new AddonComponent( 2809 );			
			AddComponent( ac, -1, 2, 0 );
			
		  ac = new AddonComponent( 2807 );
			AddComponent( ac, 0, -2, 0 );
			ac = new AddonComponent( 2750 );
			AddComponent( ac, 0, -1, 0 );
			ac = new AddonComponent( 2750 );
			AddComponent( ac, 0, 0, 0 );
			ac = new AddonComponent( 2750 );
			AddComponent( ac, 0, 1, 0 );
			ac = new AddonComponent( 2809 );			
			AddComponent( ac, 0, 2, 0 );
			
      ac = new AddonComponent( 2807 );
			AddComponent( ac, 1, -2, 0 );
			ac = new AddonComponent( 2750 );
			AddComponent( ac, 1, -1, 0 );
			ac = new AddonComponent( 2750 );
			AddComponent( ac, 1, 0, 0 );
			ac = new AddonComponent( 2750 );
			AddComponent( ac, 1, 1, 0 );
			ac = new AddonComponent( 2809 );			
			AddComponent( ac, 1, 2, 0 );
			
			ac = new AddonComponent( 2757 );
			AddComponent( ac, 2, -2, 0 );
			ac = new AddonComponent( 2808 );
			AddComponent( ac, 2, -1, 0 );
			ac = new AddonComponent( 2808 );
			AddComponent( ac, 2, 0, 0 );
			ac = new AddonComponent( 2808 );
			AddComponent( ac, 2, 1, 0 );
			ac = new AddonComponent( 2754 );
			AddComponent( ac, 2, 2, 0 );
			
		}

		public carpet1m( Serial serial ) : base( serial )
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

	public class carpet1mDeed : BaseAddonDeed
	{
		public override BaseAddon Addon
		{
			get
			{
				return new carpet1m();
			}
		}

		[Constructable]
		public carpet1mDeed()
		{
			Name = "Sredni niebieski dywan";
		}

		public carpet1mDeed( Serial serial ) : base( serial )
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
