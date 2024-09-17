namespace Server.Items
{
	public class PostrachPrzekletych : HammerPick
	{
		public override int LabelNumber { get { return 1065853; } } // Postrach Przekletych
		public override int InitMinHits => 255;
		public override int InitMaxHits => 255;

		[Constructable]
		public PostrachPrzekletych()
		{
			Hue = 0x47E;

			Slayer = SlayerName.Exorcism;
			WeaponAttributes.UseBestSkill = 1;
			Attributes.WeaponDamage = 30;
			WeaponAttributes.HitLowerAttack = 50;
			WeaponAttributes.HitLeechHits = 40;
			WeaponAttributes.HitDispel = 100;

			WeaponAttributes.ResistPhysicalBonus = 5;
			WeaponAttributes.ResistFireBonus = 5;
			WeaponAttributes.ResistColdBonus = 5;
			WeaponAttributes.ResistPoisonBonus = 5;
			WeaponAttributes.ResistEnergyBonus = 5;
		}

		public override void GetDamageTypes(Mobile wielder, out int phys, out int fire, out int cold, out int pois,
			out int nrgy, out int chaos, out int direct)
		{
			phys = 20;
			fire = 0;
			cold = 30;
			pois = 30;
			nrgy = 20;
			chaos = 0;
			direct = 0;
		}

		public PostrachPrzekletych(Serial serial)
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
