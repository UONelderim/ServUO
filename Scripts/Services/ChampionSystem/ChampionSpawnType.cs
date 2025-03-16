using Server.Mobiles;
using System;
using System.Collections.Generic;

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

        public static Dictionary<ChampionSpawnType, ChampionSpawnInfo> Table => m_Table;

        private static readonly Dictionary<ChampionSpawnType, ChampionSpawnInfo> m_Table = new()
        {
	        {
		        ChampionSpawnType.Abyss, new ChampionSpawnInfo("Abyss",
			        typeof(Semidar),
			        ["Wrog", "Zabojca", "Pogromca"],
			        [
				        [typeof(GreaterMongbat), typeof(Imp)],
				        [typeof(Gargoyle), typeof(Harpy)],
				        [typeof(FireGargoyle), typeof(StoneGargoyle)],
				        [typeof(Daemon), typeof(Succubus)]
			        ])
	        },
	        {
		        ChampionSpawnType.Arachnid, new ChampionSpawnInfo("Arachnid",
			        typeof(Mephitis),
			        ["Wrog", "Zabojca", "Pogromca"],
			        [
				        [typeof(Scorpion), typeof(GiantSpider)],
				        [typeof(TerathanDrone), typeof(TerathanWarrior)],
				        [typeof(DreadSpider), typeof(TerathanMatriarch)],
				        [typeof(PoisonElemental), typeof(TerathanAvenger)]
			        ])
	        },
	        {
		        ChampionSpawnType.ColdBlood, new ChampionSpawnInfo("Cold Blood",
			        typeof(Rikktor),
			        ["Wrog", "Zabojca", "Pogromca"],
			        [
				        [typeof(Lizardman), typeof(Snake)],
				        [typeof(LavaLizard), typeof(OphidianWarrior)],
				        [typeof(Drake), typeof(OphidianArchmage)],
				        [typeof(Dragon), typeof(OphidianKnight)]
			        ])
	        },
	        {
		        ChampionSpawnType.ForestLord, new ChampionSpawnInfo("Forest Lord",
			        typeof(LordOaks),
			        ["Wrog", "Zabojca", "Pogromca"],
			        [
				        [typeof(Pixie), typeof(ShadowWisp)],
				        [typeof(Kirin), typeof(Wisp)],
				        [typeof(Centaur), typeof(Unicorn)],
				        [typeof(EtherealWarrior), typeof(SerpentineDragon)]
			        ])
	        },
	        {
		        ChampionSpawnType.VerminHorde, new ChampionSpawnInfo("Vermin Horde",
			        typeof(Barracoon),
			        ["Wrog", "Zabojca", "Pogromca"],
			        [
				        [typeof(GiantRat), typeof(Slime)],
				        [typeof(DireWolf), typeof(Ratman)],
				        [typeof(HellHound), typeof(RatmanMage)],
				        [typeof(RatmanArcher), typeof(SilverSerpent)]
			        ])
	        },
	        {
		        ChampionSpawnType.UnholyTerror, new ChampionSpawnInfo("Unholy Terror",
			        typeof(Neira),
			        ["Wrog", "Zabojca", "Pogromca"],
			        [
				        [typeof(Bogle), typeof(Ghoul), typeof(Shade), typeof(Spectre), typeof(Wraith)],
				        [typeof(BoneMagi), typeof(Mummy), typeof(SkeletalMage)],
				        [typeof(BoneKnight), typeof(Lich), typeof(SkeletalKnight)],
				        [typeof(LichLord), typeof(RottingCorpse)]
			        ])
	        },
	        {
		        ChampionSpawnType.SleepingDragon, new ChampionSpawnInfo("Sleeping Dragon",
			        typeof(Serado),
			        ["Wrog", "Zabojca", "Pogromca"],
			        [
				        [typeof(DeathwatchBeetleHatchling), typeof(Lizardman)],
				        [typeof(DeathwatchBeetle), typeof(Kappa)],
				        [typeof(LesserHiryu), typeof(RevenantLion)],
				        [typeof(Hiryu), typeof(Oni)]
			        ])
	        },
	        {
		        ChampionSpawnType.Glade, new ChampionSpawnInfo("Glade",
			        typeof(Twaulo),
			        ["Wrog", "Zabojca", "Pogromca"],
			        [
				        [typeof(Pixie), typeof(ShadowWisp)], [typeof(Centaur), typeof(MLDryad)],
				        [typeof(Satyr), typeof(CuSidhe)],
				        [typeof(FeralTreefellow), typeof(RagingGrizzlyBear)]
			        ])
	        },
	        {
		        ChampionSpawnType.Corrupt, new ChampionSpawnInfo("Corrupt",
			        typeof(Ilhenir),
			        ["Wrog", "Zabojca", "Pogromca"],
			        [
				        [typeof(PlagueSpawn), typeof(Bogling)], [typeof(PlagueBeast), typeof(BogThing)],
				        [typeof(PlagueBeastLord), typeof(InterredGrizzle)],
				        [typeof(FetidEssence), typeof(PestilentBandage)]
			        ])
	        },
	        {
		        ChampionSpawnType.Terror, new ChampionSpawnInfo("Terror",
			        typeof(AbyssalInfernal),
			        ["Wrog", "Zabojca", "Pogromca"],
			        [
				        [typeof(HordeMinion), typeof(ChaosDaemon)],
				        [typeof(StoneHarpy), typeof(ArcaneDaemon)], [typeof(PitFiend), typeof(Moloch)],
				        [typeof(ArchDaemon), typeof(AbyssalAbomination)]
			        ])
	        },
	        {
		        ChampionSpawnType.Infuse, new ChampionSpawnInfo("Infuse",
			        typeof(PrimevalLich),
			        ["Wrog", "Zabojca", "Pogromca"],
			        [
				        [typeof(GoreFiend), typeof(VampireBat)], [typeof(FleshGolem), typeof(DarkWisp)],
				        [typeof(UndeadGargoyle), typeof(Wight)],
				        [typeof(SkeletalDrake), typeof(DreamWraith)]
			        ])
	        },
	        {
		        ChampionSpawnType.DragonTurtle, new ChampionSpawnInfo("Valley",
			        typeof(DragonTurtle),
			        ["Wrog", "Zabojca", "Pogromca"],
			        [
				        [typeof(MyrmidexDrone), typeof(MyrmidexLarvae)],
				        [typeof(SilverbackGorilla), typeof(WildTiger)],
				        [typeof(GreaterPhoenix), typeof(Infernus)],
				        [typeof(Dimetrosaur), typeof(Allosaurus)]
			        ])
	        },
	        {
		        ChampionSpawnType.Khaldun, new ChampionSpawnInfo("Khaldun",
			        typeof(KhalAnkur),
			        ["Wrog", "Zabojca", "Pogromca"],
			        [
				        [typeof(SkelementalKnight), typeof(KhaldunBlood)],
				        [typeof(SkelementalMage), typeof(Viscera)],
				        [typeof(CultistAmbusher), typeof(ShadowFiend)], [typeof(KhalAnkurWarriors)]
			        ])
	        },
	        {
					ChampionSpawnType.MeraktusTheTormented, new ChampionSpawnInfo("Minotaur",
						typeof(Meraktus),
						["Pogromca", "Pomsta", "Nemesis"],
						[
							[typeof(Minotaur), typeof(ShadowWisp)],
							[typeof(NPrzeklety), typeof(MinotaurCaptain)],
							[typeof(NZapomniany), typeof(MinotaurMage)],
							[typeof(SilverSerpent), typeof(MinotaurLord)]
						])
				},
				{
					ChampionSpawnType.Pyre, new ChampionSpawnInfo("Ogniste Ptaszysko",
						typeof(Pyre),
						["Rywal", "Pogromca", "Antagonista"],
						[
							[typeof(FireElemental), typeof(OgnistyWojownik), typeof(OgnistyNiewolnik)],
							[typeof(DullCopperElemental), typeof(FireGargoyle), typeof(GargoyleEnforcer)],
							[typeof(EnslavedGargoyle), typeof(OgnistySmok), typeof(FireBeetle)],
							[typeof(FireSteed), typeof(PrastaryOgnistySmok), typeof(Feniks)]
						])
				},
				{
					ChampionSpawnType.Morena, new ChampionSpawnInfo("Morena",
						typeof(MorenaAwatar),
						["Rywal", "Pogromca", "Antagonista"],
						[
							[typeof(Ghoul), typeof(Skeleton), typeof(PatchworkSkeleton)],
							[typeof(WailingBanshee), typeof(BoneMagi), typeof(BoneKnight)],
							[typeof(LichLord), typeof(FleshGolem), typeof(Mummy2)],
							[typeof(SkeletalDragon), typeof(RottingCorpse), typeof(AncientLich)]
						])
				},
				{
					ChampionSpawnType.OrcCommander, new ChampionSpawnInfo("Kapitan Legionu Orkow",
						typeof(KapitanIIILegionuOrkow),
						["Rywal", "Pogromca", "Antagonista"],
						[
							[typeof(Orc), typeof(Ratman), typeof(Goblin)],
							[typeof(OrcishMage), typeof(LesserGoblinSapper), typeof(Troll)],
							[typeof(JukaWarrior), typeof(OrcCaptain), typeof(TrollLord)],
							[typeof(JukaMage), typeof(OrcBomber), typeof(OgreLord)]
						])
				}
        };

        public static ChampionSpawnInfo GetInfo(ChampionSpawnType type)
        {
	        if (Table.TryGetValue(type, out var info))
	        {
		        return info;
	        }
			
	        Console.WriteLine($"Unable to get ChampionSpawnInfo for {type}");
	        return Table[ChampionSpawnType.Abyss];
        }
    }
}
