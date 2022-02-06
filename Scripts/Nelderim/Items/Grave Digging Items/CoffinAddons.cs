namespace Server.Items
{
	public class CoffinWestAddon : BaseAddon
	{
		[Constructable]
		public CoffinWestAddon()
		{
			AddComponent(new AddonComponent(0x1C42), 0, 0, 0);
			AddComponent(new AddonComponent(0x1C41), -1, 0, 0); // top
			AddComponent(new AddonComponent(0x1C43), 1, 0, 0); // bottom
		}

		public CoffinWestAddon(Serial serial)
			: base(serial)
		{
		}

		public override BaseAddonDeed Deed { get { return new CoffinWestDeed(); } }

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

	public class CoffinWestDeed : BaseAddonDeed
	{
		[Constructable]
		public CoffinWestDeed()
		{
			Name = "trumna (zachod)";
		}

		public CoffinWestDeed(Serial serial)
			: base(serial)
		{
		}

		public override BaseAddon Addon { get { return new CoffinWestAddon(); } }

		//public override int LabelNumber => 1044333;
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

	public class CoffinNorthAddon : BaseAddon
	{
		[Constructable]
		public CoffinNorthAddon()
		{
			AddComponent(new AddonComponent(0x1C4F), 0, -1, 0);
			AddComponent(new AddonComponent(0x1C50), 0, 0, 0); // top
			AddComponent(new AddonComponent(0x1C51), 0, 1, 0); // bottom
		}

		public CoffinNorthAddon(Serial serial)
			: base(serial)
		{
		}

		public override BaseAddonDeed Deed { get { return new CoffinNorthDeed(); } }

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

	public class CoffinNorthDeed : BaseAddonDeed
	{
		[Constructable]
		public CoffinNorthDeed()
		{
			Name = "trumna (polnoc)";
		}

		public CoffinNorthDeed(Serial serial)
			: base(serial)
		{
		}

		public override BaseAddon Addon { get { return new CoffinNorthAddon(); } }

		//public override int LabelNumber => 1044333;
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
