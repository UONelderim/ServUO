namespace Server.Items
{
	public class TunikaNamiestnikaSnieznejPrzystani : StuddedDo
	{
		public override int InitMinHits => 255;
		public override int InitMaxHits => 255;
		public override int BaseFireResistance { get { return 2; } }
		public override int BaseColdResistance { get { return 8; } }
		public override int BasePhysicalResistance { get { return 10; } }
		public override int BaseEnergyResistance { get { return 0; } }
		public override int BasePoisonResistance { get { return 0; } }

		[Constructable]
		public TunikaNamiestnikaSnieznejPrzystani()
		{
			Hue = 2245;
			Attributes.LowerRegCost = 20;
			Name = "Tunika Namiestnika Snieznej Przystani";
			SkillBonuses.SetValues(0, SkillName.Macing, 10.0);
			Attributes.Luck = 100;
			Attributes.ReflectPhysical = 30;
			Attributes.DefendChance = 10;
			LootType = LootType.Cursed;
		}

		public TunikaNamiestnikaSnieznejPrzystani(Serial serial)
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
