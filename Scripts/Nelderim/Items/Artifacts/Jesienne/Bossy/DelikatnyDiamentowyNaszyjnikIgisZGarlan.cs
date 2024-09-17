namespace Server.Items
{
	public class DelikatnyDiamentowyNaszyjnikIgisZGarlan : GoldNecklace
	{
		public override int InitMinHits => 255;
		public override int InitMaxHits => 255;


		[Constructable]
		public DelikatnyDiamentowyNaszyjnikIgisZGarlan()
		{
			Hue = 1167;
			Name = "Delikatny Diamentowy Naszyjnik Igis z Garlan";
			Attributes.LowerRegCost = 20;
			Attributes.LowerManaCost = 5;
			SkillBonuses.SetValues(0, SkillName.Inscribe, 5);
			Resistances.Cold = 15;
			Resistances.Physical = 15;
		}

		public DelikatnyDiamentowyNaszyjnikIgisZGarlan(Serial serial)
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
