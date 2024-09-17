namespace Server.Items
{
	public class Raikiri : Scimitar
	{
		public override int LabelNumber { get { return 1065793; } } // Raikiri
		public override int InitMinHits => 255;
		public override int InitMaxHits => 255;

		[Constructable]
		public Raikiri()
		{
			Hue = 2082;
			WeaponAttributes.HitFireball = 25;
			WeaponAttributes.HitLightning = 25;
			Attributes.Luck = 200;
			Attributes.WeaponDamage = 35;
		}

		public override void GetDamageTypes(Mobile wielder, out int phys, out int fire, out int cold, out int pois,
			out int nrgy, out int chaos, out int direct)
		{
			nrgy = fire = 50;

			pois = cold = phys = chaos = direct = 0;
		}

		public Raikiri(Serial serial)
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

			if (Attributes.WeaponDamage != 35)
				Attributes.WeaponDamage = 35;
		}
	}
}
