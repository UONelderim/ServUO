using Server;
using Server.Mobiles;
using System;
using System.Collections.Generic;

namespace Nelderim.Achievements
{
	public class CraftGoal : BasicPlayerStatisticGoal
	{
		protected override Dictionary<Type, long> GoalStatistic(PlayerMobile pm) => pm.Statistics.ItemsCrafted;

		public CraftGoal(Type craftedType, int amount): base(craftedType, amount)
		{
			EventSink.CraftSuccess += Check;
		}

		private void Check(CraftSuccessEventArgs e)
		{
			if (e.Crafter is PlayerMobile pm && e.CraftedItem.GetType() == Type)
			{
				InternalCheck(pm);
			}
		}
	}
}
