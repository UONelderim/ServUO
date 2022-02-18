#region References

using System;
using System.Diagnostics;
using Server.Commands;
using Server.Misc;

#endregion

namespace Server
{
	public class Restartb : Timer
	{
		private static readonly TimeSpan m_Delay = TimeSpan.FromSeconds(8.0);
		private static readonly TimeSpan m_Warning = TimeSpan.FromSeconds(4.0);

		private readonly bool m_TrybPracy;

		public static bool Restarting { get; private set; }

		public static void Initialize()
		{
			CommandSystem.Register("RestartB", AccessLevel.Administrator, Restartb_OnCommand);
		}

		public static string FormatTimeSpan(TimeSpan ts)
		{
			return String.Format("{0:D2}:{1:D2}:{2:D2}:{3:D2}", ts.Days, ts.Hours % 24, ts.Minutes % 60,
				ts.Seconds % 60);
		}

		[Usage("RestartB")]
		[Description("Restartuje serwer bez zapissu.")]
		public static void Restartb_OnCommand(CommandEventArgs e)
		{
			if (Restarting || AutoRestart.Restarting || Down.ShutDown || Downb.ShutDown || ResplanAlfa.Restarting ||
			    DownplanAlfa.ShutDown)
			{
				e.Mobile.SendMessage(38, "Serwer jest podczas Restartu lub Wylanczania.");
				return;
			}

			Console.WriteLine("");
			Console.WriteLine("Serwer zostaje restartowany.");
			Console.WriteLine("");
			World.Broadcast(1272, true, "Uwaga za chwile serwer zostanie zrestartowany");
			Restarting = true;
			new Restartb(true).Start();
		}

		public Restartb(bool trybpracy) : base(m_Delay - m_Warning, m_Delay)
		{
			Priority = TimerPriority.OneSecond;
			m_TrybPracy = trybpracy;
		}

		protected override void OnTick()
		{
			World.Broadcast(1272, true, "Serwer zostaje zrestartowany");
			if (m_TrybPracy)
				DelayCall(m_Warning, Resdrugi);
		}

		public static void Resdrugi()
		{
			Process.Start(Core.ExePath);
			Core.Process.Kill();
		}
	}
}
