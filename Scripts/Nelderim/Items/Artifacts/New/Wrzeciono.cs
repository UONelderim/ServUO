namespace Server.Items
{
	public class Wrzeciono : CompositeBow
	{
		public override int LabelNumber { get { return 1065752; } } // Wrzeciono
		public override int InitMinHits => 255;
		public override int InitMaxHits => 255;

		[Constructable]
		public Wrzeciono()
		{
			Hue = 2481;

			Attributes.WeaponDamage = 30;
			WeaponAttributes.HitDispel = 40;
			WeaponAttributes.HitLightning = 30;
			Attributes.WeaponSpeed = 20;
		}

		public override void GetDamageTypes(Mobile wielder, out int phys, out int fire, out int cold, out int pois,
			out int nrgy, out int chaos, out int direct)
		{
			phys = 0;
			fire = 30;
			cold = 0;
			pois = 30;
			nrgy = 40;
			chaos = 0;
			direct = 0;
		}

		public Wrzeciono(Serial serial)
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
