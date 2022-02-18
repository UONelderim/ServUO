#region References

using System;
using System.Xml;
using Server.Commands;
using Server.Items;
using Server.Network;

#endregion

namespace Server
{
	public enum SeasonList
	{
		Spring,
		Summer,
		Fall,
		Winter,
		Desolation
	}

	public enum MonthsList
	{
		Roztopy = 0,
		Kwiecien = 1,
		Sianokosy = 2,
		Zniwa = 3,
		Orka = 4,
		Listopad = 5,
		Zima = 6,
	}

	public enum NelderimDateTimeFormat
	{
		Short = 0,
		LongIs = 1,
		LongWhen = 2
	}

	public class NelderimDateTime
	{
		#region Fields

		// Zmienne okreslajace punkt synchronizacji czasu gry z czasem rzeczywistym:
		public static int SynchPointYear { get { return 2020; } } // rok czasu rzeczywistego
		public static int SynchPointMonth { get { return 06; } } // miesiac czasu rzeczywistego

		public static int
			SynchPointDay { get { return 05; } } // dzien czasu rzeczywistego (ustawiac na czwartek (Roztopy)!!)

		public static int SynchPointNelYear { get { return 1561; } } // rok czasu gry

		#endregion

		#region Properities

		public static NelderimDateTime Now { get { return new NelderimDateTime(); } }
		public static DateTime WorldStart { get; } = new DateTime(SynchPointYear, SynchPointMonth, SynchPointDay);
		public int Year { get; }
		public int Month { get; }
		public int Day { get; }
		public int TimeUnit { get; }

		#endregion

		#region Constructors

		public NelderimDateTime()
		{
			DateTime CurrentTime = DateTime.Now;
			TimeUnit = CurrentTime.Minute / 2;
			Day = CurrentTime.Hour + 1;
			int day = ((int)CurrentTime.DayOfWeek);
			Month = (day >= 4) ? day - 4 : day + 3;
			Year = (CurrentTime - (WorldStart)).Days / 7 + SynchPointNelYear;
		}

		public NelderimDateTime(DateTime CurrentTime)
		{
			TimeUnit = CurrentTime.Minute / 2;
			Day = CurrentTime.Hour + 1;
			int day = ((int)CurrentTime.DayOfWeek);
			Month = (day >= 4) ? day - 4 : day + 3;
			Year = (CurrentTime - (WorldStart)).Days / 7 + SynchPointNelYear;
		}

		public NelderimDateTime(int day, int unit)
		{
			DateTime CurrentTime = DateTime.Now;
			TimeUnit = (unit < 0) ? 0 : ((unit > 29) ? 29 : unit);
			Day = (day < 1) ? 1 : ((day > 24) ? 24 : day);
			int realday = ((int)CurrentTime.DayOfWeek);
			Month = (realday >= 4) ? realday - 4 : realday + 3;
			Year = (CurrentTime - (WorldStart)).Days / 7 + SynchPointNelYear;
		}

		public NelderimDateTime(int month, int day, int unit)
		{
			DateTime CurrentTime = DateTime.Now;
			TimeUnit = (unit < 0) ? 0 : ((unit > 29) ? 29 : unit);
			Day = (day < 1) ? 1 : ((day > 24) ? 24 : day);
			Month = (month < 0) ? 0 : ((month > 6) ? 6 : month);
			Year = (CurrentTime - (WorldStart)).Days / 7 + SynchPointNelYear;
		}

		public NelderimDateTime(int year, int month, int day, int unit)
		{
			TimeUnit = (unit < 0) ? 0 : ((unit > 29) ? 29 : unit);
			Day = (day < 1) ? 1 : ((day > 24) ? 24 : day);
			Month = (month < 0) ? 0 : ((month > 6) ? 6 : month);
			Year = (year < 0) ? 0 : year;
		}

		public NelderimDateTime(GenericReader reader)
		{
			int version = reader.ReadInt();
			TimeUnit = reader.ReadInt();
			Day = reader.ReadInt();
			Month = reader.ReadInt();
			Year = reader.ReadInt();

			TimeUnit = (TimeUnit < 0) ? 0 : ((TimeUnit > 29) ? 29 : TimeUnit);
			Day = (Day < 1) ? 1 : ((Day > 24) ? 24 : Day);
			Month = (Month < 0) ? 0 : ((Month > 6) ? 6 : Month);
			Year = (Year < 0) ? 0 : Year;
		}

		public void Serialize(GenericWriter writer)
		{
			writer.Write(0); // version
			writer.Write(TimeUnit);
			writer.Write(Day);
			writer.Write(Month);
			writer.Write(Year);
		}

		#endregion

		public override string ToString()
		{
			return ToString(NelderimDateTimeFormat.Short);
		}

		public string ToString(NelderimDateTimeFormat format)
		{
			#region Short

			if (format == NelderimDateTimeFormat.Short)
				return Year + "-" + Month + "-" + Day + ":" + TimeUnit;

			#endregion

			#region LongIs
			if (format == NelderimDateTimeFormat.LongIs)
			{
				string date = "rok " + Year + ", " + TimeUnit + " klepsydra " + Day + " doby sezonu ";

				switch (Month)
				{
					case 0:
						return date + "Roztopy";
					case 1:
						return date + "Kwiecien";
					case 2:
						return date + "Sianokosy";
					case 3:
						return date + "Zniwa";
					case 4:
						return date + "Orka";
					case 5:
						return date + "Listopad";
					case 6:
						return date + "Zima";
				}

				return date + "nieznanego";
			}

			#endregion

			#region LongWhen

			if (format == NelderimDateTimeFormat.LongWhen)
			{
				string date = "o " + TimeUnit + " klepsydrze " + Day + " doby sezonu ";

				switch (Month)
				{
					case 0:
						date += "Roztopow ";
						break;
					case 1:
						date += "Kwietnia ";
						break;
					case 2:
						date += "Sianokosow ";
						break;
					case 3:
						date += "Zniw ";
						break;
					case 4:
						date += "Orki ";
						break;
					case 5:
						date += "Listopada ";
						break;
					case 6:
						date += "Zimy ";
						break;
					default:
						date += "nieznanego ";
						break;
				}

				return date + Year + " roku";
			}

			#endregion

			return "0-0-0:00";
		}

		public static NelderimDateTime Transform(DateTime date)
		{
			return new NelderimDateTime(date);
		}

		public static DateTime Convert(NelderimDateTime date)
		{
			long ticks = WorldStart.Ticks;
			NelderimDateTime dn = new NelderimDateTime(WorldStart);
			int daysd = ((date.Year - dn.Year) * 7) + date.Month - dn.Month;

			ticks += (daysd >= 0) ? TimeSpan.FromDays(daysd).Ticks : -TimeSpan.FromDays(-daysd).Ticks;

			ticks += TimeSpan.FromMinutes((double)date.TimeUnit * 2).Ticks;
			ticks += TimeSpan.FromHours(date.Day - 1).Ticks;

			return new DateTime(ticks);
		}

		public static NelderimDateTime Parse(string date)
		{
			int year = 0;
			int month = 0;
			int day = 0;
			int timeunit = 0;

			year = XmlConvert.ToInt32(date.Substring(0, 4));
			month = XmlConvert.ToInt32(date.Substring(5, 1));

			if (date.Length > 6)
				day = XmlConvert.ToInt32(date.Substring(7, 2));

			if (date.Length > 10)
				timeunit = XmlConvert.ToInt32(date.Substring(10, 2));

			return new NelderimDateTime(year, month, day, timeunit);
		}

		public static string TransformToString(DateTime date)
		{
			return TransformToString(date, NelderimDateTimeFormat.Short);
		}

		public static string TransformToString(DateTime date, NelderimDateTimeFormat format)
		{
			NelderimDateTime ndate = new NelderimDateTime(date);

			return ndate.ToString(format);
		}
	}

	public class ServerTime
	{
		private static int m_TimeUnit = 10;
		private static int m_Day;
		private static MonthsList m_Month;

		static ServerTime()
		{
			Console.WriteLine("System: Uruchamianie Serwera Czasu...Gotowe");
			SynchronizeTime();
			//EvaluateGlobalLight();
		}

		public static void Initialize()
		{
			ServerStart = DateTime.Now;

			new ServerTimeTimer().Start();
			new ServerTimeSynchronizationTimer().Start();

			CommandSystem.Register("TimeSynchronize", AccessLevel.GameMaster, TimeSynchronize_OnCommand);
			CommandSystem.Register("TimeGet", AccessLevel.Player, TimeGet_OnCommand);
			CommandSystem.Register("TimeSet", AccessLevel.Administrator, TimeSet_OnCommand);
			CommandSystem.Register("TimeSeason", AccessLevel.Seer, TimeSeason_OnCommand);
			CommandSystem.Register("TimeStop", AccessLevel.Seer, TimeStop_OnCommand);
			CommandSystem.Register("TimeConvert", AccessLevel.Player, TimeConvert_OnCommand);
			CommandSystem.Register("TimeMonths", AccessLevel.Player, TimeMonths_OnCommand);
		}

		private static void SynchronizeTime()
		{
			if (!StopTimer)
			{
				DateTime CurrentTime = DateTime.Now;
				m_TimeUnit = CurrentTime.Minute / 2;
				m_Day = CurrentTime.Hour + 1;
				int day = ((int)CurrentTime.DayOfWeek);
				m_Month = (MonthsList)((day >= 4) ? day - 4 : day + 3);
				Year = (CurrentTime - (new DateTime(NelderimDateTime.SynchPointYear, NelderimDateTime.SynchPointMonth,
					NelderimDateTime.SynchPointDay))).Days / 7 + NelderimDateTime.SynchPointNelYear;
				//SeasonFromMonth(m_Month);
				Console.WriteLine("System: Synchronizacja czasu: {0}-{1}-{2}:{3}", Year, ((int)m_Month), m_Day,
					m_TimeUnit);
			}
			else
			{
				Console.WriteLine("Zegar Nelderim jest wylaczony. Wlacz zegar.");
				//e.Mobile.SendMessage( "Zegar Nelderim jest wylaczony. Wlacz zegar." );
				SeasonFromMonth(0);
			}
		}

		#region Commands

		[Usage("TimeMonths")]
		[Description("Wylicza miesiace Nelderim.")]
		private static void TimeMonths_OnCommand(CommandEventArgs e)
		{
			Mobile m = e.Mobile;

			m.SendMessage("Miesiace Nelderim: ");
			m.SendMessage("0. Roztopy (czwartek)");
			m.SendMessage("1. Kwiecien (piatek)");
			m.SendMessage("2. Sianokosy (sobota)");
			m.SendMessage("3. Zniwa (niedziela)");
			m.SendMessage("4. Orka (poniedzialek)");
			m.SendMessage("5. Listopad (wtorek)");
			m.SendMessage("6. Zima (sroda)");
		}

		[Usage("TimeConvert [data]")]
		[Description("Konwertuje daty")]
		private static void TimeConvert_OnCommand(CommandEventArgs e)
		{
			Mobile from = e.Mobile;
			string arg = e.ArgString;

			if (arg.Length < 1)
			{
				from.SendMessage("[TimeConvert [data]");
				from.SendMessage("[data] powinna byc w formacje:");
				from.SendMessage("yyyy-mm-dd hh:mm (dla konwersji czas rzeczywisty <-> czas Nelderim)");
				from.SendMessage("yyyy-s-dd:kk (dla konwersji czas Nelderim <-> czas rzeczywisty)");
				from.SendMessage("sezon (slowo w mianowniku, by uzyskac odpowiedni numer)");
				from.SendMessage(38, "nalezy conajmniej podac rok i sezon");
			}

			try
			{
				string convdate = "blad konwersji";

				try
				{
					DateTime d = DateTime.Parse(arg);
					convdate = new NelderimDateTime(d).ToString();
					from.SendMessage("{0} -> {1}", arg, convdate);
					return;
				}
				catch
				{
				}

				try
				{
					NelderimDateTime d = NelderimDateTime.Parse(arg);
					convdate = NelderimDateTime.Convert(d).ToString();
					from.SendMessage("{0} -> {1}", arg, convdate);
					return;
				}
				catch
				{
				}

				from.SendMessage("{0} -> {1}", arg, convdate);
			}
			catch (Exception exc)
			{
				Console.WriteLine(exc.ToString());
				from.SendMessage("Nastapil blad polecenia! Zgos go Ekipie!");
			}
		}

		[Usage("TimeStop")]
		[Description("Zatrzymuje / startuje zegar Nelderim")]
		private static void TimeStop_OnCommand(CommandEventArgs e)
		{
			StopTimer = (StopTimer) ? false : true;

			if (StopTimer)
				e.Mobile.SendMessage("Czas Nelderim stoi.");
			else
				e.Mobile.SendMessage("Czas Nelderim ruszyl. Zsynchronizuj zegar!");
		}

		[Usage("TimeSynchronize")]
		[Description("Synchronizuje czas Nelderim z czasem rzeczywistym.")]
		private static void TimeSynchronize_OnCommand(CommandEventArgs e)
		{
			e.Mobile.SendMessage("I'm synchronizing time...");
			SynchronizeTime();
			SetGlobalLight();
		}

		[Usage("TimeGet")]
		[Description("Podaje dokladny czas Nelderim.")]
		private static void TimeGet_OnCommand(CommandEventArgs e)
		{
			e.Mobile.SendMessage("Jest rok {0}, {1} klepsydra {2} doby sezonu {3}.", Year, m_TimeUnit, m_Day, m_Month);
		}

		[Usage("TimeSet [klepsydra [doba [sezon [rok]]]]")]
		[Description("Ustala czas Nelderim.")]
		private static void TimeSet_OnCommand(CommandEventArgs arg)
		{
			try
			{
				if (arg.Length < 1)
				{
					SynchronizeTime();
				}
				else if (arg.Length > 0)
				{
					TimeUnit = arg.GetInt32(0);

					if (arg.Length > 1)
					{
						Day = arg.GetInt32(1);

						if (arg.Length > 2)
						{
							Month = (MonthsList)arg.GetInt32(2);

							if (arg.Length > 3)
							{
								Year = arg.GetInt32(3);
							}
						}
					}
				}

				//LightCycle.SetGlobalLight();
				arg.Mobile.SendMessage("Data ustawiona na rok {0}, {1} klepsydra {2} doby sezonu {3}.", Year,
					m_TimeUnit, m_Day, m_Month);
			}
			catch (Exception exc)
			{
				Console.WriteLine(exc.ToString());
				arg.Mobile.SendMessage("Fatalny bd!");
			}
		}

		[Usage("TimeSeason spring | summer | fall | winter | desolation")]
		[Description("Zmienia pore roku.")]
		private static void TimeSeason_OnCommand(CommandEventArgs e)
		{
			Mobile m = e.Mobile;

			if (e.Length == 1)
			{
				string seasonType = e.GetString(0).ToLower();
				SeasonList season;

				try
				{
					season = (SeasonList)Enum.Parse(typeof(SeasonList), seasonType, true);
				}
				catch
				{
					m.SendMessage("Usage: Season spring | summer | fall | winter | desolation");
					return;
				}

				m.SendMessage("Pora roku: " + seasonType + ".");

				Map.Felucca.Season = (int)season;
				Map.Trammel.Season = (int)season;
				Map.Ilshenar.Season = (int)season;
				Map.Malas.Season = (int)season;

				SetGlobalSeason(season);
			}
			else
			{
				m.SendMessage("Usage: Season spring | summer | fall | winter | desolation");
			}
		}

		#endregion

		#region Properites

		public static DateTime WorldStart { get; set; } = new DateTime(NelderimDateTime.SynchPointYear,
			NelderimDateTime.SynchPointMonth, NelderimDateTime.SynchPointDay);

		public static bool StopTimer { get; set; }

		public static DateTime ServerStart { get; private set; }

		public static int TimeUnit
		{
			get
			{
				return m_TimeUnit;
			}
			set
			{
				m_TimeUnit = value;
				if (TimeUnit >= 30 || m_TimeUnit < 0)
				{
					m_TimeUnit = 0;
					Day++;
				}
				//LightCycle.SetGlobalLight();
			}
		}

		public static int Day
		{
			get
			{
				return m_Day;
			}
			set
			{
				m_Day = value;
				if (m_Day > 24 || m_Day < 1)
				{
					m_Day = 1;
					Month++;
				}
			}
		}

		public static MonthsList Month
		{
			get
			{
				return m_Month;
			}
			set
			{
				m_Month = value;

				if (((int)m_Month) > 6)
				{
					m_Month = 0;
					Year++;
				}

				SeasonFromMonth(m_Month);
			}
		}

		public static int Year { get; set; }

		public static SeasonList Season
		{
			get
			{
				switch ((int)m_Month)
				{
					case 2:
					case 3: return SeasonList.Summer;
					case 4:
					case 5: return SeasonList.Fall;
					case 6: return SeasonList.Winter;
					case 0:
					case 1: return SeasonList.Spring;
					default: return SeasonList.Desolation;
				}
			}
		}

		#endregion

		public static string GetMoonPhase(bool LargerMoon)
		{
			if (LargerMoon)
			{
				int yDay = (24 * ((int)Month)) + (Day);

				if (yDay < 42)
					return "Przybywa polksiezyca";
				if (yDay == 42)
					return "Pierwsza kwarta";
				if (yDay < 84)
					return "Przybywa do pelni";
				if (yDay == 84)
					return "Jest w pelni";
				if (yDay < 126)
					return "Ubywa do plksiezyca";
				if (yDay == 126)
					return "Trzecia kwarta";
				if (yDay < 168)
					return "Ubywa do nowiu";
				return "Jest w nowiu";
			}

			if (Day < 6)
				return "Przybywa polksiezyca";
			if (Day == 6)
				return "Pierwsza kwarta";
			if (Day < 12)
				return "Przybywa do pelni";
			if (Day == 12)
				return "Jest w pelni";
			if (Day < 18)
				return "Ubywa do polksiezyca";
			if (Day == 18)
				return "Trzecia kwarta";
			if (Day < 24)
				return "Ubywa do nowiu";
			return "Jest w nowiu";
		}

		public static MoonPhase GetEnumMoonPhase(bool LargerMoon)
		{
			if (LargerMoon)
			{
				int yDay = (24 * ((int)Month)) + (Day);

				if (yDay < 42)
					return MoonPhase.WaxingCrescent;
				if (yDay == 42)
					return MoonPhase.FirstQuarter;
				if (yDay < 84)
					return MoonPhase.WaxingGibbous;
				if (yDay == 84)
					return MoonPhase.Full;
				if (yDay < 126)
					return MoonPhase.WaningGibbous;
				if (yDay == 126)
					return MoonPhase.LastQuarter;
				if (yDay < 168)
					return MoonPhase.WaningCrescent;
				return MoonPhase.New;
			}

			if (Day < 6)
				return MoonPhase.WaxingCrescent;
			if (Day == 6)
				return MoonPhase.FirstQuarter;
			if (Day < 12)
				return MoonPhase.WaxingGibbous;
			if (Day == 12)
				return MoonPhase.Full;
			if (Day < 18)
				return MoonPhase.WaningGibbous;
			if (Day == 18)
				return MoonPhase.LastQuarter;
			if (Day < 24)
				return MoonPhase.WaningCrescent;
			return MoonPhase.New;
		}

		public static void SeasonFromMonth(MonthsList month)
		{
			switch ((int)month)
			{
				case 2:
				case 3:
				{
					Map.Felucca.Season = 1;
					Map.Trammel.Season = 1;
					Map.Ilshenar.Season = 1;
					Map.Malas.Season = 1;
					SetGlobalSeason(SeasonList.Summer);
				}
					break;
				case 4:
				case 5:
				{
					Map.Felucca.Season = 2;
					Map.Trammel.Season = 2;
					Map.Ilshenar.Season = 2;
					Map.Malas.Season = 2;
					SetGlobalSeason(SeasonList.Fall);
				}
					break;
				case 6:
				{
					Map.Felucca.Season = 3;
					Map.Trammel.Season = 3;
					Map.Ilshenar.Season = 3;
					Map.Malas.Season = 3;
					SetGlobalSeason(SeasonList.Winter);
				}
					break;
				case 0:
				case 1:
				{
					Map.Felucca.Season = 0;
					Map.Trammel.Season = 0;
					Map.Ilshenar.Season = 0;
					Map.Malas.Season = 0;
					SetGlobalSeason(SeasonList.Spring);
				}
					break;
			}
		}

		public static void SetSeason(Mobile m, SeasonList season)
		{
			SeasonChange.Send(m.NetState, true);
		}

		public static void SetGlobalSeason(SeasonList season)
		{
			try
			{
				for (int i = 0; i < NetState.Instances.Count; ++i)
				{
					NetState ns = NetState.Instances[i];
					Mobile m = ns.Mobile;

					if (m != null)
					{
						SetSeason(m, season);
						m.CheckLightLevels(false);
					}
				}
			}
			catch (Exception exc)
			{
				Console.WriteLine(exc.ToString());
			}
		}

		public static void SetGlobalLight()
		{
			int Message = -1;
			//EvaluateGlobalLight();

			switch (TimeUnit)
			{
				case 0:
					Message = 505469;
					break;
				case 5:
					Message = 505470;
					break;
				case 10:
					Message = 505471;
					break;
				case 15:
					Message = 505472;
					break;
				case 20:
					Message = 505473;
					break;
				case 25:
					Message = 505474;
					break;
			}


			for (int i = 0; i < NetState.Instances.Count; ++i)
			{
				NetState ns = NetState.Instances[i];
				Mobile m = ns.Mobile;

				if (m != null)
				{
					if (Message != -1) m.SendLocalizedMessage(Message, "", 0x58);
				}
			}
		}

		#region Subclassess

		private class ServerTimeTimer : Timer
		{
			public ServerTimeTimer() : base(TimeSpan.FromMinutes(2), TimeSpan.FromMinutes(2))
			{
				Priority = TimerPriority.TwoFiftyMS;
			}

			protected override void OnTick()
			{
				if (!StopTimer)
					TimeUnit++;
			}
		}

		public class ServerTimeSynchronizationTimer : Timer
		{
			public ServerTimeSynchronizationTimer() : base(TimeSpan.FromMinutes(120), TimeSpan.FromMinutes(120))
			{
				Priority = TimerPriority.OneMinute;
			}

			protected override void OnTick()
			{
				int minutes = DateTime.Now.Minute;
				int currentTimer = minutes / 2;

				if ((TimeUnit != currentTimer) && !StopTimer)
				{
					Console.WriteLine("System: Korekcja czasu...");
					SynchronizeTime();
				}
			}
		}

		#endregion
	}
}
