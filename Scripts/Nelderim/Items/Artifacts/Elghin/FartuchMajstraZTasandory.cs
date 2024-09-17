namespace Server.Items
{
	public class FartuchMajstraZTasandory : FullApron
	{
		public override int InitMinHits => 255;
		public override int InitMaxHits => 255;

		public override int BasePhysicalResistance { get { return 8; } }

		[Constructable]
		public FartuchMajstraZTasandory()
		{
			Hue = 2314;
			SkillBonuses.SetValues(0, SkillName.Tinkering, 5.0);
			Name = "Fartuch Majstra z Tasandory";
			LootType = LootType.Cursed;
		}

		public FartuchMajstraZTasandory(Serial serial)
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
