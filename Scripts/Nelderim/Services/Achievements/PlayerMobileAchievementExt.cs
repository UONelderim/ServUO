using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.ModelBinding;
using Nelderim.Achievements;
using Nelderim.Achievements.Gumps;

namespace Server.Mobiles
{
	public partial class PlayerMobile
	{
		public Dictionary<Achievement, AchievementStatus> Achievements => AchievementSystem.Get(this).Achievements;

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
				if (AchievementSystem.AchievementRegistry.Entries.TryGetValue(achievement.Id, out var achievementDefinition))
				{
					total += achievementDefinition.Points;
				}
				else
				{
					Console.WriteLine($"Invalid achievement {achievement.Id}");
				}
			}
			_AchievementPoints = total;
		}

		public int GetAchivementProgress(Achievement achievement)
		{
			if (Achievements.TryGetValue(achievement, out var value))
			{
				return value.Progress;
			}
			return 0;
		}

		public void SetAchievementProgress(Achievement achievement, int progress)
		{
			if (!Achievements.ContainsKey(achievement))
			{
				Achievements.Add(achievement, new AchievementStatus());
			}
			
			if (Achievements[achievement].Completed)
				return;

			Achievements[achievement].Progress = progress;
			if (Achievements[achievement].Progress >= achievement.Goal.Amount)
			{
				SendGump(new AchievementObtainedGump(achievement));
				Achievements[achievement].CompletedOn = DateTime.UtcNow;

				AchievementPoints += achievement.Points;

				if (achievement.Rewards == null || achievement.Rewards.Length <= 0) return;

				try
				{
					SendAsciiMessage("Otrzymales nagrode za zdobycie tego osiagniecia!");
					foreach (var rewardFunc in achievement.Rewards)
					{
						var item = rewardFunc.Invoke();
						AddToBackpack(item);
					}
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
