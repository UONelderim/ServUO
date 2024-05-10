using Server.ACC.CSS;

namespace Server.Spells.DeathKnight
{
	public class DeathKnightInitializer : BaseInitializer
	{
		public static void Configure()
		{
			Register(typeof(BanishSpell),
				"Wygnanie",
				"Odwołuje przyzwane stworzenia (działa dokładnie tak samo jak czar Dispell z Magii). Sukces zależy od siły stwora i ujemnej karmy postaci.",
				"Koszt: 56 Dusz",
				"Mana: 36 ; Skill: 40",
				2244,
				5054,
				School.DeathKnight);
			Register(typeof(DemonicTouchSpell),
				"Dotyk Demona",
				"Czar leczy wybrany cel. Siła czaru zależna od ujemnej karmy.",
				"Koszt: 21 Dusz",
				"Mana: 16 ; Skill: 15",
				20736,
				5054,
				School.DeathKnight);
			Register(typeof(DevilPactSpell),
				"Pakt Ze Smiercia",
				"Przyzywa demona (który nie jest kontrolowany przez gracza, zupełnie jak wir energii w zaklęciach Magii). Trwanie zależne od ujemnej karmy.",
				"Koszt: 98 Dusz",
				"Mana: 30; Skill: 90",
				20491,
				5054,
				School.DeathKnight);
			Register(typeof(GrimReaperSpell),
				"Ponury Zniwiarz",
				"Zwiększa zadawane obrażenia danemu typowi potworów (działanie takie jak Enemy of One z Rycerstwa). Trwanie zależne od ujemnej karmy.",
				"Koszt: 42 Dusz",
				"Mana: 28 ; Skill: 30",
				2257,
				5054,
				School.DeathKnight);
			Register(typeof(HagHandSpell),
				"Reka Wiedzmy",
				"Zdejmuje klątwy nekromanckie. Powodzenie zależne od ujemnej karmy.",
				"Koszt: 7 Dusz",
				"Mana: 8 ; Skill: 5",
				21001,
				5054,
				School.DeathKnight);
			Register(typeof(HellfireSpell),
				"Ogien Piekielny",
				"Zadaje celowi obrażenia od ognia. Siła zależna od poziomu ujemnej karmy.",
				"Koszt: 84 Dusz",
				"Mana: 52 ; Skill: 70",
				2281,
				5054,
				School.DeathKnight);
			Register(typeof(LucifersBoltSpell),
				"Promien Smierci",
				"Paraliżuje cel. Siła zależna od poziomu ujemnej karmy.",
				"Koszt: 35 Dusz",
				"Mana: 24; Skill: 25",
				20488,
				5054,
				School.DeathKnight);
			Register(typeof(OrbOfOrcusSpell),
				"Kula Smierci",
				"Dodaje tymczasowo niewielką absorbcję czarów.",
				"Koszt: 200 Dusz",
				"Mana: 26; Skill: 80",
				20745,
				5054,
				School.DeathKnight);
			Register(typeof(ShieldOfHateSpell),
				"Tarcza Nienawisci",
				"Można rzucić na wybrany cel. Odporność fizyczna rośnie do 80, odporność na ogień spada o 10, na trucizny o 20, na zimno o 10. Czas trwania zależny od ujemnej karmy",
				"Koszt: 77 Dusz",
				"Mana: 48; Skill: 60",
				20745,
				5054,
				School.DeathKnight);
			Register(typeof(SoulReaperSpell),
				"Zniwarz Dusz",
				"Pozbawia cel many. Siła zależna od ujemnej karmy.",
				"Koszt: 63 Dusz",
				"Mana: 40; Skill: 45",
				20491,
				5054,
				School.DeathKnight);
			Register(typeof(StrengthOfSteelSpell),
				"Wytrzymalosc Stali",
				"Dodaje siły wybranemu celowi. Siła czaru oraz czas trwania zależne od ujemnej karmy.",
				"Koszt: 28 Dusz",
				"Mana: 20; Skill: 20",
				20491,
				5054,
				School.DeathKnight);
			Register(typeof(StrikeSpell),
				"Uderzenie",
				"Powoduje wybuch energetyczny na danym celu. Siła zależna od ujemnej karmy",
				"Koszt: 140 Dusz",
				"Mana: 30; Skill: 80",
				20491,
				5054,
				School.DeathKnight);
			Register(typeof(SuccubusSkinSpell),
				"Skora Sukkuba",
				"Leczy cel o małe ilości życia co kilka sekund. Siła i trwanie zależne od ujemnej karmy.",
				"Koszt: 49 Dusz",
				"Mana: 32; Skill: 68",
				20491,
				5054,
				School.DeathKnight);
			Register(typeof(WeakSpotSpell),
				"Slaby Punkt",
				"",
				"Koszt: 10 Dusz",
				"Mana: 10; Skill: 15",
				20491,
				5054,
				School.DeathKnight);
			Register(typeof(WrathSpell),
				"Gniew",
				"Powoduje obszarowy atak piorunami w promieniu 5 kratek. Siła zależna od ujemnej karmy.",
				"Koszt: 700 Dusz",
				"Mana: 64; Skill: 100",
				20491,
				5054,
				School.DeathKnight);
		}
	}
}
