using System;
using Server;

namespace Server.ACC.CSS.Systems.Avatar
{
	public class AvatarInitializer : BaseInitializer
	{
		public static void Configure()
		{
			Register( typeof( AvatarHeavensGateSpell ),   "Niebiańska Brama",  "Pozwala otworzyc portal do magicznej lokacji.",      null, "Dziesiecina: 30; Skill: 80; Mana: 40", 2258, 9300, School.Avatar );
			Register( typeof( AvatarMarkOfGodsSpell ),    "Znak Bogów",   "Zsyla potezny piorun na ziemie by oznaczyc rune uzytkownika.", null, "Dziesiecina: 10; Skill: 20; Mana: 10", 2269, 9300, School.Avatar );
			Register( typeof( AvatarHeavenlyLightSpell ), "Niebiańskie Światło", "Niebiosa oswiecaja droge Twa.",                                       null, "Dziesiecina: 10; Skill: 20; Mana: 10", 2245, 9300, School.Avatar );
			Register( typeof( AvatarRestorationSpell ),   "Odrodzenie",     "Cel rzucającego zostaje wskrzeszony, w pełni uleczony i odświeżony.",                                                                                                         null, "Mana: 50;  Dziesiecina: 40",  2298,  3500, School.Avatar );
			Register( typeof( AvatarSacredBoonSpell ),    "Święty znak",     "Otoczeni przez magię wydobywajacą się z magicznego znaku, jesteście leczeni przezeń.",                                                                        null, "Mana: 11; Dziesiecina: 15",  20742, 3500, School.Avatar );
			
		}
	}
}