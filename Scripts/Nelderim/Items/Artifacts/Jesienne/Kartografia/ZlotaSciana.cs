namespace Server.Items
{
	public class ZlotaSciana : Buckler
	{
		public override int InitMinHits => 255;
		public override int InitMaxHits => 255;

		[Constructable]
		public ZlotaSciana()
		{
			ItemID = 0x2B01;
			Hue = 1281;
			Name = "Zlota Sciana";
			Attributes.DefendChance = 20;
			Attributes.WeaponDamage = 5;
			Attributes.CastSpeed = 2;
			Attributes.CastRecovery = -1;
		}

		public ZlotaSciana(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}
}
