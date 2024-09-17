namespace Server.Items
{
	public class Vijaya : Bow
	{
		public override int LabelNumber { get { return 1065850; } } // Vijaya
		public override int InitMinHits => 255;
		public override int InitMaxHits => 255;

		[Constructable]
		public Vijaya()
		{
			Hue = 1258;

			Attributes.WeaponSpeed = 20;
			Attributes.SpellChanneling = 1;
			Attributes.CastRecovery = 2;
			Attributes.WeaponDamage = 25;
		}

		public override void GetDamageTypes(Mobile wielder, out int phys, out int fire, out int cold, out int pois,
			out int nrgy, out int chaos, out int direct)
		{
			phys = 60;
			fire = 20;
			cold = 20;
			pois = 0;
			nrgy = 0;
			chaos = 0;
			direct = 0;
		}

		public Vijaya(Serial serial)
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
