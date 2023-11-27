#region References

using System;
using Server.Mobiles;
using Server.Items;
using Server.Multis;

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

            int radius = 5;
            IPooledEnumerable nearbyBoats = map.GetItemsInRange(playerLocation, radius);

            bool result = false;

            foreach (Item item in nearbyBoats)
            {
                if (item is BaseBoat)
                {
                    result = true;
                    break;
                }
            }

            nearbyBoats.Free();

            return result;
        }

        // sprawdza w obszarze 5 kratek i przesuwa o 7 w osiach X i Y
        // TODO: Delay na użycie?
        public static void CheckAndMoveCorpses(PlayerMobile player)
        {
            Map map = player.Map;
            Point3D playerLocation = player.Location;

            int radius = 5;
            IPooledEnumerable nearbyCorpses = map.GetItemsInRange(playerLocation, radius);

            foreach (Item item in nearbyCorpses)
            {
                if (item is Corpse)
                {
                    Corpse corpse = (Corpse)item;
                    Point3D newLocation = new Point3D(
                        playerLocation.X + Utility.RandomMinMax(-7, 7),
                        playerLocation.Y + Utility.RandomMinMax(-7, 7),
                        playerLocation.Z);

                    corpse.MoveToWorld(newLocation, map);
                }
            }

            nearbyCorpses.Free();
        }
    }
}
