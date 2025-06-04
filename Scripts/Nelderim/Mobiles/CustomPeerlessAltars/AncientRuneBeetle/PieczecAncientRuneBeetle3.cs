namespace Server.Items
{
	public class PieczecAncientRuneBeetle3 : PeerlessKey
	{
		[Constructable]
		public PieczecAncientRuneBeetle3()
			: base(0x1F14)
		{
			Name = "Trzecia Pieczec";
			Label0 = "Loen Torech";
			Hue = 2129;
			LootType = LootType.Blessed;
		}

		public PieczecAncientRuneBeetle3(Serial serial)
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
