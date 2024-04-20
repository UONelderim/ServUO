#region References

using System;
using Nelderim.Towns;
using Server;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Network;

#endregion

namespace Nelderim
{
	public enum CSPages
	{
		General,
		Cele,
		Historia,
		CharacterSheetSave,
		HistoriaPunktow
	}

	public class CharacterSheetGump : Gump
	{
		private readonly PlayerMobile m_FromPlayer;
		private readonly PlayerMobile m_TargetPlayer;
		private const int White = 0xFFFFFF;
		private const int Red = 0xC93A3A;

		private void AddPageButton(int x, int y, int buttonID, string text, CSPages current_page, CSPages target_page)
		{
			bool selected = (current_page == target_page);

			if (!selected)
				AddButton(x, y - 1, 4005, 4007, buttonID, GumpButtonType.Reply, 0);
			AddHtml(x + 35, y, 200, 20, Color(text, selected ? Red : White), false, false);
		}

		private void AddRadioButton(int x, int y, int buttonID, string text, bool selected)
		{
			AddButton(x, y - 1, selected ? 9027 : 9026, selected ? 9027 : 9026, buttonID, GumpButtonType.Reply, 0);
			AddHtml(x + 35, y, 200, 20, Color(text, White), false, false);
		}

		private int GetButtonID(int type, int index, bool gmRequested)
		{
			if (type > 16 || index > 256) throw new ArgumentOutOfRangeException();
			return (gmRequested ? 0x1000 : 0) + (type << 8) + index;
		}

		private bool IsGmRequestedButtonID(int buttonID)
		{
			return (buttonID & 0x1000) > 0;
		}

		private int GetTypeFromButtonID(int buttonID)
		{
			return (buttonID >> 8) & 0xF;
		}

		private int GetIndexFromButtonID(int buttonID)
		{
			return buttonID & 0xFF;
		}

		private string Color(string text, int color)
		{
			return String.Format("<BASEFONT COLOR=#{0:X6}>{1}</BASEFONT>", color, text);
		}

		public CharacterSheetGump(Mobile from, Mobile target, CSPages page, bool gmRequested)
			: base(600, 50)
		{
			from.CloseGump(typeof(CharacterSheetGump));
			Mobile m_From = from;
			Mobile m_Target = target;
			m_TargetPlayer = (PlayerMobile)m_Target;
			m_FromPlayer = (PlayerMobile)m_From;
			CharacterSheetInfo m_CharacterSheetInfo = CharacterSheet.Get(m_TargetPlayer);
			Closable = true;
			Dragable = true;

			AddPage(0);
			AddBackground(0, 65, 330, 600, 5054);
			AddImageTiled(10, 70, 310, 20, 9354);
			AddLabel(13, 70, 200, "Karta postaci");
			AddImage(300, 0, 10410);
			AddImage(300, 305, 10412);
			AddImage(300, 150, 10411);

			AddPageButton(10, 100, GetButtonID(0, 1, gmRequested), "Ogolne", page, CSPages.General);
			AddPageButton(110, 100, GetButtonID(0, 2, gmRequested), "Cele", page, CSPages.Cele);
			AddPageButton(210, 100, GetButtonID(0, 3, gmRequested), "Historia", page, CSPages.Historia);
			AddPageButton(10, 633, GetButtonID(0, 4, gmRequested), "Historia punktow fabularnych", page, CSPages.HistoriaPunktow );

			switch (page)
			{
				case CSPages.General:
				{
					AddImageTiled(10, 120, 190, 75, 9354);
					AddLabel(15, 120, 200, String.Format("Nazwa: {0}", m_Target.Name));
					AddLabel(15, 140, 200,
						String.Format("Miasto: {0}",
							TownDatabase.GetCitizenCurrentCity(m_Target) == Towns.Towns.None
								? "brak"
								: TownDatabase.GetCitizenCurrentCity(m_Target).ToString()));

					AddImageTiled(205, 120, 115, 75, 9354);
					AddLabel(210, 130, 200, "Herb: ");
					if (m_CharacterSheetInfo.Crest == 0)
					{
						AddLabel(250, 130, 200, "BRAK");
					}
					else
					{
						switch (m_CharacterSheetInfo.CrestSize)
						{
							case CrestSize.Small:
								AddImage(250, 130, m_CharacterSheetInfo.Crest);
								break;
							case CrestSize.Medium:
								AddImage(250, 125, m_CharacterSheetInfo.Crest);
								break;
							/* case CrestSizeE.Large:
							     AddImage(250, 125, m_CharacterSheetInfo.Crest);
							     break;*/
						}
					}

					AddImageTiled(10, 220, 310, 85, 9354);
					AddLabel(15, 220, 200, "Czy chcesz uczestniczyc w eventach?");
					AddRadioButton(15, 240, GetButtonID(1, 1, gmRequested), "Tak",
						m_CharacterSheetInfo.AttendenceInEvents);
					AddRadioButton(93, 240, GetButtonID(1, 2, gmRequested), "Nie",
						!m_CharacterSheetInfo.AttendenceInEvents);

					AddLabel(15, 260, 200, "Preferowany czas eventow: ");
					AddRadioButton(15, 280, GetButtonID(1, 3, gmRequested), "30min",
						m_CharacterSheetInfo.EventFrequencyAttendence == EventFrequency.F30M);
					AddRadioButton(93, 280, GetButtonID(1, 4, gmRequested), "60min",
						m_CharacterSheetInfo.EventFrequencyAttendence == EventFrequency.F60M);
					AddRadioButton(170, 280, GetButtonID(1, 5, gmRequested), "90min",
						m_CharacterSheetInfo.EventFrequencyAttendence == EventFrequency.F90M);
					AddRadioButton(242, 280, GetButtonID(1, 6, gmRequested), "dluzsze",
						m_CharacterSheetInfo.EventFrequencyAttendence == EventFrequency.FLonger);


					AddImageTiled(10, 355, 310, 90, 9354);
					AddLabel(110, 355, 200, "Punkty fabularne");


					AddImage(135, 385, 51);
					AddLabel(160, 398, 200, String.Format("{0}", m_TargetPlayer.QuestPoints.ToString()));

					AddLabel(5, 325, 200,
						String.Format("Czas przydzielenia punktow {0}", m_TargetPlayer.LastQuestPointsTime));

					if (m_From.AccessLevel >= AccessLevel.Counselor)
					{
						AddPageButton(15, 380, GetButtonID(2, 1, gmRequested), "-1", page, CSPages.CharacterSheetSave);
						AddPageButton(15, 400, GetButtonID(2, 3, gmRequested), "-3", page, CSPages.CharacterSheetSave);
						AddPageButton(15, 420, GetButtonID(2, 5, gmRequested), "-5", page, CSPages.CharacterSheetSave);

						if (m_From.AccessLevel >= AccessLevel.GameMaster)
						{
							AddPageButton(75, 380, GetButtonID(2, 10, gmRequested), "-10", page,
								CSPages.CharacterSheetSave);
							AddPageButton(75, 400, GetButtonID(2, 15, gmRequested), "-15", page,
								CSPages.CharacterSheetSave);
							AddPageButton(75, 420, GetButtonID(2, 20, gmRequested), "-20", page,
								CSPages.CharacterSheetSave);
						}

						if (CanAssignQuestPoints(m_TargetPlayer))
						{
							AddPageButton(200, 380, GetButtonID(3, 1, gmRequested), "+1", page,
								CSPages.CharacterSheetSave);
							AddPageButton(200, 400, GetButtonID(3, 3, gmRequested), "+3", page,
								CSPages.CharacterSheetSave);
							AddPageButton(200, 420, GetButtonID(3, 5, gmRequested), "+5", page,
								CSPages.CharacterSheetSave);

							if (m_From.AccessLevel >= AccessLevel.GameMaster)
							{
								AddPageButton(260, 380, GetButtonID(3, 10, gmRequested), "+10", page,
									CSPages.CharacterSheetSave);
								AddPageButton(260, 400, GetButtonID(3, 15, gmRequested), "+15", page,
									CSPages.CharacterSheetSave);
								AddPageButton(260, 420, GetButtonID(3, 20, gmRequested), "+20", page,
									CSPages.CharacterSheetSave);
							}
						}
						AddLabel(10, 450, 0x480, "Powod:");
						AddImageTiled( 10, 470, 310, 20, 0xBBC );
						AddTextEntry(10, 470, 3100,20,0,0,"");
					}
					break;
				}
			}
		}

		public override void OnResponse(NetState sender, RelayInfo info)
		{
			int type, index, buttonId = info.ButtonID;
			bool gmRequested;

			gmRequested = IsGmRequestedButtonID(buttonId);
			type = GetTypeFromButtonID(buttonId);
			index = GetIndexFromButtonID(buttonId);

			CharacterSheetInfo m_CharacterSheetInfo = CharacterSheet.Get(m_TargetPlayer);

			switch (type)
			{
				case 0:
				{
					XmlTextEntryBook book;
					object[] args = new object[2];
					switch (index)
					{
						case 1:
						{
							m_FromPlayer.SendGump(new CharacterSheetGump( m_FromPlayer, m_TargetPlayer, CSPages.General, gmRequested));
							return;
						}
						case 2:
						{
							args[0] = CSPages.Cele;
							book = new XmlTextEntryBook(0, "Cele i dazenia", m_TargetPlayer.Name, 20, !gmRequested,
								TextEntryChangedCallback, null);
							book.FillTextEntryBook(m_CharacterSheetInfo.AppearanceAndCharacteristic);
							break;
						}
						case 3:
						{
							args[0] = CSPages.Historia;
							book = new XmlTextEntryBook(0, "Historia i profesja", m_TargetPlayer.Name, 20, !gmRequested,
								TextEntryChangedCallback, null);
							book.FillTextEntryBook(m_CharacterSheetInfo.HistoryAndProfession);
							break;
						}
						case 4:
						{
							m_FromPlayer.SendGump(new CharacterSheetGump( m_FromPlayer, m_TargetPlayer, CSPages.General, gmRequested));
							m_FromPlayer.SendGump(new QuestPointsHistoryGump(m_FromPlayer, m_TargetPlayer, 0));
							return;
						}
						default: return;
					}

					args[1] = m_TargetPlayer;
					book.m_args = args;
					book.Visible = false;
					book.Movable = false;
					book.MoveToWorld(
						new Point3D(m_FromPlayer.Location.X, m_FromPlayer.Location.Y, m_FromPlayer.Location.Z - 100),
						m_FromPlayer.Map);
					book.OnDoubleClick(m_FromPlayer);

					m_FromPlayer.SendGump(new CharacterSheetGump(m_FromPlayer, m_TargetPlayer, CSPages.General,
						gmRequested));
					break;
				}
				case 1:
				{
					switch (index)
					{
						case 1:
							m_CharacterSheetInfo.AttendenceInEvents = true;
							break;
						case 2:
							m_CharacterSheetInfo.AttendenceInEvents = false;
							break;
						case 3:
							m_CharacterSheetInfo.EventFrequencyAttendence = EventFrequency.F30M;
							break;
						case 4:
							m_CharacterSheetInfo.EventFrequencyAttendence = EventFrequency.F60M;
							break;
						case 5:
							m_CharacterSheetInfo.EventFrequencyAttendence = EventFrequency.F90M;
							break;
						case 6:
							m_CharacterSheetInfo.EventFrequencyAttendence = EventFrequency.FLonger;
							break;
					}

					m_FromPlayer.SendGump(new CharacterSheetGump(m_FromPlayer, m_TargetPlayer, CSPages.General,
						gmRequested));
					break;
				}
				case 2:
				{
					m_TargetPlayer.QuestPoints -= index;
					m_TargetPlayer.QuestPointsHistory.Add(
						new QuestPointsHistoryEntry(
							DateTime.Now, m_FromPlayer.Account.Username, -index, info.TextEntries[0].Text, m_FromPlayer.Name)
					);
					m_TargetPlayer.SendMessage(0x26,
						"Twoje saldo punktow fabularnych zostalo zmniejszone przez Mistrza Gry");

					m_FromPlayer.SendGump(new CharacterSheetGump(m_FromPlayer, m_TargetPlayer, CSPages.General,
						gmRequested));
					break;
				}
				case 3:
				{
					m_TargetPlayer.QuestPoints += index;
					m_TargetPlayer.QuestPointsHistory.Add(
						new QuestPointsHistoryEntry(
							DateTime.Now, m_FromPlayer.Account.Username, index, info.TextEntries[0].Text, m_FromPlayer.Name));
					m_TargetPlayer.SendMessage(0x3A, "Zostales nagrodzony punktami fabularnymi przez Mistrza Gry");

					m_FromPlayer.SendGump(new CharacterSheetGump(m_FromPlayer, m_TargetPlayer, CSPages.General,
						gmRequested));
					break;
				}
			}
		}

		public void TextEntryChangedCallback(Mobile from, object[] args, string text)
		{
			CSPages page = (CSPages)args[0];
			PlayerMobile targetPlayer = (PlayerMobile)args[1];
			CharacterSheetInfo info = CharacterSheet.Get(targetPlayer);
			switch (page)
			{
				case CSPages.Cele:
					info.AppearanceAndCharacteristic = text;
					break;
				case CSPages.Historia:
					info.HistoryAndProfession = text;
					break;
			}
		}

		public bool CanAssignQuestPoints(PlayerMobile pm)
		{
			return DateTime.Compare(pm.LastQuestPointsTime.AddMinutes(30), DateTime.Now) <= 0;
		}
	}
}
