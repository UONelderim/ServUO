namespace Server.Items
{
	public class LukKrolaElfow : ElvenCompositeLongbow
	{
		public override int InitMinHits => 255;
		public override int InitMaxHits => 255;

		[Constructable]
		public LukKrolaElfow()
		{
			Hue = 2003;
			Name = "Luka Krola Elfow";
			Attributes.DefendChance = 10;
			Attributes.AttackChance = 12;
			Attributes.WeaponSpeed = 20;
			Attributes.WeaponDamage = 45;
		}

		public override void GetDamageTypes(Mobile wielder, out int phys, out int fire, out int cold, out int pois,
			out int nrgy, out int chaos, out int direct)
		{
			phys = fire = cold = chaos = direct = 0;
			nrgy = 50;
			pois = 50;
		}

		public LukKrolaElfow(Serial serial) : base(serial)
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
