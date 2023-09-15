using Server;
using Server.Mobiles;
using System;

namespace Nelderim.Achievements
{
	public class ParagonKillGoal : Goal
	{
		public ParagonKillGoal(int amount): base(amount)
		{
			EventSink.OnKilledBy += Progress;
		}

		private void Progress(OnKilledByEventArgs e)
		{
			if (e.KilledBy is PlayerMobile pm && e.Killed is BaseCreature bc && bc.IsParagon)
			{
				pm.SetAchievementProgress(Achievement, pm.GetAchivementProgress(Achievement) + 1);
			}
		}
	}
}
