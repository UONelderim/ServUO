namespace Server.Items
{
	public class Retorta : WarFork
	{
		public override int LabelNumber { get { return 1065766; } } // Retorta
		public override int InitMinHits => 255;
		public override int InitMaxHits => 255;

		[Constructable]
		public Retorta()
		{
			Hue = 498;
			WeaponAttributes.HitLowerDefend = 30;
			Attributes.BonusDex = 5;
			Attributes.WeaponDamage = 30;
			Attributes.WeaponSpeed = 25;
		}

		public Retorta(Serial serial)
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
