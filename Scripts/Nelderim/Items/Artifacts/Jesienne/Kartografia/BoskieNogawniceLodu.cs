namespace Server.Items
{
	public class BoskieNogawniceLodu : RingmailLegs
	{
		public override int InitMinHits => 255;
		public override int InitMaxHits => 255;
		public override int BaseFireResistance { get { return 10; } }
		public override int BaseColdResistance { get { return 30; } }
		public override int BasePhysicalResistance { get { return 5; } }
		public override int BaseEnergyResistance { get { return 2; } }
		public override int BasePoisonResistance { get { return 30; } }

		[Constructable]
		public BoskieNogawniceLodu()
		{
			Hue = 1153;
			Name = "Boskie Nogawnice Lodu";
			Attributes.RegenMana = 2;
			Attributes.RegenStam = 1;
			Attributes.BonusStam = 3;
			SkillBonuses.SetValues(0, SkillName.Parry, 10.0);
		}

		public BoskieNogawniceLodu(Serial serial)
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
