namespace Server.Items
{
	public class RightArm : Item
	{
		[Constructable]
		public RightArm()
			: base(0x1DA2)
		{
			Stackable = true;
		}

		public RightArm(Serial serial)
			: base(serial)
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
