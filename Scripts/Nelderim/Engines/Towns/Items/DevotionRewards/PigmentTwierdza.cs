namespace Server.Items
{
	public class PigmentTwierdza: BasePigment
	{
		[Constructable]
		public PigmentTwierdza()
			: this(5)
		{
		}

		[Constructable]
		public PigmentTwierdza(int uses)
			: base(PigmentTarget.Cloth, uses, 1333)
		{
			Name = "Pigment barw Twierdzy";
		}

		public PigmentTwierdza(Serial serial) : base(serial)
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
