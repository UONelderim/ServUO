namespace Server.Items
{
	public class Gungnir : Pike
	{
		public override int LabelNumber { get { return 1065768; } } // Gungnir
		public override int InitMinHits => 255;
		public override int InitMaxHits => 255;

		[Constructable]
		public Gungnir()
		{
			Hue = 1151;
			Attributes.DefendChance = 10;
			Attributes.AttackChance = 10;
			Attributes.WeaponDamage = 40;
			Attributes.BonusStr = 10;
		}

		public override void GetDamageTypes(Mobile wielder, out int phys, out int fire, out int cold, out int pois,
			out int nrgy, out int chaos, out int direct)
		{
			phys = 70;
			fire = 0;
			cold = 0;
			pois = 0;
			nrgy = 30;
			chaos = 0;
			direct = 0;
		}

		public Gungnir(Serial serial)
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
