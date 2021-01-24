using System;
using Server;
using Server.Mobiles;
using System.Text;

namespace Server.Commands
{ 
	public class RegionCommand
    {
		public static void Initialize()
       	{
          	CommandSystem.Register( "region", AccessLevel.Administrator, new CommandEventHandler( Region_OnCommand ) ); 
       	} 
       	
       	public static void Region_OnCommand( CommandEventArgs e ) 
       	{
            e.Mobile.SendMessage( "Region: " + e.Mobile.Region );
       	}
    } 
} 
