using System;
using System.Collections.Generic;
using System.Linq;
using Nelderim;
using Nelderim.Towns;
using Server.Commands;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;

namespace Server.Gumps
{
	public class TownResourcesGump : Gump
	{
		private readonly DuszaMiasta m_Town;
		private readonly TownResourcesGumpPage m_Page;

		private const int LabelColor = 0x7FFF;
		private const int SelectedColor = 0x421F;
		private const int DisabledColor = 0x4210;
		private const int WarningColor = 0x7E10;

		private const int LabelHue = 0x481;
		private const int HasResourceHue = 0x3E;
		private const int HighlightedLabelHue = 0x64;

		public void AddPageButton(int x, int y, int buttonID, int number, TownResourcesGumpPage page)
		{
			AddPageButtonCustomButton(x, y, buttonID, number, page);
		}

		public void AddPageButtonCustomButton(int x, int y, int buttonID, int number, TownResourcesGumpPage page,
			int buttonGumpNormal = 4005, int buttonGumpNormalSelection = 4006, int buttonGumpPressed = 4007,
			int customWidth = 200)
		{
			bool isSelection = (m_Page == page);

			AddButton(x, y, isSelection ? buttonGumpNormalSelection : buttonGumpNormal, buttonGumpPressed, buttonID,
				GumpButtonType.Reply, 0);
			AddHtmlLocalized(x + 45, y, customWidth, 20, number, isSelection ? SelectedColor : LabelColor, false,
				false);
		}

		public void AddButtonLabeled(int x, int y, int buttonID, int number)
		{
			AddButtonLabeled(x, y, buttonID, number, true);
		}

		public void AddButtonLabeled(int x, int y, int buttonID, int number, bool enabled)
		{
			if (enabled)
				AddButton(x, y, 4005, 4007, buttonID, GumpButtonType.Reply, 0);

			AddHtmlLocalized(x + 35, y, 240, 20, number, enabled ? LabelColor : DisabledColor, false, false);
		}

		public void AddButtonWithLabeled(int x, int y, int buttonID, string label, bool bg = true, int width = 150,
			int height = 30)
		{
			AddButton(x, y, 4006, 4007, buttonID, GumpButtonType.Reply, 0);
			AddHtml(x + 40, y, width, height, label, bg, false);
		}

		public void AddList(List<Mobile> list, int button, bool accountOf, bool leadingStar, Mobile from)
		{
			if (list == null)
				return;

			int lastPage = 0;
			int index = 0;

			for (int i = 0; i < list.Count; ++i)
			{
				int offset = index % 10;
				int page = 1 + (index / 10);

				if (page != lastPage)
				{
					if (lastPage != 0)
						AddButton(40, 360, 4005, 4007, 0, GumpButtonType.Page, page);

					AddPage(page);

					if (lastPage != 0)
						AddButton(10, 360, 4014, 4016, 0, GumpButtonType.Page, lastPage);

					lastPage = page;
				}


				Mobile m = list[i];

				string name;
				int labelHue = LabelHue;

				name = m.Name;

				if ((name = name.Trim()).Length <= 0)
					continue;

				if (button != -1)
					AddButton(10, 150 + (offset * 20), 4005, 4007, GetButtonID(button, i), GumpButtonType.Reply, 0);

				if (accountOf && m.Player && m.Account != null)
					name = "Account of " + name;

				if (leadingStar)
					name = "* " + name;

				AddLabel(button > 0 ? 45 : 10, 150 + (offset * 20), labelHue, name);
				++index;
			}
		}

		public int GetButtonID(int type, int index)
		{
			return 1 + (index * 500) + type;
		}

		private bool CheckDistance(DuszaMiasta town, Mobile from)
		{
			if (from.Account.AccessLevel >= AccessLevel.GameMaster)
				return true;
			return from.InRange(town.GetWorldLocation(), 2); // Distance <= 2
		}

		private bool CheckVisibility(DuszaMiasta town, Mobile from)
		{
			return from.InLOS(town);
		}

		private int GetLabelHue(int relations)
		{
			if (relations == 100)
			{
				return 163;
			}

			if (relations >= 70 && relations < 100)
			{
				return 169;
			}

			if (relations >= 40 && relations < 70)
			{
				return 188;
			}

			if (relations >= 10 && relations < 40)
			{
				return 193;
			}

			if (relations == 0)
			{
				return LabelHue;
			}

			if (relations <= -10 && relations > -40)
			{
				return 55;
			}

			if (relations <= -30 && relations > -70)
			{
				return 153;
			}

			if (relations <= -60 && relations > -100)
			{
				return 148;
			}

			if (relations == -100)
			{
				return 138;
			}

			return LabelHue;
		}

		private int GetItemIDForResource(TownResourceType res)
		{
			int id = 0;
			switch (res)
			{
				case TownResourceType.Zloto:
					id = 0x0EEC;
					break;
				case TownResourceType.Deski:
					id = 0x0B61;
					break;
				case TownResourceType.Sztaby:
					id = 0x1BF1;
					break;
				case TownResourceType.Skora:
					id = 0x1E54;
					break;
				case TownResourceType.Material:
					id = 0x0F95;
					break;
				case TownResourceType.Kosci:
					id = 0x1B09;
					break;
				case TownResourceType.Kamienie:
					id = 0x217B;
					break;
				case TownResourceType.Piasek:
					id = 0x1F79;
					break;
				case TownResourceType.Klejnoty:
					id = 0x0F25;
					break;
				case TownResourceType.Ziola:
					id = 0x0C85;
					break;
				case TownResourceType.Zbroje:
					id = 0x1416;
					break;
				case TownResourceType.Bronie:
					id = 0x13B5;
					break;
				case TownResourceType.Invalid:
					break;
			}

			return id;
		}

		public TownResourcesGump(TownResourcesGumpPage page, Mobile from, DuszaMiasta town,
			TownResourcesGumpSubpages subpage = TownResourcesGumpSubpages.None, int subpageindex = 0) : base(50, 40)
		{
			m_Town = town;
			m_Page = page;
			int perPage;
			int minIndex;
			int maxIndex;
			int i;
			int j;
			from.CloseGump(typeof(TownResourcesGump));

			// Check the distance and visibility
			if (!CheckDistance(town, from))
			{
				from.SendAsciiMessage("Jestes za daleko od Duszy Miasta.");
				return;
			}

			if (!CheckVisibility(town, from))
			{
				from.SendAsciiMessage("Cos blokuje dostep do Duszy Miasta.");
				return;
			}

			AddPage(0);

			AddBackground(0, 0, 420, 440, 5054);

			AddImageTiled(10, 10, 400, 100, 2624);
			AddAlphaRegion(10, 10, 400, 100);

			AddImageTiled(10, 120, 400, 260, 3604);

			AddImageTiled(10, 390, 400, 40, 2624);
			AddAlphaRegion(10, 390, 400, 40);


			AddImage(10, 10, 103);

			var lines = Wrap(m_Town.Town.ToString());

			if (lines != null)
			{
				for (int k = 0, y = (114 - (lines.Count * 14)) / 2; k < lines.Count; ++k, y += 14)
				{
					string s = (string)lines[k];

					AddLabel(10 + ((160 - (s.Length * 8)) / 2), y, 0, s);
				}
			}

			AddPageButtonCustomButton(150, 10, GetButtonID(1, 0), 1063736, TownResourcesGumpPage.Information, 4011,
				4012, 4013);
			if (town.Town == Towns.None && from.Account.AccessLevel >= AccessLevel.Administrator)
			{
				AddButtonLabeled(10, 390, GetButtonID(0, 1), 1063794); // Zmien przynaleznosc duszy do miasta
			}

			if (town.Town != Towns.None &&
			    (from.Account.AccessLevel >= AccessLevel.GameMaster ||
			     TownDatabase.IsCitizenOfGivenTown(from, m_Town.Town)))
			{
				if (town.Town != Towns.None)
				{
					AddButtonLabeled(10, 390, GetButtonID(0, 2), 1063951); // Przekaz wiele surowcow
					AddButtonLabeled(190, 390, GetButtonID(0, 3), 1063952); // Przekaz surowce z pojemnika
					AddButtonLabeled(10, 410, GetButtonID(0, 0), 1063742); // Przekaz surowce
					AddPageButtonCustomButton(150, 30, GetButtonID(1, 1), 1063737, TownResourcesGumpPage.Resources,
						4026, 4027, 4028); // Skarbiec
					AddPageButtonCustomButton(150, 50, GetButtonID(1, 2), 1063738, TownResourcesGumpPage.Citizens, 4008,
						4009, 4010); // Obywatele
					AddPageButtonCustomButton(150, 70, GetButtonID(1, 3), 1063739,
						TownResourcesGumpPage.TownDevelopment, 4029, 4030, 4031); // Rozwoj miasta
					AddPageButtonCustomButton(150, 90, GetButtonID(1, 4), 1063740, TownResourcesGumpPage.Maintance,
						4026, 4027, 4028); // Utrzymanie
				}

				AddButtonLabeled(300, 410, 0, 1060675); // Zamknij
			}

			switch (m_Page)
			{
				case TownResourcesGumpPage.Information:
				{
					if (town.Town != Towns.None)
					{
						TownManager townInfo = TownDatabase.GetTown(m_Town.Town);
						AddHtmlLocalized(20, 120, 200, 20, 1063753, LabelColor, false, false); // Owned By: 
						AddLabel(210, 120, LabelHue,
							TownDatabase.GetCitizenNameFromTownWithStatus(m_Town.Town, TownStatus.Leader));

						// Umiescic info o podatkach i ich zmianie
						AddHtmlLocalized(20, 140, 300, 20, 1063937, LabelColor, false,
							false); // Podatki dla mieszkancow tego miasta
						AddLabel(350, 140, LabelHue, String.Format("{0}%", townInfo.TaxesForThisTown));
						AddHtmlLocalized(20, 160, 300, 20, 1063938, LabelColor, false,
							false); // Podatki dla mieszkancow innych miast
						AddLabel(350, 160, LabelHue, String.Format("{0}%", townInfo.TaxesForOtherTowns));
						AddHtmlLocalized(20, 180, 350, 20, 1063939, LabelColor, false,
							false); // Podatki dla nie bedacych obywatelami zadnego miasta
						AddLabel(350, 180, LabelHue, String.Format("{0}%", townInfo.TaxesForNoTown));

						if (from.Account.AccessLevel >= AccessLevel.GameMaster ||
						    TownDatabase.IsCitizenOfGivenTown(from, m_Town.Town))
						{
							AddLabel(20, 200, LabelHue,
								String.Format("Stosunek oficjeli miasta {0} do innych miast:", m_Town.Town.ToString()));
							int yLabRel = 220;
							foreach (TownRelation tr in townInfo.TownRelations)
							{
								AddLabel(20, yLabRel, GetLabelHue(tr.AmountOfRelation), tr.ToString());
								yLabRel += 20;
							}
						}
					}

					break;
				}
				case TownResourcesGumpPage.Resources:
				{
					TownManager mTownManager = TownDatabase.GetTown(m_Town.Town);

					AddHtmlLocalized(10, 120, 240, 20, 1063764, SelectedColor, false, false); // Nazwa surowca

					AddHtmlLocalized(10, 140, 240, 20, 1063754, LabelColor, false, false); // Zloto
					AddHtmlLocalized(10, 160, 240, 20, 1063755, LabelColor, false, false); // Deski
					AddHtmlLocalized(10, 180, 240, 20, 1063756, LabelColor, false, false); // Sztaby
					AddHtmlLocalized(10, 200, 240, 20, 1063757, LabelColor, false, false); // Skora
					AddHtmlLocalized(10, 220, 240, 20, 1063758, LabelColor, false, false); // Bele materialu
					AddHtmlLocalized(10, 240, 240, 20, 1063759, LabelColor, false, false); // Kosci
					AddHtmlLocalized(10, 260, 240, 20, 1063760, LabelColor, false, false); // Kamienie
					AddHtmlLocalized(10, 280, 240, 20, 1063761, LabelColor, false, false); // Piasek
					AddHtmlLocalized(10, 300, 240, 20, 1063762, LabelColor, false, false); // Klejnoty
					AddHtmlLocalized(10, 320, 240, 20, 1063763, LabelColor, false, false); // Ziola
					AddHtmlLocalized(10, 340, 240, 20, 1063768, LabelColor, false, false); // Zbroje
					AddHtmlLocalized(10, 360, 240, 20, 1063769, LabelColor, false, false); // Bronie                    

					AddHtmlLocalized(100, 120, 240, 20, 1063765, SelectedColor, false, false); // Posiadana ilosc
					AddLabel(100, 140, LabelHue,
						mTownManager.Resources.ResourceAmount(TownResourceType.Zloto).ToString()); // Zloto
					AddLabel(100, 160, LabelHue,
						mTownManager.Resources.ResourceAmount(TownResourceType.Deski).ToString()); // Deski
					AddLabel(100, 180, LabelHue,
						mTownManager.Resources.ResourceAmount(TownResourceType.Sztaby).ToString()); // Sztaby
					AddLabel(100, 200, LabelHue,
						mTownManager.Resources.ResourceAmount(TownResourceType.Skora).ToString()); // Skora
					AddLabel(100, 220, LabelHue,
						mTownManager.Resources.ResourceAmount(TownResourceType.Material).ToString()); // Material
					AddLabel(100, 240, LabelHue,
						mTownManager.Resources.ResourceAmount(TownResourceType.Kosci).ToString()); // Kosci
					AddLabel(100, 260, LabelHue,
						mTownManager.Resources.ResourceAmount(TownResourceType.Kamienie).ToString()); // Kamienie
					AddLabel(100, 280, LabelHue,
						mTownManager.Resources.ResourceAmount(TownResourceType.Piasek).ToString()); // Piasek
					AddLabel(100, 300, LabelHue,
						mTownManager.Resources.ResourceAmount(TownResourceType.Klejnoty).ToString()); // Klejnoty
					AddLabel(100, 320, LabelHue,
						mTownManager.Resources.ResourceAmount(TownResourceType.Ziola).ToString()); // Ziola
					AddLabel(100, 340, LabelHue,
						mTownManager.Resources.ResourceAmount(TownResourceType.Zbroje).ToString()); // Zbroje
					AddLabel(100, 360, LabelHue,
						mTownManager.Resources.ResourceAmount(TownResourceType.Bronie).ToString()); // Bronie

					AddHtmlLocalized(200, 120, 240, 20, 1063766, SelectedColor, false, false); // Maksymalna ilosc
					AddLabel(200, 140, LabelHue, "Brak ograniczen"); // Zloto
					AddLabel(200, 160, LabelHue,
						mTownManager.Resources.ResourceMaxAmount(TownResourceType.Deski).ToString()); // Deski
					AddLabel(200, 180, LabelHue,
						mTownManager.Resources.ResourceMaxAmount(TownResourceType.Sztaby).ToString()); // Sztaby
					AddLabel(200, 200, LabelHue,
						mTownManager.Resources.ResourceMaxAmount(TownResourceType.Skora).ToString()); // Skora
					AddLabel(200, 220, LabelHue,
						mTownManager.Resources.ResourceMaxAmount(TownResourceType.Material).ToString()); // Material
					AddLabel(200, 240, LabelHue,
						mTownManager.Resources.ResourceMaxAmount(TownResourceType.Kosci).ToString()); // Kosci
					AddLabel(200, 260, LabelHue,
						mTownManager.Resources.ResourceMaxAmount(TownResourceType.Kamienie).ToString()); // Kamienie
					AddLabel(200, 280, LabelHue,
						mTownManager.Resources.ResourceMaxAmount(TownResourceType.Piasek).ToString()); // Piasek
					AddLabel(200, 300, LabelHue,
						mTownManager.Resources.ResourceMaxAmount(TownResourceType.Klejnoty).ToString()); // Klejnoty
					AddLabel(200, 320, LabelHue,
						mTownManager.Resources.ResourceMaxAmount(TownResourceType.Ziola).ToString()); // Ziola
					AddLabel(200, 340, LabelHue,
						mTownManager.Resources.ResourceMaxAmount(TownResourceType.Zbroje).ToString()); // Zbroje
					AddLabel(200, 360, LabelHue,
						mTownManager.Resources.ResourceMaxAmount(TownResourceType.Bronie).ToString()); // Bronie

					AddHtmlLocalized(300, 120, 240, 20, 1063767, SelectedColor, false, false); // Dzienny rozchod
					AddLabel(300, 140, LabelHue,
						mTownManager.Resources.ResourceDailyChange(TownResourceType.Zloto).ToString()); // Zloto
					AddLabel(300, 160, LabelHue,
						mTownManager.Resources.ResourceDailyChange(TownResourceType.Deski).ToString()); // Deski
					AddLabel(300, 180, LabelHue,
						mTownManager.Resources.ResourceDailyChange(TownResourceType.Sztaby).ToString()); // Sztaby
					AddLabel(300, 200, LabelHue,
						mTownManager.Resources.ResourceDailyChange(TownResourceType.Skora).ToString()); // Skora
					AddLabel(300, 220, LabelHue,
						mTownManager.Resources.ResourceDailyChange(TownResourceType.Material).ToString()); // Material
					AddLabel(300, 240, LabelHue,
						mTownManager.Resources.ResourceDailyChange(TownResourceType.Kosci).ToString()); // Kosci
					AddLabel(300, 260, LabelHue,
						mTownManager.Resources.ResourceDailyChange(TownResourceType.Kamienie).ToString()); // Kamienie
					AddLabel(300, 280, LabelHue,
						mTownManager.Resources.ResourceDailyChange(TownResourceType.Piasek).ToString()); // Piasek
					AddLabel(300, 300, LabelHue,
						mTownManager.Resources.ResourceDailyChange(TownResourceType.Klejnoty).ToString()); // Klejnoty
					AddLabel(300, 320, LabelHue,
						mTownManager.Resources.ResourceDailyChange(TownResourceType.Ziola).ToString()); // Ziola
					AddLabel(300, 340, LabelHue,
						mTownManager.Resources.ResourceDailyChange(TownResourceType.Zbroje).ToString()); // Zbroje
					AddLabel(300, 360, LabelHue,
						mTownManager.Resources.ResourceDailyChange(TownResourceType.Bronie).ToString()); // Bronie

					break;
				}
				case TownResourcesGumpPage.Citizens:
				{
					switch (subpage)
					{
						case TownResourcesGumpSubpages.None:

							AddPageButtonCustomButton(10, 120, GetButtonID(4, 0), 1063776,
								TownResourcesGumpPage.Citizens, 4008, 4008, 4010); // Lista obywateli wg nazwy
							AddPageButtonCustomButton(10, 140, GetButtonID(4, 1), 1063777,
								TownResourcesGumpPage.Citizens, 4008, 4008, 4010); // Lista obywateli wg poswiecenia
							AddPageButtonCustomButton(10, 160, GetButtonID(4, 2), 1063788,
								TownResourcesGumpPage.Citizens, 4008, 4008, 4010); // Lista obywateli wg przystapienia
							if (TownDatabase.GetCurrentTownStatus(from) == TownStatus.Leader)
							{
								AddPageButtonCustomButton(10, 200, GetButtonID(4, 20), 1063870,
									TownResourcesGumpPage.Citizens, 4008, 4008, 4010); // Mianuj glownym kanclerzem
							}

							AddPageButtonCustomButton(10, 180, GetButtonID(4, 3), 1063869,
								TownResourcesGumpPage.Citizens, 4008, 4008, 4010); // Lista kanclerzy
							if (TownDatabase.GetCurrentTownStatus(from) <= TownStatus.Leader ||
							    TownDatabase.GetCurrentTownConselourStatus(from) == TownCounsellor.Prime ||
							    from.Account.AccessLevel >= AccessLevel.GameMaster)
							{
								AddPageButtonCustomButton(10, 220, GetButtonID(4, 21), 1063871,
									TownResourcesGumpPage.Citizens, 4008, 4008, 4010); // Mianuj kanclerzem dyplomacji
								AddPageButtonCustomButton(10, 240, GetButtonID(4, 22), 1063872,
									TownResourcesGumpPage.Citizens, 4008, 4008, 4010); // Mianuj kanclerzem ekonomii
								AddPageButtonCustomButton(10, 260, GetButtonID(4, 23), 1063873,
									TownResourcesGumpPage.Citizens, 4008, 4008, 4010); // Mianuj kanclerzem armii
								AddPageButtonCustomButton(10, 280, GetButtonID(4, 24), 1063874,
									TownResourcesGumpPage.Citizens, 4008, 4008, 4010); // Mianuj kanclerzem budownictwa
							}

							if (from.Account.AccessLevel >= AccessLevel.Seer)
							{
								AddPageButtonCustomButton(10, 320, GetButtonID(4, 8), 1063778,
									TownResourcesGumpPage.Citizens, 4005, 4005); // Nadaj przedstawicielstwo
								AddPageButtonCustomButton(210, 320, GetButtonID(4, 9), 1063779,
									TownResourcesGumpPage.Citizens, 4002, 4002, 4004); // Zabierz przedstawicielstwo
							}

							if (TownDatabase.IsCitizenOfGivenTown(from, m_Town.Town))
							{
								AddPageButtonCustomButton(10, 340, GetButtonID(4, 4), 1063810,
									TownResourcesGumpPage.Citizens, 4011, 4011, 4013); // Moje poswiecenie
								AddPageButtonCustomButton(210, 340, GetButtonID(4, 10), 1063780,
									TownResourcesGumpPage.Citizens, 4002, 4002, 4004); // Porzuc obywatelstwo
							}

							if (from.Account.AccessLevel >= AccessLevel.GameMaster ||
							    TownDatabase.GetCurrentTownStatus(from) == TownStatus.Leader ||
							    TownDatabase.GetCurrentTownConselourStatus(from) == TownCounsellor.Prime ||
							    TownDatabase.GetCurrentTownConselourStatus(from) == TownCounsellor.Diplomacy)
							{
								AddPageButtonCustomButton(10, 360, GetButtonID(4, 11), 1063781,
									TownResourcesGumpPage.Citizens, 4005, 4005); // Nadaj obywatelstwo
								AddPageButtonCustomButton(210, 360, GetButtonID(4, 12), 1063782,
									TownResourcesGumpPage.Citizens, 4002, 4002, 4004); // Zabierz obywatelstwo
							}

							break;
						case TownResourcesGumpSubpages.Citizens:

							AddHtmlLocalized(10, 120, 240, 20, 1063789, LabelColor, false, false); // Nazwa
							AddHtmlLocalized(200, 120, 240, 20, 1063790, LabelColor, false, false); // Ranga
							perPage = 10;
							SortedDictionary<Mobile, string> m_cits = TownDatabase.GetCitizensByName(m_Town.Town);
							// Wyswietlenie listy nazw i rang
							minIndex = subpageindex * perPage;
							maxIndex = m_cits.Count < (subpageindex + 1) * perPage
								? m_cits.Count
								: (subpageindex + 1) * perPage;
							i = 0; // As enumarator
							j = 0; // As for aligning on Y grid
							foreach (var kvp in m_cits)
							{
								if (i >= minIndex && i < maxIndex)
								{
									if (kvp.Key == from)
									{
										AddLabel(10, 140 + j * 20, HasResourceHue, kvp.Key.Name);
									}
									else
									{
										AddLabel(10, 140 + j * 20, LabelHue, kvp.Key.Name);
									}

									AddLabel(200, 140 + j * 20, LabelHue, kvp.Value);
									j++;
								}
								else if (i >= maxIndex)
								{
									break;
								}

								i++;
							}

							// Przyciski
							if (subpageindex != 0 && m_cits.Count > perPage)
							{
								AddPageButtonCustomButton(245, 340, GetButtonID(40, subpageindex - 1), 1063785,
									TownResourcesGumpPage.Citizens, 4014, 4014, 4016); // Poprzednia strona
							}

							if (m_cits.Count > perPage && (subpageindex + 1) * perPage < m_cits.Count)
							{
								AddPageButtonCustomButton(245, 360, GetButtonID(40, subpageindex + 1), 1063784,
									TownResourcesGumpPage.Citizens, 4005, 4005); // Nastepna strona
							}

							AddPageButton(10, 360, GetButtonID(4, 112), 1063783,
								TownResourcesGumpPage.Citizens); // Wroc

							break;
						case TownResourcesGumpSubpages.ToplistCitizens:

							AddLabel(10, 120, LabelHue, ((TownResourceType)subpageindex).ToString());
							AddItem(100, 120, GetItemIDForResource((TownResourceType)subpageindex));

							AddHtmlLocalized(10, 140, 240, 20, 1063789, LabelColor, false, false); // Nazwa
							AddHtmlLocalized(200, 140, 240, 20, 1063792, LabelColor, false, false); // Oddana ilosc
							perPage = 10;
							Dictionary<Mobile, int> m_cits_by_resources =
								TownDatabase.GetCitizensByResource(m_Town.Town, (TownResourceType)subpageindex);
							var items = from pair in m_cits_by_resources
								orderby pair.Value descending
								select pair;
							// Wyswietlenie listy nazw i rang
							minIndex = 0;
							maxIndex = m_cits_by_resources.Count < perPage ? m_cits_by_resources.Count : perPage;
							i = 0; // As enumarator
							foreach (KeyValuePair<Mobile, int> kvp in items)
							{
								if (i >= minIndex && i < maxIndex)
								{
									if (kvp.Key == from)
									{
										AddLabel(10, 160 + i * 20, HasResourceHue, kvp.Key.Name);
									}
									else
									{
										AddLabel(10, 160 + i * 20, LabelHue, kvp.Key.Name);
									}

									AddLabel(200, 160 + i * 20, LabelHue, kvp.Value.ToString());
								}
								else if (i >= maxIndex)
								{
									break;
								}

								i++;
							}

							// Przyciski
							if ((int)TownResourceType.Zloto != subpageindex)
							{
								AddPageButtonCustomButton(245, 340, GetButtonID(41, subpageindex - 1), 1063787,
									TownResourcesGumpPage.Citizens, 4014, 4014, 4016); // Poprzedni surowiec
							}
							else
							{
								AddPageButtonCustomButton(245, 340, GetButtonID(41, (int)TownResourceType.Bronie),
									1063787, TownResourcesGumpPage.Citizens, 4014, 4014, 4016); // Poprzedni surowiec
							}

							if ((int)TownResourceType.Bronie != subpageindex)
							{
								AddPageButtonCustomButton(245, 360, GetButtonID(41, subpageindex + 1), 1063786,
									TownResourcesGumpPage.Citizens, 4005, 4005); // Nastepny surowiec
							}
							else
							{
								AddPageButtonCustomButton(245, 360, GetButtonID(41, (int)TownResourceType.Zloto),
									1063786, TownResourcesGumpPage.Citizens, 4005, 4005); // Nastepny surowiec
							}

							AddPageButton(10, 360, GetButtonID(4, 212), 1063783,
								TownResourcesGumpPage.Citizens); // Wroc

							break;
						case TownResourcesGumpSubpages.OldestCitizens:

							AddHtmlLocalized(10, 120, 240, 20, 1063789, LabelColor, false, false); // Nazwa
							AddHtmlLocalized(200, 120, 240, 20, 1063791, LabelColor, false,
								false); // Data przystapienia
							perPage = 10;
							Dictionary<Mobile, DateTime> m_cits_by_date =
								TownDatabase.GetCitizensByJoinDate(m_Town.Town);
							var dateItems = from pair in m_cits_by_date
								orderby pair.Value
								select pair;
							// Wyswietlenie listy nazw i rang
							minIndex = subpageindex * perPage;
							maxIndex = m_cits_by_date.Count < (subpageindex + 1) * perPage
								? m_cits_by_date.Count
								: (subpageindex + 1) * perPage;
							i = 0; // As enumarator
							j = 0; // As for aligning on Y grid
							foreach (KeyValuePair<Mobile, DateTime> kvp in dateItems)
							{
								if (i >= minIndex && i < maxIndex)
								{
									if (kvp.Key == from)
									{
										AddLabel(10, 140 + j * 20, HasResourceHue, kvp.Key.Name);
									}
									else
									{
										AddLabel(10, 140 + j * 20, LabelHue, kvp.Key.Name);
									}

									AddLabel(200, 140 + j * 20, LabelHue, kvp.Value.ToString());
									j++;
								}
								else if (i >= maxIndex)
								{
									break;
								}

								i++;
							}

							// Przyciski
							if (subpageindex != 0 && m_cits_by_date.Count > perPage)
							{
								AddPageButtonCustomButton(245, 340, GetButtonID(54, subpageindex - 1), 1063785,
									TownResourcesGumpPage.Citizens, 4014, 4014, 4016); // Poprzednia strona
							}

							if (m_cits_by_date.Count > perPage && (subpageindex + 1) * perPage < m_cits_by_date.Count)
							{
								AddPageButtonCustomButton(245, 360, GetButtonID(54, subpageindex + 1), 1063784,
									TownResourcesGumpPage.Citizens, 4005, 4005); // Nastepna strona
							}

							// Przyciski
							AddPageButton(10, 360, GetButtonID(4, 212), 1063783,
								TownResourcesGumpPage.Citizens); // Wroc

							break;
						case TownResourcesGumpSubpages.ConsellourCitizens:

							AddHtmlLocalized(10, 120, 240, 20, 1063789, LabelColor, false, false); // Nazwa
							AddHtmlLocalized(160, 120, 240, 20, 1063790, LabelColor, false, false); // Ranga
							if (TownDatabase.GetCurrentTownStatus(from) <= TownStatus.Leader ||
							    TownDatabase.GetCurrentTownConselourStatus(from) == TownCounsellor.Prime)
							{
								AddHtmlLocalized(300, 120, 240, 20, 1063881, LabelColor, false, false); // Usun tytul
							}

							perPage = 10;
							SortedDictionary<string, string> m_cons =
								TownDatabase.GetCitizensByNameWithStatusAsDict(m_Town.Town, TownStatus.Counsellor);
							// Wyswietlenie listy nazw i rang
							minIndex = subpageindex * perPage;
							maxIndex = m_cons.Count < (subpageindex + 1) * perPage
								? m_cons.Count
								: (subpageindex + 1) * perPage;
							i = 0; // As enumarator
							j = 0; // As for aligning on Y grid
							foreach (var kvp in m_cons)
							{
								if (i >= minIndex && i < maxIndex)
								{
									if (kvp.Key == from.Name)
									{
										AddLabel(10, 140 + j * 20, HasResourceHue, kvp.Key);
									}
									else
									{
										AddLabel(10, 140 + j * 20, LabelHue, kvp.Key);
									}

									AddLabel(160, 140 + j * 20, LabelHue, kvp.Value);
									if (TownDatabase.GetCurrentTownStatus(from) <= TownStatus.Leader ||
									    TownDatabase.GetCurrentTownConselourStatus(from) == TownCounsellor.Prime)
									{
										AddButton(300, 140 + j * 20, 4002, 4004, GetButtonID(49, i),
											GumpButtonType.Reply, 0);
									}

									j++;
								}
								else if (i >= maxIndex)
								{
									break;
								}

								i++;
							}

							// Przyciski
							if (subpageindex != 0 && m_cons.Count > perPage)
							{
								AddPageButtonCustomButton(245, 340, GetButtonID(40, subpageindex - 1), 1063785,
									TownResourcesGumpPage.Citizens, 4014, 4014, 4016); // Poprzednia strona
							}

							if (m_cons.Count > perPage && (subpageindex + 1) * perPage < m_cons.Count)
							{
								AddPageButtonCustomButton(245, 360, GetButtonID(40, subpageindex + 1), 1063784,
									TownResourcesGumpPage.Citizens, 4005, 4005); // Nastepna strona
							}

							AddPageButton(10, 360, GetButtonID(4, 112), 1063783,
								TownResourcesGumpPage.Citizens); // Wroc

							break;
						case TownResourcesGumpSubpages.CitizenDetails:

							TownStatus citStat = TownDatabase.GetCurrentTownStatus(from);
							TownCounsellor citConsStat = TownDatabase.GetCurrentTownConselourStatus(from);

							AddHtmlLocalized(10, 120, 240, 20, 1063812, LabelColor, false, false); // Obywatel
							AddLabel(30, 140, LabelHue, from.Name);
							AddHtmlLocalized(10, 160, 240, 20, 1063791, LabelColor, false, false); // Data przystapienia
							AddLabel(30, 180, LabelHue, TownDatabase.GetJoinDate(from).ToString());
							AddHtmlLocalized(10, 200, 240, 20, 1063790, LabelColor, false, false); // Ranga
							if (citStat == TownStatus.Citizen)
							{
								AddLabel(30, 220, LabelHue, "Obywatel");
								AddImage(110, 210, 5575);
							}
							else if (citStat == TownStatus.Counsellor)
							{
								AddLabel(30, 220, LabelHue, "Kanclerz");
								AddImage(110, 210, 5583);
								if (citConsStat == TownCounsellor.Prime)
								{
									AddLabel(10, 280, LabelHue, "Kanclerz glowny");
									AddImage(140, 275, 30013);
								}
								else if (citConsStat == TownCounsellor.Diplomacy)
								{
									AddLabel(10, 280, LabelHue, "Kanclerz dyplomacji");
									AddImage(140, 275, 30011);
								}
								else if (citConsStat == TownCounsellor.Economy)
								{
									AddLabel(10, 280, LabelHue, "Kanclerz ekonomii");
									AddImage(140, 275, 30041);
								}
								else if (citConsStat == TownCounsellor.Army)
								{
									AddLabel(10, 280, LabelHue, "Kanclerz armii");
									AddImage(140, 275, 30010);
								}
								else if (citConsStat == TownCounsellor.Architecture)
								{
									AddLabel(10, 280, LabelHue, "Kanclerz budownictwa");
									AddImage(140, 275, 30053);
								}
							}
							else if (citStat == TownStatus.Leader)
							{
								AddLabel(30, 220, LabelHue, "Przywodca");
								AddImage(110, 210, 5587);
							}

							AddLabel(10, 310, LabelHue,
								String.Format("Zuzyte poswiecenie - {0}",
									TownDatabase.GetCitinzeship(from).SpentDevotion));
							AddLabel(10, 330, LabelHue,
								String.Format("Dostepne poswiecenie - {0}",
									TownDatabase.GetCitinzeship(from).GetCurrentDevotion()));

							AddHtmlLocalized(200, 120, 240, 20, 1063764, SelectedColor, false, false); // Nazwa surowca
							AddHtmlLocalized(200, 140, 240, 20, 1063754, LabelColor, false, false); // Zloto
							AddHtmlLocalized(200, 160, 240, 20, 1063755, LabelColor, false, false); // Deski
							AddHtmlLocalized(200, 180, 240, 20, 1063756, LabelColor, false, false); // Sztaby
							AddHtmlLocalized(200, 200, 240, 20, 1063757, LabelColor, false, false); // Skora
							AddHtmlLocalized(200, 220, 240, 20, 1063758, LabelColor, false, false); // Bele materialu
							AddHtmlLocalized(200, 240, 240, 20, 1063759, LabelColor, false, false); // Kosci
							AddHtmlLocalized(200, 260, 240, 20, 1063760, LabelColor, false, false); // Kamienie
							AddHtmlLocalized(200, 280, 240, 20, 1063761, LabelColor, false, false); // Piasek
							AddHtmlLocalized(200, 300, 240, 20, 1063762, LabelColor, false, false); // Klejnoty
							AddHtmlLocalized(200, 320, 240, 20, 1063763, LabelColor, false, false); // Ziola
							AddHtmlLocalized(200, 340, 240, 20, 1063768, LabelColor, false, false); // Zbroje
							AddHtmlLocalized(200, 360, 240, 20, 1063769, LabelColor, false, false); // Bronie

							AddHtmlLocalized(300, 120, 240, 20, 1063811, SelectedColor, false,
								false); // Poswiecona ilosc
							AddLabel(300, 140, LabelHue,
								TownDatabase.GetResourceAmountOfCitizen(from, TownResourceType.Zloto)
									.ToString()); // Zloto
							AddLabel(300, 160, LabelHue,
								TownDatabase.GetResourceAmountOfCitizen(from, TownResourceType.Deski)
									.ToString()); // Deski
							AddLabel(300, 180, LabelHue,
								TownDatabase.GetResourceAmountOfCitizen(from, TownResourceType.Sztaby)
									.ToString()); // Sztaby
							AddLabel(300, 200, LabelHue,
								TownDatabase.GetResourceAmountOfCitizen(from, TownResourceType.Skora)
									.ToString()); // Skora
							AddLabel(300, 220, LabelHue,
								TownDatabase.GetResourceAmountOfCitizen(from, TownResourceType.Material)
									.ToString()); // Material
							AddLabel(300, 240, LabelHue,
								TownDatabase.GetResourceAmountOfCitizen(from, TownResourceType.Kosci)
									.ToString()); // Kosci
							AddLabel(300, 260, LabelHue,
								TownDatabase.GetResourceAmountOfCitizen(from, TownResourceType.Kamienie)
									.ToString()); // Kamienie
							AddLabel(300, 280, LabelHue,
								TownDatabase.GetResourceAmountOfCitizen(from, TownResourceType.Piasek)
									.ToString()); // Piasek
							AddLabel(300, 300, LabelHue,
								TownDatabase.GetResourceAmountOfCitizen(from, TownResourceType.Klejnoty)
									.ToString()); // Klejnoty
							AddLabel(300, 320, LabelHue,
								TownDatabase.GetResourceAmountOfCitizen(from, TownResourceType.Ziola)
									.ToString()); // Ziola
							AddLabel(300, 340, LabelHue,
								TownDatabase.GetResourceAmountOfCitizen(from, TownResourceType.Zbroje)
									.ToString()); // Zbroje
							AddLabel(300, 360, LabelHue,
								TownDatabase.GetResourceAmountOfCitizen(from, TownResourceType.Bronie)
									.ToString()); // Bronie

							AddPageButton(10, 360, GetButtonID(4, 212), 1063783,
								TownResourcesGumpPage.Citizens); // Wroc
							break;
						case TownResourcesGumpSubpages.RemoveCitizen:

							AddHtmlLocalized(10, 120, 240, 20, 1063789, LabelColor, false, false); // Nazwa
							AddHtmlLocalized(200, 120, 240, 20, 1063782, LabelColor, false,
								false); // Zabierz obywatelstwo
							perPage = 10;
							SortedDictionary<Mobile, string> m_cits_to_remove =
								TownDatabase.GetCitizensByName(m_Town.Town);
							// Wyswietlenie listy nazw i rang
							minIndex = subpageindex * perPage;
							maxIndex = m_cits_to_remove.Count < (subpageindex + 1) * perPage
								? m_cits_to_remove.Count
								: (subpageindex + 1) * perPage;
							i = 0; // As enumarator
							j = 0; // As for aligning on Y grid
							foreach (var kvp in m_cits_to_remove)
							{
								if (i >= minIndex && i < maxIndex)
								{
									AddLabel(10, 140 + j * 20, LabelHue, kvp.Key.Name);
									AddButton(200, 140 + j * 20, 4002, 4004, GetButtonID(39, i), GumpButtonType.Reply,
										0);
									j++;
								}
								else if (i >= maxIndex)
								{
									break;
								}

								i++;
							}

							// Przyciski
							if (subpageindex != 0 && m_cits_to_remove.Count > perPage)
							{
								AddPageButtonCustomButton(245, 340, GetButtonID(40, subpageindex - 1), 1063785,
									TownResourcesGumpPage.Citizens, 4014, 4014, 4016); // Poprzednia strona
							}

							if (m_cits_to_remove.Count > perPage &&
							    (subpageindex + 1) * perPage < m_cits_to_remove.Count)
							{
								AddPageButtonCustomButton(245, 360, GetButtonID(40, subpageindex + 1), 1063784,
									TownResourcesGumpPage.Citizens, 4005, 4005); // Nastepna strona
							}

							AddPageButton(10, 360, GetButtonID(4, 112), 1063783,
								TownResourcesGumpPage.Citizens); // Wroc

							break;
					}

					break;
				}
				case TownResourcesGumpPage.TownDevelopment:
				{
					List<TownBuilding> m_buildings = new List<TownBuilding>();
					m_buildings = TownDatabase.GetBuildingsListWithStatus(town.Town, TownBuildingStatus.Dostepny);
					TownBuilding building;
					int ytmp = 130;
					int xtmp = 10;
					int perBuildPage = 14;

					int pageBuild = subpageindex;
					int showed = 0;
					int showedmin = perBuildPage * pageBuild;
					int showedmax = (showedmin + perBuildPage) > m_buildings.Count
						? m_buildings.Count
						: (showedmin + perBuildPage);

					if (m_buildings != null)
					{
						for (int ib = showedmin; ib < showedmax; ib++)
						{
							showed++;
							building = m_buildings[ib];
							AddButtonWithLabeled(xtmp, ytmp, GetButtonID(43, (int)building.BuildingType),
								building.BuildingType.ToString());
							ytmp += 30;
							if (showed == perBuildPage / 2)
							{
								xtmp += 190;
								ytmp = 130;
							}
						}
					}

					// Przyciski
					if (pageBuild != 0 && m_buildings.Count > perBuildPage)
					{
						AddPageButtonCustomButton(10, 360, GetButtonID(42, pageBuild - 1), 1063785,
							TownResourcesGumpPage.TownDevelopment, 4014, 4014, 4016); // Poprzednia strona
					}

					if (m_buildings.Count > perBuildPage && (pageBuild + 1) * perBuildPage < (m_buildings.Count + 1))
					{
						AddPageButtonCustomButton(200, 360, GetButtonID(42, pageBuild + 1), 1063784,
							TownResourcesGumpPage.TownDevelopment, 4005, 4005); // Nastepna strona
					}

					break;
				}
				case TownResourcesGumpPage.BuildingOngoing:
				{
					List<TownBuilding> m_buildings = new List<TownBuilding>();
					m_buildings = TownDatabase.GetBuildingsListWithStatus(town.Town, TownBuildingStatus.Budowanie);
					TownBuilding building;
					int ytmp = 130;
					int xtmp = 10;
					int perBuildPage = 14;

					int pageBuild = subpageindex;
					int showed = 0;
					int showedmin = perBuildPage * pageBuild;
					int showedmax = (showedmin + perBuildPage) > m_buildings.Count
						? m_buildings.Count
						: (showedmin + perBuildPage);

					if (m_buildings != null)
					{
						for (int ib = showedmin; ib < showedmax; ib++)
						{
							showed++;
							building = m_buildings[ib];
							AddButtonWithLabeled(xtmp, ytmp, GetButtonID(43, (int)building.BuildingType),
								building.BuildingType.ToString());
							ytmp += 30;
							if (showed == perBuildPage / 2)
							{
								xtmp += 190;
								ytmp = 130;
							}
						}
					}

					// Przyciski
					if (pageBuild != 0 && m_buildings.Count > perBuildPage)
					{
						AddPageButtonCustomButton(10, 360, GetButtonID(50, pageBuild - 1), 1063785,
							TownResourcesGumpPage.TownDevelopment, 4014, 4014, 4016); // Poprzednia strona
					}

					if (m_buildings.Count > perBuildPage && (pageBuild + 1) * perBuildPage < (m_buildings.Count + 1))
					{
						AddPageButtonCustomButton(200, 360, GetButtonID(50, pageBuild + 1), 1063784,
							TownResourcesGumpPage.TownDevelopment, 4005, 4005); // Nastepna strona
					}

					break;
				}
				case TownResourcesGumpPage.BuildingOnHold:
				{
					List<TownBuilding> m_buildings = new List<TownBuilding>();
					m_buildings = TownDatabase.GetBuildingsListWithStatus(town.Town, TownBuildingStatus.Zawieszony);
					TownBuilding building;
					int ytmp = 130;
					int xtmp = 10;
					int perBuildPage = 14;

					int pageBuild = subpageindex;
					int showed = 0;
					int showedmin = perBuildPage * pageBuild;
					int showedmax = (showedmin + perBuildPage) > m_buildings.Count
						? m_buildings.Count
						: (showedmin + perBuildPage);

					if (m_buildings != null)
					{
						for (int ib = showedmin; ib < showedmax; ib++)
						{
							showed++;
							building = m_buildings[ib];
							AddButtonWithLabeled(xtmp, ytmp, GetButtonID(43, (int)building.BuildingType),
								building.BuildingType.ToString());
							ytmp += 30;
							if (showed == perBuildPage / 2)
							{
								xtmp += 190;
								ytmp = 130;
							}
						}
					}

					// Przyciski
					if (pageBuild != 0 && m_buildings.Count > perBuildPage)
					{
						AddPageButtonCustomButton(10, 360, GetButtonID(51, pageBuild - 1), 1063785,
							TownResourcesGumpPage.TownDevelopment, 4014, 4014, 4016); // Poprzednia strona
					}

					if (m_buildings.Count > perBuildPage && (pageBuild + 1) * perBuildPage < (m_buildings.Count + 1))
					{
						AddPageButtonCustomButton(200, 360, GetButtonID(51, pageBuild + 1), 1063784,
							TownResourcesGumpPage.TownDevelopment, 4005, 4005); // Nastepna strona
					}

					break;
				}
				case TownResourcesGumpPage.BuildingWorking:
				{
					List<TownBuilding> m_buildings = new List<TownBuilding>();
					m_buildings = TownDatabase.GetBuildingsListWithStatus(town.Town, TownBuildingStatus.Dziala);
					TownBuilding building;
					int ytmp = 130;
					int xtmp = 10;
					int perBuildPage = 14;

					int pageBuild = subpageindex;
					int showed = 0;
					int showedmin = perBuildPage * pageBuild;
					int showedmax = (showedmin + perBuildPage) > m_buildings.Count
						? m_buildings.Count
						: (showedmin + perBuildPage);

					if (m_buildings != null)
					{
						for (int ib = showedmin; ib < showedmax; ib++)
						{
							showed++;
							building = m_buildings[ib];
							AddButtonWithLabeled(xtmp, ytmp, GetButtonID(43, (int)building.BuildingType),
								building.BuildingType.ToString());
							ytmp += 30;
							if (showed == perBuildPage / 2)
							{
								xtmp += 190;
								ytmp = 130;
							}
						}
					}

					// Przyciski
					if (pageBuild != 0 && m_buildings.Count > perBuildPage)
					{
						AddPageButtonCustomButton(10, 360, GetButtonID(52, pageBuild - 1), 1063785,
							TownResourcesGumpPage.TownDevelopment, 4014, 4014, 4016); // Poprzednia strona
					}

					if (m_buildings.Count > perBuildPage && (pageBuild + 1) * perBuildPage < (m_buildings.Count + 1))
					{
						AddPageButtonCustomButton(200, 360, GetButtonID(52, pageBuild + 1), 1063784,
							TownResourcesGumpPage.TownDevelopment, 4005, 4005); // Nastepna strona
					}

					break;
				}
				case TownResourcesGumpPage.Maintance:
				{
					TownMaintananceGumpSubpages sub = (TownMaintananceGumpSubpages)subpageindex;
					switch (sub)
					{
						case TownMaintananceGumpSubpages.None:

							AddPageButtonCustomButton(10, 120, GetButtonID(6, 0), 1063861,
								TownResourcesGumpPage.Maintance, 4020, 4020, 4022); // Lista budynkow w budowie
							AddPageButtonCustomButton(10, 140, GetButtonID(6, 1), 1063862,
								TownResourcesGumpPage.Maintance, 4017, 4017,
								4018); // Lista budynkow zawieszonych w dzialaniu
							AddPageButtonCustomButton(10, 160, GetButtonID(6, 2), 1063863,
								TownResourcesGumpPage.Maintance, 4023, 4023, 4025); // Lista budynkow dzialajacych
							if (TownDatabase.GetCurrentTownStatus(from) <= TownStatus.Leader ||
							    TownDatabase.GetCurrentTownConselourStatus(from) == TownCounsellor.Prime ||
							    TownDatabase.GetCurrentTownConselourStatus(from) == TownCounsellor.Economy)
							{
								AddPageButtonCustomButton(10, 240, GetButtonID(6, 3), 1063864,
									TownResourcesGumpPage.Maintance, 4026, 4026, 4028); // Podatki
							}

							if (TownDatabase.GetCurrentTownStatus(from) <= TownStatus.Leader ||
							    TownDatabase.GetCurrentTownConselourStatus(from) == TownCounsellor.Prime ||
							    TownDatabase.GetCurrentTownConselourStatus(from) == TownCounsellor.Army)
							{
								AddPageButtonCustomButton(150, 240, GetButtonID(6, 4), 1063865,
									TownResourcesGumpPage.Maintance, 4035, 4035, 4034); // Armia
							}

							if (TownDatabase.GetCurrentTownStatus(from) <= TownStatus.Leader ||
							    TownDatabase.GetCurrentTownConselourStatus(from) == TownCounsellor.Prime ||
							    TownDatabase.GetCurrentTownConselourStatus(from) == TownCounsellor.Economy)
							{
								AddPageButtonCustomButton(10, 280, GetButtonID(6, 5), 1063866,
									TownResourcesGumpPage.Maintance, 4037, 4037, 4036); // Ekonomia
							}

							if (TownDatabase.GetCurrentTownStatus(from) <= TownStatus.Leader ||
							    TownDatabase.GetCurrentTownConselourStatus(from) == TownCounsellor.Prime ||
							    TownDatabase.GetCurrentTownConselourStatus(from) == TownCounsellor.Diplomacy)
							{
								AddPageButtonCustomButton(150, 280, GetButtonID(6, 6), 1063867,
									TownResourcesGumpPage.Maintance, 4032, 4032, 4033); // Dyplomacja 
							}

							AddPageButtonCustomButton(10, 320, GetButtonID(6, 17), 1063943,
								TownResourcesGumpPage.Maintance, 2002, 2002, 2002); // Ostatnie wydarzenia
							if (TownDatabase.GetCurrentTownStatus(from) <= TownStatus.Leader)
							{
								if (TownDatabase.GetTown(m_Town.Town).InformLeader)
									AddPageButtonCustomButton(55, 360, GetButtonID(6, 18), 1063945,
										TownResourcesGumpPage.Maintance, 55, 55, 55); // Ostatnie wydarzenia
								else
									AddPageButtonCustomButton(55, 360, GetButtonID(6, 18), 1063944,
										TownResourcesGumpPage.Maintance, 56, 56, 56); // Ostatnie wydarzenia
							}

							AddPageButtonCustomButton(200, 310, GetButtonID(6, 20), 1063962,
								TownResourcesGumpPage.Maintance, 4020, 4020, 4020); // Wydaj punkty poswiecenia
							if (from.Account.AccessLevel >= AccessLevel.Administrator)
							{
								AddPageButtonCustomButton(200, 330, GetButtonID(6, 19), 1063948,
									TownResourcesGumpPage.Maintance, 4020, 4020, 4020); // Pobierz 30k zlota na NPCa
							}

							break;
						case TownMaintananceGumpSubpages.Podatki:

							if ((TownDatabase.GetCurrentTownStatus(from) <= TownStatus.Leader ||
							     TownDatabase.GetCurrentTownConselourStatus(from) == TownCounsellor.Prime ||
							     TownDatabase.GetCurrentTownConselourStatus(from) == TownCounsellor.Economy))
							{
								if (TownDatabase.GetBuildingStatus(m_Town.Town, TownBuildingName.Ratusz) ==
								    TownBuildingStatus.Dziala)
								{
									TownManager tm = TownDatabase.GetTown(m_Town.Town);
									if (tm.TaxChangeAvailable)
									{
										int thisT, otherT, noT;
										thisT = tm.TaxesForThisTown;
										otherT = tm.TaxesForOtherTowns;
										noT = tm.TaxesForNoTown;

										// Umiescic info o podatkach i ich zmianie
										AddHtmlLocalized(20, 120, 300, 20, 1063937, LabelColor, false,
											false); // Podatki dla mieszkancow tego miasta
										AddButtonWithLabeled(20, 140, GetButtonID(7, 105), "-5%",
											thisT == -5 ? false : true, 50);
										AddButtonWithLabeled(120, 140, GetButtonID(7, 103), "-3%",
											thisT == -3 ? false : true, 50);
										AddButtonWithLabeled(220, 140, GetButtonID(7, 101), "-1%",
											thisT == -1 ? false : true, 50);
										AddButtonWithLabeled(320, 140, GetButtonID(7, 10), "0%",
											thisT == 0 ? false : true, 50);
										AddButtonWithLabeled(20, 170, GetButtonID(7, 55), "5%",
											thisT == 5 ? false : true, 50);
										AddButtonWithLabeled(120, 170, GetButtonID(7, 60), "10%",
											thisT == 10 ? false : true, 50);
										AddButtonWithLabeled(220, 170, GetButtonID(7, 70), "20%",
											thisT == 20 ? false : true, 50);
										AddButtonWithLabeled(320, 170, GetButtonID(7, 80), "30%",
											thisT == 30 ? false : true, 50);

										AddHtmlLocalized(20, 200, 300, 20, 1063938, LabelColor, false,
											false); // Podatki dla mieszkancow innych miast
										AddButtonWithLabeled(20, 220, GetButtonID(8, 105), "-5%",
											otherT == -5 ? false : true, 50);
										AddButtonWithLabeled(120, 220, GetButtonID(8, 103), "-3%",
											otherT == -3 ? false : true, 50);
										AddButtonWithLabeled(220, 220, GetButtonID(8, 101), "-1%",
											otherT == -1 ? false : true, 50);
										AddButtonWithLabeled(320, 220, GetButtonID(8, 10), "0%",
											otherT == 0 ? false : true, 50);
										AddButtonWithLabeled(20, 250, GetButtonID(8, 55), "5%",
											otherT == 5 ? false : true, 50);
										AddButtonWithLabeled(120, 250, GetButtonID(8, 60), "10%",
											otherT == 10 ? false : true, 50);
										AddButtonWithLabeled(220, 250, GetButtonID(8, 70), "20%",
											otherT == 20 ? false : true, 50);
										AddButtonWithLabeled(320, 250, GetButtonID(8, 80), "30%",
											otherT == 30 ? false : true, 50);

										AddHtmlLocalized(20, 280, 350, 20, 1063939, LabelColor, false,
											false); // Podatki dla nie bedacych obywatelami zadnego miasta
										AddButtonWithLabeled(20, 300, GetButtonID(9, 105), "-5%",
											noT == -5 ? false : true, 50);
										AddButtonWithLabeled(120, 300, GetButtonID(9, 103), "-3%",
											noT == -3 ? false : true, 50);
										AddButtonWithLabeled(220, 300, GetButtonID(9, 101), "-1%",
											noT == -1 ? false : true, 50);
										AddButtonWithLabeled(320, 300, GetButtonID(9, 10), "0%",
											noT == 0 ? false : true, 50);
										AddButtonWithLabeled(20, 330, GetButtonID(9, 55), "5%", noT == 5 ? false : true,
											50);
										AddButtonWithLabeled(120, 330, GetButtonID(9, 60), "10%",
											noT == 10 ? false : true, 50);
										AddButtonWithLabeled(220, 330, GetButtonID(9, 70), "20%",
											noT == 20 ? false : true, 50);
										AddButtonWithLabeled(320, 330, GetButtonID(9, 80), "30%",
											noT == 30 ? false : true, 50);
									}
									else
									{
										AddHtmlLocalized(20, 120, 300, 60, 1063942, LabelColor, true,
											false); // Nie mozna dokonac kolejnej zmiany podatkowych w tym okresie rozliczeniowym. Kolejny okres rozpocznie sie po oplacie za budynki.
									}
								}
								else
								{
									AddHtml(10, 120, 335, 25, "Potrzebny ratusz, by wplywac na podatki",
										true, false);
								}
							}

							break;
						case TownMaintananceGumpSubpages.Armia:

							TownManager tmArmy = TownDatabase.GetTown(m_Town.Town);
							AddHtmlLocalized(20, 130, 200, 20, 1063902, LabelColor, false,
								false); // Maksymalna ilosc straznikow
							AddLabel(210, 130, LabelHue, tmArmy.MaxGuards.ToString());
							AddHtmlLocalized(20, 150, 200, 20, 1063903, LabelColor, false,
								false); // Aktywna ilosc straznikow
							AddLabel(210, 150, LabelHue, tmArmy.GetActiveGuards().ToString());
							AddHtmlLocalized(20, 170, 200, 20, 1063904, LabelColor, false,
								false); // Maksymalna ilosc posterunkow
							AddLabel(210, 170, LabelHue, tmArmy.MaxPosts.ToString());
							AddHtmlLocalized(20, 190, 200, 20, 1063905, LabelColor, false,
								false); // Utworzona ilosc posterunkow
							AddLabel(210, 190, LabelHue, tmArmy.GetCreatedPosts().ToString());

							AddLabel(20, 210, LabelHue, "Czestotliwosc wskrzeszania straznikow");
							AddButtonWithLabeled(20, 230, GetButtonID(6, 13), "15min",
								tmArmy.RessurectFrequency == 15 ? false : true, 50);
							AddButtonWithLabeled(120, 230, GetButtonID(6, 14), "30min",
								tmArmy.RessurectFrequency == 30 ? false : true, 50);
							AddButtonWithLabeled(220, 230, GetButtonID(6, 15), "45min",
								tmArmy.RessurectFrequency == 45 ? false : true, 50);
							AddButtonWithLabeled(320, 230, GetButtonID(6, 16), "60min",
								tmArmy.RessurectFrequency == 60 ? false : true, 50);

							AddPageButtonCustomButton(20, 300, GetButtonID(6, 11), 1063906,
								TownResourcesGumpPage.Maintance, 2646, 2647, 2648); // Lista dostepnych straznikow
							AddPageButtonCustomButton(20, 340, GetButtonID(6, 12), 1063907,
								TownResourcesGumpPage.Maintance, 2646, 2647, 2648); // Posterunki

							break;
						case TownMaintananceGumpSubpages.Ekonomia:

							// Handel zagraniczny
							if ((TownDatabase.GetCurrentTownStatus(from) <= TownStatus.Leader ||
							     TownDatabase.GetCurrentTownConselourStatus(from) == TownCounsellor.Prime ||
							     TownDatabase.GetCurrentTownConselourStatus(from) == TownCounsellor.Economy))
							{
								if (TownDatabase.GetBuildingStatus(m_Town.Town, TownBuildingName.Port) ==
								    TownBuildingStatus.Dziala)
								{
									AddPageButtonCustomButton(10, 200, GetButtonID(6, 7), 1063884,
										TownResourcesGumpPage.Maintance, 2646, 2647,
										2648); // Portowy Handel Zagraniczny
								}
								else
								{
									AddHtml(10, 200, 335, 25,
										"Potrzebny port, by prowadzic handel zagraniczny", true, false);
								}
							}

							// Mnoznik cen budynkow
							AddHtml(10, 240, 335, 25,
								String.Format("Mnoznik cen budynkow wynosi {0} %.",
									(TownDatabase.ChargeMultipier() * 100).ToString()), true, false);
							// Pobieranie oplat za budynki
							if (!TownDatabase.ChargeForBuildings())
							{
								AddHtml(10, 120, 335, 25,
									"Budynki nie wymagaja dziennego oplacania za prace.", true, false);
							}
							else
							{
								// Ilosc dni jaki pozwoli na oplacenie wszystkich budynkow, bez koniecznosci zawieszania
								AddHtml(10, 120, 335, 25,
									String.Format("Skarbiec pozwala na oplacenie za {0} cykli.",
										TownAnnouncer.FullyPaidCycles(town.Town)), true, false);
								// Surowiec ktory jako pierwszy sie wyczerpie
								AddHtml(10, 160, 335, 25,
									String.Format("Pierwszy surowiec, ktory sie wyczerpie to {0}.",
										TownAnnouncer.FirstResourceToDrain(town.Town).ToString()), true, false);
							}

							break;
						case TownMaintananceGumpSubpages.Dyplomacja:

							// Wysylka surowcow
							if ((TownDatabase.GetCurrentTownStatus(from) <= TownStatus.Leader ||
							     TownDatabase.GetCurrentTownConselourStatus(from) == TownCounsellor.Prime ||
							     TownDatabase.GetCurrentTownConselourStatus(from) == TownCounsellor.Diplomacy))
							{
								if (TownDatabase.GetBuildingStatus(m_Town.Town, TownBuildingName.Ambasada) ==
								    TownBuildingStatus.Dziala)
								{
									AddPageButtonCustomButton(10, 120, GetButtonID(6, 8), 1063885,
										TownResourcesGumpPage.Maintance, 2646, 2647,
										2648); // Wyslij surowce do innego miasta
								}
								else
								{
									AddHtml(10, 120, 335, 25,
										"Potrzebna ambasada, by wyslac surowce do innego miasta", true,
										false);
								}
							}

							// Zmiana relacji
							if ((TownDatabase.GetCurrentTownStatus(from) <= TownStatus.Leader ||
							     TownDatabase.GetCurrentTownConselourStatus(from) == TownCounsellor.Prime ||
							     TownDatabase.GetCurrentTownConselourStatus(from) == TownCounsellor.Diplomacy))
							{
								if (TownDatabase.GetBuildingStatus(m_Town.Town, TownBuildingName.Torturownia) ==
								    TownBuildingStatus.Dziala)
								{
									AddPageButtonCustomButton(10, 150, GetButtonID(6, 9), 1063886,
										TownResourcesGumpPage.Maintance, 2646, 2647,
										2648); // Zmien relacje z innym miastem
								}
								else
								{
									AddHtml(10, 150, 360, 50,
										"Potrzebna torturownia, by zmieniac relacje z innymi miastami",
										true, false);
								}
							}

							// Wyswietlenie tabeli relacji
							if ((TownDatabase.GetCurrentTownStatus(from) <= TownStatus.Leader ||
							     TownDatabase.GetCurrentTownConselourStatus(from) == TownCounsellor.Prime ||
							     TownDatabase.GetCurrentTownConselourStatus(from) == TownCounsellor.Diplomacy))
							{
								if (TownDatabase.GetBuildingStatus(m_Town.Town, TownBuildingName.Torturownia) ==
								    TownBuildingStatus.Dziala &&
								    TownDatabase.GetBuildingStatus(m_Town.Town, TownBuildingName.Ambasada) ==
								    TownBuildingStatus.Dziala)
								{
									AddPageButtonCustomButton(10, 200, GetButtonID(6, 10), 1063901,
										TownResourcesGumpPage.Maintance, 2646, 2647, 2648); // Informacje wywiadu
								}
								else
								{
									AddHtml(10, 200, 360, 50,
										"Potrzebna ambasada i torturownia, by miec dostep szpiegowskich informacji",
										true, false);
								}
							}

							break;
						case TownMaintananceGumpSubpages.WydawaniePoswiecenia:

							// Szata miastowa
							if (TownDatabase.GetBuildingStatus(m_Town.Town, TownBuildingName.WarsztatKrawiecki) ==
							    TownBuildingStatus.Dziala ||
							    TownDatabase.GetBuildingStatus(m_Town.Town, TownBuildingName.WarsztatKowalski) ==
							    TownBuildingStatus.Dziala)
							{
								AddPageButtonCustomButton(20, 120, GetButtonID(53, 1), 1063963,
									TownResourcesGumpPage.Maintance, 2646, 2647, 2648,
									customWidth: 400); // Szata miastowa - 2000 pkt poswiecenia
							}
							else
							{
								AddHtml(20, 120, 360, 50,
									"Potrzebny warsztat krawiecki lub kowalski, zeby moc otrzymac miastowa szate",
									true, false);
							}

							// Miastowa szata z kapturem
							if ((TownDatabase.GetBuildingStatus(m_Town.Town, TownBuildingName.WarsztatKrawiecki) ==
							     TownBuildingStatus.Dziala ||
							     TownDatabase.GetBuildingStatus(m_Town.Town, TownBuildingName.WarsztatKowalski) ==
							     TownBuildingStatus.Dziala) &&
							    TownDatabase.GetBuildingStatus(m_Town.Town, TownBuildingName.WarsztatMaga) ==
							    TownBuildingStatus.Dziala)
							{
								AddPageButtonCustomButton(20, 160, GetButtonID(53, 2), 1063964,
									TownResourcesGumpPage.Maintance, 2646, 2647, 2648,
									customWidth: 400); // Szata miastowa z kapturem - 10000 pkt poswiecenia
							}
							else
							{
								AddHtml(20, 160, 360, 50,
									"Potrzebny warsztat krawiecki lub kowalski i maga, zeby moc otrzymac miastowa szate z kapturem",
									true, false);
							}

							// Male herby
							if (TownDatabase.GetBuildingStatus(m_Town.Town, TownBuildingName.WarsztatMajstra) ==
							    TownBuildingStatus.Dziala)
							{
								AddPageButtonCustomButton(20, 200, GetButtonID(53, 3), 1063974,
									TownResourcesGumpPage.Maintance, 2646, 2647, 2648,
									customWidth: 400); // Male herby - 10000 pkt poswiecenia
							}
							else
							{
								AddHtml(20, 200, 360, 50,
									"Potrzebny warsztat majstra, zeby moc otrzymac maly herb", true,
									false);
							}

							// Srednie herby
							if (TownDatabase.GetBuildingStatus(m_Town.Town, TownBuildingName.WarsztatMajstra) ==
							    TownBuildingStatus.Dziala &&
							    TownDatabase.GetBuildingStatus(m_Town.Town, TownBuildingName.WarsztatKowalski) ==
							    TownBuildingStatus.Dziala)
							{
								AddPageButtonCustomButton(20, 240, GetButtonID(53, 4), 1063975,
									TownResourcesGumpPage.Maintance, 2646, 2647, 2648,
									customWidth: 400); // Srednie herby - 15000 pkt poswiecenia
							}
							else
							{
								AddHtml(20, 240, 360, 50,
									"Potrzebny warsztat majstra i kowalski, zeby moc otrzymac sredni herb", true,
									false);
							}

							// Duze herby
							if (TownDatabase.GetBuildingStatus(m_Town.Town, TownBuildingName.WarsztatMajstra) ==
							    TownBuildingStatus.Dziala &&
							    TownDatabase.GetBuildingStatus(m_Town.Town, TownBuildingName.WarsztatKowalski) ==
							    TownBuildingStatus.Dziala &&
							    TownDatabase.GetBuildingStatus(m_Town.Town, TownBuildingName.Ratusz) ==
							    TownBuildingStatus.Dziala)
							{
								AddPageButtonCustomButton(20, 280, GetButtonID(53, 5), 1063976,
									TownResourcesGumpPage.Maintance, 2646, 2647, 2648,
									customWidth: 400); // Duze herby - 20000 pkt poswiecenia
							}
							else
							{
								AddHtml(20, 280, 360, 50,
									"Potrzebny warsztat majstra i kowalski i ratusz, zeby moc otrzymac duzy herb",
									true, false);
							}

							break;
					}

					break;
				}
				case TownResourcesGumpPage.Building:
				{
					TownBuilding building = TownDatabase.GetBuilding(town.Town, (TownBuildingName)subpageindex);

					// Nazwa
					AddHtml(10, 120, 135, 25, building.BuildingType.ToString(), true, false);
					// Poziom
					AddHtml(10, 145, 50, 25, "Poziom", true, false);
					AddHtml(90, 145, 20, 25, building.Poziom.ToString(), true, false);
					// Status
					AddHtml(10, 170, 50, 25, "Status", true, false);
					AddHtml(90, 170, 85, 25, building.Status.ToString(), true, false);
					// Opis
					AddHtmlLocalized(10, 195, 165, 75, building.OpisID, true, true);

					// Wyswietlenie wymaganych budynkow
					if (building.Dependecies.Count > 0)
					{
						int yOfDep = 270;
						AddHtml(10, yOfDep, 120, 25, "Wymagane budynki", true, false);
						foreach (TownBuildingName dep in building.Dependecies)
						{
							yOfDep += 25;
							AddHtml(10, yOfDep, 135, 25, dep.ToString(), true, false);
						}
					}

					switch (building.Status)
					{
						case TownBuildingStatus.Niedostepny:
							break;
						case TownBuildingStatus.Dostepny:
							// Guzik do zlecenia budowania
							if (from.Account.AccessLevel >= AccessLevel.Seer ||
							    TownDatabase.GetCurrentTownStatus(from) == TownStatus.Leader ||
							    TownDatabase.GetCurrentTownConselourStatus(from) == TownCounsellor.Prime ||
							    TownDatabase.GetCurrentTownConselourStatus(from) == TownCounsellor.Architecture)
							{
								AddButton(150, 352, 4005, 4007, GetButtonID(44, subpageindex), GumpButtonType.Reply, 0);
								AddHtml(180, 350, 80, 25, "Zlec budowe", true, false);
							}

							// Guzik do zakonczenia budowy
							if (from.Account.AccessLevel >= AccessLevel.Administrator)
							{
								AddButton(260, 352, 4005, 4007, GetButtonID(45, subpageindex), GumpButtonType.Reply, 0);
								AddHtml(290, 350, 120, 25, "Zakonczenie budowy", true, false);
							}

							break;
						case TownBuildingStatus.Budowanie:
							// Guzik do zakonczenia budowy
							if (from.Account.AccessLevel >= AccessLevel.Seer)
							{
								AddButton(260, 352, 4005, 4007, GetButtonID(45, subpageindex), GumpButtonType.Reply, 0);
								AddHtml(290, 350, 120, 25, "Zakonczenie budowy", true, false);
							}

							break;
						case TownBuildingStatus.Dziala:
							// Guzik do zawieszenia budynku
							if (from.Account.AccessLevel >= AccessLevel.Seer ||
							    TownDatabase.GetCurrentTownStatus(from) == TownStatus.Leader ||
							    TownDatabase.GetCurrentTownConselourStatus(from) == TownCounsellor.Prime ||
							    TownDatabase.GetCurrentTownConselourStatus(from) == TownCounsellor.Economy)
							{
								AddButton(150, 352, 4005, 4007, GetButtonID(47, subpageindex), GumpButtonType.Reply, 0);
								AddHtml(180, 350, 80, 25, "Zawies dzialanie", true, false);
							}

							// Guzik do zniszczenia budynku
							if (from.Account.AccessLevel >= AccessLevel.Administrator)
							{
								AddButton(260, 352, 4005, 4007, GetButtonID(46, subpageindex), GumpButtonType.Reply, 0);
								AddHtml(290, 350, 120, 25, "Zniszcz budynek", true, false);
							}

							break;
						case TownBuildingStatus.Zawieszony:
							// Guzik do wznowienia dzialania
							if (from.Account.AccessLevel >= AccessLevel.Seer ||
							    TownDatabase.GetCurrentTownStatus(from) == TownStatus.Leader ||
							    TownDatabase.GetCurrentTownConselourStatus(from) == TownCounsellor.Prime ||
							    TownDatabase.GetCurrentTownConselourStatus(from) == TownCounsellor.Economy)
							{
								AddButton(150, 352, 4005, 4007, GetButtonID(48, subpageindex), GumpButtonType.Reply, 0);
								AddHtml(180, 350, 80, 25, "Wznow dzialanie", true, false);
							}

							// Guzik do zniszczenia budynku
							if (from.Account.AccessLevel >= AccessLevel.Administrator)
							{
								AddButton(260, 352, 4005, 4007, GetButtonID(46, subpageindex), GumpButtonType.Reply, 0);
								AddHtml(290, 350, 120, 25, "Zniszcz budynek", true, false);
							}

							break;
					}


					AddHtmlLocalized(180, 120, 240, 20, 1063856, SelectedColor, false, false); // Nazwa
					AddHtmlLocalized(240, 120, 240, 20, 1063853, SelectedColor, false, false); // Cena
					AddHtmlLocalized(310, 120, 240, 20, 1063854, SelectedColor, false, false); // Max
					AddHtmlLocalized(350, 120, 240, 20, 1063855, SelectedColor, false, false); // Przyrost

					int yk = 140;
					for (int ik = 0; ik < 11; ik++)
					{
						if (building.Resources.HasResource(((TownResourceType)ik)))
						{
							AddLabel(180, yk, LabelHue, ((TownResourceType)ik).ToString());
							int resourceNeededAmount = (int)(building.Resources.ResourceAmount(((TownResourceType)ik)) *
							                                 TownDatabase.ChargeMultipier());
							if (building.Status == TownBuildingStatus.Dostepny &&
							    TownDatabase.GetTown(town.Town).Resources
								    .HasResourceAmount((TownResourceType)ik, resourceNeededAmount))
							{
								AddLabel(240, yk, HasResourceHue, resourceNeededAmount.ToString());
							}
							else
							{
								AddLabel(240, yk, LabelHue, resourceNeededAmount.ToString());
							}

							AddLabel(310, yk, LabelHue,
								building.Resources.ResourceMaxAmount(((TownResourceType)ik)).ToString());
							AddLabel(350, yk, LabelHue,
								building.Resources.ResourceDailyChange(((TownResourceType)ik)).ToString());
							yk += 20;
						}
					}

					break;
				}
			}
		}

		public override void OnResponse(NetState sender, RelayInfo info)
		{
			Mobile from = sender.Mobile;

			int val = info.ButtonID - 1;

			if (val < 0)
				return;

			int type = val % 500;
			int index = val / 500;

			// Check distance
			if (!CheckDistance(m_Town, from))
			{
				from.SendAsciiMessage("Jestes za daleko od Duszy Miasta.");
				return;
			}

			if (!CheckVisibility(m_Town, from))
			{
				from.SendAsciiMessage("Cos blokuje dostep do Duszy Miasta.");
				return;
			}

			switch (type)
			{
				// Buttons not related to pages
				case 0:
				{
					switch (index)
					{
						case 0:
							// Przekaz surowce
							from.BeginTarget(4, false, TargetFlags.None,
								TransferResources_OnTarget);
							break;
						case 1:
							from.SendGump(
								new TownRename(m_Town)); // Pozwala wybrac, ktore miasto jest odpowiedzialne za ta dusze
							break;
						case 2:
							// Przekaz wiele surowcow
							from.BeginTarget(4, false, TargetFlags.None,
								TransferManyResources_OnTarget);
							break;
						case 3:
							// Przekaz surowce z pojemnika
							from.BeginTarget(4, false, TargetFlags.None,
								TransferResourcesInBag_OnTarget);
							break;
						default: return;
					}

					break;
				}
				// Pages selectors
				case 1:
				{
					TownResourcesGumpPage page;

					switch (index)
					{
						case 0:
							page = TownResourcesGumpPage.Information;
							break;
						case 1:
							page = TownResourcesGumpPage.Resources;
							break;
						case 2:
							page = TownResourcesGumpPage.Citizens;
							break;
						case 3:
							page = TownResourcesGumpPage.TownDevelopment;
							break;
						case 4:
							page = TownResourcesGumpPage.Maintance;
							break;
						default: return;
					}

					from.SendGump(new TownResourcesGump(page, from, m_Town));

					break;
				}
				// Information buttons
				case 2:
				{
					break;
				}
				// Resources buttons
				case 3:
				{
					break;
				}
				// Citizens buttons
				case 4:
				{
					TownResourcesGumpSubpages subpage = TownResourcesGumpSubpages.None;
					bool sendGump = true;
					switch (index)
					{
						case 0:
							subpage = TownResourcesGumpSubpages.Citizens;
							break;
						case 1:
							subpage = TownResourcesGumpSubpages.ToplistCitizens;
							break;
						case 2:
							subpage = TownResourcesGumpSubpages.OldestCitizens;
							break;
						case 3:
							subpage = TownResourcesGumpSubpages.ConsellourCitizens;
							break;
						case 4:
							subpage = TownResourcesGumpSubpages.CitizenDetails;
							break;
						case 8: // Nadaj przywodctwo
							from.BeginTarget(15, false, TargetFlags.None, GiveLeadership_OnTarget);
							sendGump = false;
							break;
						case 9: // Zabierz przywodctwo
							Mobile tmpMobile;
							tmpMobile = TownDatabase.CitizenMobileFromTownWithStatus(m_Town.Town, TownStatus.Leader);
							if (tmpMobile != null)
							{
								from.SendGump(new TownPrompt(m_Town.Town, from, 3));
							}
							else
							{
								from.SendLocalizedMessage(1063807); // To miasto nie ma obecnie przedstawiciela
							}

							break;
						case 10: // Porzuc obywatelstwo
							if (TownDatabase.IsCitizenOfAnyTown(from))
							{
								from.SendGump(new TownPrompt(m_Town.Town, from, 1));
								sendGump = false;
							}
							else
							{
								from.SendLocalizedMessage(1063806); // Nie jestes obywatelem zadnego miasta
							}

							break;
						case 11: // Nadaj obywatelstwo
							from.BeginTarget(15, false, TargetFlags.None, GiveCitizenship_OnTarget);
							sendGump = false;
							break;
						case 12:
							subpage = TownResourcesGumpSubpages.RemoveCitizen;
							break;
						case 20: // Mianuj glownym kanclerzem
							from.BeginTarget(15, false, TargetFlags.None,
								MakePrimeConselour_OnTarget);
							sendGump = false;
							break;
						case 21: // Mianuj kanclerzem dyplomacji
							from.BeginTarget(15, false, TargetFlags.None,
								MakeDiplomacyConselour_OnTarget);
							sendGump = false;
							break;
						case 22: // Mianuj kanclerzem ekonomii
							from.BeginTarget(15, false, TargetFlags.None,
								MakeEconomyConselour_OnTarget);
							sendGump = false;
							break;
						case 23: // Mianuj kanclerzem armii
							from.BeginTarget(15, false, TargetFlags.None,
								MakeArmyConselour_OnTarget);
							sendGump = false;
							break;
						case 24: // Mianuj kanclerzem budownictwa
							from.BeginTarget(15, false, TargetFlags.None,
								MakeArchitectureConselour_OnTarget);
							sendGump = false;
							break;
						case 112: goto case 212;
						case 212:
							subpage = TownResourcesGumpSubpages.None;
							break;
						default: return;
					}

					if (sendGump)
						from.SendGump(new TownResourcesGump(TownResourcesGumpPage.Citizens, from, m_Town, subpage));

					break;
				}
				// TownDevelopment buttons                                                         
				case 5:
				{
					from.SendGump(new TownResourcesGump(TownResourcesGumpPage.Citizens, from, m_Town));
					break;
				}
				// Maintance buttons
				case 6:
				{
					TownMaintananceGumpSubpages subpage = TownMaintananceGumpSubpages.None;
					bool send = true;
					switch (index)
					{
						case 0:
							send = false;
							from.SendGump(new TownResourcesGump(TownResourcesGumpPage.BuildingOngoing, from, m_Town));
							break;
						case 1:
							send = false;
							from.SendGump(new TownResourcesGump(TownResourcesGumpPage.BuildingOnHold, from, m_Town));
							break;
						case 2:
							send = false;
							from.SendGump(new TownResourcesGump(TownResourcesGumpPage.BuildingWorking, from, m_Town));
							break;
						case 3:
							subpage = TownMaintananceGumpSubpages.Podatki;
							break;
						case 4:
							subpage = TownMaintananceGumpSubpages.Armia;
							break;
						case 5:
							subpage = TownMaintananceGumpSubpages.Ekonomia;
							break;
						case 6:
							subpage = TownMaintananceGumpSubpages.Dyplomacja;
							break;
						case 7: // Handel zagraniczny
							from.SendGump(new TownForeignExchangeGump(m_Town.Town, from));
							break;
						case 8: // Handel zagraniczny
							from.SendGump(new TownTownsExchangeGump(m_Town.Town, from));
							break;
						case 9: // Zmiana relacji
							from.SendGump(new TownChangeRelationsGump(m_Town.Town, from));
							break;
						case 10: // Tablica relacji
							from.SendGump(new TownRelationsTableGump(m_Town.Town, from));
							break;
						case 11: // Lista dostepnych straznikow
							from.SendGump(new TownGuardsAvailableGump(m_Town.Town, from));
							break;
						case 12: // Posterunki
							from.SendGump(new TownPostsGump(m_Town.Town, from));
							break;
						case 13: // 15 min
							subpage = TownMaintananceGumpSubpages.Armia;
							TownDatabase.GetTown(m_Town.Town).RessurectFrequency = 15;
							break;
						case 14: // 30 min
							subpage = TownMaintananceGumpSubpages.Armia;
							TownDatabase.GetTown(m_Town.Town).RessurectFrequency = 30;
							break;
						case 15: // 45 min
							subpage = TownMaintananceGumpSubpages.Armia;
							TownDatabase.GetTown(m_Town.Town).RessurectFrequency = 45;
							break;
						case 16: // 60 min
							subpage = TownMaintananceGumpSubpages.Armia;
							TownDatabase.GetTown(m_Town.Town).RessurectFrequency = 60;
							break;
						case 17: // Ostatnie wydarzenia
							subpage = TownMaintananceGumpSubpages.None;
							from.SendGump(new TownLogGump(m_Town.Town, from));
							break;
						case 18: // Informuj przedstawiciela o wszystkich wydarzeniach
							subpage = TownMaintananceGumpSubpages.None;
							TownDatabase.GetTown(m_Town.Town).InformLeader =
								!TownDatabase.GetTown(m_Town.Town).InformLeader;
							break;
						case 19: // Pobierz 30k zlota na NPCa
							if (TownDatabase.GetTown(m_Town.Town).Resources
							    .HasResourceAmount(TownResourceType.Zloto, 30000))
							{
								TownDatabase.GetTown(m_Town.Town).Resources
									.ResourceDecreaseAmount(TownResourceType.Zloto, 30000);
								from.SendLocalizedMessage(1063949);
							}
							else
							{
								from.SendLocalizedMessage(1063950);
							}

							break;
						case 20:
							subpage = TownMaintananceGumpSubpages.WydawaniePoswiecenia;
							break;
						default: return;
					}

					if (send)
						from.SendGump(new TownResourcesGump(TownResourcesGumpPage.Maintance, from, m_Town,
							TownResourcesGumpSubpages.None, (int)subpage));

					break;
				}
				case 7:
				{
					TownManager tm = TownDatabase.GetTown(m_Town.Town);
					tm.TaxChangeAvailable = false;
					if (index < 100)
					{
						if (index == 10)
						{
							tm.TaxesForThisTown = 0;
						}
						else
						{
							tm.TaxesForThisTown = index - 50;
						}
					}
					else
					{
						tm.TaxesForThisTown = (-1) * (index - 100);
					}

					TownDatabase.AddTownLog(m_Town.Town, TownLogTypes.PODATKI_ZMIENIONO, "", 0, tm.TaxesForThisTown, 0);
					from.SendGump(new TownResourcesGump(TownResourcesGumpPage.Maintance, from, m_Town,
						TownResourcesGumpSubpages.None, (int)TownMaintananceGumpSubpages.Podatki));

					break;
				}
				case 8:
				{
					TownManager tm = TownDatabase.GetTown(m_Town.Town);
					tm.TaxChangeAvailable = false;
					if (index < 100)
					{
						if (index == 10)
						{
							tm.TaxesForOtherTowns = 0;
						}
						else
						{
							tm.TaxesForOtherTowns = index - 50;
						}
					}
					else
					{
						tm.TaxesForOtherTowns = (-1) * (index - 100);
					}

					TownDatabase.AddTownLog(m_Town.Town, TownLogTypes.PODATKI_ZMIENIONO, "", 1, tm.TaxesForOtherTowns,
						0);
					from.SendGump(new TownResourcesGump(TownResourcesGumpPage.Maintance, from, m_Town,
						TownResourcesGumpSubpages.None, (int)TownMaintananceGumpSubpages.Podatki));

					break;
				}
				case 9:
				{
					TownManager tm = TownDatabase.GetTown(m_Town.Town);
					tm.TaxChangeAvailable = false;
					if (index < 100)
					{
						if (index == 10)
						{
							tm.TaxesForNoTown = 0;
						}
						else
						{
							tm.TaxesForNoTown = index - 50;
						}
					}
					else
					{
						tm.TaxesForNoTown = (-1) * (index - 100);
					}

					TownDatabase.AddTownLog(m_Town.Town, TownLogTypes.PODATKI_ZMIENIONO, "", 2, tm.TaxesForNoTown, 0);
					from.SendGump(new TownResourcesGump(TownResourcesGumpPage.Maintance, from, m_Town,
						TownResourcesGumpSubpages.None, (int)TownMaintananceGumpSubpages.Podatki));

					break;
				}
				case 39: //Remove citizen
					from.SendGump(new TownPrompt(m_Town.Town, from, 4, index));
					break;
				case 40: //Next/previous page of citizens by names
					from.SendGump(new TownResourcesGump(TownResourcesGumpPage.Citizens, from, m_Town,
						TownResourcesGumpSubpages.Citizens, index));
					break;
				case 41: //Next/previous page of citizens by resources
					from.SendGump(new TownResourcesGump(TownResourcesGumpPage.Citizens, from, m_Town,
						TownResourcesGumpSubpages.ToplistCitizens, index));
					break;
				case 42: //Next/previous page of building
					from.SendGump(new TownResourcesGump(TownResourcesGumpPage.TownDevelopment, from, m_Town,
						TownResourcesGumpSubpages.None, index));
					break;
				case 43: //Specific building info
					from.SendGump(new TownResourcesGump(TownResourcesGumpPage.Building, from, m_Town,
						TownResourcesGumpSubpages.None, index));
					break;
				case 44: //Start building

					// Developer i owner maja mozliwosc zlecania budowany bez wymaganych budynkow czy zasobow
					if (from.Account.AccessLevel >= AccessLevel.Developer)
					{
						TownDatabase.ChangeBuildingStatus(m_Town.Town, ((TownBuildingName)index),
							TownBuildingStatus.Budowanie);
						CommandHandlers.BroadcastMessage(AccessLevel.Counselor, 300,
							String.Format("Miasto {0} rozpoczyna budowe budynku {1} bez kosztow - zlecono przez {2}",
								m_Town.Town.ToString(), ((TownBuildingName)index).ToString(), from.Name));
					}
					else
					{
						// Sprawdz wymagane budynki    
						if (TownDatabase.HaveTownRequiredBuildingsToBuildGivenBuilding(m_Town.Town,
							    ((TownBuildingName)index)))
						{
							// Sprawdz skarbiec czy posiada wymagane zasoby
							if (TownDatabase.HaveTownRequiredResourcesToBuildGivenBuilding(m_Town.Town,
								    ((TownBuildingName)index)))
							{
								// Pobierz wymagane surowce ze skarbca
								TownDatabase.UseTownRequiredResources(m_Town.Town, ((TownBuildingName)index));
								// Zmien status budynku na w trakcie budowy
								TownDatabase.ChangeBuildingStatus(m_Town.Town, ((TownBuildingName)index),
									TownBuildingStatus.Budowanie);
								from.SendLocalizedMessage(1063860);
								TownDatabase.AddTownLog(m_Town.Town, TownLogTypes.BUDYNEK_ZLECONO_BUDOWE, "", index, 0,
									0);
								CommandHandlers.BroadcastMessage(AccessLevel.Counselor, 400,
									String.Format("Miasto {0} rozpoczyna budowe budynku {1}", m_Town.Town.ToString(),
										((TownBuildingName)index).ToString(), from.Name));
							}
							else
							{
								// Brak wymaganych zasobow
								from.SendLocalizedMessage(1063859,
									m_Town.Town
										.ToString()); // Miasto ~1_val~ nie posiada wymaganych budynkow, by zlecic budowe.
							}
						}
						else
						{
							// Brak wymaganych budynkow
							from.SendLocalizedMessage(1063858,
								m_Town.Town
									.ToString()); // Miasto ~1_val~ nie posiada wymaganych budynkow, by zlecic budowe.
						}
					}

					from.SendGump(new TownResourcesGump(TownResourcesGumpPage.Building, from, m_Town,
						TownResourcesGumpSubpages.None, index));

					break;
				case 45: //End building

					TownDatabase.ChangeBuildingStatus(m_Town.Town, ((TownBuildingName)index),
						TownBuildingStatus.Dziala);
					from.SendGump(new TownResourcesGump(TownResourcesGumpPage.Building, from, m_Town,
						TownResourcesGumpSubpages.None, index));
					CommandHandlers.BroadcastMessage(AccessLevel.Counselor, 250,
						String.Format("Miasto {0} konczy budowe budynku {1} - przez {2}", m_Town.Town.ToString(),
							((TownBuildingName)index).ToString(), from.Name));
					TownDatabase.AddTownLog(m_Town.Town, TownLogTypes.BUDYNEK_ZAKONCZONO_BUDOWE, "", index, 0, 0);

					break;
				case 46: //Destroy building

					TownDatabase.ChangeBuildingStatus(m_Town.Town, ((TownBuildingName)index),
						TownBuildingStatus.Dostepny);
					from.SendGump(new TownResourcesGump(TownResourcesGumpPage.Building, from, m_Town,
						TownResourcesGumpSubpages.None, index));
					CommandHandlers.BroadcastMessage(AccessLevel.Counselor, 250,
						String.Format("W miescie {0} zniszczono budynek {1} - przez {2}", m_Town.Town.ToString(),
							((TownBuildingName)index).ToString(), from.Name));
					TownDatabase.AddTownLog(m_Town.Town, TownLogTypes.BUDYNEK_ZNISZCZONO, "", index, 0, 0);

					break;
				case 47: //Zawies dzialanie

					TownDatabase.ChangeBuildingStatus(m_Town.Town, ((TownBuildingName)index),
						TownBuildingStatus.Zawieszony);
					from.SendGump(new TownResourcesGump(TownResourcesGumpPage.Building, from, m_Town,
						TownResourcesGumpSubpages.None, index));
					CommandHandlers.BroadcastMessage(AccessLevel.Counselor, 250,
						String.Format("W miescie {0} zawieszono budynek {1} - przez {2}", m_Town.Town.ToString(),
							((TownBuildingName)index).ToString(), from.Name));
					TownDatabase.AddTownLog(m_Town.Town, TownLogTypes.BUDYNEK_ZAWIESZONO_DZIALANIE, "", index, 0, 0);

					break;
				case 48: //Wznow dzialanie

					// Sprawdz skarbiec czy posiada wymagane zasoby
					if (TownDatabase.HaveTownRequiredResourcesOnePercent(m_Town.Town, ((TownBuildingName)index)))
					{
						// Pobierz wymagane surowce ze skarbca
						TownDatabase.UseTownRequiredResourcesOnePercent(m_Town.Town, ((TownBuildingName)index));
						// Zmien status budynku na w trakcie budowy
						TownDatabase.ChangeBuildingStatus(m_Town.Town, ((TownBuildingName)index),
							TownBuildingStatus.Dziala);
						TownDatabase.AddTownLog(m_Town.Town, TownLogTypes.BUDYNEK_WZNOWIONO_DZIALANIE, "", index, 0, 0);
						from.SendGump(new TownResourcesGump(TownResourcesGumpPage.Building, from, m_Town,
							TownResourcesGumpSubpages.None, index));
						CommandHandlers.BroadcastMessage(AccessLevel.Counselor, 250,
							String.Format("W miescie {0} wznowiono dzialanie budynku {1} - przez {2}",
								m_Town.Town.ToString(), ((TownBuildingName)index).ToString(), from.Name));
					}
					else
					{
						// Brak wymaganych zasobow
						from.SendLocalizedMessage(1063868,
							m_Town.Town
								.ToString()); // Miasto ~1_val~ nie posiada wymaganych budynkow, by wznowic dzialanie. Potrzebny jest jeden procent oryginalnej wartosci.
					}

					break;
				case 49: // Usun tytul kanclerza
					from.SendGump(new TownPrompt(m_Town.Town, from, 10, index));
					break;
				case 50: //Next/previous page of BuildingOngoing
					from.SendGump(new TownResourcesGump(TownResourcesGumpPage.BuildingOngoing, from, m_Town,
						TownResourcesGumpSubpages.None, index));
					break;
				case 51: //Next/previous page of BuildingOnHold
					from.SendGump(new TownResourcesGump(TownResourcesGumpPage.BuildingOnHold, from, m_Town,
						TownResourcesGumpSubpages.None, index));
					break;
				case 52: //Next/previous page of BuildingWorking
					from.SendGump(new TownResourcesGump(TownResourcesGumpPage.BuildingWorking, from, m_Town,
						TownResourcesGumpSubpages.None, index));
					break;
				case 53: // Devotion rewards

					switch (index)
					{
						case 1:
							if (TownDatabase.IsCitizenOfAnyTown(from) && TownDatabase.GetCitinzeship(from).HasDevotion(2000))
							{
								Item m_toGive;
								switch (TownDatabase.GetCitizenCurrentCity(from))
								{
									case Towns.None:
										from.SendLocalizedMessage(1063972);
										break;
									case Towns.Orod:
										TownDatabase.GetCitinzeship(from).UseDevotion(2000);
										m_toGive = new MiastowaSzataOrod();
										from.AddToBackpack(m_toGive);
										break;
									case Towns.Garlan:
										TownDatabase.GetCitinzeship(from).UseDevotion(2000);
										m_toGive = new MiastowaSzataGarlan();
										from.AddToBackpack(m_toGive);
										break;
									case Towns.Twierdza:
										TownDatabase.GetCitinzeship(from).UseDevotion(2000);
										m_toGive = new MiastowaSzataTwierdza();
										from.AddToBackpack(m_toGive);
										break;
									case Towns.LDelmah:
										TownDatabase.GetCitinzeship(from).UseDevotion(2000);
										m_toGive = new MiastowaSzataWioskaDrowow();
										from.AddToBackpack(m_toGive);
										break;
									case Towns.Lotharn:
										TownDatabase.GetCitinzeship(from).UseDevotion(2000);
										m_toGive = new MiastowaSzataLotharn();
										from.AddToBackpack(m_toGive);
										break;
									case Towns.Tirassa:
										TownDatabase.GetCitinzeship(from).UseDevotion(2000);
										m_toGive = new MiastowaSzataTirassa();
										from.AddToBackpack(m_toGive);
										break;
									default:
										from.SendLocalizedMessage(1063973);
										break;
								}
							}
							else
							{
								from.SendLocalizedMessage(1063971);
							}

							break;
						case 2:

							if (TownDatabase.GetCitinzeship(from).HasDevotion(10000))
							{
								Item m_toGive;
								switch (TownDatabase.GetCitizenCurrentCity(from))
								{
									case Towns.None:
										from.SendLocalizedMessage(1063972);
										break;
									case Towns.Orod:
										TownDatabase.GetCitinzeship(from).UseDevotion(10000);
										m_toGive = new MiastowaSzataZKapturemOrod();
										from.AddToBackpack(m_toGive);
										break;
									case Towns.Garlan:
										TownDatabase.GetCitinzeship(from).UseDevotion(10000);
										m_toGive = new MiastowaSzataZKapturemGarlan();
										from.AddToBackpack(m_toGive);
										break;
									case Towns.Twierdza:
										TownDatabase.GetCitinzeship(from).UseDevotion(10000);
										m_toGive = new MiastowaSzataZKapturemTwierdza();
										from.AddToBackpack(m_toGive);
										break;
									case Towns.LDelmah:
										TownDatabase.GetCitinzeship(from).UseDevotion(10000);
										m_toGive = new MiastowaSzataZKapturemWioskaDrowow();
										from.AddToBackpack(m_toGive);
										break;
									case Towns.Lotharn:
										TownDatabase.GetCitinzeship(from).UseDevotion(10000);
										m_toGive = new MiastowaSzataZKapturemLotharn();
										from.AddToBackpack(m_toGive);
										break;
									case Towns.Tirassa:
										TownDatabase.GetCitinzeship(from).UseDevotion(10000);
										m_toGive = new MiastowaSzataZKapturemTirassa();
										from.AddToBackpack(m_toGive);
										break;
									default:
										from.SendLocalizedMessage(1063973);
										break;
								}
							}
							else
							{
								from.SendLocalizedMessage(1063971); // Nie masz wystarczajacej ilosci poswiecenia
							}

							break;
						case 3:
							from.SendGump(new TownCrestsGump(from, CrestSize.Small));
							break;
						case 4:
							from.SendGump(new TownCrestsGump(from, CrestSize.Medium));
							break;
						case 5:
							if (TownDatabase.GetCitinzeship(from).HasDevotion(1000))
							{
								Item m_toGive;
								switch (TownDatabase.GetCitizenCurrentCity(from))
								{
									case Towns.None:
										from.SendLocalizedMessage(1063972);
										break;
									case Towns.Orod:
										TownDatabase.GetCitinzeship(from).UseDevotion(1000);
										m_toGive = new PigmentOrod();
										from.AddToBackpack(m_toGive);
										break;
									case Towns.Garlan:
										TownDatabase.GetCitinzeship(from).UseDevotion(1000);
										m_toGive = new PigmentGarlan();
										from.AddToBackpack(m_toGive);
										break;
									 case Towns.Twierdza:
									     TownDatabase.GetCitinzeship(from).UseDevotion(1000);
									     m_toGive = new PigmentTwierdza(); 
									     from.AddToBackpack(m_toGive);
									     break;
									case Towns.LDelmah:
										TownDatabase.GetCitinzeship(from).UseDevotion(1000);
										m_toGive = new PigmentDrow();
										from.AddToBackpack(m_toGive);
										break;
									case Towns.Lotharn:
										TownDatabase.GetCitinzeship(from).UseDevotion(1000);
										m_toGive = new PigmentLotharn();
										from.AddToBackpack(m_toGive);
										break;
									case Towns.Tirassa:
										TownDatabase.GetCitinzeship(from).UseDevotion(1000);
										m_toGive = new PigmentTirassa(); 
										from.AddToBackpack(m_toGive);
										break;
									default:
										from.SendLocalizedMessage(1063973);
										break;
								}
							}
							else
							{
								from.SendLocalizedMessage(1063971);
							}

							break;
					}

					from.SendGump(new TownResourcesGump(TownResourcesGumpPage.Maintance, from, m_Town,
						TownResourcesGumpSubpages.None, (int)TownMaintananceGumpSubpages.WydawaniePoswiecenia));

					break;
				case 54: //Next/previous page of oldest citizens
					from.SendGump(new TownResourcesGump(TownResourcesGumpPage.Citizens, from, m_Town,
						TownResourcesGumpSubpages.OldestCitizens, index));
					break;
				default:
					return;
			}
		}

		private void TransferResources_OnTarget(Mobile from, object obj)
		{
			// Check distance
			if (!CheckDistance(m_Town, from))
			{
				from.SendAsciiMessage("Jestes za daleko od Duszy Miasta.");
				return;
			}

			if (!CheckVisibility(m_Town, from))
			{
				from.SendAsciiMessage("Cos blokuje dostep do Duszy Miasta.");
				return;
			}

			if (!TownDatabase.IsCitizenOfGivenTown(from, m_Town.Town))
			{
				from.SendLocalizedMessage(1063770); // Nie jestes obywatelem tego miasta
				return;
			}

			if (obj is Item)
			{
				Item item = (Item)obj;
				TownManager townToTransfer = TownDatabase.GetTown(m_Town.Town);
				int itemAmount = 1;
				TownResourceType itemResourceType;

				if (item.IsChildOf(from.Backpack))
				{
					if (townToTransfer.Resources.IsResourceAcceptable(item, out itemAmount))
					{
						if (townToTransfer.Resources.IsResourceAmountProper(item))
						{
							// Przekaz surowiecitemAmount
							itemResourceType = townToTransfer.Resources.CheckResourceType(item, out itemAmount);
							townToTransfer.Resources.ResourceIncreaseAmount(itemResourceType, itemAmount);
							// Update bazy graczy
							TownDatabase.IncreaseResourceAmountForCitizen(from, itemResourceType, itemAmount);
							// Usun surowiec
							item.Delete();
							from.SendLocalizedMessage(1063775); // Surowiec zostal przekazany miastu
						}
						else
						{
							from.SendLocalizedMessage(
								1063774); // Skarbiec nie posiada odpowiednio duzo miejsca, by pomiescic wskazany surowiec
						}
					}
					else
					{
						from.SendLocalizedMessage(1063772); // Ten przedmiot nie moze byc przekazanu miastu
					}
				}
				else
				{
					from.SendLocalizedMessage(1063771); // Musisz wskazac na surowiec w twoim plecaku
				}
			}
			else
			{
				from.SendLocalizedMessage(1063773); // Nie mozesz tego przekazac miastu
			}

			from.SendGump(new TownResourcesGump(TownResourcesGumpPage.Resources, from, m_Town));
		}

		private void TransferManyResources_OnTarget(Mobile from, object obj)
		{
			// Check distance
			if (!CheckDistance(m_Town, from))
			{
				from.SendAsciiMessage("Jestes za daleko od Duszy Miasta.");
				return;
			}

			if (!CheckVisibility(m_Town, from))
			{
				from.SendAsciiMessage("Cos blokuje dostep do Duszy Miasta.");
				return;
			}

			if (!TownDatabase.IsCitizenOfGivenTown(from, m_Town.Town))
			{
				from.SendLocalizedMessage(1063770); // Nie jestes obywatelem tego miasta
				return;
			}

			if (obj is Item)
			{
				Item item = (Item)obj;
				TownManager townToTransfer = TownDatabase.GetTown(m_Town.Town);
				int itemAmount = 1;
				TownResourceType itemResourceType;

				if (item.IsChildOf(from.Backpack))
				{
					if (townToTransfer.Resources.IsResourceAcceptable(item, out itemAmount))
					{
						if (townToTransfer.Resources.IsResourceAmountProper(item))
						{
							// Przekaz surowiecitemAmount
							itemResourceType = townToTransfer.Resources.CheckResourceType(item, out itemAmount);
							townToTransfer.Resources.ResourceIncreaseAmount(itemResourceType, itemAmount);
							// Update bazy graczy
							TownDatabase.IncreaseResourceAmountForCitizen(from, itemResourceType, itemAmount);
							// Usun surowiec
							item.Delete();
							from.SendLocalizedMessage(1063775); // Surowiec zostal przekazany miastu
						}
						else
						{
							from.SendLocalizedMessage(
								1063774); // Skarbiec nie posiada odpowiednio duzo miejsca, by pomiescic wskazany surowiec
						}
					}
					else
					{
						from.SendLocalizedMessage(1063772); // Ten przedmiot nie moze byc przekazanu miastu
					}
				}
				else
				{
					from.SendLocalizedMessage(1063771); // Musisz wskazac na surowiec w twoim plecaku
				}
			}
			else
			{
				from.SendLocalizedMessage(1063773); // Nie mozesz tego przekazac miastu
			}

			from.SendLocalizedMessage(
				1063953); // Wybierz kolejny surowiec do przekazania, lub nacisnij ESCAPE zeby przerwac.
			from.BeginTarget(4, false, TargetFlags.None, TransferManyResources_OnTarget);
		}

		private bool CheckItemInBag(Mobile from, TownManager townToTransfer, Item itemFromBag)
		{
			int itemAmount = 1;
			TownResourceType itemResourceType;

			if (townToTransfer.Resources.IsResourceAcceptable(itemFromBag, out itemAmount))
			{
				if (townToTransfer.Resources.IsResourceAmountProper(itemFromBag))
				{
					// Przekaz surowiecitemAmount
					itemResourceType = townToTransfer.Resources.CheckResourceType(itemFromBag, out itemAmount);
					townToTransfer.Resources.ResourceIncreaseAmount(itemResourceType, itemAmount);
					// Update bazy graczy
					TownDatabase.IncreaseResourceAmountForCitizen(from, itemResourceType, itemAmount);
					// Usun surowiec
					itemFromBag.Delete();
					from.SendLocalizedMessage(1063775); // Surowiec zostal przekazany miastu
					return true;
				}

				from.SendLocalizedMessage(
					1063774); // Skarbiec nie posiada odpowiednio duzo miejsca, by pomiescic wskazany surowiec
			}
			else
			{
				from.SendLocalizedMessage(1063772); // Ten przedmiot nie moze byc przekazanu miastu
			}

			return false;
		}

		private void TransferResourcesInBag_OnTarget(Mobile from, object obj)
		{
			// Check distance
			if (!CheckDistance(m_Town, from))
			{
				from.SendAsciiMessage("Jestes za daleko od Duszy Miasta.");
				return;
			}

			if (!CheckVisibility(m_Town, from))
			{
				from.SendAsciiMessage("Cos blokuje dostep do Duszy Miasta.");
				return;
			}

			if (!TownDatabase.IsCitizenOfGivenTown(from, m_Town.Town))
			{
				from.SendLocalizedMessage(1063770); // Nie jestes obywatelem tego miasta
				return;
			}

			if (obj is Item)
			{
				Item item = (Item)obj;
				TownManager townToTransfer = TownDatabase.GetTown(m_Town.Town);

				if (item.IsChildOf(from.Backpack))
				{
					// Sprawdzenie czy wskazany przedmiot jest pojemnikiem
					if (item.GetType().IsSubclassOf(typeof(Container)))
					{
						int selfCheck = 0;
						// Sprawdzenie wszystkich przedmiotow w pojemniku
						while (item.Items.Count != 0)
						{
							selfCheck++;
							if (!CheckItemInBag(from, townToTransfer,
								    item.Items[Utility.RandomMinMax(0, item.Items.Count - 1)])) break;
							if (item.Items.Count == 0 || selfCheck > 100) break;
						}

						from.SendLocalizedMessage(1063955); // Przekazywanie przedmiotow z pojemnika zakonczone
					}
				}
				else
				{
					from.SendLocalizedMessage(1063954); // Musisz wskazac na pojemnik w twoim plecaku
				}
			}
			else
			{
				from.SendLocalizedMessage(1063773); // Nie mozesz tego przekazac miastu
			}

			from.SendGump(new TownResourcesGump(TownResourcesGumpPage.Resources, from, m_Town));
		}

		private void GiveCitizenship_OnTarget(Mobile from, object obj)
		{
			// Check distance
			if (!CheckDistance(m_Town, from))
			{
				from.SendAsciiMessage("Jestes za daleko od Duszy Miasta.");
				return;
			}

			if (!CheckVisibility(m_Town, from))
			{
				from.SendAsciiMessage("Cos blokuje dostep do Duszy Miasta.");
				return;
			}

			if (obj is PlayerMobile)
			{
				Mobile mob = (Mobile)obj;
				if (TownDatabase.IsCitizenOfAnyTown(mob))
				{
					from.SendLocalizedMessage(1063805); // Ta postac jest juz obywatelem jakiegos miasta
				}
				else
				{
					mob.SendGump(new TownPrompt(m_Town.Town, mob, 0));
				}

				from.SendGump(new TownResourcesGump(TownResourcesGumpPage.Citizens, from, m_Town));
			}
			else
			{
				from.SendLocalizedMessage(1063795); // Nie mozna nadac temu obywatelstwa
			}
		}

		private void GiveLeadership_OnTarget(Mobile from, object obj)
		{
			// Check distance
			if (!CheckDistance(m_Town, from))
			{
				from.SendAsciiMessage("Jestes za daleko od Duszy Miasta.");
				return;
			}

			if (!CheckVisibility(m_Town, from))
			{
				from.SendAsciiMessage("Cos blokuje dostep do Duszy Miasta.");
				return;
			}

			if (obj is PlayerMobile)
			{
				Mobile mob = (Mobile)obj;
				if (TownDatabase.IsCitizenOfGivenTown(mob, m_Town.Town))
				{
					if (!TownDatabase.HasCitizenInTownGivenStatus(m_Town.Town, TownStatus.Leader))
					{
						mob.SendGump(new TownPrompt(m_Town.Town, mob, 2));
						from.SendGump(new TownResourcesGump(TownResourcesGumpPage.Citizens, from, m_Town));
					}
					else
					{
						from.SendLocalizedMessage(1063797); // To miasto ma juz przedstawiciela
					}
				}
				else
				{
					from.SendLocalizedMessage(1063798); // Ta postac nie jest obywatelem tego miasta
				}
			}
			else
			{
				from.SendLocalizedMessage(1063796); // Nie mozna nadac temu przedstawicielstwa
			}
		}

		private void MakePrimeConselour_OnTarget(Mobile from, object obj)
		{
			// Check distance
			if (!CheckDistance(m_Town, from))
			{
				from.SendAsciiMessage("Jestes za daleko od Duszy Miasta.");
				return;
			}

			if (!CheckVisibility(m_Town, from))
			{
				from.SendAsciiMessage("Cos blokuje dostep do Duszy Miasta.");
				return;
			}

			if (obj is PlayerMobile)
			{
				Mobile mob = (Mobile)obj;
				if (TownDatabase.IsCitizenOfGivenTown(mob, m_Town.Town))
				{
					mob.SendGump(new TownPrompt(m_Town.Town, mob, 5));
				}
				else
				{
					from.SendLocalizedMessage(1063798); // Ta postac nie jest obywatelem tego miasta
				}
			}
			else
			{
				from.SendLocalizedMessage(1063875); // Nie mozna nadac temu tytulu kanclerza
			}

			from.SendGump(new TownResourcesGump(TownResourcesGumpPage.Citizens, from, m_Town));
		}

		private void MakeDiplomacyConselour_OnTarget(Mobile from, object obj)
		{
			// Check distance
			if (!CheckDistance(m_Town, from))
			{
				from.SendAsciiMessage("Jestes za daleko od Duszy Miasta.");
				return;
			}

			if (!CheckVisibility(m_Town, from))
			{
				from.SendAsciiMessage("Cos blokuje dostep do Duszy Miasta.");
				return;
			}

			if (obj is PlayerMobile)
			{
				Mobile mob = (Mobile)obj;
				if (TownDatabase.IsCitizenOfGivenTown(mob, m_Town.Town))
				{
					mob.SendGump(new TownPrompt(m_Town.Town, mob, 6));
				}
				else
				{
					from.SendLocalizedMessage(1063798); // Ta postac nie jest obywatelem tego miasta
				}
			}
			else
			{
				from.SendLocalizedMessage(1063875); // Nie mozna nadac temu tytulu kanclerza
			}

			from.SendGump(new TownResourcesGump(TownResourcesGumpPage.Citizens, from, m_Town));
		}

		private void MakeEconomyConselour_OnTarget(Mobile from, object obj)
		{
			// Check distance
			if (!CheckDistance(m_Town, from))
			{
				from.SendAsciiMessage("Jestes za daleko od Duszy Miasta.");
				return;
			}

			if (!CheckVisibility(m_Town, from))
			{
				from.SendAsciiMessage("Cos blokuje dostep do Duszy Miasta.");
				return;
			}

			if (obj is PlayerMobile)
			{
				Mobile mob = (Mobile)obj;
				if (TownDatabase.IsCitizenOfGivenTown(mob, m_Town.Town))
				{
					mob.SendGump(new TownPrompt(m_Town.Town, mob, 7));
				}
				else
				{
					from.SendLocalizedMessage(1063798); // Ta postac nie jest obywatelem tego miasta
				}
			}
			else
			{
				from.SendLocalizedMessage(1063875); // Nie mozna nadac temu tytulu kanclerza
			}

			from.SendGump(new TownResourcesGump(TownResourcesGumpPage.Citizens, from, m_Town));
		}

		private void MakeArmyConselour_OnTarget(Mobile from, object obj)
		{
			// Check distance
			if (!CheckDistance(m_Town, from))
			{
				from.SendAsciiMessage("Jestes za daleko od Duszy Miasta.");
				return;
			}

			if (!CheckVisibility(m_Town, from))
			{
				from.SendAsciiMessage("Cos blokuje dostep do Duszy Miasta.");
				return;
			}

			if (obj is PlayerMobile)
			{
				Mobile mob = (Mobile)obj;
				if (TownDatabase.IsCitizenOfGivenTown(mob, m_Town.Town))
				{
					mob.SendGump(new TownPrompt(m_Town.Town, mob, 8));
				}
				else
				{
					from.SendLocalizedMessage(1063798); // Ta postac nie jest obywatelem tego miasta
				}
			}
			else
			{
				from.SendLocalizedMessage(1063875); // Nie mozna nadac temu tytulu kanclerza
			}

			from.SendGump(new TownResourcesGump(TownResourcesGumpPage.Citizens, from, m_Town));
		}

		private void MakeArchitectureConselour_OnTarget(Mobile from, object obj)
		{
			// Check distance
			if (!CheckDistance(m_Town, from))
			{
				from.SendAsciiMessage("Jestes za daleko od Duszy Miasta.");
				return;
			}

			if (!CheckVisibility(m_Town, from))
			{
				from.SendAsciiMessage("Cos blokuje dostep do Duszy Miasta.");
				return;
			}

			if (obj is PlayerMobile)
			{
				Mobile mob = (Mobile)obj;
				if (TownDatabase.IsCitizenOfGivenTown(mob, m_Town.Town))
				{
					mob.SendGump(new TownPrompt(m_Town.Town, mob, 9));
				}
				else
				{
					from.SendLocalizedMessage(1063798); // Ta postac nie jest obywatelem tego miasta
				}
			}
			else
			{
				from.SendLocalizedMessage(1063875); // Nie mozna nadac temu tytulu kanclerza
			}

			from.SendGump(new TownResourcesGump(TownResourcesGumpPage.Citizens, from, m_Town));
		}

		private List<string> Wrap(string value)
		{
			if (value == null || (value = value.Trim()).Length <= 0)
				return null;

			string[] values = value.Split(' ');
			List<string> list = [];
			string current = "";

			for (int i = 0; i < values.Length; ++i)
			{
				string val = values[i];

				string v = current.Length == 0 ? val : current + ' ' + val;

				if (v.Length < 15)
				{
					current = v;
				}
				else if (v.Length == 15)
				{
					list.Add(v);

					if (list.Count == 8)
						return list;

					current = "";
				}
				else if (val.Length <= 15)
				{
					list.Add(current);

					if (list.Count == 8)
						return list;

					current = val;
				}
				else
				{
					while (v.Length >= 15)
					{
						list.Add(v.Substring(0, 15));

						if (list.Count == 8)
							return list;

						v = v.Substring(15);
					}

					current = v;
				}
			}

			if (current.Length > 0)
				list.Add(current);

			return list;
		}
	}

	public class TownRename : Gump
	{
		readonly DuszaMiasta m_town;
		private const int LabelColor = 0x7FFF;
		private const int SelectedColor = 0x421F;

		public void AddTownButton(int x, int y, Towns townSelected)
		{
			AddButton(x, y, 4006, 4007, (int)townSelected, GumpButtonType.Reply, 0);
			AddHtml(x + 45, y, 240, 20, townSelected.ToString(), false, false);
		}

		public TownRename(DuszaMiasta town) : base(50, 40)
		{
			m_town = town;

			AddPage(0);

			AddBackground(0, 0, 200, 450, 5054); //Increase height for every added city

			AddTownButton(10, 30, Towns.None);
			AddTownButton(10, 50, Towns.Orod);
			AddTownButton(10, 70, Towns.Garlan);
			AddTownButton(10, 90, Towns.Twierdza);
			AddTownButton(10, 110, Towns.LDelmah);
			AddTownButton(10, 130, Towns.Lotharn);
			AddTownButton(10, 150, Towns.Tirassa);
		}

		public override void OnResponse(NetState sender, RelayInfo info)
		{
			Mobile from = sender.Mobile;
			if (from.AccessLevel < AccessLevel.Administrator)
				return;
			int val = info.ButtonID;
			if (val < 0)
				return;
			// Ustaw wartosc miasta w duszy miasta i bazie danych
			m_town.Town = (Towns)val;
			TownDatabase.CreateTown(m_town.Town);
			from.SendGump(new TownResourcesGump(0, from, m_town));
		}
	}

	public class TownPrompt : Gump
	{
		readonly Mobile tmpMobileToRemove;
		private const int LabelColor = 0x7FFF;
		private const int SelectedColor = 0x421F;
		private readonly int m_quest;

		public void AddTownButton(int x, int y, Towns townSelected)
		{
			AddButton(x, y, 4006, 4007, (int)townSelected, GumpButtonType.Reply, 0);
			AddLabel(x + 45, y, SelectedColor, townSelected.ToString());
		}


		public TownPrompt(Towns town, Mobile from, int Question, int IndexOfRemove = 0)
			: base(50, 40)
		{
			m_quest = Question;
			AddPage(0);

			AddBackground(0, 0, 300, 200, 5054);

			switch (m_quest)
			{
				case 0: // Nadanie obywatelstwo
					AddHtmlLocalized(10, 10, 250, 250, 1063799, String.Format("{0}", town.ToString()), LabelColor,
						false, false); // Czy przyjmujesz obywatelstwo miasta ~1_val~?
					break;
				case 1: // Porzuc obywatelstwo
					// Send GUMP to ask if player wants it
					AddHtmlLocalized(10, 10, 250, 250, 1063802,
						String.Format("{0}", TownDatabase.IsCitizenOfWhichTown(from).ToString()), LabelColor, false,
						false); // Na pewno chcesz porzucic obywatelstwo w miescie ~1_val~? Spowoduje to strate calego poswiecenia jakiego dokonales na rzecz miasta.
					break;
				case 2: // Nadaj przedstawicielstwo
					// Send GUMP to ask if player wants it
					AddHtmlLocalized(10, 10, 250, 250, 1063801, String.Format("{0}", town.ToString()), LabelColor,
						false, false); // Czy przyjmujesz przywodctwo w miescie ~1_val~?
					break;
				case 3: // Zabierz przedstawicielstwo
					Mobile tmpMobile;
					tmpMobile = TownDatabase.CitizenMobileFromTownWithStatus(town, TownStatus.Leader);
					// Send GUMP to ask if player wants it
					AddHtmlLocalized(10, 10, 250, 250, 1063808, String.Format("{0}", tmpMobile.Name), LabelColor, false,
						false); // Czy na pewnoe chcesz pozbawic ~1_val~ przedstawicielstwa?
					break;
				case 4: // Zabierz obywatelstwo
					SortedList<string, Mobile> m_cits_to_remove = TownDatabase.GetMobilesByName(town);
					tmpMobileToRemove = m_cits_to_remove.Values[IndexOfRemove];
					// Send GUMP to ask if player wants it
					AddHtmlLocalized(10, 10, 250, 250, 1063809, String.Format("{0}", tmpMobileToRemove.Name),
						LabelColor, false, false); // Czy na pewnoe chcesz pozbawic ~1_val~ obywatelstwa?
					break;
				case 5: // Mianuj glownym kanclerzem
					// Send GUMP to ask if player wants it
					AddHtmlLocalized(10, 10, 250, 250, 1063876, String.Format("{0}", town.ToString()), LabelColor,
						false, false); // Czy przyjmujesz tytul glownego kanclerza w miescie ~1_val~?
					break;
				case 6: // Mianuj kanclerzem dyplomacji
					// Send GUMP to ask if player wants it
					AddHtmlLocalized(10, 10, 250, 250, 1063877, String.Format("{0}", town.ToString()), LabelColor,
						false, false); // Czy przyjmujesz tytul kanclerza dypolomacji w miescie ~1_val~?
					break;
				case 7: // Mianuj kanclerzem ekonomii
					// Send GUMP to ask if player wants it
					AddHtmlLocalized(10, 10, 250, 250, 1063878, String.Format("{0}", town.ToString()), LabelColor,
						false, false); // Czy przyjmujesz tytul kanclerza ekonomii w miescie ~1_val~?
					break;
				case 8: // Mianuj kanclerzem armii
					// Send GUMP to ask if player wants it
					AddHtmlLocalized(10, 10, 250, 250, 1063879, String.Format("{0}", town.ToString()), LabelColor,
						false, false); // Czy przyjmujesz tytul kanclerza armii w miescie ~1_val~?
					break;
				case 9: // Mianuj kanclerzem budownictwa
					// Send GUMP to ask if player wants it
					AddHtmlLocalized(10, 10, 250, 250, 1063880, String.Format("{0}", town.ToString()), LabelColor,
						false, false); // Czy przyjmujesz tytul kanclerza budownictwa w miescie ~1_val~?
					break;
				case 10: // Zabierz tytul kanclerza
					List<Mobile> m_cons_to_remove =
						TownDatabase.GetCitizensByNameWithStatusAsList(town, TownStatus.Counsellor);
					tmpMobileToRemove = m_cons_to_remove[IndexOfRemove];
					// Send GUMP to ask if player wants it
					AddHtmlLocalized(10, 10, 250, 250, 1063882, String.Format("{0}", tmpMobileToRemove.Name),
						LabelColor, false, false); // Czy na pewnoe chcesz pozbawic ~1_val~ statusu kanclerza?
					break;
			}

			AddButton(50, 160, 4005, 4007, (int)town, GumpButtonType.Reply, 0);
			AddHtmlLocalized(85, 160, 240, 20, 1063803, LabelColor, false, false); // Tak
			AddButton(150, 160, 4005, 4007, -1, GumpButtonType.Reply, 0);
			AddHtmlLocalized(185, 160, 240, 20, 1063804, LabelColor, false, false); // Nie
		}

		public override void OnResponse(NetState sender, RelayInfo info)
		{
			Mobile from = sender.Mobile;

			int val = info.ButtonID;
			if (val <= 0)
				return;
			switch (m_quest)
			{
				case 0: // Nadanie obywatelstwo
					if (val > 0) // Tak
					{
						TownDatabase.AddTownLog((Towns)val, TownLogTypes.OBYWATELSTWO_NADANIE, from.Name, 0, 0, 0);
						TownDatabase.AddCitizen(from, (Towns)val);
					}

					break;
				case 1: // Porzuc obywatelstwo
					if (val > 0) // Tak
					{
						TownDatabase.AddTownLog((Towns)val, TownLogTypes.OBYWATELSTWO_ZAKONCZONO, from.Name, 0, 0, 0);
						TownDatabase.LeaveCurrentTown(from);
					}

					break;
				case 2: // Nadaj przedstawicielstwo
					if (val > 0) // Tak
					{
						TownDatabase.AddTownLog((Towns)val, TownLogTypes.OBYWATELSTWO_STATUS_NADANO, from.Name,
							(int)(TownStatus.Leader), 0, 0);
						TownDatabase.ChangeCurrentTownStatus(from, TownStatus.Leader);
					}

					break;
				case 3: // Zabierz przedsawicielstwo
					if (val > 0) // Tak
					{
						Mobile tmpMobile;
						tmpMobile = TownDatabase.CitizenMobileFromTownWithStatus((Towns)val, TownStatus.Leader);
						TownDatabase.ChangeCurrentTownStatus(tmpMobile, TownStatus.Citizen);
					}

					break;
				case 4: // Zabierz obywatelstwo
					if (val > 0) // Tak
					{
						TownDatabase.AddTownLog((Towns)val, TownLogTypes.OBYWATELSTWO_ZAKONCZONO, from.Name, 0, 0, 0);
						TownDatabase.LeaveCurrentTown(tmpMobileToRemove);
						tmpMobileToRemove.SendAsciiMessage(
							String.Format("Zostales pozbawiony obywatelstwa w miescie {0}.", ((Towns)val).ToString()));
					}

					break;
				case 5: // Mianuj glownym kanclerzem
					if (val > 0) // Tak
					{
						TownDatabase.AddTownLog((Towns)val, TownLogTypes.KANCLERZ_NADANO_GLOWNY, from.Name, 0, 0, 0);
						TownDatabase.ChangeCurrentTownStatus(from, TownStatus.Counsellor);
						TownDatabase.ChangeCurrentConselourStatus(from, TownCounsellor.Prime);
					}

					break;
				case 6: // Mianuj kanclerzem dyplomacji
					if (val > 0) // Tak
					{
						TownDatabase.AddTownLog((Towns)val, TownLogTypes.KANCLERZ_NADANO_DYPLOMACJI, from.Name, 0, 0,
							0);
						TownDatabase.ChangeCurrentTownStatus(from, TownStatus.Counsellor);
						TownDatabase.ChangeCurrentConselourStatus(from, TownCounsellor.Diplomacy);
					}

					break;
				case 7: // Mianuj kanclerzem ekonomii
					if (val > 0) // Tak
					{
						TownDatabase.AddTownLog((Towns)val, TownLogTypes.KANCLERZ_NADANO_EKONOMII, from.Name, 0, 0, 0);
						TownDatabase.ChangeCurrentTownStatus(from, TownStatus.Counsellor);
						TownDatabase.ChangeCurrentConselourStatus(from, TownCounsellor.Economy);
					}

					break;
				case 8: // Mianuj kanclerzem armii
					if (val > 0) // Tak
					{
						TownDatabase.AddTownLog((Towns)val, TownLogTypes.KANCLERZ_NADANO_ARMII, from.Name, 0, 0, 0);
						TownDatabase.ChangeCurrentTownStatus(from, TownStatus.Counsellor);
						TownDatabase.ChangeCurrentConselourStatus(from, TownCounsellor.Army);
					}

					break;
				case 9: // Mianuj kanclerzem budownictwa
					if (val > 0) // Tak
					{
						TownDatabase.AddTownLog((Towns)val, TownLogTypes.KANCLERZ_NADANO_BUDOWNICTWA, from.Name, 0, 0,
							0);
						TownDatabase.ChangeCurrentTownStatus(from, TownStatus.Counsellor);
						TownDatabase.ChangeCurrentConselourStatus(from, TownCounsellor.Architecture);
					}

					break;
				case 10: // Zabierz status kanclerza
					if (val > 0) // Tak
					{
						TownDatabase.ChangeCurrentTownStatus(tmpMobileToRemove, TownStatus.Citizen);
						TownDatabase.ChangeCurrentConselourStatus(tmpMobileToRemove, TownCounsellor.None);
						TownDatabase.InformLeader((Towns)val,
							String.Format("{0} zostal pozbiawony statusu kanclerza dyplomacji w miescie {1}.",
								tmpMobileToRemove.Name, ((Towns)val).ToString()));
					}

					break;
			}
		}
	}
}
