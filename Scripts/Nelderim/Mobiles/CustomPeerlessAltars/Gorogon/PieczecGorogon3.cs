namespace Server.Items
{
	public class PieczecGorogon3 : PeerlessKey
	{
		[Constructable]
		public PieczecGorogon3()
			: base(0x1F14)
		{
			Name = "Trzecia Pieczec";
			Label0 = "Komnata Gorogona";
			Weight = 1; 
			Hue = 38; 
			LootType = LootType.Blessed;
		}

		public PieczecGorogon3(Serial serial)
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
