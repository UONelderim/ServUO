namespace Server.Items.Crops
{
	public class ZrodloPajeczyna : WeedPlantZbieractwo
	{
		public override void CreateCrop(Mobile from, int count) { from.AddToBackpack(new SurowiecPajeczyna(count)); }

		public override bool GivesSeed => false;

		[Constructable]
		public ZrodloPajeczyna() : base(0x10D6) //0x26A1
		{
			Hue = 0x481;
			Name = "Klab pajeczych sieci";
			Stackable = true;
		}

		public ZrodloPajeczyna(Serial serial) : base(serial)
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

	public class SurowiecPajeczyna : WeedCropZbieractwo
	{
		public override void CreateReagent(Mobile from, int count) { from.AddToBackpack(new SpidersSilk(count)); }

		[Constructable]
		public SurowiecPajeczyna(int amount) : base(amount, 0x0DF6)
		{
			Hue = 0;
			Name = "Nawinieta pajeczyna";
			Stackable = true;
		}

		[Constructable]
		public SurowiecPajeczyna() : this(1)
		{
		}

		public SurowiecPajeczyna(Serial serial) : base(serial)
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
