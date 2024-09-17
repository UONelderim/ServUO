namespace Server.Items
{
	public class Aderthand : Yumi
	{
		public override int InitMinHits => 255;
		public override int InitMaxHits => 255;

		[Constructable]
		public Aderthand()
		{
			Hue = 1396;
			Name = "Aderthand";
			WeaponAttributes.HitColdArea = 10;
			WeaponAttributes.HitEnergyArea = 10;
			WeaponAttributes.HitFireArea = 10;
			Attributes.WeaponSpeed = 25;
			Attributes.WeaponDamage = 35;
		}

		public Aderthand(Serial serial)
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
