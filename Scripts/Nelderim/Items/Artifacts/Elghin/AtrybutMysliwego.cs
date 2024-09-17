namespace Server.Items
{
	public class AtrybutMysliwego : HideChest
	{
		public override int InitMinHits => 255;
		public override int InitMaxHits => 255;
		public override int BaseFireResistance { get { return -10; } }
		public override int BaseColdResistance { get { return 3; } }
		public override int BasePhysicalResistance { get { return 20; } }
		public override int BaseEnergyResistance { get { return 10; } }
		public override int BasePoisonResistance { get { return 10; } }

		[Constructable]
		public AtrybutMysliwego()
		{
			Hue = 2913;
			Attributes.CastRecovery = -1;
			Name = "Atrybut Mysliwego";
			Attributes.ReflectPhysical = 15;
			SkillBonuses.SetValues(0, SkillName.DetectHidden, 5.0);
			LootType = LootType.Cursed;
		}

		public AtrybutMysliwego(Serial serial)
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
