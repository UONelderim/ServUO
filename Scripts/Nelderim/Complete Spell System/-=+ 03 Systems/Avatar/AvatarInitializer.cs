namespace Server.ACC.CSS.Systems.Avatar
{
	public class AvatarInitializer : BaseInitializer
	{
		public static void Configure()
		{
			Register(typeof(AvatarHeavensGateSpell), "Niebiańska Brama",
				"Pozwala otworzyc portal do magicznej lokacji.", null, "Dziesiecina: 30; Skill: 80; Mana: 40", 2258,
				9300, School.Avatar);
			Register(typeof(AvatarMarkOfGodsSpell), "Znak Bogów",
				"Zsyla potezny piorun na ziemie by oznaczyc rune uzytkownika.", null,
				"Dziesiecina: 10; Skill: 20; Mana: 10", 2269, 9300, School.Avatar);
			Register(typeof(AvatarHeavenlyLightSpell), "Niebiańskie Światło", "Niebiosa oswiecaja droge Twa.", null,
				"Dziesiecina: 10; Skill: 20; Mana: 10", 2245, 9300, School.Avatar);
			Register(typeof(AvatarRestorationSpell), "Odrodzenie",
				"Cel rzucającego zostaje wskrzeszony, w pełni uleczony i odświeżony.", null,
				"Mana: 50;  Dziesiecina: 40", 2298, 3500, School.Avatar);
			Register(typeof(AvatarSacredBoonSpell), "Święty znak",
				"Otoczeni przez magię wydobywajacą się z magicznego znaku, jesteście leczeni przezeń.", null,
				"Mana: 11; Dziesiecina: 15", 20742, 3500, School.Avatar);
			Register(typeof(AvatarAngelicFaithSpell), "Awatar Pradawnego",
				"Rzucający wzywa boskie moce niebios, aby przekształciły się w pradawnego mnicha. Rzucający zyskuje lepsze tempo regeneracji oraz zwiększone statystyki i umiejętności.",
				null, "Mana: 50; Skill: 80; Dzeisiecina: 100", 2295, 3500, School.Avatar);
			Register(typeof(AvatarArmysPaeonSpell), "Witalność Armii",
				"Powoli regeneruje zdrowie kompanów. [Efekt obszarowy]", null, "Dziesiecina: 50; Mana: 15; Skill: 60",
				2243, 3000, School.Avatar);
			Register(typeof(AvatarBallSpell), "Kula Sniegu",
				"Mnich Tworzy kule magicznego sniegu poprzez uderzenie w ziemie.", null,
				"Dziesiecina: 100; Skill: 70; Mana: 45", 2245, 9300, School.Avatar);
			Register(typeof(AvatarCurseRemovalSpell), "Reka Mnicha", "Usuwa wszelkie klatwy", null,
				"Dziesiecina: 20; Mana: 8; Skill: 50", 2243, 3000, School.Avatar);
		}
	}
}
