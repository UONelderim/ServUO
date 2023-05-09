namespace Scripts.Mythik.Systems.Achievements
{
	public partial class AchievementSystem
	{
		public static void RegisterAchievements()
		{
			RegisterCategory(1, 0, "Eksploracja");
			RegisterCategory(2, 1, "Miasta i wioski");
			RegisterCategory(3, 1, "Podziemia");
			RegisterCategory(4, 1, "Interesujace miejsca");

			RegisterCategory(1000, 0, "Rzemioslo");
			RegisterCategory(1001, 1000, "Alchemia");
			RegisterCategory(1002, 1000, "Kowalstwo");
			RegisterCategory(1003, 1000, "Majsterkowanie");

			RegisterCategory(2000, 0, "Surowce");
			RegisterCategory(3000, 0, "Potwory");
			RegisterCategory(4000, 0, "Rozwoj postaci");
			RegisterCategory(5000, 0, "Inne");

			RegisterDiscoveryAchievement(8888, 1, 0x14EB, true, null, "Teren zakazany", "Jak sie tu znalazles?", 5,
				"Green Acres");
			
			RegisterDiscoveryAchievement(0, 2, 0x14EB, false, null, "Tasandora", "Centrum wszystkiego",
				5, "Tasandora");
			
			//these two show examples of adding a reward or multiple rewards
			var achieve = new HarvestAchievement(500, 2000, 0x0E85, false, null, 500, "500 Iron Ore",
				"Mine 500 Iron Ore", 5, typeof(IronOre), typeof(AncientSmithyHammer));
			Definitions.Add(achieve);
			Definitions.Add(new HarvestAchievement(501, 2000, 0x0E85, false, achieve, 5000, "5000 Iron Ore",
				"Mine 5000 Iron Ore", 5, typeof(IronOre), typeof(AncientSmithyHammer), typeof(TinkerTools),
				typeof(HatOfTheMagi)));

			Definitions.Add(new HunterAchievement(1000, 3000, 0x25D1, false, null, 5, "Dog Slayer", "Slay 5 Dogs", 5,
				typeof(Dog)));
			Definitions.Add(new HunterAchievement(1001, 3000, 0x25D1, false, null, 50, "Dragon Slayer",
				"Slay 50 Dragon", 5, typeof(Dragon)));
		}

		private static void RegisterCategory(int cat, int parent, string name)
		{
			Categories.Add(cat, new AchievementCategory(cat, parent, name));
		}

		private static void RegisterDiscoveryAchievement(int id, int category, int itemIcon, bool secret,
			BaseAchievement prereq, string title, string desc, ushort points, string region)
		{
			Definitions.Add(id, new DiscoveryAchievement(id, category, itemIcon, secret, prereq, title, desc, points, region));
		}
	}
}
