namespace Server.Items
{
	public class CorlrummEronDaUmri : Nunchaku
	{
		public override int InitMinHits => 255;
		public override int InitMaxHits => 255;

		[Constructable]
		public CorlrummEronDaUmri()
		{
			Hue = 1151;
			Name = "Corlrumm Eron Da Umri";
			Attributes.WeaponDamage = 40;
			Attributes.WeaponSpeed = 20;
			WeaponAttributes.HitLeechMana = 30;
			WeaponAttributes.HitColdArea = 100;
			SkillBonuses.SetValues(0, SkillName.Bushido, 10.0);
		}

		public CorlrummEronDaUmri(Serial serial)
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
