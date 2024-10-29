using System;
using System.Collections.Generic;
using System.Linq;
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
			return achievement.Goal.GetProgress(this);
		}

		public bool IsCompleted(Achievement achievement)
		{
			return Achievements.ContainsKey(achievement) && Achievements[achievement].Completed;
		}

		public void Complete(Achievement achievement)
		{
			if (!Achievements.ContainsKey(achievement))
			{
				Achievements.Add(achievement, new AchievementStatus{Id = achievement.Id});
			}
			
			if (Achievements[achievement].Completed)
				return;

			Achievements[achievement].CompletedOn = DateTime.UtcNow;
			AchievementPoints += achievement.Points;
			SendGump(new AchievementObtainedGump(achievement));
			SendAsciiMessage("Otrzymales nagrode za zdobycie tego osiagniecia!");
			EventSink.InvokeAchievementCompleted(new AchievementCompletedEventArgs(this, achievement.Id));
			
			if (achievement.Rewards == null || achievement.Rewards.Length <= 0) return;

			try
			{
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
