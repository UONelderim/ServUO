namespace Server.Items
{
	public class OstrzePolksiezyca : CrescentBlade
	{
		public override int LabelNumber { get { return 1065812; } } // Ostrze Polksiezyca
		public override int InitMinHits => 255;
		public override int InitMaxHits => 255;

		[Constructable]
		public OstrzePolksiezyca()
		{
			Hue = 2360;
			Attributes.WeaponSpeed = 30;
			WeaponAttributes.HitLeechMana = 25;
			WeaponAttributes.HitLeechHits = 20;
			Attributes.WeaponDamage = 40;
			WeaponAttributes.UseBestSkill = 1;
			Attributes.AttackChance = 10;
		}

		public OstrzePolksiezyca(Serial serial) : base(serial)
		{
		}

		public override void GetDamageTypes(Mobile wielder, out int phys, out int fire, out int cold, out int pois,
			out int nrgy, out int chaos, out int direct)
		{
			phys = 20;
			fire = 20;
			cold = 20;
			pois = 20;
			nrgy = 20;
			chaos = direct = 0;
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
