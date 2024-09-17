namespace Server.Items
{
	public class StrzalaAbarisa : HeavyCrossbow
	{
		public override int LabelNumber { get { return 1065769; } } // StrzalaAbarisa
		public override int InitMinHits => 255;
		public override int InitMaxHits => 255;

		[Constructable]
		public StrzalaAbarisa()
		{
			Hue = 267;
			WeaponAttributes.HitLeechHits = 25;
			WeaponAttributes.HitLeechMana = 25;
			WeaponAttributes.HitLeechStam = 25;
			WeaponAttributes.HitLightning = 40;
			WeaponAttributes.HitLowerAttack = 25;
			WeaponAttributes.HitPhysicalArea = 25;
			WeaponAttributes.LowerStatReq = 10;

			Attributes.WeaponSpeed = 30;
			Attributes.WeaponDamage = 30;
		}

		public StrzalaAbarisa(Serial serial)
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

			if (Attributes.WeaponDamage != 30)
				Attributes.WeaponDamage = 30;
		}
	}
}
