namespace Server.Items
{
	public class ZlamanyGungnir : BlackStaff
	{
		public override int InitMinHits => 255;
		public override int InitMaxHits => 255;

		[Constructable]
		public ZlamanyGungnir()
		{
			Hue = 1161;
			Name = "Zlamany Gungnir";
			Attributes.DefendChance = 10;
			Attributes.AttackChance = 5;
			Attributes.WeaponDamage = 20;
			Attributes.BonusStr = 5;
		}

		public ZlamanyGungnir(Serial serial)
			: base(serial)
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
