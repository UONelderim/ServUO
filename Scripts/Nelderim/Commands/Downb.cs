#region References

using System;
using Server.Misc;

#endregion

namespace Server.Commands
{
	public class Downb : Timer
	{
		private static readonly TimeSpan m_Delay = TimeSpan.FromSeconds(8.0);
		private static readonly TimeSpan m_Warning = TimeSpan.FromSeconds(4.0);

		private readonly bool m_TrybPracy;

		public static bool ShutDown { get; private set; }

		public static void Initialize()
		{
			CommandSystem.Register("DownB", AccessLevel.Administrator, Downb_OnCommand);
		}

		public static string FormatTimeSpan(TimeSpan ts)
		{
			return String.Format("{0:D2}:{1:D2}:{2:D2}:{3:D2}", ts.Days, ts.Hours % 24, ts.Minutes % 60,
				ts.Seconds % 60);
		}

		[Usage("DownB")]
		[Description("Wylacza serwer bez zapisu.")]
		public static void Downb_OnCommand(CommandEventArgs e)
		{
			if (ShutDown || AutoRestart.Restarting || Restartb.Restarting || Down.ShutDown || ResplanAlfa.Restarting ||
			    DownplanAlfa.ShutDown)
			{
				e.Mobile.SendMessage(38, "Serwer jest podczas Restartu lub Wylanczania.");
				return;
			}

			Console.WriteLine("");
			Console.WriteLine("Serwer zostaje wylaczony.");
			Console.WriteLine("");
			World.Broadcast(1272, true, "Uwaga za chwile serwer zostanie wylaczony");
			ShutDown = true;
			new Downb(true).Start();
		}

		public Downb(bool trybpracy) : base(m_Delay - m_Warning, m_Delay)
		{
			Priority = TimerPriority.OneSecond;
			m_TrybPracy = trybpracy;
		}

		protected override void OnTick()
		{
			World.Broadcast(1272, true, "Serwer zostaje wylaczony");
			if (m_TrybPracy)
				DelayCall(m_Warning, Downdrugi);
		}

		public static void Downdrugi()
		{
			Core.Process.Kill();
		}
	}
}
