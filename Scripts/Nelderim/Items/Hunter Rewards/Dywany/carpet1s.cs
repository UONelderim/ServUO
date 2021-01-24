using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class carpet1s : BaseAddon
	{
		public override BaseAddonDeed Deed
		{
			get
			{
				return new carpet1sDeed();
			}
		}

		[ Constructable ]
		public carpet1s()
		{
			AddonComponent ac = null;
			ac = new AddonComponent( 2755 );
			AddComponent( ac, -1, -1, 0 );
			ac = new AddonComponent( 2806 );
			AddComponent( ac, -1, 0, 0 );
			ac = new AddonComponent( 2756 );
			AddComponent( ac, -1, 1, 0 );
			
			ac = new AddonComponent( 2807 );
			AddComponent( ac, 0, -1, 0 );
			ac = new AddonComponent( 2750 );
			AddComponent( ac, 0, 0, 0 );
			ac = new AddonComponent( 2809 );
			AddComponent( ac, 0, 1, 0 );
			
			ac = new AddonComponent( 2757 );
			AddComponent( ac, 1, -1, 0 );
			ac = new AddonComponent( 2808 );
			AddComponent( ac, 1, 0, 0 );
			ac = new AddonComponent( 2754 );
			AddComponent( ac, 1, 1, 0 );
			
		}

		public carpet1s( Serial serial ) : base( serial )
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

	public class carpet1sDeed : BaseAddonDeed
	{
		public override BaseAddon Addon
		{
			get
			{
				return new carpet1s();
			}
		}

		[Constructable]
		public carpet1sDeed()
		{
			Name = "Maly niebieski dywan";
		}

		public carpet1sDeed( Serial serial ) : base( serial )
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
