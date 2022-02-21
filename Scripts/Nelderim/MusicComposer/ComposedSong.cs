namespace Server.Items
{
	public class ComposedSong : BaseMusicComposedSong
	{
		[Constructable]
		public ComposedSong(string _Song, string _SongName) : base(_Song, _SongName)
		{
		}

		public ComposedSong(Serial serial) : base(serial)
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
