namespace Server.Items
{
	public class SredniOgrodAddon : BaseAddon
	{
		public override BaseAddonDeed Deed
		{
			get
			{
				return new SredniOgrodAddonDeed();
			}
		}

		[ Constructable ]
		public SredniOgrodAddon()
		{
      AddComponent( new AddonComponent( 13001 ), 1, 0, 0 );
      AddComponent( new AddonComponent( 13001 ), 0, 0, 0 );
      AddComponent( new AddonComponent( 13001 ), 0, 1, 0 );
      AddComponent( new AddonComponent( 13001 ), 1, 1, 0 );
      AddComponent( new AddonComponent( 13001 ), 0, -1, 0 );
      AddComponent( new AddonComponent( 13001 ), 1, -1, 0 );
      AddComponent( new AddonComponent( 13001 ), -1, 0, 0 );
      AddComponent( new AddonComponent( 13001 ), 0, 0, 0 );
      AddComponent( new AddonComponent( 13001 ), 0, 1, 0 );
      AddComponent( new AddonComponent( 13001 ), -1, 1, 0 );
      AddComponent( new AddonComponent( 13001 ), 0, -1, 0 );
      AddComponent( new AddonComponent( 13001 ), -1, -1, 0 );

		}

		public SredniOgrodAddon( Serial serial ) : base( serial )
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

	public class SredniOgrodAddonDeed : BaseAddonDeed
	{
		public override BaseAddon Addon
		{
			get
			{
				return new SredniOgrodAddon();
			}
		}

		[Constructable]
		public SredniOgrodAddonDeed()
		{
			Name = "Sredni Ogrod";
		}

		public SredniOgrodAddonDeed( Serial serial ) : base( serial )
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