using Server;
using Server.Engines.BulkOrders;
using Server.Items;
using Server.Mobiles;

namespace Nelderim.Achievements
{
	public partial class AchievementSystem
	{
		public static void RegisterAchievements()
		{
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
			var potwory = Register(new AchievementCategory(null, "Potwory"));
			var bossy = Register(new AchievementCategory(potwory, "Bossy"));
			var frakcje = Register(new AchievementCategory(null, "Frakcje"));
			var rozwoj = Register(new AchievementCategory(null, "Rozwoj postaci"));
			var inne = Register(new AchievementCategory(null, "Inne"));

			//Discovery
			Register(new Achievement(eksploracja, "Teren zakazany", "Odwiedz Green Acres", 0, 1, 
				false, null, new DiscoverGoal("Green Acres")));
			Register(new Achievement(miastaWioski, "Centrum Wszystkiego", "Odwiedź Tasandore", 0, 1, 
				false, null, new DiscoverGoal("Tasandora")));

			//Kill
			Register(new Achievement(potwory, "Bojka za barakami 1", "Zabij 10 razy Barracoona", 0, 1, false,
				null, new KillCreatureGoal(typeof(Barracoon), 10)));
			Register(new Achievement(bossy, "Pokraki", "Zabij wszystkie mini bossy", 0,
				5, false, null,
				new KillManyCreatureGoal(1, typeof(WladcaJezioraLawy), typeof(BagusGagakCreeper), typeof(VitVarg)),
				() => new Silver(20), () => new Gold(500)
			)); //FixMe
			Register(new Achievement(potwory, "Paragony grozy", "Zabij 1000 paragonow", 0, 1, false,
				null, new ParagonKillGoal(1000)));
			Register(new Achievement(frakcje, "Zdrajca", "Zabij czlonka swojej frakcji", 0, 1, false,
				null, new SameFactionKillGoal(1)));
			Register(new Achievement(frakcje, "Zasłużony", "Zabij 20 czlonkow wrogiej frakcji", 0, 1, false,
				null, new EnemyFactionKillGoal(20)));

			//rzemioslo
			Register(new Achievement(zlecenia, "Na zamowienie 1", "Wykonaj 100 zlecen krawca", 0, 1,
				false, null, new BulkOrderGoal(typeof(SmallTailorBOD), 100)));

			//surowce
			Register(new Achievement(surowce, "Kilofem i lopata 1", "Wydobadz 1000 rudy zelaza", 0, 1,
				false, null, new HarvestGoal(typeof(IronOre), 1000)));
			Register(new Achievement(surowce, "Wycinka 1", "Pozyskaj 1000 klod", 0, 1,
				false, null, new HarvestGoal(typeof(Log), 1000)));

			//rozwoj
			Register(new Achievement(rozwoj, "Dokonanie prawdziwnego alchemika", "Wytrenuj 120 alchemii",
				0, 1, false, null, new SkillProgressGoal(SkillName.Alchemy, 1200)));
			Register(new Achievement(rozwoj, "Pelne mozliwosci umyslu", "Osiagnij limit 120 dla 6 umiejetnosci", 
				0, 1, false, null, new SkillCapGoal(1200, 6)));
			Register(new Achievement(rozwoj, "Wiem juz wszystko", "Osiagnij limit sumy umiejetnosci",
				0, 1, false, null, new TotalSkillProgressGoal(7200)));
		}
	}
}
