using System;
using System.Collections.Generic;
using System.Linq;
using Server.Mobiles;

namespace Nelderim.Achievements
{
	public abstract class ManyPlayerStatisticGoal(int amount, params Type[] types) : PlayerStatisticGoal(amount * types.Length), IComplexGoal
	{
		protected abstract Dictionary<Type, long> GoalStatistic(PlayerMobile pm);

		protected int SingleAmount = amount;
		protected Type[] Types = types;

		public override int GetProgress(PlayerMobile pm)
		{
			return (int)GoalStatistic(pm)
				.Where(pair => Types.Contains(pair.Key))
				.Sum(pair => Math.Min(pair.Value, SingleAmount));
		}

		public string GetDetailedProgress(PlayerMobile pm)
		{
			return "Not yet implemented :)";
		}
	}
}
