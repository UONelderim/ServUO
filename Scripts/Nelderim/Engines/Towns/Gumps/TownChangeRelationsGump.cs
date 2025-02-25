using System;
using Nelderim.Towns;
using Server.Network;

namespace Server.Gumps
{
	public class TownChangeRelationsGump : Gump
	{
		private const int LabelColor = 0x7FFF;
		private const int SelectedColor = 0x7A1;
		private readonly Towns m_fromTown;
		private Towns m_toTown;

		public void AddButton(int x, int y, int buttonID, string title)
		{
			AddButton(x, y, 4006, 4007, buttonID, GumpButtonType.Reply, 0);
			AddLabel(x + 40, y, SelectedColor, title);
		}

		public TownChangeRelationsGump(Towns town, Mobile from)
			: base(50, 40)
		{
			m_fromTown = town;
			AddPage(0);

			AddBackground(0, 0, 500, 400, 5054);

			AddHtmlLocalized(10, 10, 250, 250, 1063898, LabelColor, false, false); // Relacje Dyplomatyczne

			AddLabelCropped(10, 30, 300, 40, SelectedColor, "Zmiana relacji kosztuje kazdorazowo 1000 zlota.");
			AddLabel(10, 70, SelectedColor, "Wybierz miasto do zmiany relacji");
			int yLab = 90;
			int tmpRel = 0;
			foreach (Towns t in TownDatabase.GetTownsNames())
			{
				if (t != m_fromTown && t != Towns.None)
				{
					tmpRel = TownDatabase.GetRelationOfATownWithTown(m_fromTown, t);
					AddLabel(10, yLab, SelectedColor,
						String.Format("Relacje z {0} wynosza: {1}. Zmien o:", t.ToString(), tmpRel.ToString()));
					if (tmpRel > -100)
						AddButton(300, yLab, 50 + (int)t, "-10");
					if (tmpRel < 100)
						AddButton(400, yLab, 100 + (int)t, "+10");
					yLab += 20;
				}
			}
		}

		public override void OnResponse(NetState sender, RelayInfo info)
		{
			Mobile from = sender.Mobile;

			int val = info.ButtonID;

			if (val <= 0)
				return;

			m_toTown = (Towns)(val % 50);
			bool positive = val > 99 ? true : false;

			TownResources twojeZasoby;
			twojeZasoby = TownDatabase.GetTown(m_fromTown).Resources;

			// Sprawdz czy miasto jest w stanie zaplacic za zmiane relacji
			if (twojeZasoby.HasResourceAmount(TownResourceType.Zloto, 1000))
			{
				if (positive)
				{
					TownDatabase.AddTownLog(m_fromTown, TownLogTypes.RELACJE_ZMIENIONO, "", (int)m_toTown, 10, 0);
					TownDatabase.ChangeRelationOfATownWithTownByAmount(m_fromTown, m_toTown, 10);
				}
				else
				{
					TownDatabase.AddTownLog(m_fromTown, TownLogTypes.RELACJE_ZMIENIONO, "", (int)m_toTown, -10, 0);
					TownDatabase.ChangeRelationOfATownWithTownByAmount(m_fromTown, m_toTown, -10);
				}

				twojeZasoby.ResourceDecreaseAmount(TownResourceType.Zloto, 1000);
				from.SendLocalizedMessage(1063900, "", 0x454);
			}
			else
			{
				from.SendLocalizedMessage(1063899);
			}

			from.SendGump(new TownChangeRelationsGump(m_fromTown, from));
		}
	}
}
