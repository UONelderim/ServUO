namespace Server.Items
{
	public class SpodenkiLukmistrza : ShortPants
	{
		public override int InitMinHits => 255;
		public override int InitMaxHits => 255;

		public override int BaseEnergyResistance { get { return 2; } }

		[Constructable]
		public SpodenkiLukmistrza()
		{
			Hue = 1832;
			SkillBonuses.SetValues(0, SkillName.Fletching, 5.0);
			Name = "Spodenki Lukmistrza";
			LootType = LootType.Cursed;
		}

		public SpodenkiLukmistrza(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.WriteEncodedInt(0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadEncodedInt();
		}
	}
}
