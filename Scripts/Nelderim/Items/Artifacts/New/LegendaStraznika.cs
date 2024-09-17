namespace Server.Items
{
	public class LegendaStraznika : Halberd
	{
		public override int LabelNumber { get { return 1065756; } } // Legenda Straznika
		public override int InitMinHits => 255;
		public override int InitMaxHits => 255;

		[Constructable]
		public LegendaStraznika()
		{
			Hue = 1407;
			WeaponAttributes.HitLightning = 100;
			WeaponAttributes.HitLowerDefend = 40;
			Attributes.WeaponDamage = 50;
			Slayer = SlayerName.Repond;
		}

		public LegendaStraznika(Serial serial)
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
