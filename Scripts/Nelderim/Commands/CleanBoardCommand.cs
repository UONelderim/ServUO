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
                pm.Emote("*wyrzuca zwloki za burte*");
                pm.SendSound(0x026);
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

                    int offsetX = playerLocation.X > 10 ? -10 : playerLocation.X < -10 ? 10 : Utility.RandomMinMax(-10, 10);
                    int offsetY = playerLocation.Y > 10 ? -10 : playerLocation.Y < -10 ? 10 : Utility.RandomMinMax(-10, 10);

                    Point3D newLocation = new Point3D(
	                    playerLocation.X + offsetX,
	                    playerLocation.Y + offsetY,
	                    playerLocation.Z);

                    corpse.MoveToWorld(newLocation, map);
                }
            }

            nearbyCorpses.Free();
        }
    }
}
