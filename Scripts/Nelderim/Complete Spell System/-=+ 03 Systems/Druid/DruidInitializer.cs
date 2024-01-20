namespace Server.ACC.CSS.Systems.Druid
{
	public class DruidInitializer : BaseInitializer
	{
		public static void Configure()
		{
			Register(typeof(DruidLeafWhirlwindSpell), "Wir Liści",
				"Podmuch wiatru wieje, zbierając magiczne liście, które zapamiętują, skąd pochodzą, zaznaczając runę dla rzucającego.",
				"Spring Water; Petrafied Wood; Destroying Angel", "Mana: 25; Skill: 50", 2271, 5120, School.Druid);
			Register(typeof(DruidHollowReedSpell), "Siła Natury", "Zwiększa zarówno Siłę, jak i Inteligencję maga.",
				"Bloodmoss; Mandrake Root; Nightshade", "Mana: 30; Skill: 30", 2255, 5120, School.Druid);
			Register(typeof(DruidPackOfBeastSpell), "Leśne Bestyje",
				"Przywołuje stado bestii, aby walczyły po stronie maga. Długość zaklęcia rośnie wraz z umiejętnościami.",
				"Bloodmoss; Black Pearl; Petrafied Wood", "Mana: 45; Skill: 40", 20491, 5120, School.Druid);
			Register(typeof(DruidSpringOfLifeSpell), "Źródło życia",
				"Tworzy magiczne źródło, które leczy maga i drużynę.", "Spring Water", "Mana: 60; Skill: 40", 2268,
				5120, School.Druid);
			Register(typeof(DruidGraspingRootsSpell), "Szalone Korzenie",
				"Przywołuje korzenie z ziemi, aby splątać pojedynczy cel.", "Spring Water; Bloodmoss; Spider's Silk",
				"Mana: 40; Skill: 40", 2293, 5120, School.Druid);
			Register(typeof(DruidBlendWithForestSpell), "Jedność Z Lasem",
				"Mag płynnie wtapia się w tło, stając się niewidzialny dla wrogów.", "Bloodmoss; Nightshade",
				"Mana: 60; Skill: 75", 2249, 5120, School.Druid);
			Register(typeof(DruidSwarmOfInsectsSpell), "Chmara Insektów",
				"Przywołuje rój owadów, które gryzą i kąsają wrogów.", "Garlic; Nightshade; DestroyingAngel",
				"Mana: 10; Skill: 85", 2272, 5120, School.Druid);
			Register(typeof(DruidVolcanicEruptionSpell), "Erupcja Wulkaniczna",
				"Podmuch stopionej lawy tryska z ziemi, uderzając w każdego wroga w pobliżu.",
				"Sulfurous Ash; Destroying Angel", "Mana: 85; Skill: 98", 2296, 5120, School.Druid);
			Register(typeof(DruidFamiliarSpell), "Przywołanie Przyjaciela Lasu",
				"Przywołuje wybór różnych chowańców, które mogą pomóc magowi.",
				"Mandrake Root; Spring Water; Petrafied Wood", "Mana: 17; Skill: 30", 2295, 5120, School.Druid);
			Register(typeof(DruidStoneCircleSpell), "Kamienny Krąg",
				"Tworzy nieprzekraczalny krąg kamieni, idealny do uwięzienia wrogów.",
				"Black Pearl; Ginseng; Spring Water", "Mana: 45; Skill: 60", 2263, 5120, School.Druid);
			Register(typeof(DruidEnchantedGroveSpell), "Zaklęty Gaj",
				"Tworzy gaj drzew wokół czarujacego, przywracając witalność i manę.",
				"Mandrake Root; Petrafied Wood; Spring Water", "Mana: 70; Skill: 78", 2280, 5120, School.Druid);
			Register(typeof(DruidLureStoneSpell), "Ciekawy Kamień",
				"Tworzy magiczny kamień, który przywołuje do niego wszystkie pobliskie zwierzęta.",
				"Black Pearl; Spring Water", "Mana: 30; Skill: 15", 2294, 5120, School.Druid);
			Register(typeof(DruidNaturesPassageSpell), "Naznaczenie",
				"Mag zostaje zamieniony w płatki kwiatów i niesiony wiatrem do miejsca przeznaczenia.",
				"Black Pearl; Bloodmoss; Mandrake Root", "Mana: 10; Skill: 15", 2297, 5120, School.Druid);
			Register(typeof(DruidMushroomGatewaySpell), "Przejście Natury",
				"Otwiera się magiczny krąg grzybów, pozwalając magowi przejść przez niego do innego miejsca.",
				"Black Pearl; Spring Water; Mandrake Root", "Mana: 40; Skill: 70", 2291, 5120, School.Druid);
			Register(typeof(DruidRestorativeSoilSpell), "Lecznicza Ziemia",
				"Nasyca skrawek ziemi mocą, powodując przenikanie życiodajnego błota.", "Garlic; Ginseng; Spring Water",
				"Mana: 60; Skill: 89", 2298, 5120, School.Druid);
			Register(typeof(DruidShieldOfEarthSpell), "Tarcza Ziemi",
				"Szybko rosnąca ściana liści wyrasta na rozkaz maga.", "Ginseng; Spring Water", "Mana: 15; Skill: 20",
				2254, 5120, School.Druid);
		}
	}
}
