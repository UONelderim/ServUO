using Server;
using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nelderim.Achievements
{
	public class HarvestManyGoal : ManyPlayerStatisticGoal
	{
		protected override Dictionary<Type, long> GoalStatistic(PlayerMobile pm) => pm.Statistics.ResourceHarvested;

		public HarvestManyGoal(int amount, params Type[] mobileTypes) : base(amount, mobileTypes)
		{
			EventSink.ResourceHarvestSuccess += Check;
		}

		private void Check(ResourceHarvestSuccessEventArgs e)
		{
			if (e.Harvester is PlayerMobile pm)
			{
				if(Types.Contains(e.Resource.GetType()))
					InternalCheck(pm);
				if (e.BonusResource != null && Types.Contains(e.BonusResource.GetType()))
				{
					InternalCheck(pm);
				}
			}
		}
	}
}
