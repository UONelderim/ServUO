using Server;
using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nelderim.Achievements
{
	public class CraftManyGoal : ManyPlayerStatisticGoal
	{
		protected override Dictionary<Type, long> GoalStatistic(PlayerMobile pm) => pm.Statistics.ItemsCrafted;

		public CraftManyGoal(int amount, params Type[] craftTypes) : base(amount, craftTypes)
		{
			EventSink.CraftSuccess += Check;
		}

		private void Check(CraftSuccessEventArgs e)
		{
			if (e.Crafter is PlayerMobile pm && Types.Contains(e.CraftedItem.GetType()))
			{
				InternalCheck(pm);
			}
		}
	}
}
