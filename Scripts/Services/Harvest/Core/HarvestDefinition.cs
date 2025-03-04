using Server.Items;
using System;
using System.Collections.Generic;

namespace Server.Engines.Harvest
{
    public partial class HarvestDefinition
    {
        public HarvestDefinition()
        {
            Banks = new Dictionary<Map, Dictionary<Point2D, HarvestBank>>();
        }

        public int BankWidth { get; set; }
        public int BankHeight { get; set; }
        public int MinTotal { get; set; }
        public int MaxTotal { get; set; }
        public int[] Tiles { get; set; }
        public int[] SpecialTiles { get; set; }
        public bool RangedTiles { get; set; }
        public TimeSpan MinRespawn { get; set; }
        public TimeSpan MaxRespawn { get; set; }
        public int MaxRange { get; set; }
        public int ConsumedPerHarvest { get; set; }
        public int ConsumedPerFeluccaHarvest { get; set; }
        public bool PlaceAtFeetIfFull { get; set; } = true;
        public SkillName Skill { get; set; }
        public int[] EffectActions { get; set; }
        public int[] EffectCounts { get; set; }
        public int[] EffectSounds { get; set; }
        public TimeSpan EffectSoundDelay { get; set; }
        public TimeSpan EffectDelay { get; set; }
        public object NoResourcesMessage { get; set; }
        public object OutOfRangeMessage { get; set; }
        public object TimedOutOfRangeMessage { get; set; }
        public object DoubleHarvestMessage { get; set; }
        public object FailMessage { get; set; }
        public object PackFullMessage { get; set; }
        public object ToolBrokeMessage { get; set; }
        public HarvestResource[] Resources { get; set; }
        public BonusHarvestResource[] BonusResources { get; set; }
        public bool RaceBonus { get; set; }
        public bool RandomizeVeins { get; set; }
        public Dictionary<Map, Dictionary<Point2D, HarvestBank>> Banks { get; set; }

        public void SendMessageTo(Mobile from, object message)
        {
            if (message is int)
                from.SendLocalizedMessage((int)message);
            else if (message is string)
                from.SendMessage((string)message);
        }

        public HarvestBank GetBank(Map map, int x, int y)
        {
            if (map == null || map == Map.Internal)
                return null;

            x /= BankWidth;
            y /= BankHeight;

            Banks.TryGetValue(map, out Dictionary<Point2D, HarvestBank> banks);

            if (banks == null)
                Banks[map] = banks = new Dictionary<Point2D, HarvestBank>();

            Point2D key = new Point2D(x, y);
            banks.TryGetValue(key, out HarvestBank bank);

            if (bank == null)
                banks[key] = bank = new HarvestBank(this, map, x, y);

            return bank;
        }

        public HarvestVein GetVeinAt(Map map, int x, int y)
        {
            double randomValue;

            if (RandomizeVeins)
            {
                randomValue = Utility.RandomDouble();
            }
            else
            {
                Random random = new Random((x * 17) + (y * 11) + (map.MapID * 3));
                randomValue = random.NextDouble();
            }

            return GetVeinFrom(randomValue, map, x, y);
		}

        public HarvestVein GetVeinFrom(double randomValue, Map map, int x, int y)
        {
            // pobierz liste "kolorow" surowca dla zadanej lokacji:
            HarvestVein[] regionVein;
            GetRegionVeins( out regionVein, map, x, y );
            if( regionVein == null )
                return null;

            if (regionVein.Length == 1)
                return regionVein[0];

            // suma szans w definicji HarvestVein[] nie musi juz byc rowna 100. Normalizacja nastepuje tutaj:
            double sum = 0;
            for (int i = 0; i < regionVein.Length; ++i)
                sum += regionVein[i].VeinChance;

            randomValue *= sum;

            for (int i = 0; i < regionVein.Length; ++i)
            {
                if (randomValue <= regionVein[i].VeinChance)
                    return regionVein[i];

                randomValue -= regionVein[i].VeinChance;
            }

            return null;
        }

        public BonusHarvestResource GetBonusResource()
        {
            if (BonusResources == null)
                return null;

            double randomValue = Utility.RandomDouble() * 100;

            for (int i = 0; i < BonusResources.Length; ++i)
            {
                if (randomValue <= BonusResources[i].Chance)
                    return BonusResources[i];

                randomValue -= BonusResources[i].Chance;
            }

            return null;
        }

        public bool Validate(int tileID)
        {
            if (RangedTiles)
            {
                bool contains = false;

                for (int i = 0; !contains && i < Tiles.Length; i += 2)
                    contains = tileID >= Tiles[i] && tileID <= Tiles[i + 1];

                return contains;
            }
            else
            {
                int dist = -1;

                for (int i = 0; dist < 0 && i < Tiles.Length; ++i)
                    dist = Tiles[i] - tileID;

                return dist == 0;
            }
        }

        public double TotalDelaySeconds => EffectDelay.TotalSeconds * EffectCounts[0];

        #region High Seas
        public bool ValidateSpecial(int tileID)
        {
            //No Special tiles were initiated so always true
            if (SpecialTiles == null || SpecialTiles.Length == 0)
                return true;

            for (int i = 0; i < SpecialTiles.Length; i++)
            {
                if (tileID == SpecialTiles[i])
                    return true;
            }

            return false;
        }
        #endregion
    }
}
