namespace Server.Items
{
	public class PocalunekBoginii : SilverRing
	{
		public override int LabelNumber { get { return 1065747; } } // Pocalunek Boginii
		public override int InitMinHits => 255;
		public override int InitMaxHits => 255;

		[Constructable]
		public PocalunekBoginii()
		{
			Hue = 0x47E;

			Attributes.BonusInt = 5;
			Attributes.BonusMana = 10;
			Attributes.CastRecovery = 1;
			Attributes.CastSpeed = 1;
			Attributes.NightSight = 1;
			Attributes.SpellDamage = 5;

			SkillBonuses.SetValues(0, SkillName.Spellweaving, 5.0);
		}

		public PocalunekBoginii(Serial serial) : base(serial)
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
