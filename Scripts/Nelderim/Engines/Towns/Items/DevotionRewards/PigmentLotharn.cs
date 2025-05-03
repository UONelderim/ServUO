namespace Server.Items
{
	public class PigmentLotharn : BasePigment
	{
		[Constructable]
		public PigmentLotharn()
			: this(5)
		{
        
		}
		[Constructable]
		public PigmentLotharn(int uses)
			: base(PigmentTarget.Cloth, uses, 1182)
		{
			Name = "Pigment barw Lotharn";
		}

		public PigmentLotharn(Serial serial) : base(serial)
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
