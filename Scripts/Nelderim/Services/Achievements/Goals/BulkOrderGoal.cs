using Server;
using Server.Mobiles;
using System;
using System.Collections.Generic;

namespace Nelderim.Achievements
{
	public class BulkOrderGoal : BasicPlayerStatisticGoal
	{
		protected override Dictionary<Type, long> GoalStatistic(PlayerMobile pm) => pm.Statistics.BulkOrderDeedsCompleted;

		public BulkOrderGoal(Type bodType, int amount) : base(bodType, amount)
		{
			EventSink.BODCompleted += Check;
		}
		
		private void Check(BODCompletedEventArgs e)
		{
			if (e.User is PlayerMobile pm && e.BOD.GetType() == Type)
			{
				InternalCheck(pm);
			}
		}
	}
}
