namespace Server.Items
{
	public class TrupieRece : BoneGloves
	{
		public override int InitMinHits => 255;
		public override int InitMaxHits => 255;

		public override int BasePhysicalResistance { get { return 20; } }
		public override int BaseFireResistance { get { return 15; } }
		public override int BaseColdResistance { get { return 10; } }
		public override int BasePoisonResistance { get { return 15; } }
		public override int BaseEnergyResistance { get { return -9; } }

		[Constructable]
		public TrupieRece()
		{
			Hue = 832;
			Name = "Trupie Rece";
			Attributes.BonusMana = 5;
			SkillBonuses.SetValues(0, SkillName.Necromancy, 5);
		}

		public TrupieRece(Serial serial)
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
