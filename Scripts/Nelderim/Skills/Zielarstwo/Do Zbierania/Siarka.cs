namespace Server.Items.Crops
{
	public class ZrodloSiarka : WeedPlantZbieractwo
	{
		public override void CreateCrop(Mobile from, int count) { from.AddToBackpack(new SurowiecSiarka(count)); }

		public override bool GivesSeed { get { return false; } }

		[Constructable]
		public ZrodloSiarka() : base(0x19B7)
		{
			Hue = 0x31;
			Name = "Bryla pirytu";
		}

		public ZrodloSiarka(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write(0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}

	public class SurowiecSiarka : WeedCropZbieractwo
	{
		public override void CreateReagent(Mobile from, int count) { from.AddToBackpack(new SulfurousAsh(count)); }

		[Constructable]
		public SurowiecSiarka(int amount) : base(amount, 0x2244)
		{
			Hue = 0x31;
			Name = "Odlamek pirytu";
		}

		[Constructable]
		public SurowiecSiarka() : this(1)
		{
		}

		public SurowiecSiarka(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write(0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}
}
