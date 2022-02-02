namespace Server.Items
{
	public class ReliktDrowow : GoldBracelet
	{
		public override int InitMinHits { get { return 60; } }
		public override int InitMaxHits { get { return 60; } }


		[Constructable]
		public ReliktDrowow()
		{
			Hue = 1154;
			Name = "Relikt Drowow";
			Attributes.RegenHits = 3;
			Attributes.DefendChance = -5;
			Attributes.AttackChance = 10;
			Attributes.WeaponDamage = 5;
			SkillBonuses.SetValues(0, SkillName.Tracking, 10);
		}

		public ReliktDrowow(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}
}
