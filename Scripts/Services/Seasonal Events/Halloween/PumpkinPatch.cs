using Server.Events.Halloween;
using Server.Items;
using System;

namespace Server.Engines.Events
{
    public class PumpkinPatchSpawner
    {
        private static readonly Rectangle2D[] m_PumpkinFields =
        {
            new Rectangle2D(4557, 1471, 20, 10),
            new Rectangle2D(796, 2152, 36, 24),
            new Rectangle2D(816, 2251, 16, 8),
            new Rectangle2D(816, 2261, 16, 8),
            new Rectangle2D(816, 2271, 16, 8),
            new Rectangle2D(816, 2281, 16, 8),
            new Rectangle2D(835, 2344, 16, 16),
            new Rectangle2D(816, 2344, 16, 24)
        };

        public static void Initialize()
        {
            if (DateTime.UtcNow >= HalloweenSettings.StartHalloween && DateTime.UtcNow <= HalloweenSettings.FinishHalloween)
            {
                Timer.DelayCall(TimeSpan.Zero, TimeSpan.FromSeconds(30), 0, PumpkinPatchSpawnerCallback);
            }
        }

        protected static void PumpkinPatchSpawnerCallback()
        {
            AddPumpkin(Map.Felucca);
            AddPumpkin(Map.Trammel);
        }

        private static void AddPumpkin(Map map)
        {
            for (int i = 0; i < m_PumpkinFields.Length; i++)
            {
                Rectangle2D rect = m_PumpkinFields[i];

                int spawncount = ((rect.Height * rect.Width) / 20);
                int pumpkins = 0;

                var eable = map.GetItemsInBounds(rect);
                foreach (Item item in eable)
                {
                    if (item is HalloweenPumpkin)
                    {
                        pumpkins++;
                    }
                }
                eable.Free();

                if (spawncount > pumpkins)
                {
                    Item item = new HalloweenPumpkin();

                    item.MoveToWorld(RandomPointIn(rect, map), map);
                }
            }
        }

        private static Point3D RandomPointIn(Rectangle2D rect, Map map)
        {
            int x = Utility.Random(rect.X, rect.Width);
            int y = Utility.Random(rect.Y, rect.Height);
            int z = map.GetAverageZ(x, y);

            return new Point3D(x, y, z);
        }
    }
}
