using System;
using System.Collections.Generic;
using System.Linq;
using Server;
using Server.Commands;
using Server.Network;

namespace Nelderim.Towns
{
	class TownAnnouncer
	{
		private static readonly int m_check_interval = 3600; // Co godzine nastepuje sprawdzenie stanu miast

		private static readonly int
			m_check_ressurect_interval = 600; // Co 10 minut nastepuje sprawdzenie czy nalezy wskrzesic straznikow

		private static float m_average_players;
		private static float m_price_multiplier = 0.4f;
		private static bool m_do_charge_daily;

		private static DateTime m_last_algorithm; // Zapisana data kiedy ostatni raz bylo wykonanie obliczenia dla miast

		public DateTime LastAlgorithmTime
		{
			get { return m_last_algorithm; }
			set { m_last_algorithm = value; }
		}

		public TownAnnouncer()
		{
			m_last_algorithm = DateTime.Now;
			Init();
			InitRessurect();
		}

		public TownAnnouncer(DateTime lastCheck)
		{
			m_last_algorithm = lastCheck;
			Init();
			InitRessurect();
		}

		public static List<TownResource> ForeignResourcesToSell { get; set; } = new List<TownResource>();

		public static List<TownResource> ForeignResourcesToBuy { get; set; } = new List<TownResource>();

		// Kiedy onSerialize rowny true oznacza, ze obiekt zostaje dopiero tworzony, nie ma potrzeby pobrania oplat
		// Kiedy onSerialize rowny false oznacza, ze wywolany z zapytania timera, czyli mozna wykonac porownanie ostatniego daily checka z oplatami
		public static void Check(bool forceDaily = false)
		{
			// Sprawdz czy potrzeba zrobic daily check
			DailyCheck(forceDaily);
			// Zaanonsuj wymagane budynki
			Announce(); // Wykonywane zawsze kiedy ta funkcja jest odpalana
			Init(); // Startuje timer od nowa
		}

		public static void CheckRessurect()
		{
			QuarterRessurectCheck();
			CalculateAverageAmount();
			InitRessurect(); // Startuje timer od nowa
		}

		public static void CalculateAverageAmount()
		{
			m_average_players = (m_average_players + NetState.Instances.Count) / 2;
		}

		public bool ChargeForBuildings()
		{
			return m_do_charge_daily;
		}

		private static bool ChargeForBuilding()
		{
			return m_do_charge_daily;
		}

		public float ChargeMultipier()
		{
			return m_price_multiplier;
		}

		public static void CalculatePriceMultiplier()
		{
			if (m_average_players < 6)
			{
				m_price_multiplier = 0.4f;
				m_do_charge_daily = false;
			}
			else if (m_average_players < 11)
			{
				m_price_multiplier = 0.5f;
				m_do_charge_daily = false;
			}
			else if (m_average_players < 21)
			{
				m_price_multiplier = 0.6f;
				m_do_charge_daily = false;
			}
			else if (m_average_players < 31)
			{
				m_price_multiplier = 0.8f;
				m_do_charge_daily = true;
			}
			else
			{
				m_price_multiplier = 1.0f;
				m_do_charge_daily = true;
			}
		}

		public static int FullyPaidCycles(Towns town)
		{
			TownManager tmpTown = TownDatabase.GetTown(town);
			int cycles = 666;
			int singleCycle = 0;
			foreach (TownResource tr in tmpTown.Resources.Resources)
			{
				if (tr.DailyChange < 0)
				{
					singleCycle = tr.Amount / -1 * tr.DailyChange;
					cycles = Math.Min(singleCycle, cycles);
				}
			}

			return cycles;
		}

		public static TownResourceType FirstResourceToDrain(Towns town)
		{
			TownManager tmpTown = TownDatabase.GetTown(town);
			TownResourceType resToDrain = TownResourceType.Invalid;
			int cycles = 666;
			int singleCycle = 0;
			if (ChargeForBuilding())
			{
				foreach (TownResource tr in tmpTown.Resources.Resources)
				{
					if (tr.DailyChange < 0)
					{
						singleCycle = tr.Amount / -1 * tr.DailyChange;
						if (cycles > singleCycle)
						{
							cycles = singleCycle;
							resToDrain = tr.ResourceType;
						}
					}
				}
			}

			return resToDrain;
		}

		static void CalculateResourcePricesToBuyAndSell()
		{
			// Ustaw wartosci min max cen

			TownResources resources = new TownResources();
			TownResource resource;
			resources.Resources.Add(new TownResource(TownResourceType.Deski, Utility.RandomMinMax(2, 6),
				Utility.RandomMinMax(100, 130), 0));
			resources.Resources.Add(new TownResource(TownResourceType.Sztaby, Utility.RandomMinMax(2, 7),
				Utility.RandomMinMax(100, 130), 0));
			resources.Resources.Add(new TownResource(TownResourceType.Skora, Utility.RandomMinMax(2, 6),
				Utility.RandomMinMax(100, 130), 0));
			resources.Resources.Add(new TownResource(TownResourceType.Material, Utility.RandomMinMax(2, 5),
				Utility.RandomMinMax(100, 120), 0));
			resources.Resources.Add(new TownResource(TownResourceType.Kosci, Utility.RandomMinMax(2, 6),
				Utility.RandomMinMax(100, 155), 0));
			resources.Resources.Add(new TownResource(TownResourceType.Kamienie, Utility.RandomMinMax(2, 7),
				Utility.RandomMinMax(100, 160), 0));
			resources.Resources.Add(new TownResource(TownResourceType.Piasek, Utility.RandomMinMax(2, 7),
				Utility.RandomMinMax(100, 160), 0));
			resources.Resources.Add(new TownResource(TownResourceType.Klejnoty, Utility.RandomMinMax(2, 8),
				Utility.RandomMinMax(100, 170), 0));
			resources.Resources.Add(new TownResource(TownResourceType.Ziola, Utility.RandomMinMax(2, 5),
				Utility.RandomMinMax(100, 150), 0));
			resources.Resources.Add(new TownResource(TownResourceType.Zbroje,
				resources.Resources.Find(obj => obj.ResourceType == TownResourceType.Sztaby).Amount * 7,
				resources.Resources.Find(obj => obj.ResourceType == TownResourceType.Sztaby).MaxAmount * 7,
				0));
			resources.Resources.Add(new TownResource(TownResourceType.Bronie,
				resources.Resources.Find(obj => obj.ResourceType == TownResourceType.Sztaby).Amount * 6,
				resources.Resources.Find(obj => obj.ResourceType == TownResourceType.Sztaby).MaxAmount * 6,
				0));

			// Wyczysc stare resourcy w handlu
			ForeignResourcesToBuy.RemoveRange(0, ForeignResourcesToBuy.Count);
			ForeignResourcesToSell.RemoveRange(0, ForeignResourcesToSell.Count);
			//Wylosuj nowe wartosci
			for (int i = 0; i < 3; i++)
			{
				do
				{
					resource = new TownResource(
						resources.Resources[Utility.RandomMinMax(0, resources.Resources.Count - 1)].ResourceType, 0, 0,
						0);
					resource.Amount = resources.Resources.Find(obj => obj.ResourceType == resource.ResourceType).Amount;
				} while (ForeignResourcesToBuy.Find(obj => obj.ResourceType == resource.ResourceType) != null);

				ForeignResourcesToBuy.Add(resource);
				do
				{
					resource = new TownResource(
						resources.Resources[Utility.RandomMinMax(0, resources.Resources.Count - 1)].ResourceType, 0, 0,
						0);
					resource.Amount = resources.Resources.Find(obj => obj.ResourceType == resource.ResourceType)
						.MaxAmount;
				} while (ForeignResourcesToSell.Find(obj => obj.ResourceType == resource.ResourceType) != null);

				ForeignResourcesToSell.Add(resource);
			}
		}

		static void QuarterRessurectCheck()
		{
			TownManager tmpTown;

			foreach (Towns tm in TownDatabase.GetTownsNames())
			{
				if (tm != Towns.None)
				{
					tmpTown = TownDatabase.GetTown(tm);
					if (DateTime.Now.CompareTo(tmpTown.LastRessurectTime.AddMinutes(tmpTown.RessurectFrequency)) ==
					    1) //Dodajemy czas wskrzeszenia miasta do daty ostatniego sprawdzenia dla tego miasta i porownujemy z teraz
					{
						foreach (TownPost tp in tmpTown.TownPosts)
						{
							tmpTown.LastRessurectTime = DateTime.Now;
							if (tp.PostStatus ==
							    TownBuildingStatus
								    .Dziala) // Jesli posterunek dziala, sprawdzamy czy straznik musi byc wskrzeszony, w przeciwnym razie nie robimy nic
							{
								if (!tp.IsGuardAlive())
								{
									// Sprawdz czy miasto nie ma za duzo aktywnych posterunkow
									if (!(tmpTown.GetActiveGuards() > tmpTown.MaxGuards))
									{
										// Sprawdz czy miasto nadal posiada budynek odpowiedzialny za straznika
										if (tmpTown.GetAvailableGuards().Contains(tp.TownGuard))
										{
											// Sprawdz czy miasto moze zaplacic za wskrzeszenie straznika
											if (tmpTown.HasResourcesForGuard(tp.TownGuard, TownBuildingStatus.Dziala))
											{
												// Zaplac za straznika i wskrzes
												tmpTown.UseResourcesForGuard(tp.TownGuard, TownBuildingStatus.Dziala);
												tp.RessurectGuard();
											}
											else
											{
												TownDatabase.AddTownLog(tm, TownLogTypes.POSTERUNEK_ZAWIESZONO,
													tp.PostName, 0, 0, 0);
												tp.PostStatus = TownBuildingStatus.Zawieszony;
											}
										}
										else
										{
											TownDatabase.AddTownLog(tm, TownLogTypes.POSTERUNEK_ZAWIESZONO, tp.PostName,
												0, 0, 0);
											tp.PostStatus = TownBuildingStatus.Zawieszony;
										}
									}
									else
									{
										TownDatabase.AddTownLog(tm, TownLogTypes.POSTERUNEK_ZAWIESZONO, tp.PostName, 0,
											0, 0);
										tp.PostStatus = TownBuildingStatus.Zawieszony;
									}
								}
							}
						}
					}
				}
			}
		}

		public static void DailyCheck(bool forceDaily = false)
		{
			// First calculate multipler for today
			CalculatePriceMultiplier();

			TownManager tmpTown;
			bool hasResourcesToPay;
			foreach (Towns tm in TownDatabase.GetTownsNames())
			{
				if (tm != Towns.None)
				{
					tmpTown = TownDatabase.GetTown(tm);
					// Base values - resource 
					// MAX
					tmpTown.Resources.ResourceMaxAmountSet(TownResourceType.Zloto,
						tmpTown.BaseResources.ResourceMaxAmount(TownResourceType.Zloto));
					tmpTown.Resources.ResourceMaxAmountSet(TownResourceType.Deski,
						tmpTown.BaseResources.ResourceMaxAmount(TownResourceType.Deski));
					tmpTown.Resources.ResourceMaxAmountSet(TownResourceType.Sztaby,
						tmpTown.BaseResources.ResourceMaxAmount(TownResourceType.Sztaby));
					tmpTown.Resources.ResourceMaxAmountSet(TownResourceType.Skora,
						tmpTown.BaseResources.ResourceMaxAmount(TownResourceType.Skora));
					tmpTown.Resources.ResourceMaxAmountSet(TownResourceType.Material,
						tmpTown.BaseResources.ResourceMaxAmount(TownResourceType.Material));
					tmpTown.Resources.ResourceMaxAmountSet(TownResourceType.Kosci,
						tmpTown.BaseResources.ResourceMaxAmount(TownResourceType.Kosci));
					tmpTown.Resources.ResourceMaxAmountSet(TownResourceType.Kamienie,
						tmpTown.BaseResources.ResourceMaxAmount(TownResourceType.Kamienie));
					tmpTown.Resources.ResourceMaxAmountSet(TownResourceType.Piasek,
						tmpTown.BaseResources.ResourceMaxAmount(TownResourceType.Piasek));
					tmpTown.Resources.ResourceMaxAmountSet(TownResourceType.Klejnoty,
						tmpTown.BaseResources.ResourceMaxAmount(TownResourceType.Klejnoty));
					tmpTown.Resources.ResourceMaxAmountSet(TownResourceType.Ziola,
						tmpTown.BaseResources.ResourceMaxAmount(TownResourceType.Ziola));
					tmpTown.Resources.ResourceMaxAmountSet(TownResourceType.Zbroje,
						tmpTown.BaseResources.ResourceMaxAmount(TownResourceType.Zbroje));
					tmpTown.Resources.ResourceMaxAmountSet(TownResourceType.Bronie,
						tmpTown.BaseResources.ResourceMaxAmount(TownResourceType.Bronie));
					// Daily change
					tmpTown.Resources.ResourceDailyChangeSet(TownResourceType.Zloto,
						tmpTown.BaseResources.ResourceDailyChange(TownResourceType.Zloto));
					tmpTown.Resources.ResourceDailyChangeSet(TownResourceType.Deski,
						tmpTown.BaseResources.ResourceDailyChange(TownResourceType.Deski));
					tmpTown.Resources.ResourceDailyChangeSet(TownResourceType.Sztaby,
						tmpTown.BaseResources.ResourceDailyChange(TownResourceType.Sztaby));
					tmpTown.Resources.ResourceDailyChangeSet(TownResourceType.Skora,
						tmpTown.BaseResources.ResourceDailyChange(TownResourceType.Skora));
					tmpTown.Resources.ResourceDailyChangeSet(TownResourceType.Material,
						tmpTown.BaseResources.ResourceDailyChange(TownResourceType.Material));
					tmpTown.Resources.ResourceDailyChangeSet(TownResourceType.Kosci,
						tmpTown.BaseResources.ResourceDailyChange(TownResourceType.Kosci));
					tmpTown.Resources.ResourceDailyChangeSet(TownResourceType.Kamienie,
						tmpTown.BaseResources.ResourceDailyChange(TownResourceType.Kamienie));
					tmpTown.Resources.ResourceDailyChangeSet(TownResourceType.Piasek,
						tmpTown.BaseResources.ResourceDailyChange(TownResourceType.Piasek));
					tmpTown.Resources.ResourceDailyChangeSet(TownResourceType.Klejnoty,
						tmpTown.BaseResources.ResourceDailyChange(TownResourceType.Klejnoty));
					tmpTown.Resources.ResourceDailyChangeSet(TownResourceType.Ziola,
						tmpTown.BaseResources.ResourceDailyChange(TownResourceType.Ziola));
					tmpTown.Resources.ResourceDailyChangeSet(TownResourceType.Zbroje,
						tmpTown.BaseResources.ResourceDailyChange(TownResourceType.Zbroje));
					tmpTown.Resources.ResourceDailyChangeSet(TownResourceType.Bronie,
						tmpTown.BaseResources.ResourceDailyChange(TownResourceType.Bronie));

					// Add buildings - resources max and changes
					foreach (TownBuilding tb in TownDatabase.GetBuildingsList(tm))
					{
						if (tb.Status == TownBuildingStatus.Dziala)
						{
							foreach (TownResource tr in tb.Resources.Resources)
							{
								tmpTown.Resources.ResourceMaxAmountIncrease(tr.ResourceType, tr.MaxAmount);
								tmpTown.Resources.ResourceDailyChangeIncrease(tr.ResourceType, tr.DailyChange);
								if (tb.Charge &&
								    ChargeForBuilding()) // Sprawdzamy czy budynek ma byc obciazany kosztem utrzymywania
								{
									tmpTown.Resources.ResourceDailyChangeIncrease(tr.ResourceType,
										(int)(-1 * tr.Amount *
										      0.001)); // Jesli charge ustawiony, oplata za budynek bedzie rowna 1 promil podstawowej oplaty
								}
							}
						}
					}

					// Jesli poprzednia oplata byla wykonana dzien wczesniej
					if (DateTime.Now.CompareTo(m_last_algorithm.AddDays(1)) == 1 ||
					    forceDaily) //Dodajemy jeden dzien do poprzedniej daty i sprawdzamy czy taka data jest wieksza od teraz
					{
						CalculateResourcePricesToBuyAndSell();
						tmpTown.TaxChangeAvailable = true;

						// Ustaw date ostatniego sprawdzania na teraz, oznacza ze nastepuje dzienny pobor oplat
						m_last_algorithm = DateTime.Now;
						CommandHandlers.BroadcastMessage(AccessLevel.GameMaster, 300,
							String.Format("MIASTA: dzienne sprawdzanie oplat miast za budynki {0}",
								DateTime.Now.ToString()));

						foreach (TownBuilding tb in TownDatabase.GetBuildingsList(tm))
						{
							if (tb.Status == TownBuildingStatus.Dziala)
							{
								foreach (TownResource tr in tb.Resources.Resources)
								{
									// Increase amount from buildings
									tmpTown.Resources.ResourceIncreaseAmount(tr.ResourceType, tr.DailyChange);
								}
							}
						}

						if (ChargeForBuilding())
						{
							foreach (TownBuilding tb in TownDatabase.GetBuildingsList(tm))
							{
								if (tb.Status == TownBuildingStatus.Dziala)
								{
									if (tb.Charge)
									{
										hasResourcesToPay = true;
										// Sprawdzamy czy skarbiec posiada wystarczajaca ilosc surowcow do zaplaty
										foreach (TownResource tr in tb.Resources.Resources)
										{
											if (tmpTown.Resources.ResourceAmount(tr.ResourceType) <
											    (int)(1 * tr.Amount * 0.001))
											{
												hasResourcesToPay = false;
												break;
											}
										}

										// Jesli skarbiec nie posiada surowcow ptrzebnych do zaplaty zmiana statusu na zawieszony
										if (!hasResourcesToPay)
										{
											TownDatabase.AddTownLog(tm, TownLogTypes.BUDYNEK_ZAWIESZONO_DZIALANIE, "",
												(int)tb.BuildingType, 0, 0);
											tb.Status = TownBuildingStatus.Zawieszony;
										}
										else
										{
											foreach (TownResource tr in tb.Resources.Resources)
											{
												// Pay fees for buildings
												tmpTown.Resources.ResourceDecreaseAmount(tr.ResourceType,
													(int)(1 * tr.Amount * 0.001));
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}

		static void Announce()
		{
			int m_budynkiWBudowie = 0;
			bool m_wBudowie = false;
			foreach (Towns tm in TownDatabase.GetTownsNames())
			{
				if (tm != Towns.None)
				{
					m_budynkiWBudowie = TownDatabase.GetTown(tm).Buildings
						.Count(obj => obj.Status == TownBuildingStatus.Budowanie);
					// Sprawdz budynki czy sa w budowie
					if (m_budynkiWBudowie > 0)
					{
						m_wBudowie = true;
						// Wyslij informacje na broadcascie i IRC
						if (m_budynkiWBudowie == 1)
						{
							Broadcast(String.Format("MIASTA: Miasto {0} posiada 1 budynek w budowie.", tm.ToString()));
						}
						else if (m_budynkiWBudowie < 5)
						{
							Broadcast(String.Format("MIASTA: Miasto {0} posiada {1} budynki w budowie.", tm.ToString(),
								m_budynkiWBudowie.ToString()));
						}
						else
						{
							Broadcast(String.Format("MIASTA: Miasto {0} posiada {1} budynkow w budowie.", tm.ToString(),
								m_budynkiWBudowie.ToString()));
						}
					}
				}
			}

			if (m_wBudowie)
			{
				Broadcast("Po umieszczeniu budynku w miescie, zakoncz jego budowe w Duszy Miasta.");
			}
		}

		static void Broadcast(string toSend)
		{
			// Broadcast
			CommandHandlers.BroadcastMessage(AccessLevel.GameMaster, 300, toSend);
		}

		static void Init()
		{
			Timer m_timer = new CheckTimer(m_check_interval);
			m_timer.Start();
		}

		static void InitRessurect()
		{
			Timer m_timer_ressurect = new CheckRessurectTimer(m_check_ressurect_interval);
			m_timer_ressurect.Start();
		}
	}

	class CheckTimer : Timer
	{
		public CheckTimer(int delay)
			: base(TimeSpan.FromSeconds(delay))
		{
			Priority = TimerPriority.OneMinute; // Oznacza, ze minimalna dokladnosc tego zegara to minuta
		}

		protected override void OnTick()
		{
			TownAnnouncer.Check();
		}
	}

	class CheckRessurectTimer : Timer
	{
		public CheckRessurectTimer(int delay)
			: base(TimeSpan.FromSeconds(delay))
		{
			Priority = TimerPriority.OneMinute; // Oznacza, ze minimalna dokladnosc tego zegara to minuta
		}

		protected override void OnTick()
		{
			TownAnnouncer.CheckRessurect();
		}
	}
}
