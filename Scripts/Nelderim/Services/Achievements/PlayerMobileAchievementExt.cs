using System;
using System.Collections.Generic;
using System.Linq;
using Scripts.Mythik.Systems.Achievements;
using Scripts.Mythik.Systems.Achievements.Gumps;

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

		public void SetAchievementStatus(BaseAchievement achievement, int progress)
		{
			var achieves = Achievements;

			if (achieves.ContainsKey(achievement.ID))
			{
				if (achieves[achievement.ID].Progress >= achievement.CompletionTotal)
					return;
				achieves[achievement.ID].Progress += progress;
			}
			else
			{
				achieves.Add(achievement.ID, new AchievementStatus { Progress = progress });
			}

			if (achieves[achievement.ID].Progress >= achievement.CompletionTotal)
			{
				SendGump(new AchievementObtainedGump(achievement));
				achieves[achievement.ID].CompletedOn = DateTime.UtcNow;

				AchievementPoints += achievement.Points;

				if (achievement.RewardItems == null || achievement.RewardItems.Length <= 0) return;

				try
				{
					SendAsciiMessage("Otrzymales nagrode za zdobycie tego osiagniecia!");
					var item = (Item)Activator.CreateInstance(achievement.RewardItems[0]);
					AddToBackpack(item);
				}
				catch (Exception e)
				{
					Console.WriteLine("Exception in achievement system");
					Console.WriteLine(e);
				}
			}
		}
	}
}
