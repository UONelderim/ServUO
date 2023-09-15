using Server;
using Server.Mobiles;
using System;

namespace Nelderim.Achievements
{
	public class SameFactionKillGoal : Goal
	{
		public SameFactionKillGoal(int amount): base(amount)
		{
			EventSink.OnKilledBy += Progress;
		}

		private void Progress(OnKilledByEventArgs e)
		{
			if (e.KilledBy is PlayerMobile pm && 
			    e.Killed is PlayerMobile killed && 
			    pm.Faction.Equals(killed.Faction))
			{
				pm.SetAchievementProgress(Achievement, pm.GetAchivementProgress(Achievement) + 1);
			}
		}
	}
}
