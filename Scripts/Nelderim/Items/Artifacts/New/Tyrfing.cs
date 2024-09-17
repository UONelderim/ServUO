namespace Server.Items
{
	public class Tyrfing : Kryss
	{
		public override int LabelNumber { get { return 1065797; } } // Tyrfing
		public override int InitMinHits => 255;
		public override int InitMaxHits => 255;

		[Constructable]
		public Tyrfing()
		{
			Hue = 1272;
			Attributes.AttackChance = 20;
			Attributes.WeaponSpeed = 30;
			WeaponAttributes.HitMagicArrow = 35;
			Attributes.WeaponDamage = 25;
		}

		public Tyrfing(Serial serial)
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

			if (Attributes.WeaponDamage != 25)
				Attributes.WeaponDamage = 25;
		}
	}
}
