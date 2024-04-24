#region References

using System;
using System.Text.RegularExpressions;
using Nelderim.Configuration;
using Server;
using Server.Items;
using Server.Network;

#endregion

namespace Nelderim.Time
{
	public enum Season
	{
		Spring = 0,
		Summer = 1,
		Autumn = 2,
		Winter = 3,
		Desolation = 4,
		Fall = Autumn
	}
	
	public enum Months // TODO: Change me to 1-7?
	{
		Roztopy = 0,
		Kwiecien = 1,
		Sianokosy = 2,
		Zniwa = 3,
		Orka = 4,
		Listopad = 5,
		Zima = 6
	}

	public enum NDateTimeFormat
	{
		Short = 0,
		LongIs = 1,
		LongWhen = 2
	}

	public class NDateTime
	{
		// Zmienne okreslajace punkt synchronizacji czasu gry z czasem rzeczywistym:
		internal static int SynchPointYear => 2020; // rok czasu rzeczywistego
		internal static int SynchPointMonth => 6; // miesiac czasu rzeczywistego
		internal static int SynchPointDay => 4; // dzien czasu rzeczywistego (ustawiac na czwartek (Roztopy)!!)
		internal static int SynchPointNelYear => 1561; // rok czasu gry

		public static DateTime WorldStart { get; } = new DateTime(SynchPointYear, SynchPointMonth, SynchPointDay);
		public static int DaysInMonth => 24;
		public static int DaysInYear => 7 * DaysInMonth;

		public int Year { get; }
		public Months Month { get; }
		public int Day { get; }
		public int Hour { get; }

		public Season Season => Month switch
		{
			Months.Roztopy => Season.Spring,
			Months.Kwiecien => Season.Spring,
			Months.Sianokosy => Season.Summer,
			Months.Zniwa => Season.Summer,
			Months.Orka => Season.Autumn,
			Months.Listopad => Season.Autumn,
			Months.Zima => Season.Winter,
			_ => Season.Desolation
		};

		public int DayOfYear => ((int)Month * 24) + Day;

		public MoonPhase SmallMoonPhase => Day switch
		{
			var day when day < 6 => MoonPhase.WaxingCrescent,
			var day when day == 6 => MoonPhase.FirstQuarter,
			var day when day < 12 => MoonPhase.WaxingGibbous,
			var day when day == 12 => MoonPhase.Full,
			var day when day < 18 => MoonPhase.WaningGibbous,
			var day when day == 18 => MoonPhase.LastQuarter,
			var day when day < 24 => MoonPhase.WaningCrescent,
			_ => MoonPhase.New
		};

		public MoonPhase LargeMoonPhase => DayOfYear switch
		{
			var day when day < 42 => MoonPhase.WaxingCrescent,
			var day when day == 42 => MoonPhase.FirstQuarter,
			var day when day < 84 => MoonPhase.WaxingGibbous,
			var day when day == 84 => MoonPhase.Full,
			var day when day < 126 => MoonPhase.WaningGibbous,
			var day when day == 126 => MoonPhase.LastQuarter,
			var day when day < 168 => MoonPhase.WaningCrescent,
			_ => MoonPhase.New
		};

		public NDateTime() : this(DateTime.Now)
		{
		}

		public NDateTime(DateTime dateTime)
		{
			Year = (dateTime - (WorldStart)).Days / 7 + SynchPointNelYear;
			Month = (Months)(((int)dateTime.DayOfWeek + 3) % 7);
			Day = dateTime.Hour + 1;
			Hour = dateTime.Minute / 2;
		}

		public NDateTime(int year, int month, int day, int hour)
		{
			Year = Math.Max(year, 0);
			Month = (Months)(Utility.Clamp(month, 0, 6));
			Day = Utility.Clamp(day, 1, 24);
			Hour = Utility.Clamp(hour, 0, 29);
		}

		public string ToString(NDateTimeFormat format = NDateTimeFormat.Short)
		{
			return format switch
			{
				NDateTimeFormat.LongIs => $"Rok {Year}, {Hour} klepsydra {Day} doby sezonu {Month.ToString()}",
				NDateTimeFormat.LongWhen => $"O {Hour} klepsydrze {Day} doby sezonu {Month.ToString()}",
				_ => $"{Year}-{Month}-{Day}:{Hour}"
			};
		}

		public DateTime ToDateTime()
		{
			long ticks = WorldStart.Ticks;
			NDateTime dn = new NDateTime(WorldStart);
			int daysd = ((Year - dn.Year) * 7) + Month - dn.Month;

			ticks += (daysd >= 0) ? TimeSpan.FromDays(daysd).Ticks : -TimeSpan.FromDays(-daysd).Ticks;

			ticks += TimeSpan.FromMinutes((double)Hour * 2).Ticks;
			ticks += TimeSpan.FromHours(Day - 1).Ticks;

			return new DateTime(ticks);
		}

		//YYYY-S-DD:KK
		private static Regex ShortDateTimeRegex = new Regex("([0-9]{1,4})-([0-6])-([1-2]?[0-9]):([1-6]?[0-9])");

		public static bool TryParse(string date, out NDateTime result)
		{
			if (ShortDateTimeRegex.IsMatch(date))
			{
				var parts = ShortDateTimeRegex.Split(date);
				var year = Int32.Parse(parts[0]);
				var month = Int32.Parse(parts[1]);
				var day = Int32.Parse(parts[2]);
				var timeunit = Int32.Parse(parts[3]);
				result = new NDateTime(year, month, day, timeunit);
				return true;
			}

			result = null;
			return false;
		}
	}

	public class ServerTime
	{
		private static NDateTime _CurrentTime;

		public static NDateTime CurrentTime
		{
			get
			{
				if(_CurrentTime == null);
					_CurrentTime = new NDateTime();
				return _CurrentTime;
			}
			private set
			{
				var oldTime = _CurrentTime;
				_CurrentTime = value;
				SetGlobalLight();
				if (oldTime == null || oldTime.Season != _CurrentTime.Season)
				{
					SetGlobalSeason(_CurrentTime.Season);
				}
			}
		}

		public static void Initialize()
		{
			if (NConfig.TimeSystemEnabled)
			{
				if (NDateTime.WorldStart.DayOfWeek != DayOfWeek.Thursday)
				{
					Console.WriteLine("NIEPOPRAWNY DZIEN SYNCHRONIZACJI CZASU SERWERA");
					Console.WriteLine(NDateTime.WorldStart);
					Console.WriteLine(NDateTime.WorldStart.DayOfWeek);
					Console.WriteLine("TO POWINIEN BYC CZWARTEK");
				}
				new ServerTimeTimer().Start();
			}
		}

		public static int Year => CurrentTime.Year;
		public static int Month => (int)CurrentTime.Month;
		public static Months MonthName => CurrentTime.Month;
		public static int Day => CurrentTime.Day;
		public static int DayOfYear => CurrentTime.DayOfYear;
		public static int Hour => CurrentTime.Hour;
		public static Season Season => CurrentTime.Season;
		public static MoonPhase LargeMoonPhase => CurrentTime.LargeMoonPhase;
		public static MoonPhase SmallMoonPhase => CurrentTime.SmallMoonPhase;

		// Parabola function,
		// Roots are approx 0 and 30,
		// Max is 26 at 15
		public static double SunLight => -0.115 * Math.Pow(Hour - 15, 2) + 26;

		// Modulus function
		// Roots are approx 0 and 24
		// Max is 2 at 12
		public static double SmallerMoon => -Math.Abs(-Day + NDateTime.DaysInMonth / 2) * 1.165 + 2;

		// Modulus function
		// Roots are approx 0 and 168
		// Max is 4 at 84
		public static double LargerMoon => -Math.Abs(-DayOfYear + NDateTime.DaysInYear / 2) * 0.0476 + 4;

		public static int LightLevel => (int)Math.Max(SunLight, LargerMoon + SmallerMoon);


		private static void SetSeason(Mobile m, Season season)
		{
			SeasonChange.Send(m.NetState, true);
		}

		public static void SetGlobalSeason(Season season)
		{
			try
			{
				foreach (var map in Map.AllMaps)
				{
					map.Season = (int)season;
				}

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
			int Message = Hour switch
			{
				0 => 505469,
				5 => 505470,
				10 => 505471,
				15 => 505472,
				20 => 505473,
				25 => 505474,
				_ => -1
			};

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

		public static void GetTime(out int localizedNumber, out string exactTime)
		{
			if (Hour <= 2)
				localizedNumber = 1042950; //Slonce dopiero wstalo
			else if (Hour <= 8)
				localizedNumber = 1042951; //Jest wczesnie rano
			else if (Hour <= 10)
				localizedNumber = 1042952; //Zbliza sie poludnie
			else if (Hour <= 12)
				localizedNumber = 1042953; //Jest wczesne popoludnie
			else if (Hour <= 18)
				localizedNumber = 1042954; //Jest popoludnie
			else if (Hour <= 22)
				localizedNumber = 1042955; //Jest wieczor
			else if (Hour <= 25)
				localizedNumber = 1042956; //Jest wczesna noc
			else if (Hour <= 27)
				localizedNumber = 1042957; //Jest pozna noc
			else
				localizedNumber = 1042958; //Nadchodzi swit
			exactTime = $", a dokladniej, jest {Hour} klepsydra";
		}

		public static void GetTime(out int hours, out int minutes, out int totalMinutes)
		{
			hours = Day;
			minutes = Hour * 2;
			totalMinutes = hours * 60 + minutes;
		}

		private class ServerTimeTimer : Timer
		{
			public ServerTimeTimer() : base(TimeSpan.Zero, TimeSpan.FromMinutes(2))
			{
				CurrentTime = new NDateTime();
				Priority = TimerPriority.FiveSeconds;
			}

			protected override void OnTick()
			{
				CurrentTime = new NDateTime();
			}
		}
	}
}
