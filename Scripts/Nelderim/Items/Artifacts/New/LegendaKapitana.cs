namespace Server.Items
{
	public class LegendaKapitana : ChainHatsuburi
	{
		public override int LabelNumber { get { return 1065753; } } // Legenda Kapitana
		public override int InitMinHits => 255;
		public override int InitMaxHits => 255;

		public override int BaseColdResistance { get { return 5; } }
		public override int BaseEnergyResistance { get { return 2; } }
		public override int BasePhysicalResistance { get { return 2; } }
		public override int BasePoisonResistance { get { return 2; } }
		public override int BaseFireResistance { get { return 7; } }

		[Constructable]
		public LegendaKapitana()
		{
			Hue = 1157;

			ArmorAttributes.LowerStatReq = 50;
			Attributes.AttackChance = 5;
			Attributes.DefendChance = 5;
			Attributes.RegenHits = 5;
			Attributes.WeaponDamage = 5;
		}

		public LegendaKapitana(Serial serial)
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
