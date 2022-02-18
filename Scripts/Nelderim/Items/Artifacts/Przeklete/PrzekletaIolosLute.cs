namespace Server.Items
{
	public class PrzekletaIolosLute : Lute
	{
		//public override int LabelNumber { get { return 1063479; } } // Lutnia Matki

		public override int InitMinUses { get { return 3200; } }
		public override int InitMaxUses { get { return 3200; } }

		[Constructable]
		public PrzekletaIolosLute()
		{
			Hue = 2700;
			Slayer = SlayerName.Silver;
			Slayer2 = SlayerName.Exorcism;
			LootType = LootType.Cursed;
			Name = "przekleta lutnia matki";
		}

		public PrzekletaIolosLute(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}
}
