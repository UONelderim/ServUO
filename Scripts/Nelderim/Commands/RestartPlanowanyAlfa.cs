#region References

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Server.Commands;
using Server.Gumps;
using Server.Misc;
using Server.Mobiles;

#endregion

namespace Server
{
	public class ResplanAlfa : Timer
	{
		public static bool Restarting { get; private set; }

		public static void Initialize()
		{
			CommandSystem.Register("Res120", AccessLevel.GameMaster, RestartPlanowanyAlfa_OnCommand);
		}

		public static string FormatTimeSpan(TimeSpan ts)
		{
			return String.Format("{0:D2}:{1:D2}:{2:D2}:{3:D2}", ts.Days, ts.Hours % 24, ts.Minutes % 60,
				ts.Seconds % 60);
		}

		[Usage("Res120")]
		[Description("Restart serwera po 2 minutach od wpisana komendy.")]
		public static void RestartPlanowanyAlfa_OnCommand(CommandEventArgs e)
		{
			if (Restarting || AutoRestart.Restarting || Restartb.Restarting || Down.ShutDown || Downb.ShutDown ||
			    DownplanAlfa.ShutDown)
			{
				e.Mobile.SendMessage(38, "Serwer jest podczas Restartu lub Wylaczania.");
				return;
			}

			Console.WriteLine("");
			Console.WriteLine("Restart serwera nastapi za 2 minuty.");
			Console.WriteLine("");
			List<Mobile> mobs = new List<Mobile>(World.Mobiles.Values);

			foreach (Mobile m in mobs)
			{
				if (m != null && m is PlayerMobile)
				{
					m.CloseGump(typeof(ZamykanieSwiataGump));
					m.SendGump(new ZamykanieSwiataGump());
				}
			}

			Restarting = true;
			Zapis(true);
		}

		public ResplanAlfa() : base(TimeSpan.FromSeconds(1.0), TimeSpan.FromSeconds(10.0))
		{
			Priority = TimerPriority.OneMinute;
		}

		public static void Zapis(bool normal)
		{
			StartRestartAlfa rTimer = new StartRestartAlfa(normal);
			rTimer.Start();
		}

		public static void Restart()
		{
			Save();
			DelayCall(TimeSpan.FromSeconds(4.0), RestartDrugi);
		}

		public static void Save()
		{
			World.Broadcast(0x21, true, "Rozpoczal sie zapis i restart Serwera!");
			World.Broadcast(0x35, true, "Rozpoczyna sie zapis swiata");
			//World.Save(false);
			World.Save(); //svn
			World.Broadcast(0x35, true, "Zapis swiata zakonczyl sie pomyslnie");
		}

		public static void RestartDrugi()
		{
			Process.Start(Core.ExePath);
			Core.Process.Kill();
		}
	}

	public class StartRestartAlfa : Timer
	{
		public bool m_Normal;
		public byte count = 120;

		public StartRestartAlfa(bool normal) : base(TimeSpan.FromSeconds(1.0), TimeSpan.FromSeconds(10.0))
		{
			m_Normal = normal;
		}

		protected override void OnTick()
		{
			if (count == 120)
			{
				World.Broadcast(1154, true, "Pozostalo {0} Sekund", count);
				count -= 10;
			}
			else if (count == 1)
			{
				World.Broadcast(1154, true, "Pozostala 1 Sekunda");
				count -= 1;
			}
			else if (count == 0)
			{
				Stop();
				ResplanAlfa.Restart();
			}
			else
			{
				World.Broadcast(1154, true, "Pozostalo {0} Sekund", count);
				count -= 10;
			}
		}
	}
}
