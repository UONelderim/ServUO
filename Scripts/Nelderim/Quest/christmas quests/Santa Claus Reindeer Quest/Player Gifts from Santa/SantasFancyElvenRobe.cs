namespace Server.Items
{
	[FlipableAttribute(0x2FB9, 0x3173)]
	public class SantasFancyElvenRobe : BaseOuterTorso
	{
		[Constructable]
		public SantasFancyElvenRobe() : base(0x2FB9)
		{
			Name = "Piekna szata elfa pana";
			Weight = 3.0;
		}

		public override void GetProperties(ObjectPropertyList list)
		{
			base.GetProperties(list);

			list.Add(1041052);
		}


		public SantasFancyElvenRobe(Serial serial) : base(serial)
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
