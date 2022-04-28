namespace Server.Items
{
	public class PrzekleteWidlyMroku : Pitchfork
	{
		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		[Constructable]
		public PrzekleteWidlyMroku()
		{
			Hue = 1180;
			Name = "Przeklęte Widly Mroku";
			Attributes.WeaponDamage = 50;
			WeaponAttributes.HitFireArea = 65;
			WeaponAttributes.HitFireball = 25;
			WeaponAttributes.ResistFireBonus = 5;
			Attributes.SpellChanneling = 1;
			Attributes.WeaponSpeed = -25;
			LootType = LootType.Cursed;
		}

		public PrzekleteWidlyMroku(Serial serial)
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
