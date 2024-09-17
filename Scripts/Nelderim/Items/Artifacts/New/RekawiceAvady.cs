namespace Server.Items
{
	public class RekawiceAvadaGrava : BoneGloves
	{
		public override int LabelNumber { get { return 1065800; } } // Rekawice Avady
		public override int InitMinHits => 255;
		public override int InitMaxHits => 255;

		public override int BasePhysicalResistance { get { return 8; } }
		public override int BaseFireResistance { get { return 12; } }
		public override int BaseColdResistance { get { return 8; } }
		public override int BasePoisonResistance { get { return 9; } }
		public override int BaseEnergyResistance { get { return 8; } }

		[Constructable]
		public RekawiceAvadaGrava()
		{
			Hue = 1174;
			Attributes.DefendChance = 10;
			Attributes.LowerManaCost = 5;
			Attributes.LowerRegCost = 12;
			ArmorAttributes.MageArmor = 1;
			Attributes.NightSight = 1;
		}

		public RekawiceAvadaGrava(Serial serial)
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
