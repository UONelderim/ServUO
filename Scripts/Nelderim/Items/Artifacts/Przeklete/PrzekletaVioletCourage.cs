namespace Server.Items
{
	public class PrzekletaVioletCourage : FemalePlateChest
	{
		//public override int LabelNumber { get { return 1063471; } } // Fioletowa Odwaga
		public override int InitMinHits { get { return 60; } }
		public override int InitMaxHits { get { return 60; } }

		public override int BasePhysicalResistance { get { return 14; } }
		public override int BaseFireResistance { get { return 12; } }
		public override int BaseColdResistance { get { return 12; } }
		public override int BasePoisonResistance { get { return 18; } }
		public override int BaseEnergyResistance { get { return 19; } }

		[Constructable]
		public PrzekletaVioletCourage()
		{
			Hue = 1180;
			LootType = LootType.Cursed;
			Attributes.Luck = 95;
			Attributes.DefendChance = 25;
			ArmorAttributes.LowerStatReq = 100;
			ArmorAttributes.MageArmor = 1;
		}

		public PrzekletaVioletCourage(Serial serial) : base(serial)
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
