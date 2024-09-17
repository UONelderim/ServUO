namespace Server.Items
{
	public class SpodnieOswiecenia : LeafLegs
	{
		public override int LabelNumber { get { return 1065783; } } // Spodnie Oswiecenia
		public override int InitMinHits => 255;
		public override int InitMaxHits => 255;

		public override int BaseFireResistance { get { return 9; } }
		public override int BasePoisonResistance { get { return 12; } }
		public override int BasePhysicalResistance { get { return 11; } }
		public override int BaseEnergyResistance { get { return 6; } }
		public override int BaseColdResistance { get { return 5; } }

		[Constructable]
		public SpodnieOswiecenia()
		{
			Hue = 0x487;

			SkillBonuses.SetValues(0, SkillName.EvalInt, 10.0);

			Attributes.BonusInt = 8;
			Attributes.SpellDamage = 10;
			Attributes.LowerManaCost = 5;
			Name = "Spodnie Oswiecenia";
		}

		public SpodnieOswiecenia(Serial serial)
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
