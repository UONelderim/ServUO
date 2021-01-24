using System;
using Server;
using Server.Mobiles;
using System.Text;

namespace Server.Commands
{ 
	public class StatusCommand
    {
		public static void Initialize()
       	{
          	CommandSystem.Register( "status", AccessLevel.Player, new CommandEventHandler( Status_OnCommand ) ); 
       	} 
       	
		[Usage( "Status" )]
       	[Description( "Wyswietla informacje o postaci." )] 
       	public static void Status_OnCommand( CommandEventArgs e ) 
       	{
            PlayerMobile pm = (PlayerMobile)e.Mobile;

            pm.SendMessage("Slawa: {0}", e.Mobile.Fame); 
            pm.SendMessage("Karma: {0}", e.Mobile.Karma);
            pm.SendMessage("Morderstwa: {0}", e.Mobile.Kills);

            // 22.09.2012 :: zombie :: wyswietlanie killsow poszczegolnych ras
            //string[] racialKills = new string[ Race.AllRaces.Count - 1 ];

            //for ( int i = 1, count = pm.RacialKills.Length; i < count; i++ )
            //{
            //    int kills = pm.RacialKills[ i ];
            //    bool plural = kills != 1;

            //    racialKills[ i - 1 ] = String.Format( "{0} {1}", kills, Race.AllRaces[i].GetName( Cases.Biernik, plural ) );
            //}
                
            //if( pm.AccessLevel > AccessLevel.Player )
            //    pm.SendMessage( "Zabiles: {0}", String.Join( ", ", racialKills ) );
            // zombie
       	}
    } 
} 
