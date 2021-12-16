using System;
using Server;

namespace Server.ACC.CSS.Systems.Cleric
{
	public class ClericInitializer : BaseInitializer
	{
		public static void Configure()
		{
			Register( typeof( ClericAngelicFaithSpell ),  "Anielska Wiara",   "Rzucający wzywa boskie moce niebios, aby przekształciły się w świętego anioła. Rzucający zyskuje lepsze tempo regeneracji oraz zwiększone statystyki i umiejętności.", null, "Mana: 50; Skill: 80; Tithing: 100", 2295,  3500, School.Cleric );
			Register( typeof( ClericBanishEvilSpell ),    "Wygnanie Zła",     "Rzucający przywołuje boski ogień, aby wygnać z ziemi swojego nieumarłego lub demonicznego wroga.",                                                                                   null, "Mana: 40; Skill: 60; Tithing: 30",  20739, 3500, School.Cleric );
			Register( typeof( ClericDampenSpiritSpell ),  "Stłumienie Ducha",   "Wróg rzucającego powoli traci swoją wytrzymałość, znacznie utrudniając mu walkę w walce lub ucieczkę.",                                                           null, "Mana: 11; Skill: 35; Tithing: 15",  2270,  3500, School.Cleric );
			Register( typeof( ClericDivineFocusSpell ),   "Boskie Skupienie",    "Umysł rzucającego skupia się na jego boskiej wierze, zwiększając efekt jego modlitw. Jednak rzucający staje się psychicznie zmęczony znacznie szybciej.",                            null, "Mana: 4;  Skill: 35; Tithing: 15",  2276,  3500, School.Cleric );
			Register( typeof( ClericHammerOfFaithSpell ), "Topór Wiary", "Przyzywa boską broń czystej energii, obdarzony zdolnością do pokonywania nieumarłych wrogów z większą skutecznością.",                                                     null, "Mana: 14; Skill: 40; Tithing: 20",  20741, 3500, School.Cleric );
			Register( typeof( ClericPurgeSpell ),         "Czystka",           "Cel jest wyleczony ze wszystkich trucizn i ma usunięte wszystkie klątwy.",                                                                                               null, "Mana: 20;  Skill: 70; Tithing: 5",   20744, 3500, School.Cleric );
			Register( typeof( ClericRestorationSpell ),   "Odrodzenie",     "Cel rzucającego zostaje wskrzeszony, w pełni uleczony i odświeżony.",                                                                                                         null, "Mana: 50; Skill: 100; Tithing: 40",  2298,  3500, School.Cleric );
			Register( typeof( ClericSacredBoonSpell ),    "Święty znak",     "Otoczeni przez magię wydobywajacą się z magicznego znaku, jesteście leczeni przezeń.",                                                                        null, "Mana: 20; Skill: 25; Tithing: 15",  20742, 3500, School.Cleric );
			Register( typeof( ClericSacrificeSpell ),     "Poświęcenie",       "Czarujący poświęca się dla innych. Czarujący otrzymując obrażenia, leczy swych kompanów.",           null, "Mana: 4;  Skill: 5;  Tithing: 5",   20743, 3500, School.Cleric );
			Register( typeof( ClericSmiteSpell ),         "Smagnięcie",           "Czarujący przyzywa śmiercionośny piorun, który rani jego wrogów.",                                                                            null, "Mana: 35; Skill: 80; Tithing: 60",  2269,  3500, School.Cleric );
			Register( typeof( ClericTouchOfLifeSpell ),   "Dotyk Życia",   "Cel czarującego jest leczony mocą z niebios.",                                                                                                     null, "Mana: 20;  Skill: 30; Tithing: 10",  2243,  3500, School.Cleric );
			Register( typeof( ClericTrialByFireSpell ),   "Próba Ognia",   "Święty ogień otacza czarującego. Jeśli ten uderzony zostaje bronia, to część obrażeń jest oddawana.",                                                                           null, "Mana: 9;  Skill: 45; Tithing: 25",  20736, 3500, School.Cleric );
		}
	}
}