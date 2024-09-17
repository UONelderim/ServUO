namespace Server.Items
{
	public class ZgubaDemonaOgnia : PlateArms
	{
		public override int LabelNumber { get { return 1065789; } } // Zguba Demona Ognia
		public override int InitMinHits => 255;
		public override int InitMaxHits => 255;

		public override int BaseColdResistance { get { return 0; } }
		public override int BaseEnergyResistance { get { return 3; } }
		public override int BasePhysicalResistance { get { return 10; } }
		public override int BasePoisonResistance { get { return 0; } }
		public override int BaseFireResistance { get { return 25; } }

		[Constructable]
		public ZgubaDemonaOgnia()
		{
			Hue = 1208;
			Attributes.AttackChance = 5;
			Attributes.DefendChance = 10;
			Attributes.WeaponDamage = 20;
		}

		public ZgubaDemonaOgnia(Serial serial)
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
