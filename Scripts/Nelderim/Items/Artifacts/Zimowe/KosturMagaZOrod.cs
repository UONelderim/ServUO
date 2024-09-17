namespace Server.Items
{
	public class KosturMagaZOrod : BlackStaff
	{
		public override int InitMinHits => 255;
		public override int InitMaxHits => 255;

		[Constructable]
		public KosturMagaZOrod()
		{
			Hue = 1693;
			Name = "Kostur Maga Z Orod";
			Attributes.WeaponDamage = 20;
			WeaponAttributes.HitLeechHits = 35;
			WeaponAttributes.UseBestSkill = 1;
			Attributes.WeaponSpeed = 10;
			WeaponAttributes.MageWeapon = -15;
			WeaponAttributes.HitFireball = 20;
		}

		public KosturMagaZOrod(Serial serial)
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
