namespace Server.Items
{
	public class BarrelOfWaterAddon : BaseAddon, IWaterSource
	{
		public override BaseAddonDeed Deed { get { return new BarrelOfWaterDeed(); } }

		[Constructable]
		public BarrelOfWaterAddon()
		{
			AddComponent(new AddonComponent(5453), 0, 0, 0);
		}

		public BarrelOfWaterAddon(Serial serial) : base(serial)
		{
		}

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

		public int Quantity
		{
			get { return 400; }
			set { }
		}
	}

	public class BarrelOfWaterDeed : BaseAddonDeed
	{
		public override BaseAddon Addon { get { return new BarrelOfWaterAddon(); } }
		public override int LabelNumber { get { return 1025453; } } // beczka z woda

		[Constructable]
		public BarrelOfWaterDeed()
		{
		}

		public BarrelOfWaterDeed(Serial serial) : base(serial)
		{
		}

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
