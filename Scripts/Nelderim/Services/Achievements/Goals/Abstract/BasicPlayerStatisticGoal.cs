using System;
using System.Collections.Generic;
using Server.Mobiles;

namespace Nelderim.Achievements
{
	public abstract class BasicPlayerStatisticGoal(int amount, Type type) : PlayerStatisticGoal(amount)
	{
		protected Type Type = type;
		protected abstract Dictionary<Type, long> GoalStatistic(PlayerMobile pm);

		public override sealed int GetProgress(PlayerMobile pm)
		{
			return Math.Min(Amount, (int)GoalStatistic(pm).GetValueOrDefault(Type, 0));
		}
	}
}
