namespace Server.Items
{
	public class Pogrobowce : Daisho
	{
		public override int InitMinHits => 255;
		public override int InitMaxHits => 255;

		[Constructable]
		public Pogrobowce()
		{
			Hue = 1153;
			Name = "Pogromca Krwiożerczych Stworów";

			WeaponAttributes.HitHarm = 30;
			WeaponAttributes.HitLeechHits = 25;
			WeaponAttributes.HitLeechStam = 20;
			Attributes.LowerManaCost = 5;
			Attributes.NightSight = 1;
			Attributes.SpellChanneling = 1;
			Slayer = SlayerName.BloodDrinking;
			Attributes.WeaponDamage = 45;
		}

		public Pogrobowce(Serial serial)
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

			if (Attributes.WeaponDamage != 45)
				Attributes.WeaponDamage = 45;
		}
	}
}
