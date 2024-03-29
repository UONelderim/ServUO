// 07.12.21 :: juri :: zmiana nazwy

namespace Server.Items
{
	public class carpet4s : BaseAddon
	{
		public override BaseAddonDeed Deed
		{
			get
			{
				return new carpet4sDeed();
			}
		}

		[Constructable]
		public carpet4s()
		{
			AddonComponent ac = null;
			ac = new AddonComponent(2780);
			AddComponent(ac, -1, -1, 0);
			ac = new AddonComponent(2783);
			AddComponent(ac, -1, 0, 0);
			ac = new AddonComponent(2781);
			AddComponent(ac, -1, 1, 0);

			ac = new AddonComponent(2784);
			AddComponent(ac, 0, -1, 0);
			ac = new AddonComponent(2778);
			AddComponent(ac, 0, 0, 0);
			ac = new AddonComponent(2786);
			AddComponent(ac, 0, 1, 0);

			ac = new AddonComponent(2782);
			AddComponent(ac, 1, -1, 0);
			ac = new AddonComponent(2785);
			AddComponent(ac, 1, 0, 0);
			ac = new AddonComponent(2779);
			AddComponent(ac, 1, 1, 0);
		}

		public carpet4s(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write(0); // Version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}

	public class carpet4sDeed : BaseAddonDeed
	{
		public override BaseAddon Addon
		{
			get
			{
				return new carpet4s();
			}
		}

		[Constructable]
		public carpet4sDeed()
		{
			Name = "Maly Zloty dywan";
		}

		public carpet4sDeed(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write(0); // Version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}
}
