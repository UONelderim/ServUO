namespace Server.ACC.CSS.Systems.Ancient
{
	public class AncientInitializer : BaseInitializer
	{
		public static void Configure()
		{
			Register(typeof(AncientThunderSpell), "Piorun", "Zwiastuje burze. Powoduje uderzenie pioruna.",
				"Sulfurous Ash", null, 21540, 9270, School.Ancient);
			Register(typeof(AncientWeatherSpell), "Zmiana pogody", "Może wywołać burzę lub zatrzymać istniejącą burzę.",
				"Sulfurous Ash", null, 23004, 9270, School.Ancient);
			Register(typeof(AncientIgniteSpell), "Podpalenie",
				"Generuje mały pocisk iskier, który może zapalić łatwopalny materiał.", "Black Pearl", null, 2257, 9270,
				School.Ancient);
			Register(typeof(AncientDouseSpell), "Wygaszenie", "Gasi każdy mały, niemagiczny ogień.", "Bloodmoss", null,
				2256, 9270, School.Ancient);
			Register(typeof(AncientGreatDouseSpell), "Większe Wygaszenie", "Silniejsza wersja zaklęcia Wygaszenie.",
				"Garlic, Spider's Silk", null, 20999, 9270, School.Ancient);
			Register(typeof(AncientGreatIgniteSpell), "Większe Podpalenie", "Silniejsza wersja zaklęcia Podpalenie.",
				"Sulfurous Ash, Spider's Silk", null, 21256, 9270, School.Ancient);
			Register(typeof(AncientEnchantSpell), "Magiczne nasycenie",
				"Sprawia, że broń dystansowa staje się zaczarowana i świeci na niebiesko. Ta broń staje się silniejsza i celniejsza.",
				"Black Pearl, Mandrake Root", null, 23003, 9270, School.Ancient);
			Register(typeof(AncientSwarmSpell), "Rój",
				"Przywołuje roje owadów, które atakują wrogów maga ze wszystkich kierunków.",
				"Nightshade, Mandrake Root, Bloodmoss", null, 20740, 9270, School.Ancient);
			Register(typeof(AncientPeerSpell), "Wizja", "Umożliwia magowi opuszczenie jego ciała i zbadanie okolicy.",
				"Mandrake Root, Nightshade", null, 2270, 9270, School.Ancient);
			Register(typeof(AncientSeanceSpell), "Seans", "Pozwala magowi poruszać się jako duch.",
				"Bloodmoss, Spider's Silk, Mandrake Root, Nightshade, Sulfurous Ash", null, 23014, 9270,
				School.Ancient);
			Register(typeof(AncientDanceSpell), "Taniec",
				"Sprawia, że wszyscy w zasięgu wzroku (oprócz maga i jego drużyny) zaczynają tańczyć.",
				"Garlic, Bloodmoss, Mandrake Root", null, 23005, 9270, School.Ancient);
			Register(typeof(AncientCloneSpell), "Klonowanie",
				"Tworzy dokładny duplikat dowolnego śmiertelnego stworzenia, które będzie walczyć po stronie maga.",
				"Sulfurous Ash, Spider's Silk, Bloodmoss, Ginseng, Nightshade,Mandrake Rook", null, 2261, 9270,
				School.Ancient);
			Register(typeof(AncientCauseFearSpell), "Strach", "Nic nie wiadomo o tym zaklęciu. Ponoń powoduje strach.",
				"Garlic, Nightshade, Mandrake Root", null, 2286, 9270, School.Ancient);
			Register(typeof(AncientFireRingSpell), "Pierścień Ognia", "Tworzy pierścień ognia, który otoczy cel maga.",
				"Black Pearl, Spider's Silk, Sulfurous Ash, Mandrake Root", null, 2302, 9270, School.Ancient);
			Register(typeof(AncientMassMightSpell), "Masowa Potęga", "Rzuca Bless na wszystkich członków drużyny maga.",
				"Black Pearl, Mandrake Root, Ginseng", null, 23001, 9270, School.Ancient);
			Register(typeof(AncientDeathVortexSpell), "Wir Śmierci",
				"Tworzy wirujący czarny wir w miejscu wyznaczonym przez maga, który następnie porusza się losowo.",
				"Bloodmoss, Sulfurous Ash, Mandrake Root, Nightshade", null, 21541, 9270, School.Ancient);
			Register(typeof(AncientMassDeathSpell), "Mass Death", "Kills everything within the mage's sight",
				"Bloodmoss, Ginseng, Garlic, Mandrake Root, Nightshade", null, 2285, 9270, School.Ancient);
			Register(typeof(AncientFireworksSpell), "Fireworks",
				"Creates an impressive display of multi-colored moving lights.", "Sulfurous Ash", null, 2282, 9270,
				School.Ancient);
			Register(typeof(AncientGlimmerSpell), "Glimmer",
				"Creates a small light source that lasts for a short period of time.", "Bloodmoss", null, 21280, 9270,
				School.Ancient);
			Register(typeof(AncientAwakenSpell), "Awaken", "Awakens one sleeping or unconscious creature.",
				"Sulfurous Ash", null, 2242, 9270, School.Ancient);
			Register(typeof(AncientLocateSpell), "Locate", "Reveals the position of the mage, even when underground.",
				"Nightshade", null, 2260, 9270, School.Ancient);
			Register(typeof(AncientAwakenAllSpell), "Awaken All",
				"Awakens all unconscious members of the mage's party.", "Garlic, Ginseng", null, 2292, 9270,
				School.Ancient);
			Register(typeof(AncientDetectTrapSpell), "Detect Trap", "Allows the mage to see any nearby traps.",
				"Bloodmoss, Sulfurous Ash", null, 20496, 9270, School.Ancient);
			Register(typeof(AncientFalseCoinSpell), "False Coin",
				"When cast upon any coin, this spell creates five duplicate coins.", "Nightshade, Sulfurous Ash", null,
				20481, 9270, School.Ancient);
			Register(typeof(AncientGreatLightSpell), "Great Light",
				"A more potent version of Nightsight, and has a substantially longer duration.",
				"Sulfurous Ash, Mandrake Root", null, 2245, 9270, School.Ancient);
			Register(typeof(AncientDestroyTrapSpell), "Destroy Trap",
				"Destroys any one specific trap upon which it is cast.", "Bloodmoss, Sulfurous Ash", null, 20994, 9270,
				School.Ancient);
			Register(typeof(AncientSleepSpell), "Sleep", "Causes the enchanted person to fall asleep.",
				"Spider's Silk, Nightshade, Black Pearl", null, 2277, 9270, School.Ancient);
			Register(typeof(AncientCharmSpell), "Charm",
				"Can be used either to control an enemy or creature, or to free a charmed one.",
				"Black Pearl, Nightshade, Spider's Silk", null, 23015, 9270, School.Ancient);
			Register(typeof(AncientTremorSpell), "Tremor",
				"Creates violent tremors in the earth that will cause the mage's enemies to tremble frantically.",
				"Bloodmoss, Mandrake Root, Sulfurous Ash", null, 2296, 9270, School.Ancient);
			Register(typeof(AncientSleepFieldSpell), "Sleep Field",
				"Creates a thick wall of energy field where the mage desires. All who enter this energy field will fall asleep.",
				"Black Pearl, Ginseng, Spider's Silk", null, 2283, 9270, School.Ancient);
			Register(typeof(AncientMassCharmSpell), "Mass Charm",
				"Similar to Charm, but it affects more powerful monsters, based on the mage's intellect.",
				"Black Pearl, Nightshade, Spider's Silk, Mandrake Root", null, 21000, 9270, School.Ancient);
			Register(typeof(AncientInvisibilityAllSpell), "Invisibility All",
				"Casts Invisibility upon the mage and everyone in his party.",
				"Nightshade, Bloodmoss, Black Pearl, Mandrake Root", null, 23012, 9270, School.Ancient);
		}
	}
}
