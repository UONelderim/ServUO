#region References

using System;
using System.Collections.Generic;
using System.Linq;
using Nelderim.Towns;
using Server.Network;
using Server.Prompts;

#endregion

namespace Server.Gumps
{
	public class TownPostsGump : Gump
	{
		private const int LabelColor = 0x21;
		private const int SelectedColor = 0x7A1;
		private readonly Towns m_fromTown;
		private static Dictionary<Towns, string[]> additionalTownRegions = new()
		{
			{ Towns.LDelmah, new[] {"NoamuthQuortek", "NoamuthQuortek_Kopalnia", "L'Delmah", "L'Delmah_Kopalnia" } },
			{ Towns.Garlan, new[] {"Garlan_Kopalnia" } },
			{ Towns.Orod, new[] {"Orod_Kopalnia" } },
			{ Towns.Lotharn, new[] { "Enedh_Kopalnia" } }
		};

		public void AddButton(int x, int y, int buttonID, int buttonCli)
		{
			AddButton(x, y, 4006, 4007, buttonID, GumpButtonType.Reply, 0);
			AddHtmlLocalized(x + 40, y, 250, 250, buttonCli, LabelColor, false, false);
		}

		public void AddButtonString(int x, int y, int buttonID, string title)
		{
			AddButton(x, y, 4006, 4007, buttonID, GumpButtonType.Reply, 0);
			AddHtml(x + 40, y, 350, 250, title, false, false);
		}

		public TownPostsGump(Towns town, Mobile from, int page = 0, int which = 0)
			: base(50, 40)
		{
			m_fromTown = town;
			from.CloseGump(typeof(TownPostsGump));

			AddPage(0);

			AddBackground(0, 0, 580, 800, 5054);

			AddHtmlLocalized(10, 10, 210, 28, 1063924, String.Format("{0}", m_fromTown.ToString()), LabelColor, true,
				false); // Posterunki miasta ~1_val~

			if (page == 0) // Main page
			{
				AddButton(10, 40, 1, 1063907); // Posterunki
				AddLabel(10, 100, LabelColor, "Utworzenie nowego posterunku kosztuje 5000 gp");
				AddButton(10, 120, 2, 1063908); // Utworz nowy posterunek tutaj
			}
			else if (page == 1)
			{
				#region Wyswietlanie listy posterunkow

				int m_y = 40;

				AddButton(420, 10, 50, 1063907); // Powrot

				List<TownPost> tps = TownDatabase.GetTown(m_fromTown).TownPosts;
				foreach (TownPost tp in tps)
				{
					AddButtonString(10, m_y, 100 + tps.IndexOf(tp), tp.PostName);
					switch (tp.PostStatus)
					{
						case TownBuildingStatus.Dziala:
							AddImage(395, m_y, 11402);
							break;
						case TownBuildingStatus.Zawieszony:
							AddImage(395, m_y, 11410);
							break;
					}

					AddHtml(420, m_y, 250, 250, tp.ActivatedDate.ToString(), false, false);
					m_y += 20;
				}

				#endregion
			}
			else if (page == 2)
			{
				#region Wyswietlenie konkretnego posterunku

				AddButton(220, 10, 49, 1063907); // Powrot

				List<TownPost> tps = TownDatabase.GetTown(m_fromTown).TownPosts;
				TownPost tp = tps[which];

				AddHtmlLocalized(10, 40, 250, 28, 1063909, LabelColor, false, false); // Nazwa posterunku
				AddHtml(220, 40, 320, 24, tp.PostName, true, false);
				AddHtmlLocalized(10, 60, 250, 250, 1063910, LabelColor, false, false); // Typ straznika
				AddHtml(220, 60, 250, 250, tp.TownGuard.ToString(), false, false);
				AddHtmlLocalized(10, 80, 250, 250, 1063911, LabelColor, false, false); // Status posterunku
				AddHtml(220, 80, 250, 250, tp.PostStatus.ToString(), false, false);
				AddHtmlLocalized(10, 100, 250, 250, 1063912, LabelColor, false, false); // Lacznie wskrzeszen
				AddHtml(220, 100, 250, 250, tp.RessurectAmount.ToString(), false, false);
				AddHtmlLocalized(10, 120, 250, 250, 1063913, LabelColor, false,
					false); // Lacznie aktywnych dni straznika
				AddHtml(220, 120, 250, 250, tp.ActiveDaysAmount.ToString(), false, false);
				AddHtmlLocalized(10, 140, 250, 250, 1063914, LabelColor, false, false); // Srednio wskrzeszen na dzien
				AddHtml(220, 140, 250, 250, tp.RessurectPerDay.ToString(), false, false);
				AddHtmlLocalized(10, 160, 250, 250, 1063926, LabelColor, false, false); // Srednio wskrzeszen na dzien
				AddHtml(220, 160, 250, 250, String.Format("| X:{0} | Y:{1} | Z:{1} |", tp.m_x, tp.m_y, tp.m_z), false,
					false);
				AddHtmlLocalized(10, 180, 250, 250, 1063936, LabelColor, false, false); // Status straznika
				AddHtml(220, 180, 250, 250, tp.IsGuardAlive() ? "Zywy" : "Martwy", false, false);

				AddButton(10, 200, 200 + which, 1063915); // Zmien nazwe posterunku
				AddButton(220, 200, 300 + which, 1063916); // Zmien typ przypisanego straznika
				AddButton(10, 220, 400 + which, 1063917); // Usun posterunek

				if (tp.PostStatus == TownBuildingStatus.Dziala)
				{
					AddButton(10, 260, 1600 + which, 1063957); // Przenies posterunek tutaj
					AddHtmlLocalized(220, 260, 250, 250, 1063961, LabelColor, false,
						false); // Srednio wskrzeszen na dzien
				}

				AddImage(50, 300, 30500); // Tlo

				switch (tp.PostStatus)
				{
					case TownBuildingStatus.Dostepny:
						AddButton(220, 220, 500 + which, 1063925); // Uruchom posterunek
						break;
					case TownBuildingStatus.Dziala:
						AddImage(275, 320, 11402);
						AddButton(220, 220, 600 + which, 1063918); // Zawies dzialanie posterunku
						break;
					case TownBuildingStatus.Zawieszony:
						AddImage(275, 320, 11410);
						AddButton(220, 220, 700 + which, 1063919); // Wznow dzialanie posterunku
						break;
				}

				if (which > 0)
				{
					AddButton(10, 240, 100 + which - 1, 1063920); // Poprzedni posterunek
				}

				if ((which + 1) < tps.Count)
				{
					AddButton(220, 240, 100 + which + 1, 1063921); // Nastepny posterunek
				}

				switch (tp.TownGuard)
				{
					case TownGuards.Straznik:
						AddImage(245, 390, 21632);
						break;
					case TownGuards.CiezkiStraznik:
						AddImage(245, 390, 21642);
						break;
					case TownGuards.Strzelec:
						AddImage(245, 390, 21641);
						break;
					case TownGuards.StraznikKonny:
						AddImage(245, 390, 21633);
						break;
					case TownGuards.StraznikMag:
						AddImage(245, 390, 21636);
						break;
					case TownGuards.StraznikElitarny:
						AddImage(245, 390, 21640);
						break;
				}

				AddImage(225, 480, 30501 + which % 10);

				#endregion
			}
			else if (page == 3)
			{
				#region Zmien straznika

				List<TownGuards> tg = TownDatabase.GetTown(town).GetAvailableGuards();

				TownPost tp = TownDatabase.GetTown(m_fromTown).TownPosts[which];

				AddButton(220, 10, 49, 1063907); // Powrot

				if (tg.Contains(TownGuards.Straznik) && tp.TownGuard != TownGuards.Straznik)
				{
					AddButtonString(10, 40, 1000 + which, "Straznik");
				}
				else
				{
					AddLabel(10, 40, SelectedColor, "Straznik");
				}

				if (tg.Contains(TownGuards.CiezkiStraznik) && tp.TownGuard != TownGuards.CiezkiStraznik)
				{
					AddButtonString(10, 60, 1100 + which, "CiezkiStraznik");
				}
				else
				{
					AddLabel(10, 60, SelectedColor, "CiezkiStraznik");
				}

				if (tg.Contains(TownGuards.Strzelec) && tp.TownGuard != TownGuards.Strzelec)
				{
					AddButtonString(10, 80, 1200 + which, "Strzelec");
				}
				else
				{
					AddLabel(10, 80, SelectedColor, "Strzelec");
				}

				if (tg.Contains(TownGuards.StraznikKonny) && tp.TownGuard != TownGuards.StraznikKonny)
				{
					AddButtonString(10, 100, 1300 + which, "StraznikKonny");
				}
				else
				{
					AddLabel(10, 100, SelectedColor, "StraznikKonny");
				}

				if (tg.Contains(TownGuards.StraznikMag) && tp.TownGuard != TownGuards.StraznikMag)
				{
					AddButtonString(10, 120, 1400 + which, "StraznikMag");
				}
				else
				{
					AddLabel(10, 120, SelectedColor, "StraznikMag");
				}

				if (tg.Contains(TownGuards.StraznikElitarny) && tp.TownGuard != TownGuards.StraznikElitarny)
				{
					AddButtonString(10, 140, 1500 + which, "StraznikElitarny");
				}
				else
				{
					AddLabel(10, 140, SelectedColor, "StraznikElitarny");
				}

				#endregion
			}
		}
		
		private bool IsWithinTownRegion(Mobile from, Towns town)
		{
			from.UpdateRegion();

			if (from.Region.ToString() == town.ToString())
				return true;

			if (additionalTownRegions.ContainsKey(town) && additionalTownRegions[town].Contains(from.Region.ToString()))
				return true;

			return false;
		}

		public override void OnResponse(NetState sender, RelayInfo info)
		{
			Mobile from = sender.Mobile;

			int val = info.ButtonID;

			if (val <= 0)
				return;

			#region Zmien typ straznika

			if (val >= 1600) // Przenies posterunek
			{
				if (IsWithinTownRegion(from, m_fromTown))
				{
					// Sprawdz czy skarbiec posiada zloto                          
					if (TownDatabase.GetTown(m_fromTown).Resources.HasResourceAmount(TownResourceType.Zloto, 1000))
					{
						if (TownDatabase.GetTown(m_fromTown).TownPosts[val - 1600].IsGuardAlive())
						{
							// Warunki spelnione, pobierz pieniadze ze skarbca i utworz posterunek
							TownDatabase.GetTown(m_fromTown).Resources
								.ResourceDecreaseAmount(TownResourceType.Zloto, 1000);
							TownDatabase.GetTown(m_fromTown).TownPosts[val - 1600].UpdatePostLocation(from);
							from.SendGump(new TownPostsGump(m_fromTown, from, 2, val - 1600));
							from.SendLocalizedMessage(1063958); // Posterunek zostal przeniesiony w nowe miejsce
						}
						else
						{
							from.SendLocalizedMessage(1063959); // Straznik musi zyc zeby moc przeniesc posterunek
						}
					}
					else
					{
						from.SendLocalizedMessage(1063960);
						from.SendGump(new TownPostsGump(m_fromTown, from, 2, val - 1600));
					}
				}
				else
				{
					from.SendLocalizedMessage(1063935);
					from.SendGump(new TownPostsGump(m_fromTown, from, 2, val - 1600));
				}
			}
			else if (val >= 1500)
			{
				TownDatabase.GetTown(m_fromTown).TownPosts[val - 1500].TownGuard = TownGuards.StraznikElitarny;
				from.SendGump(new TownPostsGump(m_fromTown, from, 2, val - 1500));
			}
			else if (val >= 1400)
			{
				TownDatabase.GetTown(m_fromTown).TownPosts[val - 1400].TownGuard = TownGuards.StraznikMag;
				from.SendGump(new TownPostsGump(m_fromTown, from, 2, val - 1400));
			}
			else if (val >= 1300)
			{
				TownDatabase.GetTown(m_fromTown).TownPosts[val - 1300].TownGuard = TownGuards.StraznikKonny;
				from.SendGump(new TownPostsGump(m_fromTown, from, 2, val - 1300));
			}
			else if (val >= 1200)
			{
				TownDatabase.GetTown(m_fromTown).TownPosts[val - 1200].TownGuard = TownGuards.Strzelec;
				from.SendGump(new TownPostsGump(m_fromTown, from, 2, val - 1200));
			}
			else if (val >= 1100)
			{
				TownDatabase.GetTown(m_fromTown).TownPosts[val - 1100].TownGuard = TownGuards.CiezkiStraznik;
				from.SendGump(new TownPostsGump(m_fromTown, from, 2, val - 1100));
			}
			else if (val >= 1000)
			{
				TownDatabase.GetTown(m_fromTown).TownPosts[val - 1000].TownGuard = TownGuards.Straznik;
				from.SendGump(new TownPostsGump(m_fromTown, from, 2, val - 1000));
			}

			#endregion

			#region Status posterunku

			else if (val >= 700) // Wznow posterunek
			{
				TownManager tm = TownDatabase.GetTown(m_fromTown);
				// Sprawdz czy skarbiec moze pokryc koszt wznowienia posterunku
				if (tm.HasResourcesForGuard(tm.TownPosts[val - 700].TownGuard, TownBuildingStatus.Zawieszony))
				{
					// Sprawdz czy ilosc straznikow nie przekracza ilosci maksymalnej
					if (tm.GetActiveGuards() < tm.MaxGuards)
					{
						// Wnies oplate ze skarbca za wznownienie posterunku
						tm.UseResourcesForGuard(tm.TownPosts[val - 700].TownGuard, TownBuildingStatus.Zawieszony);
						// Uruchom posterunke oraz zespawnuj straznika
						tm.TownPosts[val - 700].PostStatus = TownBuildingStatus.Dziala;
						from.SendGump(new TownPostsGump(m_fromTown, from, 2, val - 700));
						TownDatabase.AddTownLog(m_fromTown, TownLogTypes.POSTERUNEK_WZNOWIONO,
							tm.TownPosts[val - 700].PostName, 0, 0, 0);
					}
					else
					{
						from.SendLocalizedMessage(
							1063932); // Ilosc aktywnych straznikow jest zbyt duza, by aktywowac kolejnego straznika.
					}
				}
				else
				{
					from.SendLocalizedMessage(
						1063933); // Skarbiec miasta nie posiada surowcow, by reaktywowac posterunek.
				}
			}
			else if (val >= 600) // Zawies posterunek
			{
				TownManager tm = TownDatabase.GetTown(m_fromTown);
				// Zmien status posterunku na zawieszony (straznik zostaje zabity przez handler PostStatus
				tm.TownPosts[val - 600].PostStatus = TownBuildingStatus.Zawieszony;
				from.SendGump(new TownPostsGump(m_fromTown, from, 2, val - 600));
				TownDatabase.AddTownLog(m_fromTown, TownLogTypes.POSTERUNEK_ZAWIESZONO,
					tm.TownPosts[val - 600].PostName, 0, 0, 0);
			}
			else if (val >= 500) // Uruchom posterunek
			{
				TownManager tm = TownDatabase.GetTown(m_fromTown);

				// Sprawdz czy skarbiec moze pokryc koszt uruchomienia posterunku
				if (tm.HasResourcesForGuard(tm.TownPosts[val - 500].TownGuard, TownBuildingStatus.Dostepny))
				{
					// Sprawdz czy ilosc straznikow nie przekracza ilosci maksymalnej
					if (tm.GetActiveGuards() < tm.MaxGuards)
					{
						// Wnies oplate ze skarbca za uruchomienie posterunku
						tm.UseResourcesForGuard(tm.TownPosts[val - 500].TownGuard, TownBuildingStatus.Dostepny);
						// Uruchom posterunke oraz zespawnuj straznika
						tm.TownPosts[val - 500].PostStatus = TownBuildingStatus.Dziala;
						TownDatabase.AddTownLog(m_fromTown, TownLogTypes.POSTERUNEK_WYBUDOWANO,
							tm.TownPosts[val - 500].PostName, 0, 0, 0);
						from.SendGump(new TownPostsGump(m_fromTown, from, 2, val - 500));
					}
					else
					{
						from.SendLocalizedMessage(
							1063932); // Ilosc aktywnych straznikow jest zbyt duza, by aktywowac kolejnego straznika.
					}
				}
				else
				{
					from.SendLocalizedMessage(
						1063934); // Skarbiec miasta nie posiada surowcow, by aktywowac posterunek.
				}
			}
			else if (val >= 400) //Usun posterunek
			{
				TownDatabase.GetTown(m_fromTown).TownPosts[val - 400].PostStatus = TownBuildingStatus.Dostepny;
				TownDatabase.GetTown(m_fromTown).TownPosts
					.Remove(TownDatabase.GetTown(m_fromTown).TownPosts[val - 400]);
				from.SendGump(new TownPostsGump(m_fromTown, from, 1));
			}

			#endregion

			else if (val >= 300) // Zmien typ straznika
			{
				from.SendGump(new TownPostsGump(m_fromTown, from, 3, val - 300));
			}
			else if (val >= 200) // Zmiana nazwy posterunku
			{
				from.Prompt = new TownPostRenamePrompt(m_fromTown, val - 200);
				from.SendLocalizedMessage(1063927); // Jak powinien nazywac sie posterunek?
				from.SendGump(new TownPostsGump(m_fromTown, from));
			}
			else if (val >= 100) // Poprzedni/Nastepny posterunek
			{
				from.SendGump(new TownPostsGump(m_fromTown, from, 2, val - 100));
			}
			else
			{
				#region Przyciski funkcyjne

				switch (val)
				{
					case 1:
						from.SendGump(new TownPostsGump(m_fromTown, from, 1));
						break;
					case 2:
						if (IsWithinTownRegion(from, m_fromTown))
						{
							TownManager tm = TownDatabase.GetTown(m_fromTown);
							// Sprawdz czy ilosc posterunkow pozwala na zbudowanie kolejnego
							if (tm.GetCreatedPosts() < tm.MaxPosts)
							{
								// Sprawdz czy skarbiec posiada zloto                          
								if (TownDatabase.GetTown(m_fromTown).Resources
								    .HasResourceAmount(TownResourceType.Zloto, 5000))
								{
									// Warunki spelnione, pobierz pieniadze ze skarbca i utworz posterunek
									TownDatabase.GetTown(m_fromTown).Resources
										.ResourceDecreaseAmount(TownResourceType.Zloto, 5000);
									tm.CreatePostHere(from);
									from.SendGump(new TownPostsGump(m_fromTown, from, 1));
								}
								else
								{
									from.SendLocalizedMessage(1063930);
									from.SendGump(new TownPostsGump(m_fromTown, from));
								}
							}
							else
							{
								from.SendLocalizedMessage(1063931);
								from.SendGump(new TownPostsGump(m_fromTown, from));
							}
						}
						else
						{
							from.SendLocalizedMessage(1063935);
							from.SendGump(new TownPostsGump(m_fromTown, from));
						}

						break;
					case 49:
						// Powrot do posterunkow
						from.SendGump(new TownPostsGump(m_fromTown, from, 1));
						break;
					case 50:
						// Powrot do glownej
						from.SendGump(new TownPostsGump(m_fromTown, from));
						break;
					default:
						from.SendGump(new TownPostsGump(m_fromTown, from));
						break;
				}

				#endregion
			}
		}
	}
}

namespace Server.Prompts
{
	public class TownPostRenamePrompt : Prompt
	{
		private readonly int m_whichPost;
		private readonly int m_howMuch;
		private readonly Towns m_whichTown;

		public TownPostRenamePrompt(Towns town, int which)
		{
			m_whichPost = which;
			m_whichTown = town;
			m_howMuch = TownDatabase.GetTown(m_whichTown).TownPosts.Count;
		}

		public override void OnResponse(Mobile from, string text)
		{
			if (TownDatabase.GetTown(m_whichTown).TownPosts.Count == m_howMuch)
			{
				TownDatabase.GetTown(m_whichTown).TownPosts[m_whichPost].PostName = text;
				from.SendLocalizedMessage(1063928); // Nazwa posterunku zmieniona.
			}
			else
			{
				from.SendLocalizedMessage(1063929); // Liczba posterunkow sie zmienila. Nazwa nie zostala zmieniona.
			}
		}
	}
}
