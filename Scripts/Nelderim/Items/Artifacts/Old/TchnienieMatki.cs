namespace Server.Items
{
	public class TchnienieMatki : CompositeBow
	{
		public override int LabelNumber { get { return 1065821; } } // Tchnienie Matki
		public override int InitMinHits => 255;
		public override int InitMaxHits => 255;

		[Constructable]
		public TchnienieMatki()
		{
			Hue = 0x94D;
			Attributes.WeaponSpeed = 20;
			WeaponAttributes.HitFireball = 25;
			Attributes.WeaponDamage = 50;
		}

		public TchnienieMatki(Serial serial) : base(serial)
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
