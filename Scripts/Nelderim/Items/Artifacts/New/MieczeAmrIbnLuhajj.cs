namespace Server.Items
{
	public class MieczeAmrIbnLuhajj : Daisho
	{
		public override int LabelNumber => 1065795; // Miecze Amr Ibn Luhajj
		public override int InitMinHits => 255;
		public override int InitMaxHits => 255;
		[Constructable]
		public MieczeAmrIbnLuhajj()
		{
			Hue = 2213;
			WeaponAttributes.MageWeapon = 20;
			Attributes.SpellChanneling = 1;
			Attributes.CastSpeed = 1;
			Attributes.Luck = 50;
			Attributes.RegenMana = 5;
			Attributes.SpellDamage = 5;
			Attributes.WeaponDamage = 25;
		}

		public MieczeAmrIbnLuhajj(Serial serial)
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

			if (Attributes.WeaponDamage != 25)
				Attributes.WeaponDamage = 25;
		}
	}
}
