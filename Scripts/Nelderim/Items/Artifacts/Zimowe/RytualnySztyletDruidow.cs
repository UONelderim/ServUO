namespace Server.Items
{
	public class RytualnySztyletDruidow : RuneBlade
	{
		public override int InitMinHits => 255;
		public override int InitMaxHits => 255;

		[Constructable]
		public RytualnySztyletDruidow()
		{
			WeaponAttributes.HitMagicArrow = 80;
			Attributes.AttackChance = 15;
			Attributes.WeaponSpeed = 25;
			Attributes.WeaponDamage = 35;
			Name = "Rytualny Sztylet Druidow";
			Hue = 496;
		}

		public override void GetDamageTypes(Mobile wielder, out int phys, out int fire, out int cold, out int pois,
			out int nrgy, out int chaos, out int direct)
		{
			phys = fire = cold = pois = chaos = direct = 0;
			nrgy = 100;
		}

		public RytualnySztyletDruidow(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.WriteEncodedInt(0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadEncodedInt();
		}
	}
}
