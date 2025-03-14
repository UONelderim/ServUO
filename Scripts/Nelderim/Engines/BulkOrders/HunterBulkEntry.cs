namespace Server.Engines.BulkOrders
{
	public partial class SmallBulkEntry
	{
		public static SmallBulkEntry[] HunterEasy => GetEntries("Hunting", "SmallEasy");

		public static SmallBulkEntry[] HunterMedium => GetEntries("Hunting", "SmallMedium");

		public static SmallBulkEntry[] HunterHard => GetEntries("Hunting", "SmallHard");

		public static SmallBulkEntry[] HunterBoss => GetEntries("Hunting", "SmallBoss");
	}

	public partial class LargeBulkEntry
	{
		public static SmallBulkEntry[][] HunterLargeEasy =
		{
			EasyAnimal1, EasyAnimal2, EasyAnts, EasyElementals, EasyHorde1, EasyHorde2, EasyHorde3, EasyOrcs,
			EasyOreElementals, EasyPlants, EasyMisc, EasyUndead
		};
		
		private static SmallBulkEntry[] EasyAnimal1 => GetEntries("Hunting", "LargeEasyAnimal1");
		private static SmallBulkEntry[] EasyAnimal2 => GetEntries("Hunting", "LargeEasyAnimal2");
		private static SmallBulkEntry[] EasyAnts => GetEntries("Hunting", "LargeEasyAnts");
		private static SmallBulkEntry[] EasyElementals => GetEntries("Hunting", "LargeEasyElementals");
		private static SmallBulkEntry[] EasyHorde1 => GetEntries("Hunting", "LargeEasyHorde1");
		private static SmallBulkEntry[] EasyHorde2 => GetEntries("Hunting", "LargeEasyHorde2");
		private static SmallBulkEntry[] EasyHorde3 => GetEntries("Hunting", "LargeEasyHorde3");
		private static SmallBulkEntry[] EasyOrcs => GetEntries("Hunting", "LargeEasyOrcs");
		private static SmallBulkEntry[] EasyOreElementals => GetEntries("Hunting", "LargeEasyOreElementals");
		private static SmallBulkEntry[] EasyPlants => GetEntries("Hunting", "LargeEasyPlants");
		private static SmallBulkEntry[] EasyMisc => GetEntries("Hunting", "LargeEasyMisc");
		private static SmallBulkEntry[] EasyUndead => GetEntries("Hunting", "LargeEasyUndead");

		public static SmallBulkEntry[][] HunterLargeMedium =
		{
			MediumElementals, MediumGargoyles, MediumJukas, MediumMech, MediumMinotaurs, MediumOphidians,
			MediumMisc, MediumTerathans, MediumUndead
		};

		private static SmallBulkEntry[] MediumElementals => GetEntries("Hunting", "LargeMediumElementals");
		private static SmallBulkEntry[] MediumGargoyles => GetEntries("Hunting", "LargeMediumGargoyles");
		private static SmallBulkEntry[] MediumJukas => GetEntries("Hunting", "LargeMediumJukas");
		private static SmallBulkEntry[] MediumMech => GetEntries("Hunting", "LargeMediumMech");
		private static SmallBulkEntry[] MediumMinotaurs => GetEntries("Hunting", "LargeMediumMinotaurs");
		private static SmallBulkEntry[] MediumOphidians => GetEntries("Hunting", "LargeMediumOphidians");
		private static SmallBulkEntry[] MediumMisc => GetEntries("Hunting", "LargeMediumMisc");
		private static SmallBulkEntry[] MediumTerathans => GetEntries("Hunting", "LargeMediumTerathans");
		private static SmallBulkEntry[] MediumUndead => GetEntries("Hunting", "LargeMediumUndead");

		public static SmallBulkEntry[][] HunterLargeHard = { Hard1, Hard2, Hard3, Hard4, Hard5 };

		private static SmallBulkEntry[] Hard1 => GetEntries("Hunting", "LargeHard1");
		private static SmallBulkEntry[] Hard2 => GetEntries("Hunting", "LargeHard2");
		private static SmallBulkEntry[] Hard3 => GetEntries("Hunting", "LargeHard3");
		private static SmallBulkEntry[] Hard4 => GetEntries("Hunting", "LargeHard4");
		private static SmallBulkEntry[] Hard5 => GetEntries("Hunting", "LargeHard5");

		public static SmallBulkEntry[][] HunterLargeBoss = { Boss1, Boss2, Boss3, Boss4, Boss5, Boss6, Boss7, Boss8 };

		private static SmallBulkEntry[] Boss1 => GetEntries("Hunting", "LargeBoss1");
		private static SmallBulkEntry[] Boss2 => GetEntries("Hunting", "LargeBoss2");
		private static SmallBulkEntry[] Boss3 => GetEntries("Hunting", "LargeBoss3");
		private static SmallBulkEntry[] Boss4 => GetEntries("Hunting", "LargeBoss4");
		private static SmallBulkEntry[] Boss5 => GetEntries("Hunting", "LargeBoss5");
		private static SmallBulkEntry[] Boss6 => GetEntries("Hunting", "LargeBoss6");
		private static SmallBulkEntry[] Boss7 => GetEntries("Hunting", "LargeBoss7");
		private static SmallBulkEntry[] Boss8 => GetEntries("Hunting", "LargeBoss8");

	}
}
