namespace Server.Items
{
	public class PieczecSarag2 : PeerlessKey
	{
		[Constructable]
		public PieczecSarag2()
			: base(0x1F14)
		{
			Name = "Druga Pieczec";
			Label0 = "Hall Torech";
			Weight = 1; 
			Hue = 77; 
			LootType = LootType.Blessed;
		}

		public PieczecSarag2(Serial serial)
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
