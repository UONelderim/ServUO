using Server;
using Server.Mobiles;
using System;
using System.Collections.Generic;

namespace Nelderim.Achievements
{
	public class HarvestGoal : BasicPlayerStatisticGoal
	{
		protected override Dictionary<Type, long> GoalStatistic(PlayerMobile pm) => pm.Statistics.ResourceHarvested;

		public HarvestGoal(Type harvestedType, int amount): base(harvestedType, amount)
		{
			EventSink.ResourceHarvestSuccess += Check;
		}

		private void Check(ResourceHarvestSuccessEventArgs e)
		{
			if (e.Harvester is PlayerMobile pm && e.Resource.GetType() == Type)
			{
				InternalCheck(pm);
			}
		}
	}
}
