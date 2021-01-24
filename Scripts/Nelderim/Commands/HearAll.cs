// 05.04.21 :: Evantis :: przeniesienie na poziom Kanclerza, spolszczenie
// 06.02.12 :: troyan :: usuniecie z listy podsluchu wylogowujacych sie GMow
// 07.10.26 :: emfor :: Osoba o nizszym dostepie nie slyszy osob wyzszym

using System; 
using System.Collections; 
using Server;
using Server.Mobiles; 

namespace Server.Commands
{
	public class HearAll
	{
		private static ArrayList m_HearAll = new ArrayList();
		
		public static void Initialize()
		{
			CommandSystem.Register( "HearAll", AccessLevel.Counselor, new CommandEventHandler( HearAll_OnCommand ) );
         	EventSink.Speech += new SpeechEventHandler( OnSpeech ); 
			EventSink.Logout += new LogoutEventHandler( OnLogout );
      	} 
		
		public static void OnSpeech( SpeechEventArgs e )
      	{
            PlayerMobile heard = e.Mobile as PlayerMobile;
            if ( heard != null )
            {
                if ( m_HearAll.Count > 0 )
                {
                    string msg = String.Format( "({0}): {1}", e.Mobile.Name, e.Speech );

                    for ( int i = 0; i < m_HearAll.Count; ++i )
                    {
                        PlayerMobile hearing = m_HearAll[i] as PlayerMobile;
                        if ( hearing != null && heard.AccessLevel <= hearing.AccessLevel || (heard.AccessLevel >= AccessLevel.Seer && hearing.AccessLevel >= AccessLevel.Seer) )
                            hearing.SendMessage( msg );
                    }
                }
            }
      	} 

		public static void OnLogout( LogoutEventArgs e ) 
      	{
			if ( m_HearAll.Contains( e.Mobile ) ) 
         	{ 
            	m_HearAll.Remove( e.Mobile );
			}
		}

      	public static void HearAll_OnCommand( CommandEventArgs e ) 
      	{ 
         	if ( m_HearAll.Contains( e.Mobile ) ) 
         	{ 
            	m_HearAll.Remove( e.Mobile ); 
            	e.Mobile.SendMessage( "HearAll wylaczone." ); 
         	} 
         	else 
         	{ 
            	m_HearAll.Add( e.Mobile ); 
            	e.Mobile.SendMessage( "HearAll wlaczone." ); 
         	} 
      	} 
   	} 
}
