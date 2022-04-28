namespace Server.Items
{
	public class PrzekletePogrobowce : Daisho
	{
		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		[Constructable]
		public PrzekletePogrobowce()
		{
			Hue = 1180;
			Name = "Przeklęte Pogrobowce";
			WeaponAttributes.HitHarm = 30;
			Attributes.WeaponDamage = 50;
			WeaponAttributes.HitLeechHits = 25;
			WeaponAttributes.HitLeechStam = 20;
			Attributes.LowerManaCost = 15;
			Attributes.NightSight = 1;
			Attributes.SpellChanneling = 1;
			Slayer = SlayerName.BloodDrinking;
			LootType = LootType.Cursed;
		}

		public PrzekletePogrobowce(Serial serial)
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
