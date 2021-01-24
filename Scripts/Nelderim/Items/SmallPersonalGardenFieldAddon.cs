using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class SmallPersonalGardenFieldAddon : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new SmallPersonalGardenFieldAddonDeed(); } }

		[ Constructable ]
		public SmallPersonalGardenFieldAddon()
		{
			AddonComponent ac = null;
			ac = new AddonComponent( 13001 );			AddComponent( ac, 1, 0, 0 );
			ac = new AddonComponent( 13001 );			AddComponent( ac, 0, 0, 0 );
			ac = new AddonComponent( 13001 );			AddComponent( ac, 0, 1, 0 );
			ac = new AddonComponent( 13001 );			AddComponent( ac, 1, 1, 0 );
			ac = new AddonComponent( 13001 );			AddComponent( ac, 0, -1, 0 );
			ac = new AddonComponent( 13001 );			AddComponent( ac, 1, -1, 0 );
			ac = new AddonComponent( 13001 );			AddComponent( ac, 0, 2, 0 );
			ac = new AddonComponent( 13001 );			AddComponent( ac, 1, 2, 0 );
			ac = new AddonComponent( 13001 );			AddComponent( ac, 0, -2, 0 );
			ac = new AddonComponent( 13001 );			AddComponent( ac, 1, -2, 0 );
			ac = new AddonComponent( 13001 );			AddComponent( ac, 0, 3, 0 );
			ac = new AddonComponent( 13001 );			AddComponent( ac, 1, 3, 0 );
			ac = new AddonComponent( 13001 );			AddComponent( ac, 0, -3, 0 );
			ac = new AddonComponent( 13001 );			AddComponent( ac, 1, -3, 0 );
			ac = new AddonComponent( 13001 );			AddComponent( ac, 0, 4, 0 );
			ac = new AddonComponent( 13001 );			AddComponent( ac, 1, 4, 0 );
			ac = new AddonComponent( 954 );			AddComponent( ac, 0, -4, 0 );
			ac = new AddonComponent( 954 );			AddComponent( ac, 1, -4, 0 );
			ac = new AddonComponent( 13001 );			AddComponent( ac, 0, 5, 0 );
			ac = new AddonComponent( 13001 );			AddComponent( ac, 1, 5, 0 );
			ac = new AddonComponent( 13001 );			AddComponent( ac, -1, 0, 0 );
			ac = new AddonComponent( 13001 );			AddComponent( ac, -1, 1, 0 );
			ac = new AddonComponent( 13001 );			AddComponent( ac, -1, -1, 0 );
			ac = new AddonComponent( 13001 );			AddComponent( ac, -1, 2, 0 );
			ac = new AddonComponent( 13001 );			AddComponent( ac, -1, -2, 0 );
			ac = new AddonComponent( 13001 );			AddComponent( ac, -1, 3, 0 );
			ac = new AddonComponent( 13001 );			AddComponent( ac, -1, -3, 0 );
			ac = new AddonComponent( 13001 );			AddComponent( ac, -1, 4, 0 );
			ac = new AddonComponent( 954 );			AddComponent( ac, -1, -4, 0 );
			ac = new AddonComponent( 13001 );			AddComponent( ac, -1, 5, 0 );
			ac = new AddonComponent( 13001 );			AddComponent( ac, 2, 0, 0 );
			ac = new AddonComponent( 13001 );			AddComponent( ac, 2, 1, 0 );
			ac = new AddonComponent( 13001 );			AddComponent( ac, -2, 0, 0 );
			ac = new AddonComponent( 13001 );			AddComponent( ac, -2, 1, 0 );
			ac = new AddonComponent( 13001 );			AddComponent( ac, 2, -1, 0 );
			ac = new AddonComponent( 13001 );			AddComponent( ac, -2, -1, 0 );
			ac = new AddonComponent( 13001 );			AddComponent( ac, 2, 2, 0 );
			ac = new AddonComponent( 13001 );			AddComponent( ac, -2, 2, 0 );
			ac = new AddonComponent( 13001 );			AddComponent( ac, 2, -2, 0 );
			ac = new AddonComponent( 13001 );			AddComponent( ac, -2, -2, 0 );
			ac = new AddonComponent( 13001 );			AddComponent( ac, 2, 3, 0 );
			ac = new AddonComponent( 13001 );			AddComponent( ac, -2, 3, 0 );
			ac = new AddonComponent( 13001 );			AddComponent( ac, 2, -3, 0 );
			ac = new AddonComponent( 13001 );			AddComponent( ac, -2, -3, 0 );
			ac = new AddonComponent( 13001 );			AddComponent( ac, 2, 4, 0 );
			ac = new AddonComponent( 13001 );			AddComponent( ac, -2, 4, 0 );
			ac = new AddonComponent( 954 );			AddComponent( ac, 2, -4, 0 );
			ac = new AddonComponent( 954 );			AddComponent( ac, -2, -4, 0 );
			ac = new AddonComponent( 13001 );			AddComponent( ac, 2, 5, 0 );
			ac = new AddonComponent( 954 );			AddComponent( ac, 2, 5, 0 );
			ac = new AddonComponent( 13001 );			AddComponent( ac, -2, 5, 0 );
			ac = new AddonComponent( 954 );			AddComponent( ac, -2, 5, 0 );
			ac = new AddonComponent( 13001 );			AddComponent( ac, 3, 0, 0 );
			ac = new AddonComponent( 13001 );			AddComponent( ac, 3, 1, 0 );
			ac = new AddonComponent( 13001 );			AddComponent( ac, -3, 0, 0 );
			ac = new AddonComponent( 13001 );			AddComponent( ac, -3, 1, 0 );
			ac = new AddonComponent( 13001 );			AddComponent( ac, 3, -1, 0 );
			ac = new AddonComponent( 13001 );			AddComponent( ac, -3, -1, 0 );
			ac = new AddonComponent( 13001 );			AddComponent( ac, 3, 2, 0 );
			ac = new AddonComponent( 13001 );			AddComponent( ac, -3, 2, 0 );
			ac = new AddonComponent( 13001 );			AddComponent( ac, 3, -2, 0 );
			ac = new AddonComponent( 13001 );			AddComponent( ac, -3, -2, 0 );
			ac = new AddonComponent( 13001 );			AddComponent( ac, 3, 3, 0 );
			ac = new AddonComponent( 13001 );			AddComponent( ac, -3, 3, 0 );
			ac = new AddonComponent( 13001 );			AddComponent( ac, 3, -3, 0 );
			ac = new AddonComponent( 13001 );			AddComponent( ac, -3, -3, 0 );
			ac = new AddonComponent( 13001 );			AddComponent( ac, 3, 4, 0 );
			ac = new AddonComponent( 13001 );			AddComponent( ac, -3, 4, 0 );
			ac = new AddonComponent( 954 );			AddComponent( ac, 3, -4, 0 );
			ac = new AddonComponent( 954 );			AddComponent( ac, -3, -4, 0 );
			ac = new AddonComponent( 13001 );			AddComponent( ac, 3, 5, 0 );
			ac = new AddonComponent( 954 );			AddComponent( ac, 3, 5, 0 );
			ac = new AddonComponent( 13001 );			AddComponent( ac, -3, 5, 0 );
			ac = new AddonComponent( 954 );			AddComponent( ac, -3, 5, 0 );
			ac = new AddonComponent( 13001 );			AddComponent( ac, 4, 0, 0 );
			ac = new AddonComponent( 13001 );			AddComponent( ac, 4, 1, 0 );
			ac = new AddonComponent( 953 );			AddComponent( ac, 4, 0, 0 );
			ac = new AddonComponent( 953 );			AddComponent( ac, 4, 1, 0 );
			ac = new AddonComponent( 953 );			AddComponent( ac, -4, 0, 0 );
			ac = new AddonComponent( 953 );			AddComponent( ac, -4, 1, 0 );
			ac = new AddonComponent( 13001 );			AddComponent( ac, 4, -1, 0 );
			ac = new AddonComponent( 953 );			AddComponent( ac, 4, -1, 0 );
			ac = new AddonComponent( 953 );			AddComponent( ac, -4, -1, 0 );
			ac = new AddonComponent( 13001 );			AddComponent( ac, 4, 2, 0 );
			ac = new AddonComponent( 953 );			AddComponent( ac, 4, 2, 0 );
			ac = new AddonComponent( 953 );			AddComponent( ac, -4, 2, 0 );
			ac = new AddonComponent( 13001 );			AddComponent( ac, 4, -2, 0 );
			ac = new AddonComponent( 953 );			AddComponent( ac, 4, -2, 0 );
			ac = new AddonComponent( 953 );			AddComponent( ac, -4, -2, 0 );
			ac = new AddonComponent( 13001 );			AddComponent( ac, 4, 3, 0 );
			ac = new AddonComponent( 953 );			AddComponent( ac, 4, 3, 0 );
			ac = new AddonComponent( 953 );			AddComponent( ac, -4, 3, 0 );
			ac = new AddonComponent( 13001 );			AddComponent( ac, 4, -3, 0 );
			ac = new AddonComponent( 953 );			AddComponent( ac, 4, -3, 0 );
			ac = new AddonComponent( 953 );			AddComponent( ac, -4, -3, 0 );
			ac = new AddonComponent( 13001 );			AddComponent( ac, 4, 4, 0 );
			ac = new AddonComponent( 953 );			AddComponent( ac, 4, 4, 0 );
			ac = new AddonComponent( 953 );			AddComponent( ac, -4, 4, 0 );
			ac = new AddonComponent( 954 );			AddComponent( ac, 4, -4, 0 );
			ac = new AddonComponent( 955 );			AddComponent( ac, 4, -4, 0 );
			ac = new AddonComponent( 955 );			AddComponent( ac, -4, -4, 0 );
			ac = new AddonComponent( 13001 );			AddComponent( ac, 4, 5, 0 );
			ac = new AddonComponent( 953 );			AddComponent( ac, 4, 5, 0 );
			ac = new AddonComponent( 954 );			AddComponent( ac, 4, 5, 0 );
			ac = new AddonComponent( 955 );			AddComponent( ac, 4, 5, 0 );
			ac = new AddonComponent( 953 );			AddComponent( ac, -4, 5, 0 );
			ac = new AddonComponent( 955 );			AddComponent( ac, -4, 5, 2 );

 		}

		public SmallPersonalGardenFieldAddon( Serial serial ) : base( serial ) { }

		public override void Serialize( GenericWriter writer ) { base.Serialize( writer ); writer.Write( 0 ); }

		public override void Deserialize( GenericReader reader ) { base.Deserialize( reader ); int version = reader.ReadInt();}
	}

	public class SmallPersonalGardenFieldAddonDeed : BaseAddonDeed
	{
		public override BaseAddon Addon { get { return new SmallPersonalGardenFieldAddon(); } }

		[Constructable]
		public SmallPersonalGardenFieldAddonDeed()
		{
			Name = "Mały Ogródek";
		}

		public SmallPersonalGardenFieldAddonDeed( Serial serial ) : base( serial ) { }

		public override void Serialize( GenericWriter writer ) { base.Serialize( writer ); writer.Write( 0 ); }

		public override void	Deserialize( GenericReader reader ) { base.Deserialize( reader ); int version = reader.ReadInt(); }
	}
}