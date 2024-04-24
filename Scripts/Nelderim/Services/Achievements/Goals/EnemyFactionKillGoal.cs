using Server;
using Server.Mobiles;
using System;
using System.Linq;

namespace Nelderim.Achievements
{
	public class EnemyFactionKillGoal : Goal
	{
		private readonly Type _MobileType;

		public EnemyFactionKillGoal(int amount): base(amount)
		{
			EventSink.OnKilledBy += Progress;
		}

		private void Progress(OnKilledByEventArgs e)
		{
			if (e.KilledBy is PlayerMobile pm && 
			    e.Killed is PlayerMobile killed && 
			    pm.Faction.Enemies.Contains(killed.Faction))
			{
				pm.SetAchievementProgress(Achievement, pm.GetAchivementProgress(Achievement) + 1);
			}
		}
	}
}
