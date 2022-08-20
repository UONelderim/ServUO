﻿#region References

using System;
using Server;
using Server.Commands;
using Server.Mobiles;

#endregion

namespace Nelderim.Gains
{
	class Gains : NExtension<GainsInfo>
	{
		public static string ModuleName = "Gains";

		private static double GlobalGainFactor { get; set; } = 1.0;

		private static Timer _PhTimer;

		private static DateTime _PhEndDate;

		private static DateTime PhEndDate
		{
			get => _PhEndDate;
			set
			{
				_PhTimer?.Stop();
				_PhEndDate = value;
				if (_PhEndDate > DateTime.Now)
				{
					World.Broadcast(0x40, false,
						$"Power Hour o mnożniku x{GlobalGainFactor} zostało aktywowane do {PhEndDate}.");
					_PhTimer = Timer.DelayCall(_PhEndDate - DateTime.Now, () =>
					{
						GlobalGainFactor = 1.0;
						World.Broadcast(0, false, "PowerHour zakończyło się");
					});
				}
			}
		}

		public static void Initialize()
		{
			CommandSystem.Register("GainInfo", AccessLevel.Player, GainInfo);
			CommandSystem.Register("GainFactor", AccessLevel.GameMaster, GainFactor);
			CommandSystem.Register("PowerHour", AccessLevel.GameMaster, PowerHour);
			EventSink.Login += OnPlayerLogin;
			EventSink.WorldSave += Save;
			Load(ModuleName);
		}

		private static void OnPlayerLogin(LoginEventArgs e)
		{
			if (e.Mobile.NetState != null && GlobalGainFactor != 1.0)
			{
				e.Mobile.SendMessage(0x40, $"Globalny mnożnik gainów wynosi x{GlobalGainFactor}");
				if (PhEndDate > DateTime.Now) e.Mobile.SendMessage(0x40, $"Power Hour aktywne do {PhEndDate}");
			}
		}

		public static double calculateGainFactor(Mobile from)
		{
			var gainFactor = 1.0;
			gainFactor += GlobalGainFactor - 1.0;

			if (from is PlayerMobile pm)
				gainFactor += pm.GainFactor - 1.0;

			return gainFactor;
		}

		private static void GainInfo(CommandEventArgs e)
		{
			if (e.Length == 0)
			{
				e.Mobile.SendMessage($"Globalny mnożnik gainów: x{GlobalGainFactor}");
				if (PhEndDate > DateTime.Now)
					e.Mobile.SendMessage(0x40, $"Power Hour aktywne do {PhEndDate}");
				if (e.Mobile is PlayerMobile pm)
				{
					pm.SendMessage($"Personalny mnożnik gainów: x{pm.GainFactor}");
					if (pm.GainBoostEndTime > DateTime.Now)
						pm.SendMessage(0x40, $"Gain Boost aktywny do {pm.GainBoostEndTime}");
				}

				e.Mobile.SendMessage($"Wynikowy mnożnik gainów: x{calculateGainFactor(e.Mobile)}");
			}
			else
			{
				if (e.Mobile is PlayerMobile pm)
				{
					switch (e.GetString(0))
					{
						case "start":
							pm.GainDebug = true;
							pm.SendMessage(0x40, "Wlaczyles sledzenie przyrostow umiejetnosci.");
							break;
						case "stop":
							pm.GainDebug = false;
							pm.SendMessage(0x20, "Wylaczyles sledzenie przyrostow umiejetnosci.");
							break;
						default:
							pm.SendMessage("Niepoprawny parametr");
							break;
					}
				}
			}
		}

		[Usage("GainFactor [{value}]")]
		[Description("Ustala globalny mnożnik gainów na czas nieokreślony")]
		private static void GainFactor(CommandEventArgs e)
		{
			if (e.Length != 0 && e.Mobile.AccessLevel >= AccessLevel.Administrator)
			{
				var gainFactor = e.GetDouble(0);
				if (gainFactor == 0.0)
					e.Mobile.SendMessage("Niepoprawny parametr");
				else
				{
					PhEndDate = DateTime.Now;
					GlobalGainFactor = gainFactor;
				}
			}

			e.Mobile.SendMessage($"Globalny mnożnik gainów: x{GlobalGainFactor}");
		}

		[Usage("PowerHour {value} [{DurationInMinutes}]")]
		[Description("Ustala globalny modyfikator gainów na czas określony, domyślnie na czas 1h")]
		private static void PowerHour(CommandEventArgs e)
		{
			var duration = TimeSpan.FromHours(1);
			if (e.Length == 0)
			{
				if (PhEndDate > DateTime.Now)
					e.Mobile.SendMessage($"Power Hour o mnożniku x{GlobalGainFactor} jest aktywne do {PhEndDate}");
				else
					e.Mobile.SendMessage("Power Hour jest nieaktywne");
				return;
			}

			var gainFactor = e.GetDouble(0);
			if (e.Length > 1)
				duration = TimeSpan.FromMinutes(e.GetInt32(1));

			if (gainFactor == 0.0 || duration == TimeSpan.Zero)
				e.Mobile.SendMessage("Niepoprawny parametr");
			else
			{
				GlobalGainFactor = gainFactor;
				PhEndDate = DateTime.Now + duration;
			}
		}

		public static void Save(WorldSaveEventArgs args)
		{
			Save(args, ModuleName);
		}
	}
}
