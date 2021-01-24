using System;
using Server.Network;

namespace Server.Commands
{
   public class Online
   {
      public static void Initialize()
      {
          CommandSystem.Register( "gracze", AccessLevel.Player, new CommandEventHandler( Online_OnCommand ) );
      }

      public static void Online_OnCommand( CommandEventArgs e )
      {
         e.Mobile.SendMessage("Graczy online: {0}", NetState.Instances.Count);
      }
   }
}
