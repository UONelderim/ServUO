namespace Server.Items
{
	public class PieczecNelderimDragon1 : PeerlessKey
	{
		[Constructable]
		public PieczecNelderimDragon1()
			: base(0x1F14)
		{
			Name = "Pierwsza Pieczec";
			Label0 = "Jama Smoczyska";
			Hue = 1293;
			LootType = LootType.Blessed;
		}

		public PieczecNelderimDragon1(Serial serial)
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
