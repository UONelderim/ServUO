using System.Collections.Generic;
using Nelderim.Towns;
using Server.Gumps;

namespace Server.Items
{
	public class DuszaMiasta : Item
	{
		public override int LabelNumber { get { return 1063735; } } // Dusza miasta
		public Towns Town = Towns.None;

		[Constructable]
		public DuszaMiasta() : base(0x1184)
		{
			Movable = false;
			Hue = 2935;
		}

		public DuszaMiasta(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(2);

			writer.Write((int)Town);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
			switch (version)
			{
				case 2:
					Town = (Towns)reader.ReadInt();
					break;
				case 1:
					Town = (Towns)reader.ReadInt();
					goto case 0;
				case 0:
				{
					List<TownResource> Resources = new List<TownResource>();
					Resources.Add(new TownResource(TownResourceType.Zloto, reader.ReadInt(), -1, reader.ReadInt()));
					Resources.Add(new TownResource(TownResourceType.Deski, reader.ReadInt(), reader.ReadInt(),
						reader.ReadInt()));
					Resources.Add(new TownResource(TownResourceType.Sztaby, reader.ReadInt(), reader.ReadInt(),
						reader.ReadInt()));
					Resources.Add(new TownResource(TownResourceType.Skora, reader.ReadInt(), reader.ReadInt(),
						reader.ReadInt()));
					Resources.Add(new TownResource(TownResourceType.Material, reader.ReadInt(), reader.ReadInt(),
						reader.ReadInt()));
					Resources.Add(new TownResource(TownResourceType.Kosci, reader.ReadInt(), reader.ReadInt(),
						reader.ReadInt()));
					Resources.Add(new TownResource(TownResourceType.Kamienie, reader.ReadInt(), reader.ReadInt(),
						reader.ReadInt()));
					Resources.Add(new TownResource(TownResourceType.Piasek, reader.ReadInt(), reader.ReadInt(),
						reader.ReadInt()));
					Resources.Add(new TownResource(TownResourceType.Klejnoty, reader.ReadInt(), reader.ReadInt(),
						reader.ReadInt()));
					Resources.Add(new TownResource(TownResourceType.Ziola, reader.ReadInt(), reader.ReadInt(),
						reader.ReadInt()));
					Resources.Add(new TownResource(TownResourceType.Zbroje, reader.ReadInt(), reader.ReadInt(),
						reader.ReadInt()));
					Resources.Add(new TownResource(TownResourceType.Bronie, reader.ReadInt(), reader.ReadInt(),
						reader.ReadInt()));
					break;
				}
			}
		}

		public override void OnDoubleClick(Mobile m)
		{
			m.SendGump(new TownResourcesGump(TownResourcesGumpPage.Information, m, this));
		}
	}
}
