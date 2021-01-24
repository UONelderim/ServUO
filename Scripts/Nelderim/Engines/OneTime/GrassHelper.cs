using System;
using System.Collections.Generic;
using System.Threading.Tasks; //Async will need this added
using Server.OneTime.Events;

namespace Server.OneTime
{
    static class GrassHelper
    {
        private static int ItemCount { get; set; }

        private static List<NewStickyGrass> GrassList = new List<NewStickyGrass>();

        public static void Initialize()
        {
            OneTimeSecEvent.SecTimerTick += UpdateGrass;

            //OneTimeSecEvent.SecTimerTick += async (sender, e) => await UpdateGrassAsync(); //Async Event Register (example w/method below)

            ItemCount = 0;
        }

        public static void UpdateGrass(object o, EventArgs e)
        {
            if (World.Items.Count != ItemCount)
            {
                ItemCount = World.Items.Count;

                if (GrassList.Count > 0)
                    GrassList.Clear();

                foreach (Item item in World.Items.Values)
                {
                    if (item is NewStickyGrass)
                    {
                        GrassList.Add(item as NewStickyGrass);
                    }
                }
            }

            UpdateList();
        }

        //public static Task UpdateGrassAsync()
        //{
        //    if (World.Items.Count != ItemCount)
        //    {
        //        ItemCount = World.Items.Count;

        //        if (GrassList.Count > 0)
        //            GrassList.Clear();

        //        foreach (Item item in World.Items.Values)
        //        {
        //            if (item is NewStickyGrass)
        //            {
        //                GrassList.Add(item as NewStickyGrass);
        //            }
        //        }
        //    }

        //    UpdateList();

        //    return Task.CompletedTask;
        //}

        public static void UpdateList()
        {
            if (GrassList.Count > 0)
            {
                foreach (NewStickyGrass grass in GrassList)
                {
                    grass.TimerTick();
                }
            }
        }
    }
}
