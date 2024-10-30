namespace Server.Items
{
	public class WielkiOgrodAddon : BaseAddon
	{
		public override BaseAddonDeed Deed
		{
			get
			{
				return new WielkiOgrodAddonDeed();
			}
		}

		[ Constructable ]
		public WielkiOgrodAddon()
		{
      AddComponent( new AddonComponent( 13001 ), 1, 0, 0 );
      AddComponent( new AddonComponent( 13001 ), 0, 0, 0 );
      AddComponent( new AddonComponent( 13001 ), 0, 1, 0 );
      AddComponent( new AddonComponent( 13001 ), 1, 1, 0 );
      AddComponent( new AddonComponent( 13001 ), 0, -1, 0 );
      AddComponent( new AddonComponent( 13001 ), 1, -1, 0 );
            AddComponent( new AddonComponent( 13001 ), 0, 2, 0 );
      AddComponent( new AddonComponent( 13001 ), 1, 2, 0 );
      AddComponent( new AddonComponent( 13001 ), 0, -2, 0 );
      AddComponent( new AddonComponent( 13001 ), 1, -2, 0 );
            AddComponent( new AddonComponent( 13001 ), -1, 0, 0 );
      AddComponent( new AddonComponent( 13001 ), 0, 0, 0 );
      AddComponent( new AddonComponent( 13001 ), 0, 1, 0 );
      AddComponent( new AddonComponent( 13001 ), -1, 1, 0 );
      AddComponent( new AddonComponent( 13001 ), 0, -1, 0 );
      AddComponent( new AddonComponent( 13001 ), -1, -1, 0 );
            AddComponent( new AddonComponent( 13001 ), 0, 2, 0 );
      AddComponent( new AddonComponent( 13001 ), -1, 2, 0 );
      AddComponent( new AddonComponent( 13001 ), 0, -2, 0 );
      AddComponent( new AddonComponent( 13001 ), -1, -2, 0 );
                  AddComponent( new AddonComponent( 13001 ), -2, 0, 0 );
      AddComponent( new AddonComponent( 13001 ), 0, 0, 0 );
      AddComponent( new AddonComponent( 13001 ), 0, 1, 0 );
      AddComponent( new AddonComponent( 13001 ), -2, 1, 0 );
      AddComponent( new AddonComponent( 13001 ), 0, -1, 0 );
      AddComponent( new AddonComponent( 13001 ), -2, -1, 0 );
            AddComponent( new AddonComponent( 13001 ), 0, 2, 0 );
      AddComponent( new AddonComponent( 13001 ), -2, 2, 0 );
      AddComponent( new AddonComponent( 13001 ), 0, -2, 0 );
      AddComponent( new AddonComponent( 13001 ), -2, -2, 0 );



		}

		public WielkiOgrodAddon( Serial serial ) : base( serial )
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

	public class WielkiOgrodAddonDeed : BaseAddonDeed
	{
		public override BaseAddon Addon
		{
			get
			{
				return new WielkiOgrodAddon();
			}
		}

		[Constructable]
		public WielkiOgrodAddonDeed()
		{
			Name = "Wielki Ogrod";
		}

		public WielkiOgrodAddonDeed( Serial serial ) : base( serial )
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