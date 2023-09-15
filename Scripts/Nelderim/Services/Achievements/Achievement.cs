using System;
using Server;

namespace Nelderim.Achievements
{
	public interface IEntry
	{
		public int Id { get; set; }
		public string Name { get; }
	}

	public class AchievementCategory : IEntry
	{
		public AchievementCategory(AchievementCategory parent, string name)
		{
			Parent = parent;
			Name = name;
		}

		public AchievementCategory Parent {get; set; }
		public string Name { get; set; }
		public int Id { get; set; }
	}
	
	public record Achievement : IEntry
	{
		public Achievement(AchievementCategory category, string name, string description, int icon, ushort points, 
			bool secret, Achievement preReq, Goal goal, params Func<Item>[] rewards)
		{
			Category = category;
			Name = name;
			Description = description;
			Icon = icon;
			Points = points;
			PreReq = preReq;
			Secret = secret;
			Goal = goal;
			Rewards = rewards;

			goal.Achievement = this;
		}

		public int Id { get; set; }
		public AchievementCategory Category { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public int Icon { get; set; }
		public ushort Points { get; set; }
		public Achievement PreReq { get; set; }
		public bool Secret { get; private set; }
		public Goal Goal { get; set; }
		public Func<Item>[] Rewards { get; set; }
		
	}
}
