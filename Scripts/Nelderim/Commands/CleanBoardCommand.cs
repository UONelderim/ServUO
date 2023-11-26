#region References

using System;
using Server.Mobiles;
using Server.Items;
using Server.Multis;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Server.Network;
using Server.Regions;
using Server.ContextMenus;

#endregion

namespace Server.Commands
{
    public class CleanBoardCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register("czyscpoklad", AccessLevel.Player, CleanBoard_OnCommand);
        }

        [Usage("czyscpoklad")]
        [Description("Komenda ta czysci poklad Twojej lodzi")]
        public static void CleanBoard_OnCommand(CommandEventArgs e)
        {
            PlayerMobile pm = (PlayerMobile)e.Mobile;


            if (IsPlayerNearBaseBoat(pm))
            {
                pm.SendMessage("Oczysciles poklad ze zwlok.");


                CheckAndMoveCorpses(pm);
            }
            else
            {
                pm.SendMessage("Musisz byc w poblizu swojej lodzi, aby uzyc tej komendy.");
            }
        }

        private static bool IsPlayerNearBaseBoat(PlayerMobile player)
        {
            Map map = player.Map;
            Point3D playerLocation = player.Location;

            // Search for BaseBoat within a specified radius (adjust as needed)
            int radius = 5;
            IPooledEnumerable nearbyBoats = map.GetItemsInRange(playerLocation, radius);

            try
            {
                foreach (Item item in nearbyBoats)
                {
                    if (item is BaseBoat)
                    {
                        return true;
                    }
                }

                return false;
            }
            finally
            {
                nearbyBoats.Free();
            }
        }
		// sprawdza w obszarze 5 kratek i przesuwa o 7 w osiach X i Y
		//TODO: Delay na użycie?
        private static void CheckAndMoveCorpses(PlayerMobile player)
        {
            Map map = player.Map;
            Point3D playerLocation = player.Location;
            
            int radius = 5;
            IPooledEnumerable nearbyCorpses = map.GetItemsInRange(playerLocation, radius);

            try
            {
                foreach (Item item in nearbyCorpses)
                {
                    if (item is Corpse corpse)
                    {
	                    Point3D newLocation = new Point3D(
                            playerLocation.X + Utility.RandomMinMax(-7, 7),
                            playerLocation.Y + Utility.RandomMinMax(-7, 7),
                            playerLocation.Z);

                        corpse.MoveToWorld(newLocation, map);
                    }
                }
            }
            finally
            {
                nearbyCorpses.Free();
            }
        }
    }
}
