namespace Server.Items
{
	public class PieczecSarag3 : PeerlessKey
	{
		[Constructable]
		public PieczecSarag3()
			: base(0x1F14)
		{
			Name = "Trzecia Pieczec";
			Label0 = "Hall Torech";
			Weight = 1; 
			Hue = 77; 
			LootType = LootType.Blessed;
		}

		public PieczecSarag3(Serial serial)
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
