namespace Server.Items
{
	public class ArkanaZywiolow : Spellbook
	{
		public override SpellbookType SpellbookType { get { return SpellbookType.Regular; } }

		public override int LabelNumber { get { return 1065804; } } // Arkana Zywiolow
		public override int InitMinHits => 255;
		public override int InitMaxHits => 255;

		[Constructable]
		public ArkanaZywiolow()
		{
			Hue = 1381;
			Attributes.CastRecovery = 1;
			Attributes.CastSpeed = 1;
			Attributes.RegenMana = 2;
			Attributes.SpellDamage = 6;
			Slayer = SlayerName.ElementalBan;
			LootType = LootType.Regular;
		}

		public ArkanaZywiolow(Serial serial) : base(serial)
		{
		}

		public override bool OnDragLift(Mobile from)
		{
			//This should override regular spellbook OnDragLift
			bool result = base.OnDragLift(from);
			LootType = LootType.Regular;
			return result;
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
