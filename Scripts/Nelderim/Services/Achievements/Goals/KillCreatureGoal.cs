using Server;
using Server.Mobiles;
using System;
using System.Collections.Generic;

namespace Nelderim.Achievements
{
	public class KillCreatureGoal : BasicPlayerStatisticGoal
	{
		protected override Dictionary<Type, long> GoalStatistic(PlayerMobile pm) => pm.Statistics.CreaturesKilled;

		public KillCreatureGoal(Type mobileType, int amount): base(mobileType, amount)
		{
			EventSink.OnKilledBy += Check;
		}

		private void Check(OnKilledByEventArgs e)
		{
			if (e.KilledBy is PlayerMobile pm && e.Killed.GetType() == Type)
			{
				InternalCheck(pm);
			}
		}
	}
}
