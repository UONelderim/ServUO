namespace Server.Items
{
	public class OstrzanyKijMnichaZTasandory : BladedStaff
	{
		public override int InitMinHits => 255;
		public override int InitMaxHits => 255;

		[Constructable]
		public OstrzanyKijMnichaZTasandory()
		{
			Hue = 2300;
			Name = "Ostrzany Kij Mnicha z Tasandory";
			Attributes.WeaponDamage = 25;
			WeaponAttributes.HitLeechHits = 40;
			WeaponAttributes.HitLeechStam = 100;
			Attributes.RegenStam = 3;
			Attributes.WeaponSpeed = 30;
			Attributes.AttackChance = 30;
			Attributes.DefendChance = -40;
			SkillBonuses.SetValues(0, SkillName.Begging, 10.0);
		}

		public OstrzanyKijMnichaZTasandory(Serial serial)
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
