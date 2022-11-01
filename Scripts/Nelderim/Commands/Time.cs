using System;
using System.Globalization;
using Server;
using Server.Commands;

namespace Nelderim.Time
{
	public class Time
	{
		public static void Initialize()
		{
			CommandSystem.Register("TimeGet", AccessLevel.Player, TimeGet_OnCommand);
			CommandSystem.Register("TimeConvert", AccessLevel.Player, TimeConvert_OnCommand);
			CommandSystem.Register("TimeMonths", AccessLevel.Player, TimeMonths_OnCommand);
			// CommandSystem.Register("TimeSetSeason", AccessLevel.Seer, TimeSeason_OnCommand);
		}
		

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
				from.SendMessage(67, "Obslugiwane formaty:");
				from.SendMessage("YYYY-MM-DD HH:MM (czas rzeczywisty -> czas Nelderim)");
				from.SendMessage("YYYY-S-DD:KK (czas Nelderim -> czas rzeczywisty)");
			}

			try
			{
				if (DateTime.TryParse(arg, out DateTime d))
				{
					from.SendMessage($"{arg} -> {new NDateTime(d).ToString()}");
				} 
				else if(NDateTime.TryParse(arg, out NDateTime nd))
				{
					from.SendMessage($"{arg} -> {nd.ToDateTime().ToString(CultureInfo.InvariantCulture)}");
				}
				else
				{
					from.SendMessage(38, "Blad konwersji!");
				}
			}
			catch (Exception exc)
			{
				Console.WriteLine(exc.ToString());
				from.SendMessage("Nastapil blad polecenia! Zglos go Ekipie!");
			}
		}
		
		[Usage("TimeGet")]
		[Description("Podaje dokladny czas Nelderim.")]
		private static void TimeGet_OnCommand(CommandEventArgs e)
		{
			e.Mobile.SendMessage(ServerTime.CurrentTime.ToString(NDateTimeFormat.LongIs));
		}
		
		// [Usage("TimeSeason spring | summer | fall | winter | desolation")]
		// [Description("Zmienia pore roku.")]
		// private static void TimeSeason_OnCommand(CommandEventArgs e)
		// {
		// 	Mobile m = e.Mobile;
		//
		// 	if (e.Length == 1)
		// 	{
		// 		string seasonType = e.GetString(0).ToLower();
		// 		Season season;
		//
		// 		try
		// 		{
		// 			season = (Season)Enum.Parse(typeof(Season), seasonType, true);
		// 		}
		// 		catch
		// 		{
		// 			m.SendMessage("Usage: Season spring | summer | fall | winter | desolation");
		// 			return;
		// 		}
		//
		// 		m.SendMessage("Pora roku: " + seasonType + ".");
		//
		// 		Map.Felucca.Season = (int)season;
		// 		Map.Trammel.Season = (int)season;
		// 		Map.Ilshenar.Season = (int)season;
		// 		Map.Malas.Season = (int)season;
		//
		// 		SetGlobalSeason(season);
		// 	}
		// 	else
		// 	{
		// 		m.SendMessage("Usage: Season spring | summer | fall | winter | desolation");
		// 	}
		// }
	}
}
