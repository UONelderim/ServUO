namespace Server.Items
{
	public class Przysiega : PlateChest
	{
		public override int LabelNumber { get { return 1065763; } } // Przysiega
		public override int InitMinHits => 255;
		public override int InitMaxHits => 255;

		public override int BasePhysicalResistance { get { return 8; } }
		public override int BaseFireResistance { get { return 12; } }
		public override int BaseColdResistance { get { return 10; } }
		public override int BasePoisonResistance { get { return 7; } }
		public override int BaseEnergyResistance { get { return 10; } }


		[Constructable]
		public Przysiega()
		{
			Hue = 1388;
			ArmorAttributes.MageArmor = 1;
			ArmorAttributes.LowerStatReq = 50;
			Attributes.BonusHits = 10;
			Attributes.BonusMana = 10;
			Attributes.BonusStam = 10;
			Attributes.ReflectPhysical = 25;
		}

		public Przysiega(Serial serial)
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
