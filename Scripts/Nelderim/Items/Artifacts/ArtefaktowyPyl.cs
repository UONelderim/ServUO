namespace Server
{
	public class ArtefaktowyPyl : Item
	{
		public override double DefaultWeight
		{
			get { return 0.01; }
		}

		[Constructable]
		public ArtefaktowyPyl() : this(1)
		{
		}

		[Constructable]
		public ArtefaktowyPyl(int amount) : base(0x26B8)
		{
			Name = "Artefaktowy Pyl";
			Hue = 1079;
			Stackable = true;
			Amount = amount;
		}

		public ArtefaktowyPyl(Serial serial) : base(serial)
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
