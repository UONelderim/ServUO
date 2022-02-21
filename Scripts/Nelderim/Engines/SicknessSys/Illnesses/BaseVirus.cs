#region References

using System.Collections.Generic;

#endregion

namespace Server.SicknessSys.Illnesses
{
	public static class BaseVirus
	{
		public static string GetRandomName(int illness)
		{
			List<string> names = null;

			if (illness == 1)
			{
				names = IllnessNameLists.GetColdNameList();
			}

			if (illness == 2)
			{
				names = IllnessNameLists.GetFluNameList();
			}

			if (illness == 3)
			{
				names = IllnessNameLists.GetVirusNameList();
			}

			if (illness == 101)
			{
				names = IllnessNameLists.GetVampireNameList();
			}

			if (illness == 102)
			{
				names = IllnessNameLists.GetLycanthropiaNameList();
			}

			return names[Utility.Random(0, names.Count)];
		}

		public static int GetRandomDegen(int illness)
		{
			if (illness < 100)
			{
				int rate = Utility.RandomMinMax(1, 3 * illness);

				return rate;
			}

			return 9;
		}

		public static int GetRandomDamage(int illness)
		{
			int DamageEntry = 0;

			if (illness < 100)
				DamageEntry = Utility.RandomMinMax(1, 3) * illness;
			else
				DamageEntry = Utility.RandomMinMax(6, 9);

			return DamageEntry;
		}
	}
}
