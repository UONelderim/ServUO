using System;
using Server.Regions;

namespace Server.Commands
{
    public class CheckRegions
    {
        public static void Initialize()
        {
            CommandSystem.Register("checkRegions", AccessLevel.Administrator, new CommandEventHandler(CheckRegions_OnCommand));
        }

        public static void CheckRegions_OnCommand(CommandEventArgs e)
        {
            foreach (var map in Map.AllMaps)
            {
                foreach (var r1 in map.Regions.Values)
                {
                    if(CanOverlap(r1)) continue;
                        foreach (var r2 in map.Regions.Values)
                        {
                            if (r1 == r2 || CanOverlap(r2)) continue;
                            foreach (var a1 in r1.Area)
                            {
                                foreach (var a2 in r2.Area)
                                {
                                    if (Overlaps(a1, a2)) Console.WriteLine(r1.Name + " overlaps " + r2.Name);
                                }
                            }
                        }
                }
            }
            
        }

        private static bool CanOverlap (Region r)
        {
            return r is CityRegion || r is VillageRegion || r is GeographicRegion || r is TravelRegion 
                   || r is SpawnRegion || r is HousingRegion || r is ParagonsRegion || r is Undershadow 
                   || r is TavernRegion || r is RaceRoomRegion;
        }
        
        private static bool Overlaps(Rectangle3D r1, Rectangle3D r2)
        {
            return r1.Start.X <= r2.End.X && r1.End.X >= r2.Start.X && r1.Start.Y <= r2.End.Y && r1.End.Y >= r2.Start.Y;
        }
    }
}