namespace Server.Items
{
	public class SpodniePodstepu : LeafLegs
	{
		public override int LabelNumber { get { return 1065784; } } // Spodnie Podstepu
		public override int InitMinHits => 255;
		public override int InitMaxHits => 255;

		public override int BasePhysicalResistance { get { return 8; } }
		public override int BaseFireResistance { get { return 6; } }
		public override int BaseColdResistance { get { return 5; } }
		public override int BasePoisonResistance { get { return 15; } }
		public override int BaseEnergyResistance { get { return 12; } }

		[Constructable]
		public SpodniePodstepu()
		{
			Hue = 38;
			Attributes.AttackChance = 5;
			Attributes.DefendChance = 10;
			Attributes.LowerManaCost = 5;
			Attributes.NightSight = 1;
			Name = "Spodnie Podstepu";
		}

		public SpodniePodstepu(Serial serial)
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
