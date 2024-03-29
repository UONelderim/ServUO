// 07.12.21 :: juri :: zmiana nazwy

namespace Server.Items
{
	public class carpet3m : BaseAddon
	{
		public override BaseAddonDeed Deed
		{
			get
			{
				return new carpet3mDeed();
			}
		}

		[Constructable]
		public carpet3m()
		{
			AddonComponent ac = null;
			ac = new AddonComponent(2771);
			AddComponent(ac, -2, -2, 0);
			ac = new AddonComponent(2774);
			AddComponent(ac, -2, -1, 0);
			ac = new AddonComponent(2774);
			AddComponent(ac, -2, 0, 0);
			ac = new AddonComponent(2774);
			AddComponent(ac, -2, 1, 0);
			ac = new AddonComponent(2772);
			AddComponent(ac, -2, 2, 0);

			ac = new AddonComponent(2775);
			AddComponent(ac, -1, -2, 0);
			ac = new AddonComponent(2769);
			AddComponent(ac, -1, -1, 0);
			ac = new AddonComponent(2769);
			AddComponent(ac, -1, 0, 0);
			ac = new AddonComponent(2769);
			AddComponent(ac, -1, 1, 0);
			ac = new AddonComponent(2777);
			AddComponent(ac, -1, 2, 0);

			ac = new AddonComponent(2775);
			AddComponent(ac, 0, -2, 0);
			ac = new AddonComponent(2769);
			AddComponent(ac, 0, -1, 0);
			ac = new AddonComponent(2769);
			AddComponent(ac, 0, 0, 0);
			ac = new AddonComponent(2769);
			AddComponent(ac, 0, 1, 0);
			ac = new AddonComponent(2777);
			AddComponent(ac, 0, 2, 0);

			ac = new AddonComponent(2775);
			AddComponent(ac, 1, -2, 0);
			ac = new AddonComponent(2769);
			AddComponent(ac, 1, -1, 0);
			ac = new AddonComponent(2769);
			AddComponent(ac, 1, 0, 0);
			ac = new AddonComponent(2769);
			AddComponent(ac, 1, 1, 0);
			ac = new AddonComponent(2777);
			AddComponent(ac, 1, 2, 0);

			ac = new AddonComponent(2773);
			AddComponent(ac, 2, -2, 0);
			ac = new AddonComponent(2776);
			AddComponent(ac, 2, -1, 0);
			ac = new AddonComponent(2776);
			AddComponent(ac, 2, 0, 0);
			ac = new AddonComponent(2776);
			AddComponent(ac, 2, 1, 0);
			ac = new AddonComponent(2770);
			AddComponent(ac, 2, 2, 0);
		}

		public carpet3m(Serial serial) : base(serial)
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

	public class carpet3mDeed : BaseAddonDeed
	{
		public override BaseAddon Addon
		{
			get
			{
				return new carpet3m();
			}
		}

		[Constructable]
		public carpet3mDeed()
		{
			Name = "Sredni Zlocisty dywan";
		}

		public carpet3mDeed(Serial serial) : base(serial)
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
