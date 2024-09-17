namespace Server.Items
{
	public class PlaszczPoszukiwaczaPrzygod : Cloak
	{
		public override int InitMinHits => 255;
		public override int InitMaxHits => 255;
		public override int BaseFireResistance { get { return 2; } }

		[Constructable]
		public PlaszczPoszukiwaczaPrzygod()
		{
			Hue = 2849;
			ItemID = 0x2B05;
			Name = "Plaszcz Poszukiwacza Przygod";
			SkillBonuses.SetValues(0, SkillName.Tracking, 10.0);
			LootType = LootType.Cursed;
		}

		public PlaszczPoszukiwaczaPrzygod(Serial serial)
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
