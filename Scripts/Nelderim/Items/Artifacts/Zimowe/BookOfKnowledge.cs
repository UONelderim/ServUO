namespace Server.Items
{
	public class BookOfKnowledge : Spellbook
	{
		[Constructable]
		public BookOfKnowledge() : base()
		{
			Name = "Ksiega Wiedzy";
			Hue = 1171;

			Attributes.SpellDamage = 25;
			Attributes.LowerManaCost = 5;
			Attributes.LowerRegCost = 5;
			Attributes.CastSpeed = -1;
			Attributes.CastRecovery = 1;
			LootType = LootType.Regular;
		}

		public BookOfKnowledge(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}
}
