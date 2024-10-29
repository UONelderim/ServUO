using Server;
using Server.Engines.BulkOrders;
using Server.Engines.Quests.Doom;
using Server.Items;
using Server.Mobiles;

namespace Nelderim.Achievements
{
	public partial class AchievementSystem
	{
		public static void RegisterAchievements()
		{
			//Keep child categories right under its parent or display will be broken
			var eksploracja = Register(new AchievementCategory(null, "Eksploracja"));
			var miastaWioski = Register(new AchievementCategory(eksploracja, "Miasta i wioski"));
			var podziemia = Register(new AchievementCategory(eksploracja, "Podziemia"));
			var interesujace = Register(new AchievementCategory(eksploracja, "Interesujace miejsca"));

			var rzemioslo = Register(new AchievementCategory(null, "Rzemioslo"));
			var alchemia = Register(new AchievementCategory(rzemioslo, "Alchemia"));
			var kowalstwo = Register(new AchievementCategory(rzemioslo, "Kowalstwo"));
			var majsterkowanie = Register(new AchievementCategory(rzemioslo, "Majsterkowanie"));
			
			var zlecenia = Register(new AchievementCategory(rzemioslo, "Zlecenia"));

			var surowce = Register(new AchievementCategory(null, "Surowce"));
			
			var stworzenia = Register(new AchievementCategory(null, "Stworzenia"));
			var potwory = Register(new AchievementCategory(stworzenia, "Potwory"));
			var bossy = Register(new AchievementCategory(stworzenia, "Bossy"));
			var przywolance = Register(new AchievementCategory(stworzenia, "Przywołańce"));
			
			var frakcje = Register(new AchievementCategory(null, "Frakcje"));
			var rozwoj = Register(new AchievementCategory(null, "Rozwoj postaci"));
			var inne = Register(new AchievementCategory(null, "Inne"));

			//Discovery
			Register(new Achievement(interesujace, "Teren zakazany", "Odwiedz Green Acres", 0, 1, 
				false, null, new DiscoverGoal("GreenAcres")));
			
			var citiesAchievments = new[]
			{
				Register(new Achievement(miastaWioski, "Na starych śmieciach", "Odwiedź Tasandore", 0, 1, 
					false, null, new DiscoverGoal("Tasandora"))),
				Register(new Achievement(miastaWioski, "Niezbezpieczne tereny", "Odwiedź L'Delmah", 0, 1, 
					false, null, new DiscoverGoal("L'Delmah"))),
				Register(new Achievement(miastaWioski, "Nie lubię piasku", "Odwiedź Tirassę", 0, 1,
					false, null, new DiscoverGoal("Tirassa"))),
				Register(new Achievement(miastaWioski, "Nowe tereny", "Odwiedź Orod", 0, 1,
					false, null, new DiscoverGoal("Orod"))),
				Register(new Achievement(miastaWioski, "Wyspiarskie życie", "Odwiedź Lotharn", 0, 1,
					false, null, new DiscoverGoal("Lotharn"))),
				Register(new Achievement(miastaWioski, "Zimno, zimnoooo", "Odwiedź Garlan", 0, 1,
					false, null, new DiscoverGoal("Garlan"))),
				Register(new Achievement(miastaWioski, "Długie brody i ciemne komnaty", "Odwiedź Twierdzę", 0, 1,
					false, null, new DiscoverGoal("Twierdza")))
			};
			Register(new Achievement(miastaWioski, "Obieżyświat", "Odwiedź wszystkie miasta", 0, 1, false, null,
				new ManyAchievementsGoal(citiesAchievments)));
			
			//Podziemia
			var dungeonAchievements = new[]
			{
				Register(new Achievement(podziemia, "Kryształowe jaskinie", "Odwiedź Kryształowe Jaksinie", 0, 1, 
					false, null, new DiscoverGoal("Gorogon_VeryEasy"))),
				Register(new Achievement(podziemia, "Leże lodowych smoków", "Odwiedź Leże Odowych Smoków", 0, 1,
					false, null, new DiscoverGoal("LezeLodowychSmokow_LVL1_VeryEasy"))),
				Register(new Achievement(podziemia, "Piramida", "Odwiedź Piramidę", 0, 1,
					false, null, new DiscoverGoal("Piramida_VeryEasy"))),
				Register(new Achievement(podziemia, "Siedziba Demonów", "Odwiedź Siedzibę Demonów", 0, 1,
					false, null, new DiscoverGoal("Demonowo"))),
				Register(new Achievement(podziemia, "Leże Kryształowych Smoków", "Odwiedź Leże Kryształowych Smoków", 0, 1,
					false, null, new DiscoverGoal("KrysztaloweSmoki_VeryEasy"))),
			};
			Register(new Achievement(podziemia, "Penetruję lochy", "Odwiedź wszystkie podziemia", 0, 1, 
				false, null, new ManyAchievementsGoal(dungeonAchievements)));

			//Kill
			Register(new Achievement(potwory, "Bojka za barakami 1", "Zabij 10 razy Barracoona", 0, 1, false,
				null, new KillCreatureGoal(10, typeof(Barracoon))));
			Register(new Achievement(potwory, "Bojka za barakami 2", "Zabij 100 razy Barracoona", 0, 1, false,
				null, new KillCreatureGoal(100, typeof(Barracoon))));
			Register(new Achievement(potwory, "Bojka za barakami 3", "Zabij 1000 razy Barracoona", 0, 1, false,
				null, new KillCreatureGoal(1000, typeof(Barracoon))));
			Register(new Achievement(potwory, "Jestem twoim ojcem... 1", "Zabij 50 razy Mrocznego Ojca", 0, 1, false,
				null, new KillCreatureGoal(50, typeof(DemonKnight))));
			Register(new Achievement(potwory, "Jestem twoim ojcem... 2", "Zabij 500 razy Mrocznego Ojca", 0, 1, false,
				null, new KillCreatureGoal(500, typeof(DemonKnight))));
			Register(new Achievement(potwory, "Jestem twoim ojcem... 3", "Zabij 5000 razy Mrocznego Ojca", 0, 1, false,
				null, new KillCreatureGoal(5000, typeof(DemonKnight))));
			Register(new Achievement(bossy, "Pogromca władców Podziemi", "Zabij 1x wszystkie bossy", 0,
				5, false, null,
				new KillManyCreatureGoal(1, 
					typeof(NGorogon),
					typeof(Sfinks),
					typeof(NSzeol),
					typeof(NBurugh),
					typeof(NKatrill),
					typeof(NDeloth),
					typeof(NDzahhar),
					typeof(NSarag),
					typeof(NSkeletalDragon),
					typeof(NStarozytnyLodowySmok),
					typeof(StarozytnyDiamentowySmok),
					typeof(NStarozytnySmok),
					typeof(WladcaDemonow),
					typeof(Zhoaminth),
					typeof(MinotaurBoss),
					typeof(DreadHorn),
					typeof(LadyMelisande),
					typeof(Travesty),
					typeof(ChiefParoxysmus),
					typeof(Harrower),
					typeof(AncientRuneBeetle),
					typeof(Serado),
					typeof(BetrayerBoss),
					typeof(HalrandBoss)),
				() => new Silver(20)
			));
			Register(new Achievement(bossy, "Pogromca Pomniejszych Władców Podziemi", "Zabij 1x wszystkie mini bossy", 0,
				5, false, null,
				new KillManyCreatureGoal(1, 
					typeof(WladcaJezioraLawy), 
					typeof(BagusGagakCreeper),  
					typeof(VitVarg), 
					typeof(TilkiBug), 
					typeof(NelderimDragon), 
					typeof(ShimmeringEffusion), 
					typeof(MonstrousInterredGrizzle), 
					typeof(NSilshashaszals), 
					typeof(SaragAwatar), 
					typeof(WladcaPiaskow), 
					typeof(EvilSpellbook), 
					typeof(ExodusBoss), 
					typeof(CountDracula)),
				() => new Silver(20)
			));
			Register(new Achievement(bossy, "Pogromca Championów", "Zabij 1x wszystkie championy", 0,
				5, false, null,
				new KillManyCreatureGoal(1, 
					typeof(Semidar),
					typeof(Mephitis),
					typeof(Rikktor),
					typeof(LordOaks),
					typeof(Barracoon),
					typeof(Neira),
					typeof(Serado),
					typeof(Twaulo),
					typeof(Ilhenir),
					typeof(AbyssalInfernal),
					typeof(PrimevalLich),
					typeof(DragonTurtle),
					typeof(KhalAnkur)),
				() => new Silver(20)
			));
			Register(new Achievement(stworzenia, "Pogromca Przedwiecznego Zła ", "Zabij Przedwiecznego", 0, 1, false,
				null, new KillCreatureGoal(1, typeof(Harrower))));
			Register(new Achievement(stworzenia, "Pogromca Przedwiecznego Zła 2", "Zabij 10x Przedwiecznego", 0, 1, false,
				null, new KillCreatureGoal(10, typeof(Harrower))));
			Register(new Achievement(stworzenia, "Pierwszy kompan", "Oswój 1 zwierzę", 0, 1, false,
				null, new TamedTypesGoal(1)));
			Register(new Achievement(stworzenia, "Zaklinacz zwierząt", "Oswój 50 różnych zwierząt", 0, 1, false,
				null, new TamedTypesGoal(50)));
			Register(new Achievement(stworzenia, "Złap je wszystkie", "Oswój 200 różnych zwierząt", 0, 1, false,
				null, new TamedTypesGoal(200)));
			Register(new Achievement(stworzenia, "Koniobijca", "Zabij 100 koni", 0, 1, false,
				null, new KillCreatureGoal(100, typeof(Horse))));
			Register(new Achievement(potwory, "Myśliwy", "Zabij 100 Jeleni", 0, 1, false,
				null, new KillCreatureGoal(100, typeof(GreatHart))));
			Register(new Achievement(potwory, "Paragony grozy", "Zabij 1000 paragonow", 0, 1, false,
				null, new ParagonKillGoal(1000)));
			Register(new Achievement(przywolance, "Może i zimna, ale za to sztywna 1", "Stwórz 10 szkieletów", 0, 1, false,
				null, new CraftNecromancySummon(10, typeof(Skeleton))));
			Register(new Achievement(przywolance, "Może i zimna, ale za to sztywna 2", "Stwórz 10 zombie", 0, 1, false,
				null, new CraftNecromancySummon(10, typeof(Zombie))));
			Register(new Achievement(przywolance, "Może i zimna, ale za to sztywna 3", "Stwórz 10 ghuli", 0, 1, false,
				null, new CraftNecromancySummon(10, typeof(Ghoul))));
			Register(new Achievement(przywolance, "Może i zimna, ale za to sztywna 4", "Stwórz 10 kościanych rycerzy", 0, 1, false,
				null, new CraftNecromancySummon(10, typeof(BoneKnight))));
			Register(new Achievement(przywolance, "Może i zimna, ale za to sztywna 5", "Stwórz 10 kościanych magów", 0, 1, false,
				null, new CraftNecromancySummon(10, typeof(BoneMagi))));
			Register(new Achievement(przywolance, "Może i zimna, ale za to sztywna 6", "Stwórz 10 liczy", 0, 1, false,
				null, new CraftNecromancySummon(10, typeof(Lich))));
			Register(new Achievement(przywolance, "Może i zimna, ale za to sztywna 7", "Stwórz 10 starożytnych liczy", 0, 1, false,
				null, new CraftNecromancySummon(10, typeof(AncientLich))));
			Register(new Achievement(przywolance, "Może i zimna, ale za to sztywna 8", "Stwórz 10 kościejów", 0, 1, false,
				null, new CraftNecromancySummon(10, typeof(Boner))));
			
			//Frakcje
			Register(new Achievement(frakcje, "Zdrajca", "Zabij czlonka swojej frakcji", 0, 1, false,
				null, new SameFactionKillGoal(1)));
			Register(new Achievement(frakcje, "Zasłużony", "Zabij 20 czlonkow wrogiej frakcji", 0, 1, false,
				null, new EnemyFactionKillGoal(20)));

			//rzemioslo
			Register(new Achievement(zlecenia, "Na zamowienie 1", "Wykonaj 100 malych zlecen krawca", 0, 1,
				false, null, new BulkOrderGoal(100, typeof(SmallTailorBOD))));
			Register(new Achievement(zlecenia, "Na zamowienie 2", "Wykonaj 100 malych zlecen kowala", 0, 1,
				false, null, new BulkOrderGoal(100, typeof(SmallSmithBOD))));
			Register(new Achievement(zlecenia, "Na zamowienie 3", "Wykonaj 100 malych zlecen kucharza", 0, 1,
				false, null, new BulkOrderGoal(100, typeof(SmallCookingBOD))));
			Register(new Achievement(zlecenia, "Na zamowienie 4", "Wykonaj 100 malych zlecen alchemika", 0, 1, 
				false, null, new BulkOrderGoal(100, typeof(SmallAlchemyBOD))));
			Register(new Achievement(zlecenia, "Na zamowienie 5", "Wykonaj 100 malych zlecen stolarza", 0, 1,
				false, null, new BulkOrderGoal(100, typeof(SmallCarpentryBOD))));
			Register(new Achievement(zlecenia, "Na zamowienie 6", "Wykonaj 100 malych zlecen łukmistrza", 0, 1,
				false, null, new BulkOrderGoal(100, typeof(SmallFletcherBOD))));
			Register(new Achievement(zlecenia, "Na zamowienie 7", "Wykonaj 100 malych zlecen skryby", 0, 1,
				false, null, new BulkOrderGoal(100, typeof(SmallInscriptionBOD))));
			Register(new Achievement(zlecenia, "Na zamowienie 8", "Wykonaj 100 malych zlecen myśliwego", 0, 1,
				false, null, new BulkOrderGoal(100, typeof(SmallHunterBOD))));

			//surowce
			Register(new Achievement(surowce, "Skórkowany 1", "Zbierz 1000 skór", 0, 1,
				false, null, new HarvestGoal(1000, typeof(Leather))));
			Register(new Achievement(surowce, "Skórkowany 2", "Zbierz 1000 niebieskich skór", 0, 1,
				false, null, new HarvestGoal(1000, typeof(SpinedLeather))));
			Register(new Achievement(surowce, "Skórkowany 3", "Zbierz 1000 czerwonych skór", 0, 1,
				false, null, new HarvestGoal(1000, typeof(HornedLeather))));
			Register(new Achievement(surowce, "Skórkowany 4", "Zbierz 1000 zielonych skór", 0, 1,
				false, null, new HarvestGoal(1000, typeof(BarbedLeather))));
			Register(new Achievement(surowce, "Tanio skóry nie sprzedam", "Zbierz 5000 każdego typu skór", 
				0, 1, false, null, new HarvestManyGoal(5000, 
					typeof(Leather), typeof(SpinedLeather), typeof(HornedLeather), typeof(BarbedLeather))));
			
			Register(new Achievement(surowce, "Kilofem i lopata 1", "Wydobadz 1000 rudy zelaza", 0, 1,
				false, null, new HarvestGoal(1000, typeof(IronOre))));
			Register(new Achievement(surowce, "Kilofem i lopata 2", "Wydobadz 1000 rudy matowej miedzi", 0, 1,
				false, null, new HarvestGoal(1000, typeof(DullCopperOre))));
			Register(new Achievement(surowce, "Kilofem i lopata 3", "Wydobadz 1000 rudy shadowa", 0, 1,
				false, null, new HarvestGoal(1000, typeof(ShadowIronOre))));
			Register(new Achievement(surowce, "Kilofem i lopata 4", "Wydobadz 1000 rudy miedzi", 0, 1,
				false, null, new HarvestGoal(1000, typeof(CopperOre))));
			Register(new Achievement(surowce, "Kilofem i lopata 5", "Wydobadz 1000 rudy brązu", 0, 1,
				false, null, new HarvestGoal(1000, typeof(BronzeOre))));
			Register(new Achievement(surowce, "Kilofem i lopata 6", "Wydobadz 1000 rudy złota", 0, 1,
				false, null, new HarvestGoal(1000, typeof(GoldOre))));
			Register(new Achievement(surowce, "Kilofem i lopata 7", "Wydobadz 1000 rudy agapitu", 0, 1,
				false, null, new HarvestGoal(1000, typeof(AgapiteOre))));
			Register(new Achievement(surowce, "Kilofem i lopata 8", "Wydobadz 1000 rudy verytu", 0, 1,
				false, null, new HarvestGoal(1000, typeof(VeriteOre))));
			Register(new Achievement(surowce, "Kilofem i lopata 9", "Wydobadz 1000 rudy valorytu", 0, 1,
				false, null, new HarvestGoal(1000, typeof(ValoriteOre))));
			Register(new Achievement(surowce, "Krasnoludzki upór", "Zbierz 5000 każdego typu skór", 
				0, 1, false, null, new HarvestManyGoal(5000, 
					typeof(IronOre), typeof(DullCopperOre), typeof(ShadowIronOre), typeof(CopperOre), 
					typeof(BronzeOre), typeof(GoldOre), typeof(AgapiteOre), typeof(VeriteOre), typeof(ValoriteOre))));
			
			Register(new Achievement(surowce, "Wycinka 1", "Pozyskaj 1000 klod", 0, 1,
				false, null, new HarvestGoal(1000, typeof(Log))));

			//rozwoj
			Register(new Achievement(rozwoj, "Dokonanie prawdziwnego alchemika", "Wytrenuj 120 alchemii",
				0, 1, false, null, new SkillProgressGoal(SkillName.Alchemy, 1200)));
			Register(new Achievement(rozwoj, "Pelne mozliwosci umyslu", "Osiagnij limit 120 dla 6 umiejetnosci", 
				0, 1, false, null, new SkillCapGoal(1200, 6)));
			Register(new Achievement(rozwoj, "Wiem juz wszystko", "Osiagnij limit sumy umiejetnosci",
				0, 1, false, null, new TotalSkillProgressGoal(7200)));
			
			//inne
			Register(new Achievement(inne, "Ale boli", "Rzuc 10 razy Bole", 0, 1, false,
				null, new ThrowBolaGoal(10)));
			Register(new Achievement(inne, "Alkoholik", "Wypij 1000 butelek wina", 0, 1, false,
				null, new ConsumeFoodGoal(1000, typeof(BottleOfWine))));
			Register(new Achievement(inne, "Złote, lecz skromne", "Zbierz 100 złotych czaszek", 0, 1, false,
				null, new QuestCompletedGoal(100, typeof(VanquishDaemonObjective))));
		}
	}
}
