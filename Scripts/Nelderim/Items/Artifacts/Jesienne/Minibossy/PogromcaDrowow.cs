namespace Server.Items
{
	public class PogromcaDrowow : RepeatingCrossbow
	{
		public override int InitMinHits { get { return 60; } }
		public override int InitMaxHits { get { return 60; } }

		[Constructable]
		public PogromcaDrowow()
		{
			Hue = 2909;
			Name = "Pogromca Drowow";
			Attributes.WeaponDamage = 25;
			WeaponAttributes.HitLeechMana = 30;
			WeaponAttributes.HitColdArea = 100;
			SkillBonuses.SetValues(0, SkillName.Bushido, 10.0);
		}

		public PogromcaDrowow(Serial serial)
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
