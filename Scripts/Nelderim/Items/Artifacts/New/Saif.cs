namespace Server.Items
{
	public class Saif : DoubleBladedStaff
	{
		public override int LabelNumber { get { return 1065778; } } // Saif
		public override int InitMinHits => 255;
		public override int InitMaxHits => 255;

		[Constructable]
		public Saif()
		{
			Hue = 1072;
			Attributes.AttackChance = 10;
			WeaponAttributes.HitLightning = 35;
			WeaponAttributes.HitLowerAttack = 30;
			WeaponAttributes.HitLeechMana = 35;
			DexRequirement = 60;
			StrRequirement = 40;
			Attributes.WeaponDamage = 25;
		}

		public override void GetDamageTypes(Mobile wielder, out int phys, out int fire, out int cold, out int pois,
			out int nrgy, out int chaos, out int direct)
		{
			phys = 40;
			fire = 0;
			cold = 40;
			pois = 0;
			nrgy = 20;
			chaos = 0;
			direct = 0;
		}

		public Saif(Serial serial)
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
