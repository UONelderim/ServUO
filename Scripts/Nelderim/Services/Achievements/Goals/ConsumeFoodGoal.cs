using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;

namespace Nelderim.Achievements
{
	public class ConsumeFoodGoal : BasicPlayerStatisticGoal
	{
		protected override Dictionary<Type, long> GoalStatistic(PlayerMobile pm) => pm.Statistics.FoodConsumed;

		public ConsumeFoodGoal(int amount, Type type) : base(amount, type)
		{
			EventSink.OnConsume += Check;
		}

		private void Check(OnConsumeEventArgs e)
		{
			if(e.Consumer is PlayerMobile pm && e.Consumed.GetType() == Type)
			{
				InternalCheck(pm);
			}
		}
	}
}
