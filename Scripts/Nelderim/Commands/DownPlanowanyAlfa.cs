#region References

using System;
using System.Collections.Generic;
using Server.Commands;
using Server.Gumps;
using Server.Misc;
using Server.Mobiles;

#endregion

namespace Server
{
	public class DownplanAlfa : Timer
	{
		public static bool ShutDown { get; private set; }

		public static void Initialize()
		{
			CommandSystem.Register("Down120", AccessLevel.GameMaster, DownPlanowanyAlfa_OnCommand);
		}

		public static string FormatTimeSpan(TimeSpan ts)
		{
			return String.Format("{0:D2}:{1:D2}:{2:D2}:{3:D2}", ts.Days, ts.Hours % 24, ts.Minutes % 60,
				ts.Seconds % 60);
		}

		[Usage("Down120")]
		[Description("Wylaczenie serwera po 2 minutach od wpisana komendy.")]
		public static void DownPlanowanyAlfa_OnCommand(CommandEventArgs e)
		{
			if (ShutDown || AutoRestart.Restarting || Restartb.Restarting || Down.ShutDown || Downb.ShutDown ||
			    ResplanAlfa.Restarting)
			{
				e.Mobile.SendMessage(38, "Serwer jest podczas Restartu lub Wylaczania.");
				return;
			}

			Console.WriteLine("");
			Console.WriteLine("Wylaczenie serwera nastapi za 2 minuty.");
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

			ShutDown = true;
			Zapis(true);
		}

		public DownplanAlfa() : base(TimeSpan.FromSeconds(1.0), TimeSpan.FromSeconds(10.0))
		{
			Priority = TimerPriority.OneMinute;
		}

		public static void Zapis(bool normal)
		{
			StartDownAlfa rTimer = new StartDownAlfa(normal);
			rTimer.Start();
		}

		public static void DownStr()
		{
			Save();
			DelayCall(TimeSpan.FromSeconds(4.0), DownDrugi);
		}

		public static void Save()
		{
			World.Broadcast(0x21, true, "Rozpoczal sie zapis i wylaczenie Serwera!");
			World.Broadcast(0x35, true, "Rozpoczyna sie zapis swiata");
			//World.Save(false);
			World.Save(); //svn
			World.Broadcast(0x35, true, "Zapis swiata zakonczyl sie pomyslnie");
		}

		public static void DownDrugi()
		{
			Core.Process.Kill();
		}
	}

	public class StartDownAlfa : Timer
	{
		public bool m_Normal;
		public byte count = 120;

		public StartDownAlfa(bool normal) : base(TimeSpan.FromSeconds(1.0), TimeSpan.FromSeconds(10.0))
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
				DownplanAlfa.DownStr();
			}
			else
			{
				World.Broadcast(1154, true, "Pozostalo {0} Sekund", count);
				count -= 10;
			}
		}
	}
}
