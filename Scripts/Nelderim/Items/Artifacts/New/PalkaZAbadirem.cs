namespace Server.Items
{
	public class PalkaZAbadirem : Hatchet
	{
		public override int InitMinHits => 255;
		public override int InitMaxHits => 255;

		[Constructable]
		public PalkaZAbadirem()
		{
			Hue = 1157;
			Name = "Oręż wysadzany rubinami";
			Attributes.WeaponDamage = 50;
			WeaponAttributes.HitLeechHits = 35;
			WeaponAttributes.UseBestSkill = 1;
			Attributes.WeaponSpeed = 10;
			WeaponAttributes.HitFireball = 20;
		}

		public PalkaZAbadirem(Serial serial)
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
