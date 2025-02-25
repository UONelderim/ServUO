using System;
using System.Linq;
using Nelderim.Towns;
using Server.Network;

namespace Server.Gumps
{
	public class TownDatabaseGump : Gump
	{
		enum TownDatabaseGumpPage
		{
			Glowna,
			Budowa,
			Obywatele,
			StatystykiObywatele,
			StatystykiBudynki,
			Zamknij,
			PrzegladWladzMiasta,
		}

		public void AddButtonWithLabeled(int x, int y, int page)
		{
			AddButton(x, y, 4006, 4007, page, GumpButtonType.Reply, 0);
			AddHtml(x + 40, y, 150, 30, ((TownDatabaseGumpPage)page).ToString(), false, false);
		}

		public TownDatabaseGump(Mobile from, int page = 0) : base(50, 40)
		{
			AddBackground(0, 0, 600, 400, 5054);
			TownDatabaseGumpPage pageE = (TownDatabaseGumpPage)page;
			AddHtml(250, 0, 100, 30, "Nelderim Towns", true, false);
			if (TownDatabase.GetTownsNames() != null)
			{
				switch (pageE)
				{
					case TownDatabaseGumpPage.Glowna:
						AddButtonWithLabeled(10, 30, (int)TownDatabaseGumpPage.Budowa);
						AddButtonWithLabeled(10, 50, (int)TownDatabaseGumpPage.Obywatele);
						AddButtonWithLabeled(10, 70, (int)TownDatabaseGumpPage.StatystykiObywatele);
						AddButtonWithLabeled(10, 90, (int)TownDatabaseGumpPage.StatystykiBudynki);
						AddButtonWithLabeled(10, 110, (int)TownDatabaseGumpPage.PrzegladWladzMiasta);
						AddButtonWithLabeled(400, 370, (int)TownDatabaseGumpPage.Zamknij);
						break;
					case TownDatabaseGumpPage.Budowa:
						int yBudowa = 30;
						foreach (Towns tm in TownDatabase.GetTownsNames())
						{
							if (tm != Towns.None)
							{
								foreach (TownBuilding tb in TownDatabase.GetTown(tm).Buildings)
								{
									if (tb.Status == TownBuildingStatus.Budowanie)
									{
										AddHtml(10, yBudowa, 500, 30,
											String.Format("{0} - {1}", tm.ToString(), tb.BuildingType.ToString()), true,
											false);
										yBudowa += 30;
									}
								}
							}
						}

						AddButtonWithLabeled(10, 370, (int)TownDatabaseGumpPage.Glowna);
						AddButtonWithLabeled(400, 370, (int)TownDatabaseGumpPage.Zamknij);
						break;
					case TownDatabaseGumpPage.Obywatele:
						string ObywateleToShow = "| Serial | Miasto | Imie | Status |";
						var citNum = TownDatabase.GetCitizens().GetEnumerator();
						while (citNum.MoveNext())
						{
							Mobile mob = World.FindMobile(citNum.Current.Key);
							if (mob != null)
							{
								if (mob.Player)
								{
									ObywateleToShow = String.Format("{0}\n| {1} | {2} | {3} | {4} |",
										ObywateleToShow,
										citNum.Current.Key,
										citNum.Current.Value.CurrentTown.ToString(),
										mob.Name,
										citNum.Current.Value.CurrentTownStatus.ToString());
								}
								else
								{
									Console.WriteLine("{0} is not player, will be deleted on restart", mob.Name);
								}
							}
						}

						AddHtml(10, 30, 580, 300, ObywateleToShow, true, true);

						AddButtonWithLabeled(10, 370, (int)TownDatabaseGumpPage.Glowna);
						AddButtonWithLabeled(400, 370, (int)TownDatabaseGumpPage.Zamknij);
						break;
					case TownDatabaseGumpPage.PrzegladWladzMiasta:
						string ObywateleToShowz = "| Serial | Miasto | Imie | Status |";
						var citNumz = TownDatabase.GetCitizens().GetEnumerator();
						while (citNumz.MoveNext())
						{
							if (TownDatabase.GetCitizenCurrentStatus(citNumz.Current.Key) != TownStatus.Citizen)
							{
								Mobile mob = World.FindMobile(citNumz.Current.Key);

								if (mob != null)
								{
									if (mob.Player)
									{
										ObywateleToShowz = String.Format("{0}\n| {1} | {2} | {3} | {4} |",
											ObywateleToShowz,
											citNumz.Current.Key,
											citNumz.Current.Value.CurrentTown.ToString(),
											mob.Name,
											citNumz.Current.Value.CurrentTownStatus.ToString());
									}
									else
									{
										Console.WriteLine("{0} is not player, will be deleted on restart", mob.Name);
									}
								}
							}
						}

						AddHtml(10, 30, 580, 300, ObywateleToShowz, true, true);

						AddButtonWithLabeled(10, 370, (int)TownDatabaseGumpPage.Glowna);
						AddButtonWithLabeled(400, 370, (int)TownDatabaseGumpPage.Zamknij);
						break;
					case TownDatabaseGumpPage.StatystykiObywatele:
						AddHtml(10, 30, 100, 30, "Miasto", true, false);
						AddHtml(150, 30, 100, 30, "Obywatele", true, false);
						AddHtml(250, 30, 100, 30, "Przywodcy", true, false);
						AddHtml(350, 30, 100, 30, "Kanclerze", true, false);
						int yStatystykiObywatele = 60;
						foreach (Towns tm in TownDatabase.GetTownsNames())
						{
							if (tm != Towns.None)
							{
								AddHtml(10, yStatystykiObywatele, 100, 30, tm.ToString(), true, false);
								AddHtml(200, yStatystykiObywatele, 30, 30,
									TownDatabase.GetAmountOfCitizensWithGivenStatus(tm, TownStatus.Citizen).ToString(),
									false, false);
								AddHtml(300, yStatystykiObywatele, 30, 30,
									TownDatabase.GetAmountOfCitizensWithGivenStatus(tm, TownStatus.Leader).ToString(),
									false, false);
								AddHtml(400, yStatystykiObywatele, 30, 30,
									TownDatabase.GetAmountOfCitizensWithGivenStatus(tm, TownStatus.Counsellor)
										.ToString(), false, false);
								yStatystykiObywatele += 30;
							}
						}

						AddButtonWithLabeled(10, 370, (int)TownDatabaseGumpPage.Glowna);
						AddButtonWithLabeled(400, 370, (int)TownDatabaseGumpPage.Zamknij);
						break;
					case TownDatabaseGumpPage.StatystykiBudynki:
						AddHtml(10, 30, 100, 30, "Miasto", true, false);
						AddHtml(150, 30, 100, 30, "Dostepny", true, false);
						AddHtml(250, 30, 100, 30, "Budowanie", true, false);
						AddHtml(350, 30, 100, 30, "Dziala", true, false);
						AddHtml(450, 30, 100, 30, "Zawieszony", true, false);
						int yStatystykiBudynki = 60;
						foreach (Towns tm in TownDatabase.GetTownsNames())
						{
							if (tm != Towns.None)
							{
								AddHtml(10, yStatystykiBudynki, 100, 30, tm.ToString(), true, false);
								AddHtml(200, yStatystykiBudynki, 30, 30,
									(TownDatabase.GetTown(tm).Buildings
										.Count(obj => obj.Status == TownBuildingStatus.Dostepny)).ToString(), false,
									false);
								AddHtml(300, yStatystykiBudynki, 30, 30,
									(TownDatabase.GetTown(tm).Buildings
										.Count(obj => obj.Status == TownBuildingStatus.Budowanie)).ToString(), false,
									false);
								AddHtml(400, yStatystykiBudynki, 30, 30,
									(TownDatabase.GetTown(tm).Buildings
										.Count(obj => obj.Status == TownBuildingStatus.Dziala)).ToString(), false,
									false);
								AddHtml(500, yStatystykiBudynki, 30, 30,
									(TownDatabase.GetTown(tm).Buildings
										.Count(obj => obj.Status == TownBuildingStatus.Zawieszony)).ToString(), false,
									false);
								yStatystykiBudynki += 30;
							}
						}

						AddButtonWithLabeled(10, 370, (int)TownDatabaseGumpPage.Glowna);
						AddButtonWithLabeled(400, 370, (int)TownDatabaseGumpPage.Zamknij);
						break;
					default:
						return;
				}
			}
			else
			{
				AddHtml(50, 50, 500, 300,
					"Brak istniejacych miast w bazie, nalezy wpierw stworzyc dusze miasta i przypisac do miast, przez klikniecie zmien przynaleznosc duszy do miasta i wyborze miasta",
					true, false);
			}
		}

		public override void OnResponse(NetState sender, RelayInfo info)
		{
			Mobile from = sender.Mobile;

			int val = info.ButtonID;
			if ((TownDatabaseGumpPage)val == TownDatabaseGumpPage.Zamknij)
				return;

			from.SendGump(new TownDatabaseGump(from, val));
		}
	}
}
