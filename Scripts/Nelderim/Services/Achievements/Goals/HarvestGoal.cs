using Server;
using Server.Mobiles;
using System;

namespace Nelderim.Achievements
{
	public class HarvestGoal : Goal
	{
		private readonly Type _HarvestedType;

		public HarvestGoal(Type harvestedType, int amount): base(amount)
		{
			_HarvestedType = harvestedType;
			EventSink.ResourceHarvestSuccess += Progress;
		}

		private void Progress(ResourceHarvestSuccessEventArgs e)
		{
			if (e.Harvester is PlayerMobile pm && e.Resource.GetType() == _HarvestedType)
			{
				pm.SetAchievementProgress(Achievement, pm.GetAchivementProgress(Achievement) + e.Resource.Amount);
			}
		}
	}
}
