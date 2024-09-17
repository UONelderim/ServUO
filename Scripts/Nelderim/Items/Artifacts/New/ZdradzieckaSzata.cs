namespace Server.Items
{
	public class ZdradzieckaSzata : Robe
	{
		public override int LabelNumber { get { return 1065765; } } // Zdradziecka Szata
		public override int InitMinHits => 255;
		public override int InitMaxHits => 255;

		public override int BasePhysicalResistance { get { return -5; } }
		public override int BaseFireResistance { get { return -5; } }
		public override int BaseColdResistance { get { return -5; } }
		public override int BasePoisonResistance { get { return -5; } }
		public override int BaseEnergyResistance { get { return -5; } }

		[Constructable]
		public ZdradzieckaSzata()
		{
			Hue = 1107;
			Attributes.Luck = 50;
			Attributes.ReflectPhysical = 10;
			Attributes.SpellDamage = 5;
		}

		public ZdradzieckaSzata(Serial serial)
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
