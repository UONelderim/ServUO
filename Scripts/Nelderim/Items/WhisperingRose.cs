namespace Server.Items
{
	public class WhisperingRose : Item
	{
		[Constructable]
		public WhisperingRose() : base(0x18E9)
		{
			Stackable = false;

			Name = "Szepczaca roza";

			Weight = 1.0;
		}

		public WhisperingRose(Serial serial) : base(serial)
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
