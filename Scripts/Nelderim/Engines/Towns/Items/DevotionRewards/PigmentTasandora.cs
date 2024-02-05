namespace Server.Items
{
	public class PigmentTasandora : BasePigment
	{
		[Constructable]
		public PigmentTasandora()
			: this(5)
		{
        
		}
		[Constructable]
		public PigmentTasandora(int uses)
			: base(PigmentTarget.Cloth, uses, 2894)
		{
			Name = "Pigment barw tasandory";
		}

		public PigmentTasandora(Serial serial) : base(serial)
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
