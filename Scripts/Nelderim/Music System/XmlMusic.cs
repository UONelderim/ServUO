#region References

using System.Collections;
using Server.Engines.XmlSpawner2;

#endregion

namespace Server
{
	public class XmlMusic : XmlAttachment
	{
		[CommandProperty(AccessLevel.GameMaster)]
		public Queue PlayList { get; set; }

		[CommandProperty(AccessLevel.GameMaster)]
		public bool FilterMusic { get; set; }

		[CommandProperty(AccessLevel.GameMaster)]
		public bool Playing { get; set; }

		[CommandProperty(AccessLevel.GameMaster)]
		public string Song { get; set; }

		[Attachable]
		public XmlMusic()
			: this(new Queue())
		{
		}

		[Attachable]
		public XmlMusic(Queue playlist)
		{
			FilterMusic = false;
			Playing = false;
			PlayList = playlist;
		}

		public XmlMusic(ASerial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0); // Version

			writer.Write(FilterMusic);
			writer.Write(Song);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			switch (version)
			{
				case 0:
				{
					FilterMusic = reader.ReadBool();
					Song = reader.ReadString();
					Playing = false;
					PlayList = new Queue();
					break;
				}
			}
		}
	}
}
