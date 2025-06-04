namespace Server.Items
{
	public class PieczecAncientRuneBeetle2 : PeerlessKey
	{
		[Constructable]
		public PieczecAncientRuneBeetle2()
			: base(0x1F14)
		{
			Name = "Druga Pieczec";
			Label0 = "Loen Torech";
			Hue = 2129;
			LootType = LootType.Blessed;
		}

		public PieczecAncientRuneBeetle2(Serial serial)
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
