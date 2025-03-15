using Server.Mobiles;
using System;

namespace Server.Engines.CannedEvil
{
    public enum ChampionSpawnType
    {
        Abyss,
        Arachnid,
        ColdBlood,
        ForestLord,
        VerminHorde,
        UnholyTerror,
        SleepingDragon,
        Glade,
        Corrupt,
        Terror,
        Infuse,
        DragonTurtle,
        Khaldun,
        MeraktusTheTormented = 100,
        Pyre,
        Morena,
        OrcCommander
    }

    public class ChampionSpawnInfo
    {
        public string Name { get; }
        public Type Champion { get; }
        public Type[][] SpawnTypes { get; }
        public string[] LevelNames { get; }

        public ChampionSpawnInfo(string name, Type champion, string[] levelNames, Type[][] spawnTypes)
        {
            Name = name;
            Champion = champion;
            LevelNames = levelNames;
            SpawnTypes = spawnTypes;
        }

        public static ChampionSpawnInfo[] Table => m_Table;

        private static readonly ChampionSpawnInfo[] m_Table =
        {
            new ChampionSpawnInfo("Abyss", typeof(Semidar), new[] { "Wrog", "Zabojca", "Pogromca" }, new[] // Abyss
            {
                new[] { typeof(GreaterMongbat), typeof(Imp) }, // Level 1
                new[] { typeof(Gargoyle), typeof(Harpy) }, // Level 2
                new[] { typeof(FireGargoyle), typeof(StoneGargoyle) }, // Level 3
                new[] { typeof(Daemon), typeof(Succubus) }// Level 4
            }),
            new ChampionSpawnInfo("Arachnid", typeof(Mephitis), new[] { "Wrog", "Zabojca", "Pogromca" }, new[] // Arachnid
            {
                new[] { typeof(Scorpion), typeof(GiantSpider) }, // Level 1
                new[] { typeof(TerathanDrone), typeof(TerathanWarrior) }, // Level 2
                new[] { typeof(DreadSpider), typeof(TerathanMatriarch) }, // Level 3
                new[] { typeof(PoisonElemental), typeof(TerathanAvenger) }// Level 4
            }),
            new ChampionSpawnInfo("Cold Blood", typeof(Rikktor), new[] { "Wrog", "Zabojca", "Pogromca" }, new[] // Cold Blood
            {
                new[] { typeof(Lizardman), typeof(Snake) }, // Level 1
                new[] { typeof(LavaLizard), typeof(OphidianWarrior) }, // Level 2
                new[] { typeof(Drake), typeof(OphidianArchmage) }, // Level 3
                new[] { typeof(Dragon), typeof(OphidianKnight) }// Level 4
            }),
            new ChampionSpawnInfo("Forest Lord", typeof(LordOaks), new[] { "Wrog", "Zabojca", "Pogromca" }, new[] // Forest Lord
            {
                new[] { typeof(Pixie), typeof(ShadowWisp) }, // Level 1
                new[] { typeof(Kirin), typeof(Wisp) }, // Level 2
                new[] { typeof(Centaur), typeof(Unicorn) }, // Level 3
                new[] { typeof(EtherealWarrior), typeof(SerpentineDragon) }// Level 4
            }),
            new ChampionSpawnInfo("Vermin Horde", typeof(Barracoon), new[] { "Wrog", "Zabojca", "Pogromca" }, new[] // Vermin Horde
            {
                new[] { typeof(GiantRat), typeof(Slime) }, // Level 1
                new[] { typeof(DireWolf), typeof(Ratman) }, // Level 2
                new[] { typeof(HellHound), typeof(RatmanMage) }, // Level 3
                new[] { typeof(RatmanArcher), typeof(SilverSerpent) }// Level 4
            }),
            new ChampionSpawnInfo("Unholy Terror", typeof(Neira), new[] { "Wrog", "Zabojca", "Pogromca" }, new[] // Unholy Terror
            {
                new[] { typeof(Bogle), typeof(Ghoul), typeof(Shade), typeof(Spectre), typeof(Wraith) }, // Level 1
                new[] { typeof(BoneMagi), typeof(Mummy), typeof(SkeletalMage) }, // Level 2
                new[] { typeof(BoneKnight), typeof(Lich), typeof(SkeletalKnight) }, // Level 3
                new[] { typeof(LichLord), typeof(RottingCorpse) }// Level 4
            }),
            new ChampionSpawnInfo("Sleeping Dragon", typeof(Serado), new[] { "Wrog", "Zabojca", "Pogromca" }, new[] // Sleeping Dragon
            { 
                new[] { typeof(DeathwatchBeetleHatchling), typeof(Lizardman) },
                new[] { typeof(DeathwatchBeetle), typeof(Kappa) },
                new[] { typeof(LesserHiryu), typeof(RevenantLion) },
                new[] { typeof(Hiryu), typeof(Oni) }
            }),
            new ChampionSpawnInfo("Glade", typeof(Twaulo), new[] { "Wrog", "Zabojca", "Pogromca" }, new[] // Glade
            { 
                new[] { typeof(Pixie), typeof(ShadowWisp) },
                new[] { typeof(Centaur), typeof(MLDryad) },
                new[] { typeof(Satyr), typeof(CuSidhe) },
                new[] { typeof(FeralTreefellow), typeof(RagingGrizzlyBear) }
            }),
            new ChampionSpawnInfo("Corrupt", typeof(Ilhenir), new[] { "Wrog", "Zabojca", "Pogromca" }, new[] // Corrupt
            {
                new[] { typeof(PlagueSpawn), typeof(Bogling) },
                new[] { typeof(PlagueBeast), typeof(BogThing) },
                new[] { typeof(PlagueBeastLord), typeof(InterredGrizzle) },
                new[] { typeof(FetidEssence), typeof(PestilentBandage) }
            }),
            new ChampionSpawnInfo("Terror", typeof(AbyssalInfernal), new[] { "Wrog", "Zabojca", "Pogromca" }, new[] // Terror
            {
                new[] { typeof(HordeMinion), typeof(ChaosDaemon) }, 
                new[] { typeof(StoneHarpy), typeof(ArcaneDaemon) }, 
                new[] { typeof(PitFiend), typeof(Moloch) }, 
                new[] { typeof(ArchDaemon), typeof(AbyssalAbomination) }
            }),
            new ChampionSpawnInfo("Infuse", typeof(PrimevalLich), new[] { "Wrog", "Zabojca", "Pogromca" }, new[] // Infused
            { 
                new[] { typeof(GoreFiend), typeof(VampireBat) },
                new[] { typeof(FleshGolem), typeof(DarkWisp) }, 
                new[] { typeof(UndeadGargoyle), typeof(Wight) },
                new[] { typeof(SkeletalDrake), typeof(DreamWraith) }
            }),
            new ChampionSpawnInfo("Valley", typeof(DragonTurtle), new[] { "Wrog", "Zabojca", "Pogromca" }, new[] // Dragon Turtle
            {                                                             
                new[] { typeof(MyrmidexDrone), typeof(MyrmidexLarvae) }, 
                new[] { typeof(SilverbackGorilla), typeof(WildTiger) }, 
                new[] { typeof(GreaterPhoenix), typeof(Infernus) }, 
                new[] { typeof(Dimetrosaur), typeof(Allosaurus) }                                    
            }),
            new ChampionSpawnInfo("Khaldun", typeof(KhalAnkur), new[] { "Wrog", "Zabojca", "Pogromca" }, new[] // Khal Ankur
            {                                                                     
                new[] { typeof(SkelementalKnight), typeof(KhaldunBlood) },                            
                new[] { typeof(SkelementalMage), typeof(Viscera) },                                    
                new[] { typeof(CultistAmbusher), typeof(ShadowFiend) },                                
                new[] { typeof(KhalAnkurWarriors) }                                                    
            }),
            // MeraktusTheTormented - keeping original format
            new ChampionSpawnInfo("Glade", typeof(Meraktus), new[] { "Pogromca", "Pomsta", "Nemesis" }, new[] // Merek
            {
                new[] { typeof(Minotaur), typeof(ShadowWisp) },
                new[] { typeof(NPrzeklety), typeof(MinotaurCaptain) },
                new[] { typeof(NZapomniany), typeof(MinotaurMage) },
                new[] { typeof(SilverSerpent), typeof(MinotaurLord) }
            }),
            // Properly formatting Pyre but keeping original content
            new ChampionSpawnInfo("Ogniste Ptaszysko", typeof(Pyre), new[] { "Rywal", "Pogromca", "Antagonista" }, new[]
            {
                new[] { typeof(FireElemental), typeof(OgnistyWojownik), typeof(OgnistyNiewolnik) },
                new[] { typeof(DullCopperElemental), typeof(FireGargoyle), typeof(GargoyleEnforcer) },
                new[] { typeof(EnslavedGargoyle), typeof(OgnistySmok), typeof(FireBeetle) },
                new[] { typeof(FireSteed), typeof(PrastaryOgnistySmok), typeof(Phoenix) }
            }),
            // Properly formatting Morena entry
            new ChampionSpawnInfo("Morena", typeof(MorenaAwatar), new[] { "Rywal", "Pogromca", "Antagonista" }, new[]
            {
                new[] { typeof(Ghoul), typeof(Skeleton), typeof(PatchworkSkeleton) },
                new[] { typeof(WailingBanshee), typeof(BoneMagi), typeof(BoneKnight) },
                new[] { typeof(LichLord), typeof(FleshGolem), typeof(Mummy2) },
                new[] { typeof(SkeletalDragon), typeof(RottingCorpse), typeof(AncientLich) }
            }),
            // Properly formatting OrcCommander entry
            new ChampionSpawnInfo("Kapitan Legionu Orkow", typeof(KapitanIIILegionuOrkow), new[] { "Rywal", "Pogromca", "Antagonista" }, new[]
            {
                new[] { typeof(Orc), typeof(Ratman), typeof(Goblin) },
                new[] { typeof(OrcishMage), typeof(LesserGoblinSapper), typeof(Troll) },
                new[] { typeof(JukaWarrior), typeof(OrcCaptain), typeof(TrollLord) },
                new[] { typeof(JukaMage), typeof(OrcBomber), typeof(OgreLord) }
            })
        };

        public static ChampionSpawnInfo GetInfo(ChampionSpawnType type)
        {
            int v = (int)type;

            if (v < 0 || v >= m_Table.Length)
            {
                // Handle special case for MeraktusTheTormented which has value 100
                if (type == ChampionSpawnType.MeraktusTheTormented)
                    return m_Table[14]; // Index for Meraktus
                
                v = 0;
            }

            return m_Table[v];
        }
    }
}
