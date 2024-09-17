namespace Server.Items
{
	public class HelmWladcyMorrlokow : HeavyPlateJingasa
	{
		public override int InitMinHits => 255;
		public override int InitMaxHits => 255;

		public override int BasePhysicalResistance { get { return 10; } }
		public override int BaseFireResistance { get { return 10; } }
		public override int BaseColdResistance { get { return 7; } }
		public override int BasePoisonResistance { get { return 1; } }
		public override int BaseEnergyResistance { get { return 0; } }

		[Constructable]
		public HelmWladcyMorrlokow()
		{
			Hue = 1165;
			Name = "Helm Wladcy Morrlokow";
			Attributes.BonusHits = 15;
			Attributes.RegenHits = 2;
			Attributes.WeaponSpeed = 10;
		}

		public HelmWladcyMorrlokow(Serial serial)
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
