using System;
using System.Collections.Generic;
using Nelderim.Towns;
using Server.Network;

namespace Server.Gumps
{
	public class TownLogGump : Gump
	{
		private const int LabelColor = 0x21;
		private const int SelectedColor = 0x7A1;
		private readonly Towns m_fromTown;

		public void AddButton(int x, int y, int buttonID, int buttonCli)
		{
			AddButton(x, y, 4006, 4007, buttonID, GumpButtonType.Reply, 0);
			AddHtmlLocalized(x + 40, y, 250, 250, buttonCli, LabelColor, false, false);
		}

		int GetLabel(TownLogTypes tlp)
		{
			switch (tlp)
			{
				case TownLogTypes.OBYWATELSTWO_NADANIE:
					return 0x40;
				case TownLogTypes.OBYWATELSTWO_ZAKONCZONO:
					return 0x21;
				case TownLogTypes.OBYWATELSTWO_STATUS_NADANO:
				case TownLogTypes.KANCLERZ_NADANO_GLOWNY:
				case TownLogTypes.KANCLERZ_NADANO_DYPLOMACJI:
				case TownLogTypes.KANCLERZ_NADANO_ARMII:
				case TownLogTypes.KANCLERZ_NADANO_EKONOMII:
				case TownLogTypes.KANCLERZ_NADANO_BUDOWNICTWA:
					return 0x40;
				case TownLogTypes.BUDYNEK_ZLECONO_BUDOWE:
					return 0x58;
				case TownLogTypes.BUDYNEK_ZAKONCZONO_BUDOWE:
					return 0x40;
				case TownLogTypes.BUDYNEK_ZAWIESZONO_DZIALANIE:
					return 0x2D;
				case TownLogTypes.BUDYNEK_WZNOWIONO_DZIALANIE:
					return 0x40;
				case TownLogTypes.BUDYNEK_ZNISZCZONO:
					return 0x21;
				case TownLogTypes.PODATKI_ZMIENIONO:
					return 0x76;
				case TownLogTypes.POSTERUNEK_WYBUDOWANO:
					return 0x40;
				case TownLogTypes.POSTERUNEK_ZAWIESZONO:
					return 0x2D;
				case TownLogTypes.POSTERUNEK_WZNOWIONO:
					return 0x40;
				case TownLogTypes.SUROWCE_WYSLANO:
				case TownLogTypes.SUROWCE_OTRZYMANO:
				case TownLogTypes.RELACJE_ZMIENIONO:
					return 0x76;
				default:
					return 0x21;
			}
		}

		public TownLogGump(Towns town, Mobile from)
			: base(50, 40)
		{
			m_fromTown = town;
			from.CloseGump(typeof(TownLogGump));

			AddPage(0);

			AddBackground(0, 0, 620, 800, 5054);

			AddHtmlLocalized(10, 10, 210, 28, 1063943, LabelColor, true, false); // Ostatnie wydarzenia
			AddButton(250, 10, 10, 1063946); // Odswiez

			int y = 40;
			List<TownLog> tls = new List<TownLog>();
			foreach (TownLog tl in TownDatabase.GetTown(m_fromTown).TownLogs)
			{
				tls.Add(tl);
			}

			tls.Reverse();
			foreach (TownLog tl in tls)
			{
				AddLabel(10, y, GetLabel(tl.LogType), String.Format("{0}", tl));
				y += 20;
			}
		}

		public override void OnResponse(NetState sender, RelayInfo info)
		{
			Mobile from = sender.Mobile;

			int val = info.ButtonID;

			if (val <= 0)
				return;

			from.SendGump(new TownLogGump(m_fromTown, from));
		}
	}
}
