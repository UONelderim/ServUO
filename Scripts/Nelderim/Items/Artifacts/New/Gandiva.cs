namespace Server.Items
{
	public class Gandiva : RepeatingCrossbow
	{
		public override int LabelNumber { get { return 1065855; } } // Gandiva
		public override int InitMinHits => 255;
		public override int InitMaxHits => 255;

		[Constructable]
		public Gandiva()
		{
			Hue = 1393;

			Slayer = SlayerName.ArachnidDoom;
			Attributes.WeaponDamage = 30;
			Attributes.WeaponSpeed = 30;
			Attributes.AttackChance = 15;
			Attributes.DefendChance = -15;
			WeaponAttributes.HitEnergyArea = 100;
		}

		public override void GetDamageTypes(Mobile wielder, out int phys, out int fire, out int cold, out int pois,
			out int nrgy, out int chaos, out int direct)
		{
			phys = 40;
			fire = 0;
			cold = 0;
			pois = 0;
			nrgy = 60;
			chaos = 0;
			direct = 0;
		}

		public Gandiva(Serial serial)
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
