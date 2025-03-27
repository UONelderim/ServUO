namespace Server.Items
{
	public class PieczecBurugh3 : PeerlessKey
	{
		[Constructable]
		public PieczecBurugh3()
			: base(0x1F14)
		{
			Name = "Trzecia Pieczec";
			Label0 = "Komnata Burugha";
			Weight = 1; 
			Hue = 0x89F; 
			LootType = LootType.Blessed;
		}

		public PieczecBurugh3(Serial serial)
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
