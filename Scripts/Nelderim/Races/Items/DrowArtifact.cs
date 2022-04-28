namespace Server.Items
{
	public class DrowArtifact : RaceChoiceItem
	{
		public DrowArtifact(Serial serial) : base(serial)
		{
		}

		[Constructable]
		public DrowArtifact() : base(0x1401)
		{
			Name = "Artefakt Drowow";
			Hue = 2082;
			m_Race = Race.NDrow;
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
