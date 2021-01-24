// 05.09.06 :: eth
// 05.11.17 :: troyan :: logowanie
// 06.02.02 :: troyan :: lokalizacja + konto celu

using System;
using Server;
using Server.Network;
using Server.Mobiles;
using System.Collections;
using Server.Accounting;

namespace Server.Commands
{
	public class LsCommand
	{
		public static void Initialize()
		{
			CommandSystem.Register( "Ls", AccessLevel.GameMaster, new CommandEventHandler( Ls_OnCommand ) );
		}
		[Usage( "Ls" )]
		[Description( "Przenosi do losowego gracza." )]
		private static void Ls_OnCommand( CommandEventArgs args )
		{
			ArrayList players = new ArrayList();
			Mobile from = args.Mobile;
			
			try
			{
				foreach ( NetState ns in NetState.Instances )
				{
					Mobile m = ns.Mobile;
	
					if ( m != null && from != m && m.AccessLevel == AccessLevel.Player && !from.InRange( m.Location, 5 ) && m.Map != Map.Internal )
						players.Add( m );
				}
				
				if ( players.Count == 0 )
					args.Mobile.SendLocalizedMessage( 505707 ); // "There is noone to visit."
				else
				{
					Mobile m = ( Mobile ) players[ Utility.Random( players.Count ) ];
	
					from.MoveToWorld( m.Location, m.Map );
					
					from.SendLocalizedMessage( 505708, m.Name ); // Imie: ~1_NAME~
					from.SendLocalizedMessage( 505711, ( ( Account ) m.Account ).Username ); // Konto: ~1_ACCOUNT~
					from.SendLocalizedMessage(505709, m.Race.GetName(Cases.Mianownik)); // Rasa: ~1_RACE~
					
					string log = args.Mobile.AccessLevel + " " + CommandLogging.Format( args.Mobile );
					log += " randomly moved to player " + CommandLogging.Format( m ) + " [Ls]";
								
					CommandLogging.WriteLine( args.Mobile, log );
				}
			}
			catch ( Exception exc )
			{
				from.SendLocalizedMessage( 505054 ); // Wystapil niespodziewany blad polecenia! Zglos go Ekipie.
				Console.WriteLine( exc.ToString() );
			}
		}
	}
}
