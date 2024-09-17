namespace Server.Items
{
	public class ChwytTeczy : Dagger
	{
		public override int InitMinHits => 255;
		public override int InitMaxHits => 255;

		[Constructable]
		public ChwytTeczy()
		{
			Hue = 1064;
			Name = "Chwyt Teczy";
			Attributes.WeaponDamage = 40;
			WeaponAttributes.HitLeechHits = 100;
			WeaponAttributes.HitLeechStam = 10;
			Attributes.AttackChance = 30;
			Attributes.DefendChance = -10;
			SkillBonuses.SetValues(0, SkillName.Camping, 5.0);
		}

		public ChwytTeczy(Serial serial)
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
