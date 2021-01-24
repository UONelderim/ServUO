using System;
using Server;

namespace Server.ACC.CSS.Systems.Bard
{
    public class BardInitializer : BaseInitializer
    {
		public static void Configure()
		{
			Register( typeof( BardArmysPaeonSpell ),      "Śpiew Armii",     "Powoli regeneruje zdrowie kompanów. [Efekt obszarowy]",       null, "Mana: 15; Skill: 60", 2243,  3000, School.Bard );
			Register( typeof( BardEnchantingEtudeSpell ), "Wzmacniająca Etiud", "Podnosi inteligencję twojej drużyny. [Efekt obszarowy]",       null, "Mana: 40; Skill: 80", 2242,  3000, School.Bard );
			Register( typeof( BardEnergyCarolSpell ),     "Pobudzająca Pieśń",   "Podnosi odporność energetyczną drużyny. [Efekt obszarowy]",  null, "Mana: 12; Skill: 30", 2289,  3000, School.Bard );
			Register( typeof( BardEnergyThrenodySpell ),  "Porażający Tren",  "Obniża odporność energetyczną celu.",               null, "Mana: 7;  Skill: 35", 2281,  3000, School.Bard );
			Register( typeof( BardFireCarolSpell ),       "Pieśń Ognia",       "Podnosi odporność na ogień drużyny. [Efekt obszarowy]",    null, "Mana: 12; Skill: 30", 2267,  3000, School.Bard );
			Register( typeof( BardFireThrenodySpell ),    "Palący Tren",    "Obniża odporność na ogień celu.",                 null, "Mana: 7;  Skill: 35", 2257,  3000, School.Bard );
			Register( typeof( BardFoeRequiemSpell ),      "Soniczny Podmuch",  "Zadaje obrażenia celowi eksplozją energii dźwiękowej.",          null, "Mana: 25; Skill: 55", 2270,  3000, School.Bard );
			Register( typeof( BardIceCarolSpell ),        "Pieśń Lodu",        "Podnosi odporność na zimno twojej drużyny. [Efekt obszarowy]",    null, "Mana: 12; Skill: 30", 2286,  3000, School.Bard );
			Register( typeof( BardIceThrenodySpell ),     "Lodowy Tren",     "Obniża odporność na zimno twojego celu.",                  null, "Mana: 7;  Skill: 35", 2269,  3000, School.Bard );
			Register( typeof( BardKnightsMinneSpell ),    "Wzmacniający Okrzyk",   "Podnosi fizyczny opór drużyny. [Efekt obszarowy]",    null, "Mana: 12; Skill: 45", 2273,  3000, School.Bard );
			Register( typeof( BardMagesBalladSpell ),     "Pieśń Do Magów",    "Powoli regeneruje manę drużyny. [Efekt obszarowy]",        null, "Mana: 30; Skill: 85", 2292,  3000, School.Bard );
			Register( typeof( BardMagicFinaleSpell ),     "Magiczny Finał",     "Usuwa wszystkie przywołane stworzenia wokół ciebie. [Efekt obszarowy]",   null, "Mana: 15; Skill: 80", 2280,  3000, School.Bard );
			Register( typeof( BardPoisonCarolSpell ),     "Wężowa Pieśń",     "Zwiększa odporność drużyny na trucizny. [Efekt obszarowy]", null, "Mana: 12; Skill: 30", 2285,  3000, School.Bard );
			Register( typeof( BardPoisonThrenodySpell ),  "Tren Jadu",  "Obniża odporność twojego celu na truciznę.",               null, "Mana: 7;  Skill: 35", 20488, 3000, School.Bard );
			Register( typeof( BardSheepfoeMamboSpell ),   "Pasterska Przyśpiewka",   "Zwiększa zręczność twojej drużyny. [Efekt obszarowy]",          null, "Mana: 40; Skill: 80", 2248,  3000, School.Bard );
			Register( typeof( BardSinewyEtudeSpell ),     "Przyśpiewka Górników",     "Podnosi siłę twojej drużyny. [Efekt obszarowy]",           null, "Mana: 40; Skill: 80", 20741, 3000, School.Bard );
		}
	}
}
