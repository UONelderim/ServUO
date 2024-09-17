namespace Server.Items
{
	public class OgienZKuchniMurdulfa : LeafGorget
	{
		public override int InitMinHits => 255;
		public override int InitMaxHits => 255;

		public override int BasePhysicalResistance { get { return 15; } }
		public override int BaseFireResistance { get { return 10; } }
		public override int BaseColdResistance { get { return 10; } }
		public override int BasePoisonResistance { get { return 8; } }
		public override int BaseEnergyResistance { get { return 18; } }

		[Constructable]
		public OgienZKuchniMurdulfa()
		{
			Hue = 1161;
			Name = "Ogien Z Kuchni Murdulfa";
			Attributes.BonusDex = 5;
			SkillBonuses.SetValues(0, SkillName.Cooking, 5);
		}

		public OgienZKuchniMurdulfa(Serial serial)
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
