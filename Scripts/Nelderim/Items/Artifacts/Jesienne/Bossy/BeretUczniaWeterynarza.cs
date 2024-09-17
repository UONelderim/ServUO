namespace Server.Items
{
	public class BeretUczniaWeterynarza : Bonnet
	{
		public override int InitMinHits => 255;
		public override int InitMaxHits => 255;

		public override int BasePhysicalResistance { get { return 10; } }
		public override int BaseFireResistance { get { return 12; } }
		public override int BaseColdResistance { get { return 7; } }
		public override int BasePoisonResistance { get { return 1; } }
		public override int BaseEnergyResistance { get { return 0; } }

		[Constructable]
		public BeretUczniaWeterynarza()
		{
			Hue = 731;
			Name = "Beret Ucznia Weterynarza";
			Attributes.BonusInt = 5;
			SkillBonuses.SetValues(0, SkillName.Veterinary, 10);
		}

		public BeretUczniaWeterynarza(Serial serial)
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
