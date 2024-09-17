namespace Server.Items
{
	public class MaskaZabojcy : StuddedMempo
	{
		public override int InitMinHits => 255;
		public override int InitMaxHits => 255;
		public override int BaseFireResistance { get { return 2; } }
		public override int BaseColdResistance { get { return 3; } }
		public override int BasePhysicalResistance { get { return 10; } }
		public override int BaseEnergyResistance { get { return 20; } }
		public override int BasePoisonResistance { get { return 10; } }

		[Constructable]
		public MaskaZabojcy()
		{
			Hue = 299;
			Attributes.CastSpeed = 1;
			Name = "Maska Zabojcy";
			Attributes.RegenMana = 2;
			Attributes.AttackChance = 10;
			LootType = LootType.Cursed;
		}

		public MaskaZabojcy(Serial serial)
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
