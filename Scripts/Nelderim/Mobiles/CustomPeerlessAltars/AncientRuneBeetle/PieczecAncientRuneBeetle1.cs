namespace Server.Items
{
	public class PieczecAncientRuneBeetle1 : PeerlessKey
	{
		[Constructable]
		public PieczecAncientRuneBeetle1()
			: base(0x1F14)
		{
			Name = "Pierwsza Pieczec";
			Label0 = "Loen Torech";
			Hue = 2129;
			LootType = LootType.Blessed;
		}

		public PieczecAncientRuneBeetle1(Serial serial)
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
