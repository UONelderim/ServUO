namespace Server.Items
{
	public class PieczecExoddusBoss2 : PeerlessKey
	{
		[Constructable]
		public PieczecExoddusBoss2()
			: base(0x1F14)
		{
			Name = "Druga Pieczec";
			Label0 = "Krypta Mechanicznego Straznika";
			Weight = 1;
			Hue = 2110;
			LootType = LootType.Blessed;
		}

		public PieczecExoddusBoss2(Serial serial)
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
