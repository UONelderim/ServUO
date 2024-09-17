namespace Server.Items
{
	public class ToporWysokichElfow : OrnateAxe
	{
		public override int InitMinHits => 255;
		public override int InitMaxHits => 255;

		[Constructable]
		public ToporWysokichElfow()
		{
			Hue = 1687;
			Name = "Topor Wysokich Elfow";
			Attributes.WeaponDamage = 55;
			WeaponAttributes.HitLeechHits = 25;
			Attributes.WeaponSpeed = 30;
			Attributes.AttackChance = 10;
			SkillBonuses.SetValues(0, SkillName.Tactics, 5.0);
		}

		public ToporWysokichElfow(Serial serial)
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
