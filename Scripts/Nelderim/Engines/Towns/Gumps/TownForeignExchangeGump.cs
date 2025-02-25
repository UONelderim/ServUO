using System;
using Nelderim.Towns;
using Server.Network;

namespace Server.Gumps
{
	public class TownForeignExchangeGump : Gump
	{
		private const int LabelColor = 0x7FFF;
		private const int SelectedColor = 0x7A1;
		private readonly Towns m_town;

		public void AddButton(int x, int y, int buttonID, string title)
		{
			AddButton(x, y, 4006, 4007, buttonID, GumpButtonType.Reply, 0);
			AddLabel(x + 40, y, SelectedColor, title);
		}

		public TownForeignExchangeGump(Towns town, Mobile from)
			: base(50, 40)
		{
			m_town = town;
			AddPage(0);

			AddBackground(0, 0, 355, 210, 5054);

			AddHtmlLocalized(10, 10, 250, 250, 1063884, LabelColor, false, false); // Portowy Handel Zagraniczny

			AddHtmlLocalized(10, 30, 250, 250, 1063888, LabelColor, false, false); // Export
			if (TownAnnouncer.ForeignResourcesToBuy.Count == 0)
			{
				AddLabel(10, 50, SelectedColor, "Statek przyplynal pusty");
			}
			else
			{
				AddLabel(10, 50, SelectedColor,
					String.Format("{0} po cenie {1} za sztuke", TownAnnouncer.ForeignResourcesToBuy[0].ResourceType,
						TownAnnouncer.ForeignResourcesToBuy[0].Amount));
				AddButton(210, 50, 1, "100");
				AddButton(280, 50, 2, "1000");
				AddLabel(10, 70, SelectedColor,
					String.Format("{0} po cenie {1} za sztuke", TownAnnouncer.ForeignResourcesToBuy[1].ResourceType,
						TownAnnouncer.ForeignResourcesToBuy[1].Amount));
				AddButton(210, 70, 11, "100");
				AddButton(280, 70, 12, "1000");
				AddLabel(10, 90, SelectedColor,
					String.Format("{0} po cenie {1} za sztuke", TownAnnouncer.ForeignResourcesToBuy[2].ResourceType,
						TownAnnouncer.ForeignResourcesToBuy[2].Amount));
				AddButton(210, 90, 21, "100");
				AddButton(280, 90, 22, "1000");
			}

			AddHtmlLocalized(10, 120, 250, 250, 1063887, LabelColor, false, false); // Import
			if (TownAnnouncer.ForeignResourcesToSell.Count == 0)
			{
				AddLabel(10, 140, SelectedColor, "Statek nie przyjmuje towarow");
			}
			else
			{
				AddLabel(10, 140, SelectedColor,
					String.Format("{0} po cenie {1} za sztuke", TownAnnouncer.ForeignResourcesToSell[0].ResourceType,
						TownAnnouncer.ForeignResourcesToSell[0].Amount));
				AddButton(210, 140, 31, "10");
				AddButton(280, 140, 32, "100");
				AddLabel(10, 160, SelectedColor,
					String.Format("{0} po cenie {1} za sztuke", TownAnnouncer.ForeignResourcesToSell[1].ResourceType,
						TownAnnouncer.ForeignResourcesToSell[1].Amount));
				AddButton(210, 160, 41, "10");
				AddButton(280, 160, 42, "100");
				AddLabel(10, 180, SelectedColor,
					String.Format("{0} po cenie {1} za sztuke", TownAnnouncer.ForeignResourcesToSell[2].ResourceType,
						TownAnnouncer.ForeignResourcesToSell[2].Amount));
				AddButton(210, 180, 51, "10");
				AddButton(280, 180, 52, "100");
			}
		}

		public override void OnResponse(NetState sender, RelayInfo info)
		{
			Mobile from = sender.Mobile;

			int val = info.ButtonID;

			if (val <= 0)
				return;

			int howMuch = val % 10 == 1 ? 100 : 1000;
			int which = val / 10;

			TownManager m_townM = TownDatabase.GetTown(m_town);

			// Export
			if (val < 30)
			{
				// Sprawdz czy miasto posiada wymagany surowiec
				if (m_townM.Resources.HasResourceAmount(TownAnnouncer.ForeignResourcesToBuy[which].ResourceType,
					    howMuch))
				{
					from.SendAsciiMessage("Export towaru {0} w ilosci {1} za lacznej ilosc zlota = {2}",
						TownAnnouncer.ForeignResourcesToBuy[which].ResourceType.ToString(),
						howMuch.ToString(),
						TownAnnouncer.ForeignResourcesToBuy[which].Amount * howMuch);
					m_townM.Resources.ResourceDecreaseAmount(TownAnnouncer.ForeignResourcesToBuy[which].ResourceType,
						howMuch);
					m_townM.Resources.ResourceIncreaseAmount(TownResourceType.Zloto,
						TownAnnouncer.ForeignResourcesToBuy[which].Amount * howMuch);
				}
				else
				{
					from.SendLocalizedMessage(1063889);
				}
			}
			// Import
			else
			{
				// Sprawdz czy miasto posiada wymagane zloto na zakup
				if (m_townM.Resources.HasResourceAmount(TownResourceType.Zloto,
					    TownAnnouncer.ForeignResourcesToSell[which - 3].Amount * howMuch / 10))
				{
					// Sprawdz czy miasto posiada miejsce by pomiescic dodatkowe zasoby
					if (m_townM.Resources.IsResourceAmountProper(
						    TownAnnouncer.ForeignResourcesToSell[which - 3].ResourceType, howMuch / 10))
					{
						from.SendAsciiMessage(String.Format("Import towaru {0} w ilosci {1} o lacznej cenie {2}",
							TownAnnouncer.ForeignResourcesToSell[which - 3].ResourceType.ToString(),
							(howMuch / 10).ToString(),
							TownAnnouncer.ForeignResourcesToSell[which - 3].Amount * howMuch / 10));
						m_townM.Resources.ResourceDecreaseAmount(TownResourceType.Zloto,
							TownAnnouncer.ForeignResourcesToSell[which - 3].Amount * howMuch / 10);
						m_townM.Resources.ResourceIncreaseAmount(
							TownAnnouncer.ForeignResourcesToSell[which - 3].ResourceType, howMuch / 10);
					}
					else
					{
						from.SendLocalizedMessage(1063891);
					}
				}
				else
				{
					from.SendLocalizedMessage(1063890);
				}
			}

			from.SendGump(new TownForeignExchangeGump(m_town, from));
		}
	}
}
