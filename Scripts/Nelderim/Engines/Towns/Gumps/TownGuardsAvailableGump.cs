using System.Collections.Generic;
using Nelderim.Towns;
using Server.Network;

namespace Server.Gumps
{
	public class TownGuardsAvailableGump : Gump
	{
		private const int LabelColor = 0x21;
		private const int SelectedColor = 0x7A1;
		private readonly Towns m_fromTown;

		public void AddButton(int x, int y, int buttonID, string title)
		{
			AddButton(x, y, 4006, 4007, buttonID, GumpButtonType.Reply, 0);
			AddLabel(x + 40, y, SelectedColor, title);
		}

		void AddPriceLabels(TownGuards tg, int height)
		{
			TownResources tr = TownDatabase.GetTown(m_fromTown).GetGuardPrice(tg);
			// Aktywacja
			AddLabel(350, height, SelectedColor, tr.ResourceAmount(TownResourceType.Zloto).ToString());
			AddLabel(400, height, SelectedColor, tr.ResourceAmount(TownResourceType.Zbroje).ToString());
			AddLabel(450, height, SelectedColor, tr.ResourceAmount(TownResourceType.Bronie).ToString());
			AddLabel(500, height, SelectedColor, tr.ResourceAmount(TownResourceType.Ziola).ToString());
			AddLabel(550, height, SelectedColor, tr.ResourceAmount(TownResourceType.Klejnoty).ToString());
			// Reaktywacja
			AddLabel(650, height, SelectedColor, tr.ResourceMaxAmount(TownResourceType.Zloto).ToString());
			AddLabel(700, height, SelectedColor, tr.ResourceMaxAmount(TownResourceType.Zbroje).ToString());
			AddLabel(750, height, SelectedColor, tr.ResourceMaxAmount(TownResourceType.Bronie).ToString());
			AddLabel(800, height, SelectedColor, tr.ResourceMaxAmount(TownResourceType.Ziola).ToString());
			AddLabel(850, height, SelectedColor, tr.ResourceMaxAmount(TownResourceType.Klejnoty).ToString());
			// Wskrzeszenie
			AddLabel(950, height, SelectedColor, tr.ResourceDailyChange(TownResourceType.Zloto).ToString());
			AddLabel(1000, height, SelectedColor, tr.ResourceDailyChange(TownResourceType.Zbroje).ToString());
			AddLabel(1050, height, SelectedColor, tr.ResourceDailyChange(TownResourceType.Bronie).ToString());
			AddLabel(1100, height, SelectedColor, tr.ResourceDailyChange(TownResourceType.Ziola).ToString());
			AddLabel(1150, height, SelectedColor, tr.ResourceDailyChange(TownResourceType.Klejnoty).ToString());
		}

		public TownGuardsAvailableGump(Towns town, Mobile from)
			: base(50, 40)
		{
			m_fromTown = town;
			AddPage(0);

			AddBackground(0, 0, 1200, 190, 5054);

			AddHtmlLocalized(10, 10, 210, 28, 1063923, LabelColor, true, false); // Dostepni straznicy
			AddLabel(350, 10, SelectedColor, "Cena aktywacji");
			AddLabel(350, 30, SelectedColor, "Zloto");
			AddLabel(400, 30, SelectedColor, "Zbroje");
			AddLabel(450, 30, SelectedColor, "Bronie");
			AddLabel(500, 30, SelectedColor, "Ziola");
			AddLabel(550, 30, SelectedColor, "Klejnoty");

			AddLabel(650, 10, SelectedColor, "Cena odwieszenia");
			AddLabel(650, 30, SelectedColor, "Zloto");
			AddLabel(700, 30, SelectedColor, "Zbroje");
			AddLabel(750, 30, SelectedColor, "Bronie");
			AddLabel(800, 30, SelectedColor, "Ziola");
			AddLabel(850, 30, SelectedColor, "Klejnoty");

			AddLabel(950, 10, SelectedColor, "Cena wskrzeszenia");
			AddLabel(950, 30, SelectedColor, "Zloto");
			AddLabel(1000, 30, SelectedColor, "Zbroje");
			AddLabel(1050, 30, SelectedColor, "Bronie");
			AddLabel(1100, 30, SelectedColor, "Ziola");
			AddLabel(1150, 30, SelectedColor, "Klejnoty");

			List<TownGuards> tg = TownDatabase.GetTown(town).GetAvailableGuards();

			AddLabel(10, 50, SelectedColor, "Straznik");
			if (tg.Contains(TownGuards.Straznik))
			{
				AddLabel(150, 50, SelectedColor, "- dostepny");
			}

			AddPriceLabels(TownGuards.Straznik, 50);

			AddLabel(10, 70, SelectedColor, "Ciezki straznik");
			if (tg.Contains(TownGuards.CiezkiStraznik))
			{
				AddLabel(150, 70, SelectedColor, "- dostepny");
			}
			else
			{
				AddLabel(150, 70, LabelColor, "- potrzeba Placu Treningowego");
			}

			AddPriceLabels(TownGuards.CiezkiStraznik, 70);

			AddLabel(10, 90, SelectedColor, "Strzelec");
			if (tg.Contains(TownGuards.Strzelec))
			{
				AddLabel(150, 90, SelectedColor, "- dostepny");
			}
			else
			{
				AddLabel(150, 90, LabelColor, "- potrzeba Placu Treningowego");
			}

			AddPriceLabels(TownGuards.Strzelec, 90);

			AddLabel(10, 110, SelectedColor, "Straznik Konny");
			if (tg.Contains(TownGuards.StraznikKonny))
			{
				AddLabel(150, 110, SelectedColor, "- dostepny");
			}
			else
			{
				AddLabel(150, 110, LabelColor, "- potrzeba Koszar");
			}

			AddPriceLabels(TownGuards.StraznikKonny, 110);

			AddLabel(10, 130, SelectedColor, "Straznik Mag");
			if (tg.Contains(TownGuards.StraznikMag))
			{
				AddLabel(150, 130, SelectedColor, "- dostepny");
			}
			else
			{
				AddLabel(150, 130, LabelColor, "- potrzeba Bibliotekii");
			}

			AddPriceLabels(TownGuards.StraznikMag, 130);

			AddLabel(10, 150, SelectedColor, "Straznik Elitarny");
			if (tg.Contains(TownGuards.StraznikElitarny))
			{
				AddLabel(150, 150, SelectedColor, "- dostepny");
			}
			else
			{
				AddLabel(150, 150, LabelColor, "- potrzeba Twierdzy");
			}

			AddPriceLabels(TownGuards.StraznikElitarny, 150);
		}

		public override void OnResponse(NetState sender, RelayInfo info)
		{
			Mobile from = sender.Mobile;

			int val = info.ButtonID;

			if (val <= 0)
				return;

			from.SendGump(new TownGuardsAvailableGump(m_fromTown, from));
		}
	}
}
