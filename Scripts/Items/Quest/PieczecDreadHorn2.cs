namespace Server.Items
{
	public class PieczecDreadHorn2 : PeerlessKey
	{
		[Constructable]
		public PieczecDreadHorn2()
			: base(0x1F14)
		{
			Name = "Druga Pieczec";
			Label0 = "Jaskinia Spaczonego Jednorożca";
			Weight = 1;
			Hue = 0x1F0;
			LootType = LootType.Blessed;
		}

		public PieczecDreadHorn2(Serial serial)
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
