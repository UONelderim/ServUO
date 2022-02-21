#region References

using System;

#endregion

namespace Server.SicknessSys
{
	static class SicknessTime
	{
		private static readonly DateTime WorldStart = new DateTime(1997, 9, 1);

		public const double SecondsPerUOMinute = 5.0;

		public static void GetTime(Map map, int x, int y, out int hours, out int minutes)
		{
			int totalMinutes;

			GetTime(map, x, y, out hours, out minutes, out totalMinutes);
		}

		public static void GetTime(Map map, int x, int y, out int hours, out int minutes, out int totalMinutes)
		{
			var timeSpan = DateTime.UtcNow - WorldStart;

			totalMinutes = (int)(timeSpan.TotalSeconds / SecondsPerUOMinute);

			if (map != null)
			{
				totalMinutes += map.MapIndex * 320;
			}

			totalMinutes += x / 16;

			hours = (totalMinutes / 60) % 24;
			minutes = totalMinutes % 60;
		}

		public static int GetTime(Mobile from)
		{
			int LocalNum;
			string ExactTime;

			GetTime(from, out LocalNum, out ExactTime);

			return LocalNum;
		}

		public static string GetTimeLiteral(Mobile from)
		{
			int LocalNum;
			string ExactTime;

			GetTime(from, out LocalNum, out ExactTime);

			return ExactTime;
		}

		public static void GetTime(Mobile from, out int generalNumber, out string exactTime)
		{
			GetTime(from.Map, from.X, from.Y, out generalNumber, out exactTime);
		}

		public static void GetTime(Map map, int x, int y, out int generalNumber, out string exactTime)
		{
			int hours, minutes;

			GetTime(map, x, y, out hours, out minutes);

			// 00:00 AM - 00:59 AM : Witching hour
			// 01:00 AM - 03:59 AM : Middle of night
			// 04:00 AM - 07:59 AM : Early morning
			// 08:00 AM - 11:59 AM : Late morning
			// 12:00 PM - 12:59 PM : Noon
			// 01:00 PM - 03:59 PM : Afternoon
			// 04:00 PM - 07:59 PM : Early evening
			// 08:00 PM - 11:59 AM : Late at night

			if (hours >= 20)
			{
				generalNumber = 1042957; // It's late at night
			}
			else if (hours >= 16)
			{
				generalNumber = 1042956; // It's early in the evening
			}
			else if (hours >= 13)
			{
				generalNumber = 1042955; // It's the afternoon
			}
			else if (hours >= 12)
			{
				generalNumber = 1042954; // It's around noon
			}
			else if (hours >= 08)
			{
				generalNumber = 1042953; // It's late in the morning
			}
			else if (hours >= 04)
			{
				generalNumber = 1042952; // It's early in the morning
			}
			else if (hours >= 01)
			{
				generalNumber = 1042951; // It's the middle of the night
			}
			else
			{
				generalNumber = 1042950; // 'Tis the witching hour. 12 Midnight.
			}

			hours %= 12;

			if (hours == 0)
			{
				hours = 12;
			}

			exactTime = String.Format("{0}:{1:D2}", hours, minutes);
		}
	}
}
