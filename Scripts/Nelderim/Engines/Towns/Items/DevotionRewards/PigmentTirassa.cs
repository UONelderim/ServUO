namespace Server.Items
{
	public class PigmentTirassa : BasePigment
	{
		[Constructable]
		public PigmentTirassa()
			: this(5)
		{
		}

		[Constructable]
		public PigmentTirassa(int uses)
			: base(PigmentTarget.Cloth, uses, 1253)
		{
			Name = "Pigment barw Tirassy";
		}

		public PigmentTirassa(Serial serial) : base(serial)
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
