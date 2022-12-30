namespace Server.Items.Crops
{
	// TODO: ustawic dodatkowy skill krawiectwo i zwiekszyc progi umozliwiajace zbieranie

	public class SzczepkaLen : WeedSeedZiolaUprawne
	{
		public override Item CreateWeed() { return new KrzakLen(); }

		[Constructable]
		public SzczepkaLen(int amount) : base(amount, 6946)
		{
			Hue = 51;
			Name = "Ziarno lnu";
			Stackable = true;
		}

		[Constructable]
		public SzczepkaLen() : this(1)
		{
		}

		public SzczepkaLen(Serial serial) : base(serial)
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

	public class KrzakLen : WeedPlantZiolaUprawne
	{
		public override void CreateCrop(Mobile from, int count) { from.AddToBackpack(new PlonLen(count / 2)); }

		public override void CreateSeed(Mobile from, int count) { from.AddToBackpack(new SzczepkaLen(count * 3)); }

		[Constructable]
		public KrzakLen() : base(6811)
		{
			Hue = 0;
			Name = "Krzak lnu";
			Stackable = true;
		}

		public KrzakLen(Serial serial) : base(serial)
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

	public class PlonLen : WeedCropZiolaUprawne
	{
		public override void CreateReagent(Mobile from, int count) { from.AddToBackpack(new Flax(count)); }

		[Constructable]
		public PlonLen(int amount) : base(amount, 6809)
		{
			Hue = 0;
			Name = "Lodyga lnu";
			Stackable = true;
		}

		[Constructable]
		public PlonLen() : this(1)
		{
		}

		public PlonLen(Serial serial) : base(serial)
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
