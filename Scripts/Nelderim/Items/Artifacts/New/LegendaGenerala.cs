namespace Server.Items
{
	public class LegendaGenerala : GoldBracelet
	{
		public override int LabelNumber { get { return 1065754; } } // Legenda Generala
		public override int InitMinHits => 255;
		public override int InitMaxHits => 255;

		[Constructable]
		public LegendaGenerala()
		{
			Hue = 2117;

			Attributes.AttackChance = 10;
			Attributes.DefendChance = 10;
			Attributes.RegenHits = 1;
			Attributes.RegenStam = 1;
			Attributes.RegenMana = 1;
			SkillBonuses.SetValues(0, SkillName.Tactics, 20.0);
		}

		public LegendaGenerala(Serial serial)
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
