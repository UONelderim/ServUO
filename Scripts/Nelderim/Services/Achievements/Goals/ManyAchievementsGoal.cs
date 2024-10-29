using System.Collections.Generic;
using System.Linq;
using Server;
using Server.Mobiles;

namespace Nelderim.Achievements
{
	public class ManyAchievementsGoal : Goal, IComplexGoal
	{
		private List<Achievement> _Achievements;
		
		public ManyAchievementsGoal(params Achievement[] achievements) : base(achievements.Length)
		{
			_Achievements = new List<Achievement>(achievements);
			EventSink.AchievementCompleted += Check;
		}

		private void Check(AchievementCompletedEventArgs e)
		{
			if(e.Mobile is PlayerMobile pm && GetProgress(pm) >= Amount)
			{
				pm.Complete(Achievement);
			}
		}

		public override int GetProgress(PlayerMobile pm)
		{
			return pm.Achievements
				.Where(a => _Achievements.Contains(a.Key))
				.Count(a => a.Value.Completed);
		}

		public string GetDetailedProgress(PlayerMobile pm)
		{
			return "Not implemented :)";
		}
	}
}
