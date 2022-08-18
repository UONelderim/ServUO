#region References

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
		
		private static double _GlobalGainFactor = 1.0;
		
		public static double GlobalGainFactor
		{
			get => _GlobalGainFactor;
			set => _GlobalGainFactor = value;
		}

		private static DateTime PHEndDate;
		private static Timer PHTimer;


		public static void Initialize()
		{
			CommandSystem.Register("GainFactor", AccessLevel.Player, GainFactor);
			CommandSystem.Register("PowerHour", AccessLevel.GameMaster, PowerHour);
			CommandSystem.Register("PH", AccessLevel.GameMaster, PowerHour);
			EventSink.WorldSave += Save;
			Load(ModuleName);
		}
		
		public static double calculateGainFactor(Mobile from)
		{
			var gainFactor = 1.0;
			gainFactor += GlobalGainFactor - 1.0;
			
			if (from is PlayerMobile pm)
				gainFactor += pm.GainFactor - 1.0;

			return gainFactor;
		}

		[Usage("GainFactor [{value}]")]
		[Description("Ustala globalny modyfikator gainów na czas nieokreślony")]
		private static void GainFactor(CommandEventArgs e)
		{
			if (e.Length != 0 && e.Mobile.AccessLevel >= AccessLevel.Administrator)
			{
				var gainFactor = e.GetDouble(0);
				if (gainFactor == 0.0)
					e.Mobile.SendMessage("Niepoprawny parametr");
				else
				{
					if (PHTimer != null)
						PHTimer.Stop();
					GlobalGainFactor = gainFactor;
				}
			}
			e.Mobile.SendMessage($"Globalny mnożnik gainów: {GlobalGainFactor}");
			if(e.Mobile is PlayerMobile pm)
				pm.SendMessage($"Personalny mnożnik gainów: {pm.GainFactor}");
			e.Mobile.SendMessage($"Wynikowy mnożnik gainów: {calculateGainFactor(e.Mobile)}");
		}

		[Usage("globalPH {value} [{DurationInMinutes}]")]
		[Description("Ustala globalny modyfikator gainów na czas określony, domyślnie na czas 1h")]
		private static void PowerHour(CommandEventArgs e)
		{
			var duration = TimeSpan.FromHours(1);
			if (e.Length == 0)
			{
				if (PHTimer != null && PHTimer.Running)
					e.Mobile.SendMessage(
						$"Globalne PowerHour o mnożnku x{GlobalGainFactor} jest aktywne do {PHEndDate}");
				else
					e.Mobile.SendMessage("Global PowerHour jest nieaktywne");
				return;
			}

			var gainFactor = e.GetDouble(0);
			if(e.Length > 1)
				duration = TimeSpan.FromMinutes(e.GetInt32(1));
			
			if (gainFactor == 0.0 || duration == TimeSpan.Zero)
					e.Mobile.SendMessage("Niepoprawny parametr");
			else
			{
				var oldGainFactor = GlobalGainFactor;
				if (PHTimer != null)
					PHTimer.Stop();
				GlobalGainFactor = gainFactor;
				PHEndDate = DateTime.Now + duration;
				PHTimer = Timer.DelayCall(duration, () =>
				{
					GlobalGainFactor = oldGainFactor;
					World.Broadcast(0, false, "Globalne PowerHour zakończyło się");
				});
				World.Broadcast(0, false, $"Globalne PowerHour o mnozniku x{GlobalGainFactor} zostalo aktywowane do {PHEndDate}." );
			}
		}

		public static void Save(WorldSaveEventArgs args)
		{
			Save(args, ModuleName);
		}
	}
}
