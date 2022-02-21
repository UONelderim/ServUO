#region References

using Server.Gumps;
using Server.Multis;
using Server.Network;

#endregion

namespace Server.Items
{
	public class GardenGroundAddon : BaseAddon
	{
		public override BaseAddonDeed
			Deed
		{
			get { return new GardenGroundAddonDeed(); }
		} //Oddawaj Deed Po Zrombaniu (powiazania z GardenGroundAddonDeed)

		#region Constructors

		[Constructable]
		public GardenGroundAddon(GardenGroundType type, int width, int height) : this((int)type, width, height)
		{
		}

		public GardenGroundAddon(int type, int width, int height)
		{
			GardenGroundInfo info = GardenGroundInfo.GetInfo(type);

			AddComponent(new AddonComponent(info.GetItemPart(GPosition.Top).ItemID), 0, 0, 0);
			AddComponent(new AddonComponent(info.GetItemPart(GPosition.Right).ItemID), width, 0, 0);
			AddComponent(new AddonComponent(info.GetItemPart(GPosition.Left).ItemID), 0, height, 0);
			AddComponent(new AddonComponent(info.GetItemPart(GPosition.Bottom).ItemID), width, height, 0);

			int w = width - 1;
			int h = height - 1;

			for (int y = 1; y <= h; ++y)
				AddComponent(new AddonComponent(info.GetItemPart(GPosition.West).ItemID), 0, y, 0);

			for (int x = 1; x <= w; ++x)
				AddComponent(new AddonComponent(info.GetItemPart(GPosition.North).ItemID), x, 0, 0);

			for (int y = 1; y <= h; ++y)
				AddComponent(new AddonComponent(info.GetItemPart(GPosition.East).ItemID), width, y, 0);

			for (int x = 1; x <= w; ++x)
				AddComponent(new AddonComponent(info.GetItemPart(GPosition.South).ItemID), x, height, 0);

			for (int x = 1; x <= w; ++x)
			for (int y = 1; y <= h; ++y)
				AddComponent(new AddonComponent(info.GetItemPart(GPosition.Center).ItemID), x, y, 0);
		}

		public GardenGroundAddon(Serial serial) : base(serial)
		{
		}
		//#endregion

		public override void OnDoubleClick(Mobile from)
		{
			BaseHouse house = BaseHouse.FindHouseAt(this);

			if (house != null && house.IsCoOwner(from))
			{
				if (from.InRange(GetWorldLocation(), 3))
				{
					from.SendGump(new RemovalGump(this));
				}
				else
				{
					from.SendLocalizedMessage(500295); // You are too far away to do that.
				}
			}
		}

		#endregion

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

	public enum GardenGroundType
	{
		FurrowNoBorder,
		FurrowBorder,
		PlainNoBorder,
		PlainBorder
	}

	public enum GPosition
	{
		Top,
		Bottom,
		Left,
		Right,
		West,
		North,
		East,
		South,
		Center
	}

	public class GardenGroundInfo
	{
		public GroundPart[] Entries { get; }

		public GardenGroundInfo(GroundPart[] entries)
		{
			Entries = entries;
		}

		public GroundPart GetItemPart(GPosition pos)
		{
			int i = (int)pos;

			if (i < 0 || i >= Entries.Length)
				i = 0;

			return Entries[i];
		}

		public static GardenGroundInfo GetInfo(int type)
		{
			if (type < 0 || type >= Infos.Length)
				type = 0;

			return Infos[type];
		}

		#region GardenGroundInfo definitions

		#endregion

		public static GardenGroundInfo[] Infos { get; } =
		{
/* FurrowNoBorder */
			new GardenGroundInfo(new[]
			{
				new GroundPart(0x32C9, GPosition.Top, -1, -1), new GroundPart(0x32C9, GPosition.Bottom, -1, -1),
				new GroundPart(0x32C9, GPosition.Left, -1, -1), new GroundPart(0x32C9, GPosition.Right, -1, -1),
				new GroundPart(0x32C9, GPosition.West, -1, -1), new GroundPart(0x32C9, GPosition.North, -1, -1),
				new GroundPart(0x32C9, GPosition.East, -1, -1), new GroundPart(0x32C9, GPosition.South, -1, -1),
				new GroundPart(0x32C9, GPosition.Center, 44, 24)
			}),
/* FurrowBorder */
			new GardenGroundInfo(new[]
			{
				new GroundPart(0x1B30, GPosition.Top, 44, 7), new GroundPart(0x1B2F, GPosition.Bottom, 44, 68),
				new GroundPart(0x1B31, GPosition.Left, 0, 35), new GroundPart(0x1B32, GPosition.Right, 88, 32),
				new GroundPart(0x1B29, GPosition.West, 22, 12), new GroundPart(0x1B2A, GPosition.North, 66, 12),
				new GroundPart(0x1B28, GPosition.East, 66, 46), new GroundPart(0x1B27, GPosition.South, 22, 46),
				new GroundPart(0x32C9, GPosition.Center, 44, 24)
			}),
/* PlainNoBorder */
			new GardenGroundInfo(new[]
			{
				new GroundPart(0x31F4, GPosition.Top, -1, -1), new GroundPart(0x31F4, GPosition.Bottom, -1, -1),
				new GroundPart(0x31F4, GPosition.Left, -1, -1), new GroundPart(0x31F4, GPosition.Right, -1, -1),
				new GroundPart(0x31F4, GPosition.West, -1, -1), new GroundPart(0x31F4, GPosition.North, -1, -1),
				new GroundPart(0x31F4, GPosition.East, -1, -1), new GroundPart(0x31F4, GPosition.South, -1, -1),
				new GroundPart(0x31F4, GPosition.Center, 44, 24)
			}),
/* PlainBorder */
			new GardenGroundInfo(new[]
			{
				new GroundPart(0x1B30, GPosition.Top, 44, 7), new GroundPart(0x1B2F, GPosition.Bottom, 44, 68),
				new GroundPart(0x1B31, GPosition.Left, 0, 35), new GroundPart(0x1B32, GPosition.Right, 88, 32),
				new GroundPart(0x1B29, GPosition.West, 22, 11), new GroundPart(0x1B2A, GPosition.North, 66, 12),
				new GroundPart(0x1B28, GPosition.East, 66, 46), new GroundPart(0x1B27, GPosition.South, 22, 46),
				new GroundPart(0x31F4, GPosition.Center, 44, 24)
			})
		};
	}

	public class GroundPart
	{
		public int ItemID { get; }

		public GPosition GPosition { get; }

		// For Gump Rendering
		public int OffsetX { get; }

		// For Gump Rendering
		public int OffsetY { get; }

		public GroundPart(int itemID, GPosition info, int offsetX, int offsetY)
		{
			ItemID = itemID;
			GPosition = info;
			OffsetX = offsetX;
			OffsetY = offsetY;
		}
	}

	public class RemovalGump : Gump
	{
		private GardenGroundAddon m_GGAddon;

		public RemovalGump(GardenGroundAddon ggaddon)
			: base(50, 50)
		{
			m_GGAddon = ggaddon;

			AddBackground(0, 0, 450, 260, 9270);

			AddAlphaRegion(12, 12, 426, 22);
			AddTextEntry(13, 13, 379, 20, 32, 0, @"Uwaga!");

			AddAlphaRegion(12, 39, 426, 209);

			AddHtml(15, 50, 420, 185, "<BODY>" +
			                          "<BASEFONT COLOR=YELLOW>Twoj ogrod zostanie usuniety!<BR><BR>" +
			                          "<BASEFONT COLOR=YELLOW>Usun wszystkie sadzaki jakie zasadziles.<BR><BR>" +
			                          "<BASEFONT COLOR=YELLOW>Po zniszczeniu tego ogrodu ,deed znajdzie sie w twoim plecaku " +
			                          "<BASEFONT COLOR=YELLOW>Czy napewno chcesz zniszczyc ogrod?<BR><BR>" +
			                          "</BODY>", false, false);

			AddButton(13, 220, 0xFA5, 0xFA6, 1, GumpButtonType.Reply, 0);
			AddHtmlLocalized(47, 222, 150, 20, 1052072, 0x7FFF, false, false); // Continue

			//AddButton(200, 245, 0xFB1, 0xFB2, 0, GumpButtonType.Reply, 0);
			//AddHtmlLocalized(47, 247, 450, 20, 1060051, 0x7FFF, false, false); // CANCEL
			AddButton(350, 220, 0xFB1, 0xFB2, 0, GumpButtonType.Reply, 0);
			AddHtmlLocalized(385, 222, 100, 20, 1060051, 0x7FFF, false, false); // CANCEL
		}

		public override void OnResponse(NetState sender, RelayInfo info)
		{
			if (info.ButtonID == 0)
				return;

			Mobile from = sender.Mobile;

			//from.AddToBackpack(new GardenGroundAddonDeed());
			//m_GGAddon.Delete();

			from.SendMessage("Skasowano twoj ogrod.");
		}
	}
}
