namespace Server.Items
{
	public class MalyOgrodAddon : BaseAddon
	{
		public override BaseAddonDeed Deed
		{
			get
			{
				return new MalyOgrodAddonDeed();
			}
		}

		[ Constructable ]
		public MalyOgrodAddon()
		{
      AddComponent( new AddonComponent( 13001 ), 1, 0, 0 );
      AddComponent( new AddonComponent( 13001 ), 0, 0, 0 );
      AddComponent( new AddonComponent( 13001 ), 0, 1, 0 );
      AddComponent( new AddonComponent( 13001 ), 1, 1, 0 );
      AddComponent( new AddonComponent( 13001 ), 0, -1, 0 );
      AddComponent( new AddonComponent( 13001 ), 1, -1, 0 );

		}

		public MalyOgrodAddon( Serial serial ) : base( serial )
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

	public class MalyOgrodAddonDeed : BaseAddonDeed
	{
		public override BaseAddon Addon
		{
			get
			{
				return new MalyOgrodAddon();
			}
		}

		[Constructable]
		public MalyOgrodAddonDeed()
		{
			Name = "Maly Ogrod";
		}

		public MalyOgrodAddonDeed( Serial serial ) : base( serial )
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