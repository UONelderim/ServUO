using Server;
using Server.Gumps;
using Server.Network;

namespace Nelderim.Towns
{
	public class TownCrestsGump : Gump
	{
		private const int LabelColor = 0x7FFF;
		private const int SelectedColor = 0x7A1;
		private readonly CrestSize m_CrestSize;
		private readonly Mobile m_from;

		public void AddButton(int x, int y, int buttonID, bool selected, int betweenButtons)
		{
			AddImage(x, y, buttonID);
			AddButton(x + betweenButtons, y, selected ? 9027 : 9026, selected ? 9027 : 9026, buttonID,
				GumpButtonType.Reply, 0);
		}

		public TownCrestsGump(Mobile from, CrestSize CrestSize)
			: base(50, 40)
		{
			m_CrestSize = CrestSize;
			m_from = from;
			from.CloseGump(typeof(TownCrestsGump));

			AddPage(0);
			AddBackground(0, 0, m_CrestSize == CrestSize.Medium ? 110 : 280,
				m_CrestSize == CrestSize.Medium ? 580 : 690, 5054);
			AddHtmlLocalized(10, 10, 250, 250, 1063977, LabelColor, false, false); // Herby

			int iStart = 0;
			int crestAmount = 0;
			int line = 0, rowCounter = 0, ySeparator = 30, xSeparator = 80, betweenButtons = 40;
			int alreadySelectedCrest = CharacterSheet.Get(m_from).Crest;

			switch (m_CrestSize)
			{
				case CrestSize.Small:
					iStart = 30010;
					crestAmount = 48;
					ySeparator = 30;
					betweenButtons = 40;
					break;
				case CrestSize.Medium:
					iStart = 105;
					crestAmount = 9;
					ySeparator = 60;
					betweenButtons = 60;
					break;
				/*case CrestSizeE.Large:
				    iStart = 1001;
				    crestAmount = 18;
				    ySeparator = 70;
				    xSeparator = 120;
				    betweenButtons = 80;
				    break; */
			}

			for (int i = 0; i < crestAmount; i++)
			{
				AddButton(15 + line * xSeparator, 50 + rowCounter * ySeparator, iStart + i,
					alreadySelectedCrest == (iStart + i), betweenButtons);
				rowCounter += 1;
				if (rowCounter > 20 || (m_CrestSize == CrestSize.Large && rowCounter == 9))
				{
					if (m_CrestSize == CrestSize.Large) iStart = 1029 - 9;
					rowCounter = 0;
					line++;
				}
			}
		}

		public override void OnResponse(NetState sender, RelayInfo info)
		{
			Mobile from = sender.Mobile;

			int val = info.ButtonID;

			if (val <= 0)
				return;

			switch (m_CrestSize)
			{
				case CrestSize.Small:
					if (TownDatabase.GetCitinzeship(from).HasDevotion(10000))
					{
						TownDatabase.GetCitinzeship(from).UseDevotion(10000);
						CharacterSheet.Get(m_from).Crest = val;
						CharacterSheet.Get(m_from).CrestSize = m_CrestSize;
					}
					else
					{
						from.SendLocalizedMessage(1063971); // Nie masz wystarczajacej ilosci poswiecenia
					}

					break;
				case CrestSize.Medium:
					if (TownDatabase.GetCitinzeship(from).HasDevotion(15000))
					{
						TownDatabase.GetCitinzeship(from).UseDevotion(15000);
						CharacterSheet.Get(m_from).Crest = val;
						CharacterSheet.Get(m_from).CrestSize = m_CrestSize;
					}
					else
					{
						from.SendLocalizedMessage(1063971); // Nie masz wystarczajacej ilosci poswiecenia
					}

					break;
				case CrestSize.Large:
					if (TownDatabase.GetCitinzeship(from).HasDevotion(20000))
					{
						TownDatabase.GetCitinzeship(from).UseDevotion(20000);
						CharacterSheet.Get(m_from).Crest = val;
						CharacterSheet.Get(m_from).CrestSize = m_CrestSize;
					}
					else
					{
						from.SendLocalizedMessage(1063971); // Nie masz wystarczajacej ilosci poswiecenia
					}

					break;
			}

			from.SendGump(new TownCrestsGump(m_from, m_CrestSize));
		}
	}
}
