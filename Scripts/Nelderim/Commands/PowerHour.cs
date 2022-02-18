#region References

using System;
using Server.Mobiles;

#endregion

namespace Server.Commands
{
	public class PowerHour
	{
		public static TimeSpan Duration = TimeSpan.FromHours(1.0);

		public static void Initialize()
		{
			CommandSystem.Register("PowerHour", AccessLevel.Player, PowerHour_OnCommand);
		}

		[Usage("PowerHour <start|czas>")]
		[Description(
			"start - rozpoczyna godzine efektywnego treningu, czas - podaje informacje o mozliwosci uzycia PowerHour w biezacym dniu. Pokazuje czas do konca uplywu PowerHour.")]
		public static void PowerHour_OnCommand(CommandEventArgs e)
		{
			Mobile m = e.Mobile;
			if (m is PlayerMobile)
			{
				PlayerMobile pm = (PlayerMobile)m;

				if (e.Length > 0 && e.GetString(0) == "start" && pm.AllowPowerHour)
				{
					pm.LastPowerHour = DateTime.Now;

					pm.SendMessage(0x40,
						"Rozpoczales PowerHour! Przez najblizsze 60 minut umiejetnosci i statystyki postaci beda rosnac szybciej.");
				}
				else
				{
					if (pm.HasPowerHour)
						pm.SendMessage(0x40,
							"Wlasnie trwa PowerHour! Przez kolejne {0} minut umiejetnosci i statystyki postaci beda rosnac szybciej.",
							(int)Math.Ceiling(Duration.TotalMinutes - (DateTime.Now - pm.LastPowerHour).TotalMinutes));
					else if (pm.AllowPowerHour)
						pm.SendMessage(0x30,
							"Nie wykorzystales jeszcze PowerHour w tym dniu. Aby to zrobic uzyj komendy [PowerHour start.");
					else
						pm.SendMessage(0x30, "Wykorzystales juz dzis PowerHour.");
				}
			}
		}
	}
}
