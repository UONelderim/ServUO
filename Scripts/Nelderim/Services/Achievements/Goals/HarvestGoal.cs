﻿using Server;
using Server.Mobiles;
using System;
using System.Collections.Generic;

namespace Nelderim.Achievements
{
	public class HarvestGoal : BasicPlayerStatisticGoal
	{
		protected override Dictionary<Type, long> GoalStatistic(PlayerMobile pm) => pm.Statistics.ResourceHarvested;

		public HarvestGoal(int amount, Type harvestedType): base(amount, harvestedType)
		{
			EventSink.ResourceHarvestSuccess += Check;
		}

		private void Check(ResourceHarvestSuccessEventArgs e)
		{
			if (e.Harvester is PlayerMobile pm)
			{
				if(e.Resource.GetType() == Type)
					InternalCheck(pm);
				if (e.BonusResource != null && e.BonusResource.GetType() == Type)
				{
					InternalCheck(pm);
				}
			}
		}
	}
}
