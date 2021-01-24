using System;
using Server;
using Server.SkillHandlers;
using Server.Mobiles;

namespace Server.Commands
{ 
	public class KoxCommand
    {
		public static void Initialize()
       	{
          	CommandSystem.Register( "kox", AccessLevel.Player, new CommandEventHandler( Kox_OnCommand ) ); 
       	} 
       	
		[Usage( "Kox" )]
       	[Description( "Wlacza/wylacza wyswietlanie informacji o przyrostach umiejetnosci" )] 
       	public static void Kox_OnCommand( CommandEventArgs e ) 
       	{
            PlayerMobile pm = (PlayerMobile)e.Mobile;

            pm.SendMessage( pm.GainsDebugEnabled ? 0x20 : 0x40, "{0} sledzenie przyrostow umiejetnosci.",pm.GainsDebugEnabled ? "Wylaczyles" : "Wlaczyles" );
            
            pm.GainsDebugEnabled = !pm.GainsDebugEnabled;
       	} 
   	} 
} 
