namespace Server.SicknessSys
{
	public static class SicknessRO
	{
		public static bool GetContagious(IllnessType type)
		{
			switch (type)
			{
				case IllnessType.Cold: return Utility.RandomBool();
				case IllnessType.Flu: return Utility.RandomBool();
				case IllnessType.Virus: return Utility.RandomBool();
				case IllnessType.Vampirism: return false;
				case IllnessType.Lycanthropia: return false;
				default: return false;
			}
		}
	}
}
