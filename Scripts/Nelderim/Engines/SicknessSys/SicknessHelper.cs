using Server.Misc;
using Server.Mobiles;
using Server.SicknessSys.Gumps;
using Server.SicknessSys.Items;
using System.Collections.Generic;
using System.Linq;

namespace Server.SicknessSys
{
    public static class SicknessHelper
    {
        public static bool IsSpecialVirus(VirusCell cell)
        {
            bool IsSpecial = false;

            if (cell.Illness == IllnessType.Lycanthropia)
                IsSpecial = true;
            if (cell.Illness == IllnessType.Vampirism)
                IsSpecial = true;

            return IsSpecial;
        }

        public static void SendHeartGump(VirusCell cell)
        {
            if (cell != null)
            {
                PlayerMobile pm = cell.PM;

                if (pm != null && cell != null)
                {
                    Illnesses.IllnessMutationLists.SetMutation(cell);

                    int HeartBaseHue = cell.BaseHeart;

                    int stageAdjusted = GetStageAdjustment(cell);

                    int beatMod = GetPowerMod(cell);

                    int powerCheck = Utility.RandomMinMax(1, cell.PowerDegenRate * 2);

                    if (!cell.IsMovingGump)
                    {
                        PowerGump pg;

                        if (cell.HeartBeat < 4 - beatMod)
                        {
                            if (powerCheck > cell.PowerDegenRate)
                                ImmuneSystem.UpdateImmuneNegative(cell);

                            pg = new PowerGump(pm, cell, HeartBaseHue, cell.GumpX, cell.GumpY, false);

                            cell.HeartBeat++;
                        }
                        else
                        {
                            if (cell.IsContagious && !IsSpecialVirus(cell))
                            {
                                SicknessInfect.SpreadVirus(pm, cell);
                            }

                            bool RunImmune = Utility.RandomBool();

                            if(RunImmune)
                                ImmuneSystem.UpdateImmuneNegative(cell);

                            pg = new PowerGump(pm, cell, stageAdjusted, cell.GumpX, cell.GumpY, true);

                            cell.HeartBeat = 0;
                        }

                        pm.SendGump(pg);
                    }
                    else
                    {
                        if (cell.IsMovingRelease < 10)
                        {
                            cell.IsMovingRelease++;
                        }
                        else
                        {
                            cell.IsMovingGump = false;
                            cell.IsMovingRelease = 0;
                        }
                    }
                }
                else
                {
                    cell.Delete();
                }
            }
            else
            {
                
            }
        }

        private static int GetPowerMod(VirusCell cell)
        {
            int mod = 0;

            if (!IsLowHealth(cell.PM))
            {
                if (cell.Power < (cell.MaxPower/4) * 3 || cell.PM.Hits < (cell.PM.Hits / 4) * 3)
                {
                    mod++;

                    if (cell.Power < (cell.MaxPower / 4) * 2 || cell.PM.Hits < cell.PM.Hits / 2)
                    {
                        mod++;

                        if (cell.Power < cell.MaxPower / 4 || cell.PM.Hits < cell.PM.Hits / 4)
                        {
                            mod++;
                        }
                    }
                }
            }
            else
            {
                mod = 3;
            }

            if (cell.Level == 100)
                mod = 3;
            else if (cell.Stage == 3 && mod < 2)
            {
                mod = 2;
            }
            else if (cell.Stage == 2 && mod < 1)
            {
                mod = 1;
            }

            return mod;
        }

        private static int GetStageAdjustment(VirusCell cell)
        {
            int adj = 0;

            if (cell.Stage == 1)
            {
                adj = 2751;
            }
            if (cell.Stage == 2)
            {
                adj = 2752;
            }
            if (cell.Stage == 3)
            {
                adj = 2750;
            }

            return adj;
        }

        public static bool IsDark(PlayerMobile pm)
        {
            if (pm != null)
            {
                if (pm.LightLevel > 5)
                    return false;
                else
                    return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsNight(PlayerMobile pm)
        {
            if (pm != null)
            {
                int i_Time = SicknessTime.GetTime(pm);

                bool GetTime = false;

                if (i_Time == 1042957)      // It's late at night
                    GetTime = true;
                if (i_Time == 1042950)      //'Tis the witching hour. 12 Midnight
                    GetTime = true;
                else if (i_Time == 1042951) // It's the middle of the night
                    GetTime = true;

                return GetTime;
            }
            else
            {
                return false;
            }
        }

        public static int GetSickChance(PlayerMobile pm, int chance)
        {
            int ClothingMod;

            if (IsFullyExposed(pm))
            {
                ClothingMod = 5;
            }
            else if (IsPartiallyExposed(pm))
            {
                ClothingMod = 0;
            }
            else
            {
                ClothingMod = -5;
            }

            if (chance >= 0)
            {
                if (IsNight(pm))
                    chance = chance + (5 + ClothingMod);
                if (IsForest(pm))
                    chance = chance + (7 + ClothingMod);
                if (IsJungle(pm))
                    chance = chance + (11 + ClothingMod);
                if (IsSand(pm))
                    chance = chance + (13 + ClothingMod);
                if (IsSnow(pm))
                    chance = chance + (17 + ClothingMod);
                if (IsCave(pm))
                    chance = chance + (19 + ClothingMod);
                if (IsSwamp(pm))
                    chance = chance + (21 + ClothingMod);

                if (IsWeather(pm))
                {
                    chance = chance + (31 + ClothingMod);
                }

                if (IsLowHealth(pm))
                    chance = chance + (41 + ClothingMod);

                if (AreRatsClose(pm))
                    chance = chance + (50 + ClothingMod);
            }

            if (chance < 1)
                return 1;
            else if (chance < 100)
                return chance;
            else
                return 100;
        }

        public static bool IsForest(PlayerMobile pm)
        {
            return CheckTile(pm, "forest");
        }

        public static bool IsJungle(PlayerMobile pm)
        {
            return CheckTile(pm, "jungle");
        }

        public static bool IsSand(PlayerMobile pm)
        {
            return CheckTile(pm, "sand");
        }

        public static bool IsSnow(PlayerMobile pm)
        {
            return CheckTile(pm, "snow");
        }

        public static bool IsCave(PlayerMobile pm)
        {
            return CheckTile(pm, "cave");
        }

        public static bool IsSwamp(PlayerMobile pm)
        {
            return CheckTile(pm, "NoName");
        }

        private static bool CheckTile(PlayerMobile pm, string name)
        {
            if (pm != null)
            {
                LandTile landTile = pm.Map.Tiles.GetLandTile(pm.X, pm.Y);

                LandData landData = TileData.LandTable[landTile.ID & 0x3FFF];

                if (landData.Name.Contains(name))
                    return true;
                else
                    return false;
            }
            else
            {
                return false;
            }
        }

        public static bool InDoors(PlayerMobile pm)
        {
            bool CheckAbove = false;

            if (pm != null)
            {
                StaticTile[] tiles = pm.Map.Tiles.GetStaticTiles(pm.X, pm.Y);

                if (tiles.Length > 0)
                {
                    foreach (var item in tiles)
                    {
                        if (item.Z > pm.Z)
                            CheckAbove = true;
                    }
                }
            }
            return CheckAbove;
        }

        public static bool IsWeather(PlayerMobile pm)
        {
            Map facet = pm.Map;
            bool weather = false;

            if (facet == null)
                return false;

            List<Weather> list = Weather.GetWeatherList(facet);

            for (int i = 0; i < list.Count; ++i)
            {
                Weather w = list[i];

                for (int j = 0; j < w.Area.Length; ++j)
                {
                    int weatherX = w.Area[j].X - (w.Area[j].Width / 2);
                    int weatherXX = w.Area[j].X + (w.Area[j].Width / 2);

                    int weatherY = w.Area[j].Y - (w.Area[j].Height / 2);
                    int weatherYY = w.Area[j].Y + (w.Area[j].Height / 2);

                    if (weatherX < pm.X && weatherXX > pm.X)
                    {
                        if (weatherY < pm.Y && weatherYY > pm.Y)
                        {
                            if (!InDoors(pm))
                                weather = true;
                        }

                    }
                }
            }
            return weather;
        }

        public static bool IsFullyCovered(PlayerMobile pm)
        {
            bool IsCovered = false;

            if (pm != null)
            {
                if (GetClothing(pm) > 5)
                    IsCovered = true;
            }

            return IsCovered;
        }

        public static bool IsFullyExposed(PlayerMobile pm)
        {
            bool IsExposed = false;

            if (pm != null)
            {
                if (GetClothing(pm) == 0)
                    IsExposed = true;
            }

            return IsExposed;
        }

        public static bool IsPartiallyExposed(PlayerMobile pm)
        {
            bool IsExposed = false;

            if (pm != null)
            {
                if (GetClothing(pm) < 5)
                    IsExposed = true;
            }

            return IsExposed;
        }

        private static int GetClothing(PlayerMobile pm)
        {
            int NumberOfClothing = 0;

            Item item = pm.FindItemOnLayer(Layer.OuterTorso);

            if (pm.FindItemOnLayer(Layer.Helm) != null)
                NumberOfClothing++;
            if (pm.FindItemOnLayer(Layer.Face) != null)
                NumberOfClothing++;
            if (pm.FindItemOnLayer(Layer.Neck) != null)
                NumberOfClothing++;
            if (pm.FindItemOnLayer(Layer.InnerTorso) != null)
                NumberOfClothing++;
            if (pm.FindItemOnLayer(Layer.MiddleTorso) != null)
                NumberOfClothing++;
            if (pm.FindItemOnLayer(Layer.OuterTorso) != null)
            {
                if (item is VampireRobe)
                    NumberOfClothing += 5;
                else
                    NumberOfClothing++;
            }
            if (pm.FindItemOnLayer(Layer.Pants) != null)
                NumberOfClothing++;
            if (pm.FindItemOnLayer(Layer.Arms) != null)
                NumberOfClothing++;
            if (pm.FindItemOnLayer(Layer.Gloves) != null)
                NumberOfClothing++;
            if (pm.FindItemOnLayer(Layer.OuterLegs) != null)
                NumberOfClothing++;

            return NumberOfClothing;
        }

        public static bool IsLowHealth(PlayerMobile pm)
        {
            if (pm != null)
            {
                if (pm.Hits < pm.HitsMax / 4)
                    return true;
                else
                    return false;
            }
            else
            {
                return false;
            }
        }

        public static bool AreRatsClose(PlayerMobile pm)
        {
            IEnumerable<Rat> result = from c in pm.GetMobilesInRange(3)
                                      where c is Rat
                                      select c as Rat;

            return result.Any();
        }
    }
}
