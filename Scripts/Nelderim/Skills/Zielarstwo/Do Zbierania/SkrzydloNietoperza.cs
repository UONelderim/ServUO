namespace Server.Items.Crops
{
	public class ZrodloSkrzydloNietoperza : WeedPlantZbieractwo
	{
		public override void CreateCrop(Mobile from, int count)
		{
			from.AddToBackpack(new SurowiecSkrzydloNietoperza(count));
		}

		public override bool GivesSeed => false;

		[Constructable]
		public ZrodloSkrzydloNietoperza() : base(0x2631)
		{
			Hue = 0x420;
			Name = "Martwy nietoperz";
			Stackable = true;
		}

		public ZrodloSkrzydloNietoperza(Serial serial) : base(serial)
		{
			//m_plantedTime = DateTime.Now;	// ???
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

	public class SurowiecSkrzydloNietoperza : WeedCropZbieractwo
	{
		public override void CreateReagent(Mobile from, int count) { from.AddToBackpack(new BatWing(count)); }

		[Constructable]
		public SurowiecSkrzydloNietoperza(int amount) : base(amount, 0x20F9)
		{
			Hue = 0x415;
			Name = "Szczatki nietoperza";
			Stackable = true;
		}

		[Constructable]
		public SurowiecSkrzydloNietoperza() : this(1)
		{
		}

		public SurowiecSkrzydloNietoperza(Serial serial) : base(serial)
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
