namespace Server.Items
{
	public class LodowaPoswiata : Lajatang
	{
		public override int InitMinHits { get { return 60; } }
		public override int InitMaxHits { get { return 60; } }

		[Constructable]
		public LodowaPoswiata()
		{
			Hue = 1152;
			Name = "Lodowa Poswiata";
			Attributes.WeaponDamage = 35;
			WeaponAttributes.HitLeechHits = 15;
			WeaponAttributes.HitLeechMana = 15;
			Attributes.WeaponSpeed = 20;
			Attributes.AttackChance = 10;
			WeaponAttributes.HitColdArea = 100;
			SkillBonuses.SetValues(0, SkillName.Fencing, 10.0);
		}

		public LodowaPoswiata(Serial serial)
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
