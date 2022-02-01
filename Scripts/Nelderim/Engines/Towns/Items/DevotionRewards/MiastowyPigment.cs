namespace Server.Items
{
	public class PigmentTasandora : BasePigment
	{
		[Constructable]
		public PigmentTasandora()
			: base((int)PigmentTarget.Cloth, 5, 2894)
		{
		}

		[Constructable]
		public PigmentTasandora(int uses)
			: base((int)PigmentTarget.Cloth, uses, 2894)
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
