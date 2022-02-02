namespace Server.Items
{
	public class SzponyOgnistegoSmoka : Sai
	{
		public override int InitMinHits { get { return 60; } }
		public override int InitMaxHits { get { return 60; } }

		[Constructable]
		public SzponyOgnistegoSmoka()
		{
			Hue = 1161;
			Name = "Szpony Ognistego Smoka";
			Attributes.WeaponDamage = 40;
			WeaponAttributes.HitLeechHits = 10;
			WeaponAttributes.HitFireArea = 20;
			Attributes.DefendChance = -20;
			Attributes.AttackChance = 20;
		}

		public SzponyOgnistegoSmoka(Serial serial)
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
