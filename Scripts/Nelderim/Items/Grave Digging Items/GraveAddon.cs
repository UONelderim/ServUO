namespace Server.Items
{
	public class GraveWestAddon : BaseAddon
	{
		[Constructable]
		public GraveWestAddon()
		{
			AddComponent(new AddonComponent(0xEE2), 0, 0, 0);
			AddComponent(new AddonComponent(0xEE0), 1, 0, 0); // bottom
		}

		public GraveWestAddon(Serial serial)
			: base(serial)
		{
		}

		public override BaseAddonDeed Deed { get { return new GraveWestDeed(); } }

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}

	public class GraveWestDeed : BaseAddonDeed
	{
		[Constructable]
		public GraveWestDeed()
		{
			Name = "gorb(zachod)";
		}

		public GraveWestDeed(Serial serial)
			: base(serial)
		{
		}

		public override BaseAddon Addon { get { return new GraveWestAddon(); } }

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}

	public class GraveNorthAddon : BaseAddon
	{
		[Constructable]
		public GraveNorthAddon()
		{
			AddComponent(new AddonComponent(0xEE1), 0, -1, 0);
			AddComponent(new AddonComponent(0xED3), 0, 0, 0); // bottom
		}

		public GraveNorthAddon(Serial serial)
			: base(serial)
		{
		}

		public override BaseAddonDeed Deed { get { return new GraveNorthDeed(); } }

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}

	public class GraveNorthDeed : BaseAddonDeed
	{
		[Constructable]
		public GraveNorthDeed()
		{
			Name = "grob (polnoc)";
		}

		public GraveNorthDeed(Serial serial)
			: base(serial)
		{
		}

		public override BaseAddon Addon { get { return new GraveNorthAddon(); } }

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}
}
