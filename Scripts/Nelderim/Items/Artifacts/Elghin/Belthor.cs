namespace Server.Items
{
	public class Belthor : MagicalShortbow
	{
		public override int InitMinHits => 255;
		public override int InitMaxHits => 255;

		[Constructable]
		public Belthor()
		{
			Hue = 842;
			Name = "Belthor";
			Attributes.WeaponDamage = 45;
			WeaponAttributes.HitLeechMana = 20;
			Attributes.WeaponSpeed = 10;
			Attributes.CastSpeed = -1;
			Attributes.AttackChance = 10;
			Attributes.DefendChance = -20;
			LootType = LootType.Cursed;
		}

		public Belthor(Serial serial)
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
