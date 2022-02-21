namespace Server.Items
{
	public class Quernbiter : DoubleAxe
	{
		public override int LabelNumber { get { return 1065785; } } // Quernbiter
		public override int InitMinHits { get { return 60; } }
		public override int InitMaxHits { get { return 60; } }

		[Constructable]
		public Quernbiter()
		{
			Hue = 1161;
			WeaponAttributes.HitColdArea = 25;
			WeaponAttributes.HitHarm = 40;
			Attributes.WeaponDamage = 50;
			Attributes.WeaponSpeed = 20;
		}

		public override void GetDamageTypes(Mobile wielder, out int phys, out int fire, out int cold, out int pois,
			out int nrgy, out int chaos, out int direct)
		{
			phys = 70;
			fire = 0;
			cold = 30;
			pois = 0;
			nrgy = 0;
			chaos = 0;
			direct = 0;
		}

		public Quernbiter(Serial serial)
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
