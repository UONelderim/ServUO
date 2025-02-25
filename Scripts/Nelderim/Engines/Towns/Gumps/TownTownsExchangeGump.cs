using System;
using Nelderim.Towns;
using Server.Network;

namespace Server.Gumps
{
	public class TownTownsExchangeGump : Gump
	{
		private const int LabelColor = 0x7FFF;
		private const int SelectedColor = 0x7A1;
		private readonly Towns m_town;
		private readonly Towns m_townToSend;

		public void AddButton(int x, int y, int buttonID, string title)
		{
			AddButton(x, y, 4006, 4007, buttonID, GumpButtonType.Reply, 0);
			AddLabel(x + 40, y, SelectedColor, title);
		}

		public TownTownsExchangeGump(Towns town, Mobile from, bool main = true, int townToSend = 0)
			: base(50, 40)
		{
			m_town = town;
			m_townToSend = (Towns)townToSend;

			AddPage(0);

			AddBackground(0, 0, 350, 400, 5054);

			AddHtmlLocalized(10, 10, 250, 250, 1063892, LabelColor, false, false); // Handel Miedzymiastowy

			if (main)
			{
				AddLabelCropped(10, 30, 300, 40, SelectedColor, "Wyslanie kazdej karawny kosztuje 1000 zlota.");
				AddLabel(10, 70, SelectedColor, "Wybierz miasto ktore ma otrzymac surowce");
				int yLab = 90;
				foreach (Towns t in TownDatabase.GetTownsNames())
				{
					if (t != m_town && t != Towns.None)
					{
						AddButton(10, yLab, 1 + (int)t, t.ToString());
						yLab += 20;
					}
				}
			}
			else
			{
				AddLabel(10, 30, SelectedColor,
					String.Format("Wyslanie surowcow do miasta {0}", m_townToSend.ToString()));
				int yLab = 80;
				foreach (TownResource t in TownDatabase.GetTown(m_town).Resources.Resources)
				{
					AddLabel(10, yLab, SelectedColor, t.ResourceType.ToString());
					if (t.ResourceType == TownResourceType.Zloto)
					{
						AddButton(150, yLab, 100 + (int)t.ResourceType, "5000");
						AddButton(230, yLab, 200 + (int)t.ResourceType, "10000");
					}
					else
					{
						AddButton(150, yLab, 100 + (int)t.ResourceType, "100");
						AddButton(230, yLab, 200 + (int)t.ResourceType, "1000");
					}

					yLab += 20;
				}

				AddButton(50, 360, 300, "Powrot do wyboru miast");
			}
		}

		public override void OnResponse(NetState sender, RelayInfo info)
		{
			Mobile from = sender.Mobile;

			int val = info.ButtonID;

			TownResourceType res = TownResourceType.Invalid;
			int amount = 0;

			if (val >= 200)
			{
				res = (TownResourceType)(val - 200);
				if (res == TownResourceType.Zloto)
				{
					amount = 10000;
				}
				else
				{
					amount = 1000;
				}
			}
			else if (val >= 100)
			{
				res = (TownResourceType)(val - 100);
				if (res == TownResourceType.Zloto)
				{
					amount = 5000;
				}
				else
				{
					amount = 100;
				}
			}

			if (val <= 0)
				return;

			// Main
			if (val < 100)
			{
				from.SendGump(new TownTownsExchangeGump(m_town, from, false, val - 1));
			}
			// Wysylanie
			else
			{
				// Powrot
				if (val == 300)
				{
					from.SendGump(new TownTownsExchangeGump(m_town, from));
					return;
				}

				TownResources twojeZasoby, zasobyMiastaDocelowego;
				twojeZasoby = TownDatabase.GetTown(m_town).Resources;
				zasobyMiastaDocelowego = TownDatabase.GetTown(m_townToSend).Resources;
				// Sprawdz czy miasto ma pieniadze na zaplate za karawane
				if (twojeZasoby.HasResourceAmount(TownResourceType.Zloto, 1000))
				{
					// Sprawdz czy miasto ma wystarczajaca ilosc surowcow by je wyslac - zloto - sume z zaplata)
					if (res == TownResourceType.Zloto &&
					    !twojeZasoby.HasResourceAmount(TownResourceType.Zloto, 1000 + amount))
					{
						from.SendLocalizedMessage(1063894);
					}
					else
					{
						// Sprawdz czy miasto ma wystarczajaca ilosc surowcow by je wyslac
						if (!twojeZasoby.HasResourceAmount(res, amount))
						{
							from.SendLocalizedMessage(1063895);
						}
						else
						{
							// Sprawdz czy miasto docelowe ma miejsce w skarbcu by przyjac surowiec
							if ((res == TownResourceType.Zloto &&
							     zasobyMiastaDocelowego.IsResourceAmountProper(TownResourceType.Zloto,
								     amount + 1000)) ||
							    zasobyMiastaDocelowego.IsResourceAmountProper(res, amount))
							{
								// Wyslij surowce i pobierz oplate
								twojeZasoby.ResourceDecreaseAmount(TownResourceType.Zloto, 1000);
								twojeZasoby.ResourceDecreaseAmount(res, amount);
								TownDatabase.AddTownLog(m_town, TownLogTypes.SUROWCE_WYSLANO, "", amount, (int)(res),
									(int)m_townToSend);
								TownDatabase.AddTownLog(m_townToSend, TownLogTypes.SUROWCE_OTRZYMANO, "", amount,
									(int)(res), (int)m_town);
								zasobyMiastaDocelowego.ResourceIncreaseAmount(res, amount);
								from.SendLocalizedMessage(1063897, "", 0x454);
							}
							else
							{
								from.SendLocalizedMessage(1063896);
							}
						}
					}
				}
				else
				{
					from.SendLocalizedMessage(1063893);
				}

				from.SendGump(new TownTownsExchangeGump(m_town, from, false, (int)m_townToSend));
			}
		}
	}
}
