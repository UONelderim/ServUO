namespace Server.SicknessSys.Illnesses
{
	public static class Virus
	{
		public static int IllnessType { get => 3; }
		public static string Name { get => BaseVirus.GetRandomName(IllnessType); }
		public static int StatDrain { get => BaseVirus.GetRandomDamage(IllnessType); }
		public static int BaseDamage { get => BaseVirus.GetRandomDamage(IllnessType); }
		public static int PowerDegenRate { get => BaseVirus.GetRandomDegen(IllnessType); }

		public static void UpdateBody(VirusCell cell)
		{
			IllnessMutationLists.SetMutation(cell);
		}
	}
}
