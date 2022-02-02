namespace Server.Items
{
	public class HelmTarana : DecorativePlateKabuto
	{
		public override int InitMinHits { get { return 60; } }
		public override int InitMaxHits { get { return 60; } }

		public override int BasePhysicalResistance { get { return 35; } }
		public override int BaseFireResistance { get { return 30; } }
		public override int BaseColdResistance { get { return 3; } }
		public override int BasePoisonResistance { get { return -30; } }
		public override int BaseEnergyResistance { get { return 7; } }

		[Constructable]
		public HelmTarana()
		{
			Hue = 2693;
			Name = "Helm Tarana";
			Attributes.BonusMana = 10;
			Attributes.RegenMana = 1;
			SkillBonuses.SetValues(0, SkillName.Parry, 5);
		}

		public HelmTarana(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}
}
