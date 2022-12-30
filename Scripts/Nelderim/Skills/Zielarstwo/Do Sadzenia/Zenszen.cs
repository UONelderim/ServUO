namespace Server.Items.Crops
{
	public class SzczepkaZenszen : WeedSeedZiolaUprawne
	{
		public override Item CreateWeed() { return new KrzakZenszen(); }

		[Constructable]
		public SzczepkaZenszen(int amount) : base(amount, 0x18EB)
		{
			Hue = 0;
			Name = "Szczepka zen-szeniu";
			Stackable = true;
		}

		[Constructable]
		public SzczepkaZenszen() : this(1)
		{
		}

		public SzczepkaZenszen(Serial serial) : base(serial)
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

	public class KrzakZenszen : WeedPlantZiolaUprawne
	{
		public override void CreateCrop(Mobile from, int count) { from.AddToBackpack(new PlonZenszen(count)); }

		public override void CreateSeed(Mobile from, int count) { from.AddToBackpack(new SzczepkaZenszen(count)); }

		[Constructable]
		public KrzakZenszen() : base(0x18E9)
		{
			Hue = 0;
			Name = "Zen-szen";
			Stackable = true;
		}

		public KrzakZenszen(Serial serial) : base(serial)
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

	public class PlonZenszen : WeedCropZiolaUprawne
	{
		public override void CreateReagent(Mobile from, int count) { from.AddToBackpack(new Ginseng(count)); }

		[Constructable]
		public PlonZenszen(int amount) : base(amount, 0x18EC)
		{
			Hue = 0;
			Name = "Surowy zen-szen";
			Stackable = true;
		}

		[Constructable]
		public PlonZenszen() : this(1)
		{
		}

		public PlonZenszen(Serial serial) : base(serial)
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
