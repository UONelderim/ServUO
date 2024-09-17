namespace Server.Items
{
	public class StraznikPolnocy : LargeBattleAxe
	{
		public override int LabelNumber { get { return 1065818; } } // Straznik Polnocy
		public override int InitMinHits => 255;
		public override int InitMaxHits => 255;

		[Constructable]
		public StraznikPolnocy()
		{
			Hue = 0x482;
			Attributes.AttackChance = 10;
			WeaponAttributes.HitLeechStam = 30;
			Attributes.WeaponDamage = 50;
			Attributes.WeaponSpeed = 20;
		}

		public override void GetDamageTypes(Mobile wielder, out int phys, out int fire, out int cold, out int pois,
			out int nrgy, out int chaos, out int direct)
		{
			phys = nrgy = 15;
			cold = 70;
			fire = pois = chaos = direct = 0;
		}

		public StraznikPolnocy(Serial serial) : base(serial)
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
