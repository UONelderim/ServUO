namespace Scripts.Mythik.Systems.Achievements
{
	public class AchievementCategory
	{
		public int ID { get; set; }
		public int Parent { get; set; }
		public string Name { get; set; }

		public AchievementCategory(int id, int parent, string name)
		{
			ID = id;
			Parent = parent;
			Name = name;
		}
	}
}
