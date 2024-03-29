namespace Server.Items
{
	public class PrzekleteFeyLeggings : ChainLegs
	{
		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		public override int BasePhysicalResistance { get { return 12; } }
		public override int BaseFireResistance { get { return 18; } }
		public override int BaseColdResistance { get { return 17; } }
		public override int BasePoisonResistance { get { return 4; } }
		public override int BaseEnergyResistance { get { return 19; } }

		[Constructable]
		public PrzekleteFeyLeggings()
		{
			Name = "Przeklęte Kolcze Spodnie Błogosławione Przez Naneth";
			Hue = 1180;
			LootType = LootType.Cursed;
			Attributes.BonusHits = 6;
			Attributes.DefendChance = 30;

			ArmorAttributes.MageArmor = 1;
		}

		public PrzekleteFeyLeggings(Serial serial) : base(serial)
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
