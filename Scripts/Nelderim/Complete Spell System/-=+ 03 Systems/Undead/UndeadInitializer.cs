using System;
using Server;

namespace Server.ACC.CSS.Systems.Undead
{
    public class UndeadInitializer : BaseInitializer
    {
		public static void Configure()
		{
            Register( typeof( UndeadLeafWhirlwindSpell ),    "Piętno",    "Podmuch mroku, zbierając kości, które zapamiętują, skąd pochodzą, zaznaczają runę dla rzucającego.", "Spring Water; Petrafied Wood; Destroying Angel", "Mana: 25;", 2271,  5120, School.Undead );
            Register( typeof( UndeadHollowReedSpell ),       "Hedonizm",       "Zwiększa zarówno Siłę, jak i Inteligencję czarującego.",                                                       "Bloodmoss; Mandrake Root; Nightshade",           "Mana: 30;", 2255,  5120, School.Undead );
            Register( typeof ( UndeadSeanceSpell ),      "Seans",    "Pozwala magowi poruszać się jako duch.",                                 "Bloodmoss; Black Pearl; Petrafied Wood",         "Mana: 50; Skill: 80", 20491, 5120, School.Undead );
            Register( typeof( UndeadGraspingRootsSpell ),    "Uchwyt Zza Grobu",    "Przywołuje rękę z ziemi, aby splątać pojedynczy cel.",                                                           "Spring Water; Bloodmoss; Spider's Silk",         "Mana: 40;", 2293,  5120, School.Undead );
            Register( typeof( UndeadSwarmOfInsectsSpell ),   "Chmara Insektów", "Przywołuje rój owadów, które gryzą i kąsają wrogów.",                                                  "Garlic; Nightshade; DestroyingAngel",            "Mana: 10;", 2272,  5120, School.Undead );
            Register( typeof( UndeadVolcanicEruptionSpell ), "Erupcja Wulkaniczna", "Podmuch stopionej lawy tryska z ziemi, uderzając w każdego w pobliżu.",                                           "Sulfurous Ash; Destroying Angel",                "Mana: 85;", 2296,  5120, School.Undead );
            Register( typeof( UndeadLureStoneSpell ),        "Gnijące Zwłoki",        "Tworzy zwłoki, który przywołują do siebie wszystkie pobliskie zwierzęta.",                                                         "Black Pearl; Spring Water",                      "Mana: 30;", 2294,  5120, School.Undead );
            Register( typeof( UndeadNaturesPassageSpell ),   "Ścieżka Śmierci",  "Mag zostaje zamieniony w pył i niesiony wiatrem do miejsca przeznaczenia.",                                 "Black Pearl; Bloodmoss; Mandrake Root",          "Mana: 10;", 2297,  5120, School.Undead );
            Register( typeof( UndeadMushroomGatewaySpell ),  "Limbo",  "Otwiera się magiczny krąg, pozwalając magowi przejść przez niego do innego miejsca.",                      "Black Pearl; Spring Water; Mandrake Root",       "Mana: 40;", 2291,  5120, School.Undead );
           Register( typeof( UndeadAngelicFaithSpell ),         "Awatar Smierci",   "Rzucający wzywa boskie moce podziemi, aby przekształciły się w awatara śmierci. Rzucający zyskuje mniejszy koszt many i szybsze ukonczenie zaklec oraz zwiększone statystyki i umiejętności, kosztem jego odproności.",                                                       "Mandrake Root; Spring Water; Petrafied Wood",    "Mana: 17;", 2295,  5120, School.Undead );
            Register( typeof( UndeadHammerOfFaithSpell ),      "Sierp Wiary Smierci",      "Przyzywa broń czystej energii, obdarzoną zdolnością do pokonywania tajemniczych wrogów z większą skutecznością.",                                                    "Black Pearl; Ginseng; Spring Water",             "Mana: 45; Skill: 60", 2263,  5120, School.Undead );
            Register( typeof( UndeadCauseFearSpell ),  "Strach",  "Nic nie wiadomo o tym zaklęciu. Ponoń powoduje strach.",                                       "Garlic; Ginseng; Spring Water",                  "Mana: 60;", 2298,  5120, School.Undead );
    	}
	}
}
