using System;
using Server;

namespace Server.ACC.CSS.Systems.Rogue
{
    public class RogueList : BaseInitializer
    {
		public static void Configure()
		{
			Register( typeof( RogueFalseCoinSpell ),    "Falszywa moneta",   "Łotrzyk szybkim ruchem ręki podaje fałszywe złoto.",          "",                null, 20481, 5100, School.Rogue );
			Register( typeof( RogueShieldOfEarthSpell ),    "Klody pod nogi",   "Łotrzyk rzuca kłody pod nogi celu, blokując go.",          "",                null, 2254,  5120, School.Rogue );
			Register( typeof( RogueCharmSpell ),        "Zaklecie",        "Łotrzyk hipnotyzuje cel swoimi złymi oczami.",                 "",   null, 21282, 5100, School.Rogue );
			Register( typeof( RogueSlyFoxSpell ),       "Przebiegła forma",      "Łotrzyk zmienia kształt w panterę, która zwiększa jego siłę, zręczność i inteligencję.",                  "",  null, 20491, 5100, School.Rogue );
			Register( typeof( RogueShadowSpell ),       "Cien",       "Postać znika w cieniu.",                                  "", null, 21003, 5100, School.Rogue );
			Register( typeof( RogueIntimidationSpell ), "Zastraszenie", "Porazajacy piorun wystrzeliwany jest w kierunku wroga.", "Proszek translokacji", null, 20485, 5100, School.Rogue );
		}
	}
}
