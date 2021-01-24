using System;
using System.Collections;
using System.Diagnostics;
using Server;
using Server.Items;
using Server.Commands;
using Server.Commands.Generic;
using Server.Mobiles;

using System.IO;
using Server.Misc;

namespace Server
{
	public class Down : Timer
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
			CommandSystem.Register( "Down", AccessLevel.Administrator, new CommandEventHandler( Down_OnCommand ) );
		}
		
		public static string FormatTimeSpan( TimeSpan ts )
		{
			return String.Format( "{0:D2}:{1:D2}:{2:D2}:{3:D2}", ts.Days, ts.Hours % 24, ts.Minutes % 60, ts.Seconds % 60 );
		}

		[Usage( "Down" )]
		[Description( "Wylacza serwer." )]
		public static void Down_OnCommand( CommandEventArgs e )
		{
			if ( m_ShutDown || AutoRestart.Restarting || Restartb.Restarting || Downb.ShutDown || ResplanAlfa.Restarting || DownplanAlfa.ShutDown )
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
				new Down( true ).Start();
			}		
		}
		
		public Down(bool trybpracy) : base( m_Delay - m_Warning, m_Delay )
		{
			Priority = TimerPriority.OneSecond;
			m_TrybPracy = trybpracy;
		}
		protected override void OnTick()
		{
			World.Broadcast( 1272, true, "Serwer zostaje wylaczony" ); 
			if(m_TrybPracy == true)
			Timer.DelayCall( m_Warning, new TimerCallback( Down2 ) );
		}
		public static void Save()
		{
			//World.Save(false);
			World.Save(); //svn
		}
		public static void Down2()
		{
			m_ShutDown = false;  
			Save();
			Timer.DelayCall( TimeSpan.FromSeconds( 4.0 ), new TimerCallback( Downdrugi) );
		}
		public static void Downdrugi()
		{
			Core.Process.Kill();
		}
	}
}
