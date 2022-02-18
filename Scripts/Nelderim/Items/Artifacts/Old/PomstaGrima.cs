namespace Server.Items
{
	public class PomstaGrima : Crossbow
	{
		public override int LabelNumber { get { return 1065814; } } // Pomsta Grima
		public override int InitMinHits { get { return 60; } }
		public override int InitMaxHits { get { return 60; } }

		[Constructable]
		public PomstaGrima()
		{
			Hue = 0x560;
			Attributes.CastSpeed = 1;
			Attributes.BonusStam = 10;
			Attributes.WeaponDamage = 50;
			Attributes.AttackChance = 10;
			Slayer = SlayerName.Silver;
		}

		public override void GetDamageTypes(Mobile wielder, out int phys, out int fire, out int cold, out int pois,
			out int nrgy, out int chaos, out int direct)
		{
			phys = pois = nrgy = 20;
			fire = 30;
			cold = 10;
			chaos = 0;
			direct = 0;
		}

		public PomstaGrima(Serial serial) : base(serial)
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
