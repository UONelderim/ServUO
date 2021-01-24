using System;
using System.Collections;
using System.Diagnostics;
using Server;
using Server.Items;
using Server.Commands.Generic;
using System.IO;
using Server.Misc;

namespace Server.Commands
{
	public class Downb : Timer
	{
		private static TimeSpan m_Delay = TimeSpan.FromSeconds( 8.0 );
		private static TimeSpan m_Warning = TimeSpan.FromSeconds( 4.0 );

		private static bool m_ShutDown = false;
		private bool m_TrybPracy;
		
		public static bool ShutDown
		{
			get{ return m_ShutDown; }
		}
		
		public static void Initialize()
		{
			CommandSystem.Register( "DownB", AccessLevel.Administrator, new CommandEventHandler( Downb_OnCommand ) );
		}
		
		public static string FormatTimeSpan( TimeSpan ts )
		{
			return String.Format( "{0:D2}:{1:D2}:{2:D2}:{3:D2}", ts.Days, ts.Hours % 24, ts.Minutes % 60, ts.Seconds % 60 );
		}

		[Usage( "DownB" )]
		[Description( "Wylacza serwer bez zapisu." )]
		public static void Downb_OnCommand( CommandEventArgs e )
		{
			if ( m_ShutDown || AutoRestart.Restarting || Restartb.Restarting || Down.ShutDown  || ResplanAlfa.Restarting || DownplanAlfa.ShutDown )
			{
				e.Mobile.SendMessage( 38, "Serwer jest podczas Restartu lub Wylanczania." );
				return;
			}
			else 
			{
				Console.WriteLine("");
				Console.WriteLine( "Serwer zostaje wylaczony." );
				Console.WriteLine("");
				World.Broadcast( 1272, true, "Uwaga za chwile serwer zostanie wylaczony" );
				m_ShutDown = true;
				new Downb( true ).Start();
			}
		}

		public Downb(bool trybpracy) : base( m_Delay - m_Warning, m_Delay )
		{
			Priority = TimerPriority.OneSecond;
			m_TrybPracy = trybpracy;
		}
		protected override void OnTick()
		{
			World.Broadcast( 1272, true, "Serwer zostaje wylaczony" ); 
			if(m_TrybPracy == true)
			Timer.DelayCall( m_Warning, new TimerCallback( Downdrugi ) );
		}
		public static void Downdrugi()
		{
			Core.Process.Kill();
		}
	}
}
