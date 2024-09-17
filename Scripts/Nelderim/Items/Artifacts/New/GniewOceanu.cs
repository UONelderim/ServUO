namespace Server.Items
{
	public class GniewOceanu : Spear
	{
		public override int LabelNumber { get { return 1065798; } } // Gniew Oceanu
		public override int InitMinHits => 255;
		public override int InitMaxHits => 255;

		[Constructable]
		public GniewOceanu()
		{
			Hue = 2083;
			Attributes.AttackChance = 30;
			Attributes.WeaponDamage = 40;
			Attributes.BonusStam = 10;
			Attributes.BonusMana = 5;
			WeaponAttributes.HitMagicArrow = 20;
		}

		public override void GetDamageTypes(Mobile wielder, out int phys, out int fire, out int cold, out int pois,
			out int nrgy, out int chaos, out int direct)
		{
			phys = 20;
			fire = 0;
			cold = 40;
			pois = 0;
			nrgy = 40;
			chaos = 0;
			direct = 0;
		}

		public GniewOceanu(Serial serial)
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
