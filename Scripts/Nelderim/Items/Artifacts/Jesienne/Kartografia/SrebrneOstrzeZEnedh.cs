namespace Server.Items
{
	public class SrebrneOstrzeZEnedh : ElvenMachete
	{
		public override int InitMinHits => 255;
		public override int InitMaxHits => 255;

		[Constructable]
		public SrebrneOstrzeZEnedh()
		{
			Hue = 610;
			Name = "Srebrne Ostrze z Enedh";
			Slayer = SlayerName.Silver;
			Attributes.WeaponDamage = 50;
			Attributes.SpellChanneling = 1;
			WeaponAttributes.UseBestSkill = 1;
			WeaponAttributes.HitLeechHits = 20;
			WeaponAttributes.HitLeechMana = 20;
			WeaponAttributes.HitLeechStam = 20;
		}

		public SrebrneOstrzeZEnedh(Serial serial)
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
