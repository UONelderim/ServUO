using System;
using Server;

namespace Server.ACC.CSS.Systems.Rogue
{
    public class RogueList : BaseInitializer
    {
		public static void Configure()
		{
			Register( typeof( RogueFalseCoinSpell ),    "Falszywa moneta",   "Łotrzyk sztuczką ręki podaje fałszywe złoto.",          "Sulfurous Ash; Nightshade",                null, 20481, 5100, School.Rogue );
			Register( typeof( RogueCharmSpell ),        "Zaklecie",        "Łotrzyk hipnotyzuje cel swoimi złymi oczami.",                 "Black Pearl; Nightshade; Spider's Silk",   null, 21282, 5100, School.Rogue );
			Register( typeof( RogueSlyFoxSpell ),       "Przebiegła forma",      "Łotrzyk zmienia kształt w ukradkowego lisa.",                  "Petrafied Wood; Nox Crystal; Nightshade",  null, 20491, 5100, School.Rogue );
			Register( typeof( RogueShadowSpell ),       "Cien",       "Postac wymyka się z cienia.",                                  "Spider's Silk; Daemon Blood; Black Pearl", null, 21003, 5100, School.Rogue );
			Register( typeof( RogueIntimidationSpell ), "Zastraszenie", "The Rogue begins to look angry and mean at the loss of his skills.", null,                                       null, 20485, 5100, School.Rogue );
		}
	}
}
