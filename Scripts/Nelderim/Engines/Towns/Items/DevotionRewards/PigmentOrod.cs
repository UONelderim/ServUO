namespace Server.Items
{
	public class PigmentOrod : BasePigment
	{
		[Constructable]
		public PigmentOrod()
			: this(5)
		{
		}

		[Constructable]
		public PigmentOrod(int uses)
			: base(PigmentTarget.Cloth, uses, 2081)
		{
			Name = "Pigment barw Orod";
		}

		public PigmentOrod(Serial serial) : base(serial)
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
