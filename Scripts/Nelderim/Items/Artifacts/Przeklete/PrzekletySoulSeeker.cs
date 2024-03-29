namespace Server.Items
{
	public class PrzekletySoulSeeker : RadiantScimitar
	{
		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		[Constructable]
		public PrzekletySoulSeeker()
		{
			Hue = 1180;
			Name = "Przeklęty Poszukiwacz Dusz";
			WeaponAttributes.HitLeechStam = 40;
			WeaponAttributes.HitLeechMana = 40;
			WeaponAttributes.HitLeechHits = 60;
			Attributes.WeaponDamage = 30;
			Attributes.WeaponSpeed = 60;
			Slayer = SlayerName.Repond;
			LootType = LootType.Cursed;
		}

		public override void GetDamageTypes(Mobile wielder, out int phys, out int fire, out int cold, out int pois,
			out int nrgy, out int chaos, out int direct)
		{
			cold = 100;

			pois = fire = phys = nrgy = chaos = direct = 0;
		}

		public PrzekletySoulSeeker(Serial serial) : base(serial)
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
