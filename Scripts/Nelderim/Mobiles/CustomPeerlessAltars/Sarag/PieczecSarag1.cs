namespace Server.Items
{
	public class PieczecSarag1 : PeerlessKey
	{
		[Constructable]
		public PieczecSarag1()
			: base(0x1F14)
		{
			Name = "Pierwsza Pieczec";
			Label0 = "Hall Torech";
			Weight = 1;
			Hue = 77;
			LootType = LootType.Blessed;
		}

		public PieczecSarag1(Serial serial)
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
