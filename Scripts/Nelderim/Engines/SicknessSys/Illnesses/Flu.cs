namespace Server.SicknessSys.Illnesses
{
    public static class Flu
    {
        public static int IllnessType { get => 2; }
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
