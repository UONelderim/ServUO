#region References

using System;
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
            var m = e.Mobile;

            if (IsMobileAtBoat(m))
            {
                if (CheckAndMoveCorpses(m))
                {
	                m.SendMessage("Oczysciles poklad ze zwlok.");
	                m.Emote("*wyrzuca zwloki za burte*");
	                m.SendSound(0x026);
                }
                else
                {
	                m.SendMessage("Nie ma nic do czyszczenia.");
                }
            }
            else
            {
                m.SendMessage("Musisz byc w poblizu swojej lodzi, aby uzyc tej komendy.");
            }
        }

        private static bool IsMobileAtBoat(Mobile m)
        {
	        return BaseBoat.FindBoatAt(m) != null;
        }
        
        // TODO: Delay na użycie?
        public static bool CheckAndMoveCorpses(Mobile m)
        {
	        var result = false;
            IPooledEnumerable nearbyItems = m.GetItemsInRange(4);
            foreach (Item item in nearbyItems)
            {
                if (item is Corpse)
                {
	                int offsetX = item.Location.X - m.Location.X;
	                int offsetY = item.Location.Y - m.Location.Y;
                    if (offsetX == 0 && offsetY == 0)
                    {
	                    offsetX = 5;
                    }
                    if (Math.Abs(offsetX) > Math.Abs(offsetY))
                    {
	                    offsetY = 0;
                    }
                    if(Math.Abs(offsetX) < Math.Abs(offsetY))
                    {
	                    offsetX = 0;
                    }
                    item.MoveToWorld(new Point3D(
	                    m.Location.X + Utility.Clamp(offsetX, -1, 1) * 5,
	                    m.Location.Y + Utility.Clamp(offsetY, -1, 1) * 5,
	                    item.Location.Z));
                    result = true;
                }
            }
            nearbyItems.Free();
            return result;
        }
    }
}
