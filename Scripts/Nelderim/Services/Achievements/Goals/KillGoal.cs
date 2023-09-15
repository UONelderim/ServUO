using Server;
using Server.Mobiles;
using System;

namespace Nelderim.Achievements
{
	public class KillGoal : Goal
	{
		private readonly Type _MobileType;

		public KillGoal(Type mobileType, int amount): base(amount)
		{
			_MobileType = mobileType;
			EventSink.OnKilledBy += Progress;
		}

		private void Progress(OnKilledByEventArgs e)
		{
			if (e.KilledBy is PlayerMobile pm && e.Killed.GetType() == _MobileType)
			{
				pm.SetAchievementProgress(Achievement, pm.GetAchivementProgress(Achievement) + 1);
			}
		}
	}
}
