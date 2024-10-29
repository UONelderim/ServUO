using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;

namespace Nelderim.Achievements
{
	public class QuestCompletedGoal : BasicPlayerStatisticGoal
	{
		protected override Dictionary<Type, long> GoalStatistic(PlayerMobile pm) => pm.Statistics.QuestsCompleted;

		public QuestCompletedGoal(int amount, Type type) : base(amount, type)
		{
			EventSink.QuestComplete += Check;
		}
		
		private void Check(QuestCompleteEventArgs e)
		{
			if (e.Mobile is PlayerMobile pm && e.QuestType == Type)
			{
				InternalCheck(pm);
			}
		}
	}
}
