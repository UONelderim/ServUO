namespace Server.Items
{
	public class MagicznySaif : DoubleBladedStaff
	{
		public override int LabelNumber { get { return 1065779; } } // Magiczny Saif
		public override int InitMinHits => 255;
		public override int InitMaxHits => 255;

		[Constructable]
		public MagicznySaif()
		{
			Hue = 500;
			WeaponAttributes.HitLightning = 10;
			WeaponAttributes.HitLowerAttack = 10;
			WeaponAttributes.HitMagicArrow = 5;
			SkillBonuses.SetValues(0, SkillName.Magery, 5.0);
			Attributes.BonusMana = 15;
			Attributes.ReflectPhysical = 5;
			Attributes.SpellChanneling = 1;
			IntRequirement = 100;
			Attributes.WeaponDamage = 25;
		}

		public MagicznySaif(Serial serial)
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
