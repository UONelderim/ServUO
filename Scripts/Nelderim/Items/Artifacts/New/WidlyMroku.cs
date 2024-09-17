namespace Server.Items
{
	public class WidlyMroku : Pitchfork
	{
		public override int LabelNumber { get { return 1065803; } } // Widly Mroku
		public override int InitMinHits => 255;
		public override int InitMaxHits => 255;

		[Constructable]
		public WidlyMroku()
		{
			Hue = 2977;
			Attributes.WeaponDamage = 50;
			WeaponAttributes.HitFireArea = 15;
			WeaponAttributes.HitFireball = 25;
			WeaponAttributes.ResistFireBonus = 5;
			Attributes.SpellChanneling = 1;
			Attributes.WeaponSpeed = -25;
		}

		public WidlyMroku(Serial serial)
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
