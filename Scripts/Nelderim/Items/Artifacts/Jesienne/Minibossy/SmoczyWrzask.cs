namespace Server.Items
{
	public class SmoczyWrzask : Yumi
	{
		public override int InitMinHits => 255;
		public override int InitMaxHits => 255;

		[Constructable]
		public SmoczyWrzask()
		{
			Hue = 0x530;
			Name = "Smoczy Wrzask";
			Slayer = SlayerName.DragonSlaying;
			Attributes.WeaponDamage = 50;
			Attributes.RegenHits = 3;
			Attributes.WeaponSpeed = 50;
			WeaponAttributes.ResistFireBonus = 20;
		}

		public SmoczyWrzask(Serial serial)
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
