namespace Server.Items
{
	public class SiekieraDrwalaZCelendir : Hatchet
	{
		public override int InitMinHits => 255;
		public override int InitMaxHits => 255;

		[Constructable]
		public SiekieraDrwalaZCelendir()
		{
			Hue = 38;
			Name = "Siekiera Drwala z Celendir";
			Attributes.WeaponDamage = 50;
			WeaponAttributes.HitLeechHits = 35;
			WeaponAttributes.HitMagicArrow = 40;
			Attributes.WeaponSpeed = 10;
			SkillBonuses.SetValues(0, SkillName.Lumberjacking, 10.0);
		}

		public SiekieraDrwalaZCelendir(Serial serial)
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
