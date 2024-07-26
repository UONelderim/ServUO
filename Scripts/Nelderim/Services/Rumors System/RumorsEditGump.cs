// 05.06.26 :: LogoS
// 05.06.26 :: LogoS :: zmiana formatu wprowadzania endrumor and startrumor
// 05.07.01 :: troyan :: zabezpieczenie przed crashem
// 05.08.23 :: troyan :: usuniecie warrninga
// 05.12.13 :: troyan :: przebudowa
// 05.12.19 :: troyan :: podglad

#region References

using System;
using System.Collections.Generic;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;

#endregion

namespace Server.Nelderim
{
	public enum PageName
	{
		None,
		List,
		Edit,
		New,
		Delete,
		Paste
	}

	public class RumorsEditGump : Gump
	{
		private const int LabelColor32 = 0xFFFFFF;
		private const char Separator = '#';

		private readonly NelderimRegion m_Region;
		private readonly PageName m_Page;
		private RumorRecord m_Rumor;
		private readonly List<RumorRecord> m_RumorsList;
		private readonly int m_ListPage;
		private readonly Mobile m_From;

		public string Color(string text)
		{
			return Color(text, LabelColor32);
		}

		public string Color(string text, int color)
		{
			return String.Format("<BASEFONT COLOR=#{0:X6}>{1}</BASEFONT>", color, text);
		}

		public string Center(string text)
		{
			return String.Format("<CENTER>{0}</CENTER>", text);
		}

		public bool RumorExist()
		{
			return (m_Rumor != null);
		}

		public int GetButtonID(int type, int index)
		{
			return 1 + (index * 10) + type;
		}

		public bool WhichRadio(RumorRecord rumor, int typeprio)
		{
			if (rumor == null)
				return typeprio == 1;

			return (int)rumor.Priority == typeprio;
		}

		public bool WhichTypeRadio(RumorRecord rumor, int typeprio)
		{
			if (rumor == null)
				return typeprio == 0;

			return typeprio == (int)rumor.Type;
		}

		public void AddButtonLabeled(int x, int y, int buttonID, string label)
		{
			AddButton(x, y - 1, 4005, 4007, buttonID, GumpButtonType.Reply, 0);
			AddHtml(x + 35, y, 240, 20, Color(label), false, false);
		}

		public RumorsEditGump(Mobile from, NelderimRegion region, PageName page) : this(from, region, "", page,
			null, 0)
		{
		}

		public RumorsEditGump(Mobile from, NelderimRegion region, string info, PageName page) : this(from, region,
			info, page, null, 0)
		{
		}

		public RumorsEditGump(Mobile from, NelderimRegion region, PageName page, int listPage) : this(from, region,
			"", page, null, listPage)
		{
		}

		public RumorsEditGump(Mobile from, NelderimRegion region, PageName page, RumorRecord rumor) : this(from,
			region, "", page, rumor, 0)
		{
		}

		public RumorsEditGump(Mobile from, NelderimRegion region, string info, PageName page, RumorRecord rumor,
			int listPage) : base(30, 30)
		{
			m_Region = region;
			m_From = from;
			m_Page = page;
			m_Rumor = rumor;
			m_ListPage = listPage;

			try
			{
				#region MainPage

				AddPage(0);

				AddBackground(0, 0, 550, 442, 5054);
				AddImageTiled(10, 10, 530, 19, 0xA40);
				AddAlphaRegion(10, 10, 530, 19);

				AddImageTiled(10, 32, 530, 340, 0xA40);
				AddAlphaRegion(10, 32, 530, 340);

				AddImageTiled(10, 375, 530, 53, 0xA40);
				AddAlphaRegion(10, 375, 530, 53);

				if (info != "")
					AddHtml(15, 380, 500, 20, Color(info), false, false);

				string TitleText = "Plotki dla regionu " + RegionsGump.StyleText(region.Name);

				AddHtml(10, 10, 530, 20, Color(Center(TitleText), LabelColor32), false, false);
				// List Button
				AddButton(10, 32, 0xFA5, 0xFA7, GetButtonID(0, 1), GumpButtonType.Reply, 0);
				AddHtml(44, 34, 100, 20, Color("Lista"), false, false); // Lista

				AddButton(85, 32, 0xFA5, 0xFA7, GetButtonID(0, 2), GumpButtonType.Reply, 0);
				AddHtml(119, 34, 100, 20, Color("Nowa"), false, false); // Nowa

				AddButton(315, 32, 0xFA5, 0xFA7, GetButtonID(0, 5), GumpButtonType.Reply, 0);
				AddHtml(350, 34, 100, 20, Color("Dolacz"), false, false); // Dolacz

				if (RumorExist())
				{
					AddButton(160, 32, 0xFA5, 0xFA7, GetButtonID(0, 3), GumpButtonType.Reply, 0);
					AddHtml(194, 34, 100, 20, Color("Edytuj"), false, false); // Edytuj

					AddButton(235, 32, 0xFA5, 0xFA7, GetButtonID(0, 4), GumpButtonType.Reply, 0);
					AddHtml(269, 34, 100, 20, Color("Usun"), false, false); // Usun

					AddButton(395, 32, 0xFA5, 0xFA7, GetButtonID(2, 7), GumpButtonType.Reply, 0);
					AddHtml(429, 34, 100, 20, Color("Kopiuj"), false, false); // Kopiuj

					if (m_Rumor.Type != NewsType.News)
					{
						AddButton(470, 32, 0xFA5, 0xFA7, GetButtonID(2, 8), GumpButtonType.Reply, 0);
						AddHtml(504, 34, 100, 20, Color("Podglad"), false, false); // Podglad
					}
				}

				#endregion

				switch (page)
				{
					#region Lista

					case PageName.List:
					{
						m_RumorsList = RumorsSystem.GetRumors(m_Region, PriorityLevel.VeryLow, NewsType.All);

						if (m_RumorsList.Count <= 0 || m_RumorsList == null)
						{
							AddHtml(10, 80, 530, 20, Center(Color("W regionie brak jakichkolwiek plotek")), false,
								false); //<CENTER>W regionie brak jakichkolwiek plotek</CENTER>
						}
						else
						{
							if (listPage > 0)
								AddButton(500, 60, 0x15E3, 0x15E7, GetButtonID(1, 0), GumpButtonType.Reply, 0);

							if ((listPage + 1) * 12 < m_RumorsList.Count)
								AddButton(520, 60, 0x15E1, 0x15E5, GetButtonID(1, 1), GumpButtonType.Reply, 0);

							//HEAD
							AddHtml(15, 80, 160, 20, Color("Tytul"), false, false); // Tytul
							AddHtml(190, 80, 60, 20, Color("Typ"), false, false); // Typ
							AddHtml(250, 80, 60, 20, Color("Priorytet"), false, false); // Priorytet
							AddHtml(320, 80, 60, 20, Color("Poczatek"), false, false); // Poczatek
							AddHtml(415, 80, 60, 20, Color("Koniec"), false, false); // Koniec

							int y = 100;

							for (int i = 0, index = (listPage * 12);
							     i < 12 && index >= 0 && index < m_RumorsList.Count;
							     ++i, ++index)
							{
								RumorRecord rum = m_RumorsList[index];
								int color = (rum.Expired || !rum.IsValid()) ? 0xFF8866 : LabelColor32;

								AddHtml(15, y + (i * 20), 160, 20, Color(rum.Title, color), false, false);
								AddHtml(190, y + (i * 20), 60, 20, Color(rum.Type.ToString(), color), false, false);
								AddHtml(250, y + (i * 20), 60, 20, Color(rum.Priority.ToString(), color), false, false);
								AddHtml(320, y + (i * 20), 80, 20, Color(rum.StartRumor.ToString(), color), false,
									false);
								AddHtml(415, y + (i * 20), 80, 20, Color(rum.EndRumor.ToString(), color), false, false);
								AddButton(510, y + (i * 20), 0xFA5, 0xFA7, GetButtonID(1, index + 2),
									GumpButtonType.Reply, 0);
							}
						}

						break;
					}

					#endregion

					#region Wklej (lista wszystkich)

					case PageName.Paste:
					{
						m_RumorsList = RumorsSystem.GetRumorsList(region.Name);

						if (m_RumorsList.Count <= 0 || m_RumorsList == null)
						{
							AddHtml(10, 80, 530, 20, Color("W regionie brak jakichkolwiek plotek"), false,
								false); //<CENTER>W regionie brak jakichkolwiek plotek</CENTER>
						}
						else
						{
							if (listPage > 0)
								AddButton(500, 60, 0x15E3, 0x15E7, GetButtonID(3, 0), GumpButtonType.Reply, 0);

							if ((listPage + 1) * 12 < m_RumorsList.Count)
								AddButton(520, 60, 0x15E1, 0x15E5, GetButtonID(3, 1), GumpButtonType.Reply, 0);

							//HEAD
							AddHtml(15, 80, 160, 20, Color("Tytul"), false, false); // Tytul
							AddHtml(190, 80, 60, 20, Color("Typ"), false, false); // Typ
							AddHtml(265, 80, 60, 20, Color("Regiony"), false, false); // Regiony

							int y = 100;

							for (int i = 0, index = (listPage * 12);
							     i < 12 && index >= 0 && index < m_RumorsList.Count;
							     ++i, ++index)
							{
								RumorRecord rum = m_RumorsList[index];
								string regionslist = "";

								for (int j = 0; j < rum.Regions.Count; j++)
								{
									string regname = rum.Regions[j];
									if (j == (rum.Regions.Count - 1))
										regionslist += regname;
									else
										regionslist += regname + ", ";
								}

								int color = (rum.Expired || !rum.IsValid()) ? 0xFF8866 : LabelColor32;

								AddHtml(15, y + (i * 20), 160, 20, Color(rum.Title, color), false, false);
								AddHtml(190, y + (i * 20), 60, 20, Color(rum.Type.ToString(), color), false, false);
								AddHtml(265, y + (i * 20), 230, 20, Color(regionslist, color), false, false);

								AddButton(510, y + (i * 20), 0xFA5, 0xFA7, GetButtonID(3, index + 2),
									GumpButtonType.Reply, 0);
							}
						}

						break;
					}

					#endregion

					#region Nowa / Edycja

					case PageName.New:
					case PageName.Edit:
					{
						int x = 15;
						int y = 80;

						string[] phrases = (rumor != null) ? rumor.Text.Split(Separator) : new[] { "" };

						if (m_ListPage > phrases.Length)
							m_ListPage--;

						string txt = (phrases.Length > m_ListPage) ? phrases[m_ListPage] : "";

						AddHtml(x, y, 60, 20, Color("Tytul"), false, false); // Tytul
						AddHtml(x, y + 25, 60, 20, Color("Zagajenie"), false, false); // Zagajenie
						AddHtml(x, y + 75, 160, 20, Color("Hasla"), false, false); // Hasla
						AddHtml(x, y + 100, 60, 20, Color("Poczatek"), false, false); // Poczatek
						AddHtml(x + 320, y + 100, 160, 20, Color("Koniec"), false, false); // Koniec
						AddHtml(x, y + 125, 160, 20, Color("Tresc"), false, false); // Tresc

						if (listPage > 0)
							AddButton(450, y + 125, 0x15E3, 0x15E7, GetButtonID(2, 0), GumpButtonType.Reply, 0);
						AddHtml(485, y + 125, 20, 20, Center(Color((m_ListPage + 1).ToString(), LabelColor32)), false,
							false); // numer strony
						AddButton(510, y + 125, 0x15E1, 0x15E5, GetButtonID(2, 1), GumpButtonType.Reply, 0);

						x += 65;
						// Tytul
						AddBackground(x - 2, y - 2, 440 + 4, 20 + 4, 0x2486);
						AddTextEntry(x + 2, y + 2, 440 - 4, 20 - 4, 0, 0, rumor != null ? rumor.Title : "");

						y += 25;
						// Zagajenie
						AddBackground(x - 2, y - 2, 440 + 4, 45 + 4, 0x2486);
						AddTextEntry(x + 2, y + 2, 440 - 4, 45 - 4, 0, 1, rumor != null ? rumor.Coppice : "");

						y += 50;
						// Hasla
						AddBackground(x - 2, y - 2, 440 + 4, 20 + 4, 0x2486);
						AddTextEntry(x + 2, y + 2, 440 - 4, 20 - 4, 0, 2, rumor != null ? rumor.KeyWord : "");

						y += 25;
						// Start
						AddBackground(x - 2, y - 2, 140 + 4, 20 + 4, 0x2486);
						AddTextEntry(x + 2, y + 2, 140 - 4, 20 - 4, 0, 3,
							rumor != null ? rumor.StartRumor.ToString() : DateTime.Now.ToString());

						x += 300;
						// Koniec
						AddBackground(x - 2, y - 2, 140 + 4, 20 + 4, 0x2486);
						AddTextEntry(x + 2, y + 2, 140 - 4, 20 - 4, 0, 4,
							rumor != null ? rumor.EndRumor.ToString() : DateTime.Now.ToString());

						x = 25;
						y += 50;

						AddBackground(x - 2, y - 2, 500 + 4, 80 + 4, 0x2486);
						AddTextEntry(x + 2, y + 2, 500 - 4, 80 - 4, 0, 5, txt);

						x = 25;
						y = 320;
						AddHtml(x, y, 60, 20, Color("Priorytet"), false, false); // Priorytet

						x = 100;
						y = 320;
						AddRadio(x, y, 208, 209, WhichRadio(rumor, 1), 1001);
						x = 125;
						y = 320;
						AddHtml(x, y, 60, 20, Color("b. niski"), false, false); // b. niski

						x = 175;
						y = 320;
						AddRadio(x, y, 208, 209, WhichRadio(rumor, 2), 1002);
						x = 200;
						y = 320;
						AddHtml(x, y, 60, 20, Color("niski"), false, false); // niski

						x = 25;
						y = 340;
						AddRadio(x, y, 208, 209, WhichRadio(rumor, 4), 1004);
						x = 50;
						y = 340;
						AddHtml(x, y, 60, 20, Color("sredni"), false, false); // sredni

						x = 100;
						y = 340;
						AddRadio(x, y, 208, 209, WhichRadio(rumor, 8), 1008);
						x = 125;
						y = 340;
						AddHtml(x, y, 60, 20, Color("wysoki"), false, false); // wysoki

						x = 175;
						y = 340;
						AddRadio(x, y, 208, 209, WhichRadio(rumor, 16), 1016);
						x = 200;
						y = 340;
						AddHtml(x, y, 60, 20, Color("b. wysoki"), false, false); // b. wysoki

						x = 260;
						y = 320;
						AddRadio(x, y, 208, 209, WhichTypeRadio(rumor, 0), 2000);
						x = 285;
						y = 320;
						AddHtml(x, y, 60, 20, Color("MOTD"), false, false); // MOTD

						x = 260;
						y = 340;
						AddRadio(x, y, 208, 209, WhichTypeRadio(rumor, 1), 2001);
						x = 285;
						y = 340;
						AddHtml(x, y, 60, 20, Color("Ogloszenie"), false, false); // Ogloszenie

						if (rumor != null) AddButtonLabeled(420, 315, GetButtonID(2, 6), "Anuluj");
						AddButtonLabeled(420, 340, GetButtonID(2, 5), "Zapisz");

						break;
					}

					#endregion
				}
			}
			catch (Exception exc)
			{
				Console.WriteLine(exc.ToString());
			}
		}

		public override void OnResponse(NetState state, RelayInfo info)
		{
			try
			{
				int val = info.ButtonID - 1;

				if (info.ButtonID == 0)
				{
					switch (m_Page)
					{
						case PageName.New:
						case PageName.Paste:
							state.Mobile.SendGump(new RumorsEditGump(m_From, m_Region, "", PageName.List, null, 0));
							break;
						default:
							state.Mobile.SendGump(new RegionsGump(state.Mobile));
							break;
					}

					return;
				}

				if (val < 0)
					return;

				Mobile from = state.Mobile;

				int type = val % 10;
				int index = val / 10;

				switch (type)
				{
					#region Lista

					case 0:
					{
						PageName page = PageName.None;
						bool rumor = false;
						string information = "";

						switch (index)
						{
							case 1:
								page = PageName.List;
								rumor = false;
								break;
							case 2:
								page = PageName.New;
								rumor = false;
								break;
							case 3:
								page = PageName.Edit;
								rumor = true;
								break;
							case 5:
								page = PageName.Paste;
								rumor = false;
								break;
							case 4:
							{
								if (m_Rumor != null)
									RumorsSystem.DeleteRumor(m_Rumor);

								page = PageName.List;
								rumor = false;
								information = "Usunales pozycje"; // Usunales pozycje		
								break;
							}
						}

						if (m_Rumor != null && rumor)
							from.SendGump(new RumorsEditGump(from, m_Region, page, m_Rumor));
						else
							from.SendGump(new RumorsEditGump(from, m_Region, information, page));
						break;
					}

					#endregion

					#region Lista -> Przewin

					case 1:
					{
						switch (index)
						{
							case 0:
							{
								if (m_RumorsList != null && m_ListPage > 0)
									from.SendGump(new RumorsEditGump(from, m_Region, m_Page, m_ListPage - 1));

								break;
							}
							case 1:
							{
								if ((m_ListPage + 1) * 12 < m_RumorsList.Count)
									from.SendGump(new RumorsEditGump(from, m_Region, m_Page, m_ListPage + 1));

								break;
							}
							default:
							{
								index -= 2;
								if (m_RumorsList != null && index >= 0 && index < m_RumorsList.Count)
								{
									RumorRecord record = m_RumorsList[index];
									from.SendGump(new RumorsEditGump(from, m_Region, PageName.Edit, record));
								}

								break;
							}
						}

						break;
					}

					#endregion

					#region Wstaw

					case 3:
					{
						switch (index)
						{
							case 0:
							{
								if (m_RumorsList != null && m_ListPage > 0)
									from.SendGump(new RumorsEditGump(from, m_Region, m_Page, m_ListPage - 1));

								break;
							}
							case 1:
							{
								if ((m_ListPage + 1) * 12 < m_RumorsList.Count)
									from.SendGump(new RumorsEditGump(from, m_Region, m_Page, m_ListPage + 1));

								break;
							}
							default:
							{
								index -= 2;
								if (m_RumorsList != null && index >= 0 && index < m_RumorsList.Count)
								{
									string information = "";

									try
									{
										RumorRecord record = m_RumorsList[index];
										record.AddRegion(m_Region.Name);
										information =
											"Wstawiles plotke do nowego regionu"; // Wstawiles plotke do nowego regionu
									}
									catch (Exception e)
									{
										information =
											"Wystapil wyjatkowy blad skryptu"; // Wystapil wyjatkowy blad skryptu
										Console.WriteLine(e.ToString());
									}

									from.SendGump(new RumorsEditGump(from, m_Region, information, PageName.List));
								}

								break;
							}
						}

						break;
					}

					#endregion

					#region Edytuj / Nowa

					case 2:
					{
						if (index == 7)
						{
							RumorRecord rr = new RumorRecord(m_Rumor);

							rr.EndRumor = DateTime.Now;
							rr.AddRegion("Default");

							RumorsSystem.AddRumor(rr);

							from.SendGump(new RumorsEditGump(from, m_Region,
								"Pozycja zostala skopiowana i dolaczona do regionu glownego",
								PageName.List)); // Pozycja zostala skopiowana i dolaczona do regionu glownego
						}
						else if (index == 6)
						{
							m_Rumor.AddRegionExclude(m_Region.Name);
							from.SendGump(new RumorsEditGump(from, m_Region,
								"Wylaczyles pozycje dla wskazanego regionu",
								PageName.List)); // Wylaczyles pozycje dla wskazanego regionu
							TownCrier.UpdateNews();
						}
						else
						{
							PageName page = PageName.Edit;
							bool newRecord = (m_Rumor == null);
							string[] phrases = (!newRecord) ? m_Rumor.Text.Split(Separator) : new[] { "" };

							if (newRecord)
								m_Rumor = new RumorRecord();

							foreach (int s in info.Switches)
							{
								if (s >= 2000)
								{
									m_Rumor.Type = (NewsType)(s - 2000);
									m_Rumor.Priority = PriorityLevel.VeryLow;
								}
								else
								{
									m_Rumor.Priority = (PriorityLevel)(s - 1000);
									m_Rumor.Type = NewsType.Rumor;
								}
							}

							string infromation = "";

							// Name ( Title )
							TextRelay tr = info.GetTextEntry(0);
							m_Rumor.Title = tr.Text;

							// Coppice
							tr = info.GetTextEntry(1);
							m_Rumor.Coppice = tr.Text;

							// Key Word
							tr = info.GetTextEntry(2);
							m_Rumor.KeyWord = tr.Text;

							// Start Rumor
							tr = info.GetTextEntry(3);
							try
							{
								m_Rumor.StartRumor = DateTime.Parse(tr.Text);
							}
							catch
							{
								m_Rumor.StartRumor = DateTime.Now;
							}

							// End Rumor
							tr = info.GetTextEntry(4);
							try
							{
								m_Rumor.EndRumor = DateTime.Parse(tr.Text);
							}
							catch
							{
								m_Rumor.EndRumor = DateTime.Now;
							}

							// Text
							tr = info.GetTextEntry(5);

							if (newRecord)
								m_Rumor.Text = tr.Text;
							else if (m_ListPage == phrases.Length)
							{
								if (index != 0 && tr.Text != "")
									m_Rumor.Text += "#" + tr.Text;
							}
							else
							{
								if (phrases[m_ListPage] != tr.Text)
								{
									phrases[m_ListPage] = tr.Text;
									m_Rumor.Text = String.Join("#", phrases);
								}
							}

							if (m_Rumor.Regions.Count == 0)
								m_Rumor.Regions.Add(m_Region.Name);

							if (m_Rumor.EndRumor == DateTime.MinValue)
							{
								infromation = "Bledna data zakonczenia"; // Bledna data zakonczenia 
							}
							else if (m_Rumor.StartRumor == DateTime.MaxValue)
							{
								infromation = "Bledna data rozpoczecia"; // Bledna data rozpoczecia 
							}
							else if (m_Rumor.Title.Length == 0)
							{
								infromation = "Brak tytulu"; // Brak tytulu	 
							}
							else if (m_Rumor.Coppice.Length == 0)
							{
								infromation = "Brak zagajenia"; // Brak zagajenia
							}
							else if (m_Rumor.KeyWord.Length == 0)
							{
								infromation = "Brak slow kluczowych"; // Brak slow kluczowych	
							}
							else if (m_Rumor.Text.Length == 0)
							{
								infromation = "Brak tresci"; // Brak tresci				
							}
							else
							{
								if (!newRecord)
									infromation = "Edycja zakonczona sukcesem"; // Edycja zakonczona sukcesem
								else
								{
									infromation = infromation = "Dodales nowa pozycje"; // Dodales nowa pozycje
								}

								page = PageName.List;
							}

							if (newRecord)
								RumorsSystem.AddRumor(m_Rumor);

							switch (index)
							{
								case 0:
									from.SendGump(new RumorsEditGump(from, m_Region, "Pierwsza strona", PageName.Edit,
										m_Rumor, m_ListPage - 1));
									break;
								case 1:
									from.SendGump(new RumorsEditGump(from, m_Region, "Druga strona", PageName.Edit,
										m_Rumor, m_ListPage + 1));
									break;
								case 8:
								{
									from.SendGump(new RumorsEditGump(from, m_Region, infromation, page, m_Rumor, 0));

									//if ( m_Rumor.Type == NewsType.MOTD )
									//	from.SendGump( new MOTDGump( m_Rumor ) );
									//else
									from.SendGump(new SayRumorGump(from, m_Rumor));

									break;
								}
								default:
									from.SendGump(new RumorsEditGump(from, m_Region, infromation, page, m_Rumor, 0));
									TownCrier.UpdateNews();
									break;
							}
						}

						break;
					}

					#endregion
				}
			}
			catch (Exception exc)
			{
				Console.WriteLine(exc.ToString());
			}
		}
	}
}
