namespace Server.Items.Crops
{
	public class ZrodloKrysztalTrucizny : WeedPlantZbieractwo
	{
		public override void CreateCrop(Mobile from, int count)
		{
			from.AddToBackpack(new SurowiecKrysztalTrucizny(count));
		}

		public override bool GivesSeed => false;

		[Constructable]
		public ZrodloKrysztalTrucizny() : base(0x35DA)
		{
			Hue = 0x44;
			Name = "Krysztaly trucizny";
			Stackable = true;
		}

		public ZrodloKrysztalTrucizny(Serial serial) : base(serial)
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

	public class SurowiecKrysztalTrucizny : WeedCropZbieractwo
	{
		public override void CreateReagent(Mobile from, int count) { from.AddToBackpack(new NoxCrystal(count)); }

		[Constructable]
		public SurowiecKrysztalTrucizny(int amount) : base(amount, 0x2244)
		{
			Hue = 0x44;
			Name = "Nieksztaltny krysztal trucizny";
			Stackable = true;
		}

		[Constructable]
		public SurowiecKrysztalTrucizny() : this(1)
		{
		}

		public SurowiecKrysztalTrucizny(Serial serial) : base(serial)
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
