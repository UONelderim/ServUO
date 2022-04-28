namespace Server.Items
{
	public class PrzekleteOrleSkrzydla : BladedStaff
	{
		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		[Constructable]
		public PrzekleteOrleSkrzydla()
		{
			Hue = 1180;
			Name = "Przeklęte Orle Skrzydla";
			Attributes.AttackChance = 5;
			Attributes.DefendChance = 15;
			WeaponAttributes.HitLeechHits = 10;
			WeaponAttributes.HitFireball = 100;
			WeaponAttributes.HitMagicArrow = 10;
			Attributes.WeaponDamage = 38;
			LootType = LootType.Cursed;
		}

		public PrzekleteOrleSkrzydla(Serial serial)
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
