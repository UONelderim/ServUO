using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class carpet11m : BaseAddon
	{
		public override BaseAddonDeed Deed
		{
			get
			{
				return new carpet11mDeed();
			}
		}

		[ Constructable ]
		public carpet11m()
		{
			
			AddComponent( new AddonComponent( 2755 ), -2, -2, 0 );
			
			AddComponent( new AddonComponent( 2806 ), -2, -1, 0 );
			
			AddComponent( new AddonComponent( 2806 ), -2, 0, 0 );
			
			AddComponent( new AddonComponent( 2806 ), -2, 1, 0 );
			
			AddComponent( new AddonComponent( 2756 ), -2, 2, 0 );
			
			
			AddComponent( new AddonComponent( 2807 ), -1, -2, 0 );
			
			AddComponent( new AddonComponent( 2751 ), -1, -1, 0 );
			
			AddComponent( new AddonComponent( 2751 ), -1, 0, 0 );
			
			AddComponent( new AddonComponent( 2751 ), -1, 1, 0 );
						
			AddComponent( new AddonComponent( 2809 ), -1, 2, 0 );
			
		  
			AddComponent( new AddonComponent( 2807 ), 0, -2, 0 );
			
			AddComponent( new AddonComponent( 2751 ), 0, -1, 0 );
			
			AddComponent( new AddonComponent( 2751 ), 0, 0, 0 );
			
			AddComponent( new AddonComponent( 2751 ), 0, 1, 0 );
						
			AddComponent( new AddonComponent( 2809 ), 0, 2, 0 );
			
      
			AddComponent( new AddonComponent( 2807 ), 1, -2, 0 );
			
			AddComponent( new AddonComponent( 2751 ), 1, -1, 0 );
			
			AddComponent( new AddonComponent( 2751 ), 1, 0, 0 );
			
			AddComponent( new AddonComponent( 2751 ), 1, 1, 0 );
						
			AddComponent( new AddonComponent( 2809 ), 1, 2, 0 );
			
			
			AddComponent( new AddonComponent( 2757 ), 2, -2, 0 );
			
			AddComponent( new AddonComponent( 2808 ), 2, -1, 0 );
			
			AddComponent( new AddonComponent( 2808 ), 2, 0, 0 );
			
			AddComponent( new AddonComponent( 2808 ), 2, 1, 0 );
			
			AddComponent( new AddonComponent( 2754 ), 2, 2, 0 );
			
		}

		public carpet11m( Serial serial ) : base( serial )
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

	public class carpet11mDeed : BaseAddonDeed
	{
		public override BaseAddon Addon
		{
			get
			{
				return new carpet11m();
			}
		}
		

		[Constructable]
		public carpet11mDeed()
		{
			Name = "Sredni niebieski dywan z wzorem [E]";
		}

		public carpet11mDeed( Serial serial ) : base( serial )
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
