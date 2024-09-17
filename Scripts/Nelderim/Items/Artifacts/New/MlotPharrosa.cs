namespace Server.Items
{
	public class MlotPharrosa : HammerPick
	{
		public override int LabelNumber { get { return 1065749; } } // Mlot Pharrosa
		public override int InitMinHits => 255;
		public override int InitMaxHits => 255;

		[Constructable]
		public MlotPharrosa()
		{
			Hue = 2151;

			WeaponAttributes.HitLeechHits = 35;
			WeaponAttributes.HitLeechMana = 45;
			WeaponAttributes.HitLeechStam = 25;
			WeaponAttributes.HitPhysicalArea = 100;
			Attributes.WeaponDamage = 35;
		}

		public MlotPharrosa(Serial serial)
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

			if (Attributes.WeaponDamage != 35)
				Attributes.WeaponDamage = 35;
		}
	}
}
