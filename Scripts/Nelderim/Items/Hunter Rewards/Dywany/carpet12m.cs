using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class carpet12m : BaseAddon
	{
		public override BaseAddonDeed Deed
		{
			get
			{
				return new carpet12mDeed();
			}
		}

		[ Constructable ]
		public carpet12m()
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
			ac = new AddonComponent( 2752 );
			AddComponent( ac, -1, -1, 0 );
			ac = new AddonComponent( 2752 );
			AddComponent( ac, -1, 0, 0 );
			ac = new AddonComponent( 2752 );
			AddComponent( ac, -1, 1, 0 );
			ac = new AddonComponent( 2809 );			
			AddComponent( ac, -1, 2, 0 );
			
		  ac = new AddonComponent( 2807 );
			AddComponent( ac, 0, -2, 0 );
			ac = new AddonComponent( 2752 );
			AddComponent( ac, 0, -1, 0 );
			ac = new AddonComponent( 2752 );
			AddComponent( ac, 0, 0, 0 );
			ac = new AddonComponent( 2752 );
			AddComponent( ac, 0, 1, 0 );
			ac = new AddonComponent( 2809 );			
			AddComponent( ac, 0, 2, 0 );
			
      ac = new AddonComponent( 2807 );
			AddComponent( ac, 1, -2, 0 );
			ac = new AddonComponent( 2752 );
			AddComponent( ac, 1, -1, 0 );
			ac = new AddonComponent( 2752 );
			AddComponent( ac, 1, 0, 0 );
			ac = new AddonComponent( 2752 );
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

		public carpet12m( Serial serial ) : base( serial )
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

	public class carpet12mDeed : BaseAddonDeed
	{
		public override BaseAddon Addon
		{
			get
			{
				return new carpet12m();
			}
		}

		[Constructable]
		public carpet12mDeed()
		{
			Name = "Sredni niebieski dywan z wzorem [S]";
		}

		public carpet12mDeed( Serial serial ) : base( serial )
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
