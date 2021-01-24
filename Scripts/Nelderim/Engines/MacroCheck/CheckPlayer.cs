// 05.09.20 :: troyan :: dodanie opcji "zla odpowiedz" do macrochecka
// 05.09.20 :: troyan :: przywrocenie automatycznego wiezienia
// 05.09.20 :: troyan :: przeniesienie do przestrzeni Engines
// 06.01.14 :: troyan :: wylaczenie autojaila


using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Arya.Jail;

namespace Server.Engines
{
	public class CheckPlayer
	{
			private Mobile m_Player;
			private Mobile m_GM;
			private MacroCheckTimer m_Timer;
			private DateTime m_Start;

			public CheckPlayer( Mobile player , Mobile gamemaster )
			{
				m_Player = player;
				m_GM = gamemaster;
				m_Start = DateTime.Now;
				
				PlayerMobile pm = player as PlayerMobile;
				
				pm.LastMacroCheck = DateTime.Now;
				player.SendGump( new MacroCheckGump( this ) );


				m_Timer = new MacroCheckTimer( this );
				m_Timer.Start();
			}
			
			public void PlayerRequest( bool isTrue )
			{
				if ( m_Timer != null )
				{
					m_Timer.Stop();
					m_Timer = null;
				}
				
				m_Player.CloseGump( typeof( MacroCheckGump ) );
				TimeSpan clickbutton = DateTime.Now - m_Start;
				
				if ( isTrue )
					m_GM.SendMessage( 0x40, "Gracz {0} poprawnie odpowiedzial na wezwanie w czasie {1} sekund.", m_Player.Name , TimeSpanFormat( clickbutton ) );
				else
					m_GM.SendMessage( 0x20, "Gracz {0} blednie odpowiedzial na wezwanie w czasie {1} sekund.", m_Player.Name , TimeSpanFormat( clickbutton ) );
			}
		
			public void PlayerRequest()
			{
			 	PlayerRequest( true );
			}
				
			
			public static string TimeSpanFormat( TimeSpan time )
			{
				return String.Format("{0}", time.Seconds );
			}
			
			public void TimeOut()
			{
				if ( m_Timer != null )
				{
					m_Timer.Stop();
					m_Timer = null;
				}
			
				m_Player.CloseGump( typeof( MacroCheckGump ) );

//				Server.Accounting.Account acc = m_Player.Account as Server.Accounting.Account;
//
//				if ( acc == null )
//					return; // Char deleted, too bad
//
//				JailEntry jail = new JailEntry( m_Player , acc,  m_GM ,
//			        TimeSpan.FromDays( 1 ) , "Bierne Makro", "Ukarany automatycznie przez AntiMacroSystem",
//			                               true, true, ( m_Player.Race == RaceType.DarkElf) ? JailSystem.m_Jail[ 0 ] : JailSystem.m_Jail[ 1 ] );
//
//				JailSystem.Jailings.Add( jail );
//				JailSystem.FinalizeJail( m_Player );
				
				m_GM.SendMessage("Gracz {0}, nie odpowiedzial w czasie 1 minuty i zostal zamkniety w wiezieniu.", m_Player.Name );				
			}
		}			
}
