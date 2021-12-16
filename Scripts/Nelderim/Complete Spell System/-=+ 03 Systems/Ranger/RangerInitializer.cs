using System;
using Server;

namespace Server.ACC.CSS.Systems.Ranger
{
    public class RangerList : BaseInitializer
    {
		public static void Configure()
		{
			Register( typeof( RangerHuntersAimSpell ),    "Celność łowcy",     "Zwiększa łucznictwo strażników i taktykę na krótki czas.",                                                             "Wilcze Jagody; Woda wiosenna; Krwawy Mech",            "Mana: 25; Skill: 50", 2244,  5054, School.Ranger );
			Register( typeof( RangerPhoenixFlightSpell ), "Lot Feniksa",   "Wzywa Feniksa, który przeniesie cię w wybrane miejsce.",                                                           "Siarka; Spetryfikowane drzewo",                  "Mana: 10; Skill: 15", 20736, 5054, School.Ranger );
			Register( typeof( RangerFamiliarSpell ),      "Zwierzęcy kompan", "Strażnik przywołuje zwierzęcego towarzysza (w zależności od poziomu umiejętności), aby pomógł mu w jego zadaniach.",                                           "Niszczejący Anioł; Woda wiosenna; Spetryfikowane drzewo", "Mana: 17; Skill: 30", 20491, 5054, School.Ranger );
			Register( typeof( RangerFireBowSpell ),       "Ognisty Łuk",       "Strażnik wykorzystuje swoją wiedzę o łucznictwie i łowiectwie, aby stworzyć tymczasowy łuk żywiołu ognia, który wystarczy na krótki czas.",      "Chrust; Siarka",                        "Mana: 30; Skill: 85", 2257,  5054, School.Ranger );
			Register( typeof( RangerIceBowSpell ),        "Lodowy Łuk",        "Strażnik wykorzystuje swoją wiedzę o łucznictwie i łowiectwie, aby stworzyć tymczasowy łuk żywiołu lodu, który wystarczy na krótki czas.",       "Chrust; Woda wiosenna",                         "Mana: 30; Skill: 85", 21001, 5054, School.Ranger );
			Register( typeof( RangerLightningBowSpell ),  "Piorunujący Łuk",   "Strażnik wykorzystuje swoją wiedzę o łucznictwie i łowiectwie, aby stworzyć tymczasowy łuk żywiołu błyskawic, który wystarczy na krótki czas.", "Chrust; Czarna perła",                          "Mana: 30; Skill: 90", 2281,  5054, School.Ranger );
			Register( typeof( RangerNoxBowSpell ),        "Wężowy Łuk",       "Strażnik wykorzystuje swoją wiedzę o łucznictwie i łowiectwie, aby stworzyć tymczasowy łuk żywiołu trucizny, który wystarczy na krótki czas.",    "Chrust; Wilcze Jagody",                           "Mana: 30; Skill: 95", 20488, 5054, School.Ranger );
			Register( typeof( RangerSummonMountSpell ),   "Przyzwanie Wierzcha",       "Strażnik woła do Dziczy, wzywając szybkiego wierzchowca po swojej stronie.",                                                               "Woda wiosenna; Czarna Perła; Siarka",       "Mana: 15; Skill: 30", 20745, 5054, School.Ranger );
			Register( typeof( RangerTrialByFireSpell ),   "Magiczne ziola",       "Strażnik zarzywa zioła zmarżające jego krew. Magia ziół dobija obrażenia zdane strażnikowi.",                                                               "Krwawy Mech",       "Mana: 19; Skill: 85", 20745, 5054, School.Ranger );
			Register( typeof( RangerThrowSwordSpell ),      "Rzut mieczem", "Rzuca mieczem w wybrany cel. Obrażenia zależą od broni trzymanej przez strażnika.",                                           "Krwawy Mech, Woda Wiosenna, Wilcze Jaogdy", "Mana: 25; Skill: 50", 20491, 5054, School.Ranger );
		}
	}
}
