using System;
using Server;

namespace Server.ACC.CSS.Systems.Ranger
{
    public class RangerList : BaseInitializer
    {
		public static void Configure()
		{
			Register( typeof( RangerHuntersAimSpell ),    "Celność łowcy",     "Zwiększa łucznictwo strażników i taktykę na krótki czas.",                                                             "Nightshade; Spring Water; Bloodmoss",            "Mana: 25; Skill: 50", 2244,  5054, School.Ranger );
			Register( typeof( RangerPhoenixFlightSpell ), "Lot Feniksa",   "Wzywa Feniksa, który przeniesie cię w wybrane miejsce.",                                                           "Sulfurous Ash; Petrafied Wood",                  "Mana: 10; Skill: 15", 20736, 5054, School.Ranger );
			Register( typeof( RangerFamiliarSpell ),      "Zwierzęcy kompan", "Strażnik przywołuje zwierzęcego towarzysza (w zależności od poziomu umiejętności), aby pomógł mu w jego zadaniach.",                                           "Destroying Angel; Spring Water; Petrafied Wood", "Mana: 17; Skill: 30", 20491, 5054, School.Ranger );
			Register( typeof( RangerFireBowSpell ),       "Ognisty Łuk",       "Strażnik wykorzystuje swoją wiedzę o łucznictwie i łowiectwie, aby stworzyć tymczasowy łuk żywiołu ognia, który wystarczy na krótki czas.",      "Kindling; Sulfurous Ash",                        "Mana: 30; Skill: 85", 2257,  5054, School.Ranger );
			Register( typeof( RangerIceBowSpell ),        "Lodowy Łuk",        "Strażnik wykorzystuje swoją wiedzę o łucznictwie i łowiectwie, aby stworzyć tymczasowy łuk żywiołu lodu, który wystarczy na krótki czas.",       "Kindling; Spring Water",                         "Mana: 30; Skill: 85", 21001, 5054, School.Ranger );
			Register( typeof( RangerLightningBowSpell ),  "Piorunujący Łuk",   "Strażnik wykorzystuje swoją wiedzę o łucznictwie i łowiectwie, aby stworzyć tymczasowy łuk żywiołu błyskawic, który wystarczy na krótki czas.", "Kindling; Black Pearl",                          "Mana: 30; Skill: 90", 2281,  5054, School.Ranger );
			Register( typeof( RangerNoxBowSpell ),        "Wężowy Łuk",       "Strażnik wykorzystuje swoją wiedzę o łucznictwie i łowiectwie, aby stworzyć tymczasowy łuk żywiołu trucizny, który wystarczy na krótki czas.",    "Kindling; Nightshade",                           "Mana: 30; Skill: 95", 20488, 5054, School.Ranger );
			Register( typeof( RangerSummonMountSpell ),   "Przyzwanie Wierzcha",       "Strażnik woła do Dziczy, wzywając szybkiego wierzchowca po swojej stronie.",                                                               "Spring Water; Black Pearl; Sulfurous Ash",       "Mana: 15; Skill: 30", 20745, 5054, School.Ranger );
		}
	}
}
