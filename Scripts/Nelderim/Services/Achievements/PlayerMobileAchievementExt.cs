using System;
using System.Collections.Generic;
using System.Linq;
using Scripts.Mythik.Systems.Achievements;

namespace Server.Mobiles
{
	public partial class PlayerMobile
	{
		public Dictionary<int, AchievementStatus> Achievements => AchievementSystem.Get(this).Achievements;

		private int _AchievementPoints = -1;

		public int AchievementPoints
		{
			get
			{
				if (_AchievementPoints == -1)
					InitAchievementPoints();
				return _AchievementPoints;
			}
			set => _AchievementPoints = value;
		}

		private void InitAchievementPoints()
		{
			var total = 0;
			foreach (var achievement in Achievements.Values.Where(achievement => achievement.Completed))
			{
				if (AchievementSystem.Definitions.TryGetValue(achievement.ID, out var achievementDefinition))
				{
					total += achievementDefinition.Points;
				}
				else
				{
					Console.Write($"Invalid achievement {achievement.ID}");
				}
			}
			_AchievementPoints = total;
		}
	}
}
