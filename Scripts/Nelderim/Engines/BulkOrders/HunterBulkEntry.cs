#region References

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

#endregion

namespace Server.Engines.BulkOrders
{
	public partial class SmallBulkEntry
	{
		public static SmallBulkEntry[] Easy => GetEntries("Hunting", "SmallEasy");

		public static SmallBulkEntry[] Medium => GetEntries("Hunting", "SmallMedium");

		public static SmallBulkEntry[] Hard => GetEntries("Hunting", "SmallHard");

		public static SmallBulkEntry[] Boss => GetEntries("Hunting", "SmallBoss");
	}

	public partial class LargeBulkEntry
	{
		public static SmallBulkEntry[][] LargeEasy =
		{
			EasyAnimal1, EasyAnimal2, EasyAnimal1, EasyElementals, EasyHorde1, EasyHorde2, EasyHorde3, EasyOrcs,
			EasyOreElementals, EasyPlants, EasyMisc, EasyUndead
		};
		
		public static SmallBulkEntry[] EasyAnimal1 => GetEntries("Hunting", "LargeEasyAnimal1");
		public static SmallBulkEntry[] EasyAnimal2 => GetEntries("Hunting", "LargeEasyAnimal2");
		public static SmallBulkEntry[] EasyAnts => GetEntries("Hunting", "LargeEasyAnts");
		public static SmallBulkEntry[] EasyElementals => GetEntries("Hunting", "LargeEasyElementals");
		public static SmallBulkEntry[] EasyHorde1 => GetEntries("Hunting", "LargeEasyHorde1");
		public static SmallBulkEntry[] EasyHorde2 => GetEntries("Hunting", "LargeEasyHorde2");
		public static SmallBulkEntry[] EasyHorde3 => GetEntries("Hunting", "LargeEasyHorde3");
		public static SmallBulkEntry[] EasyOrcs => GetEntries("Hunting", "LargeEasyOrcs");
		public static SmallBulkEntry[] EasyOreElementals => GetEntries("Hunting", "LargeEasyOreElementals");
		public static SmallBulkEntry[] EasyPlants => GetEntries("Hunting", "LargeEasyPlants");
		public static SmallBulkEntry[] EasyMisc => GetEntries("Hunting", "LargeEasyMisc");
		public static SmallBulkEntry[] EasyUndead => GetEntries("Hunting", "LargeEasyUndead");

		public static SmallBulkEntry[][] LargeMedium =
		{
			MediumElementals, MediumGargoyles, MediumJukas, MediumMech, MediumMinotaurs, MediumOphidians,
			MediumMisc, MediumTerathans, MediumUndead
		};

		public static SmallBulkEntry[] MediumElementals => GetEntries("Hunting", "LargeMediumElementals");
		public static SmallBulkEntry[] MediumGargoyles => GetEntries("Hunting", "LargeMediumGargoyles");
		public static SmallBulkEntry[] MediumJukas => GetEntries("Hunting", "LargeMediumJukas");
		public static SmallBulkEntry[] MediumMech => GetEntries("Hunting", "LargeMediumMech");
		public static SmallBulkEntry[] MediumMinotaurs => GetEntries("Hunting", "LargeMediumMinotaurs");
		public static SmallBulkEntry[] MediumOphidians => GetEntries("Hunting", "LargeMediumOphidians");
		public static SmallBulkEntry[] MediumMisc => GetEntries("Hunting", "LargeMediumMisc");
		public static SmallBulkEntry[] MediumTerathans => GetEntries("Hunting", "LargeMediumTerathans");
		public static SmallBulkEntry[] MediumUndead => GetEntries("Hunting", "LargeMediumUndead");

		public static SmallBulkEntry[][] LargeHard = { Hard1, Hard2, Hard3, Hard4, Hard5 };

		public static SmallBulkEntry[] Hard1 => GetEntries("Hunting", "LargeHard1");
		public static SmallBulkEntry[] Hard2 => GetEntries("Hunting", "LargeHard2");
		public static SmallBulkEntry[] Hard3 => GetEntries("Hunting", "LargeHard3");
		public static SmallBulkEntry[] Hard4 => GetEntries("Hunting", "LargeHard4");
		public static SmallBulkEntry[] Hard5 => GetEntries("Hunting", "LargeHard5");

		public static SmallBulkEntry[][] LargeBoss = { Boss1, Boss2, Boss3, Boss4, Boss5, Boss6, Boss7, Boss8 };

		public static SmallBulkEntry[] Boss1 => GetEntries("Hunting", "LargeBoss1");
		public static SmallBulkEntry[] Boss2 => GetEntries("Hunting", "LargeBoss2");
		public static SmallBulkEntry[] Boss3 => GetEntries("Hunting", "LargeBoss3");
		public static SmallBulkEntry[] Boss4 => GetEntries("Hunting", "LargeBoss4");
		public static SmallBulkEntry[] Boss5 => GetEntries("Hunting", "LargeBoss5");
		public static SmallBulkEntry[] Boss6 => GetEntries("Hunting", "LargeBoss6");
		public static SmallBulkEntry[] Boss7 => GetEntries("Hunting", "LargeBoss7");
		public static SmallBulkEntry[] Boss8 => GetEntries("Hunting", "LargeBoss8");

	}
}
