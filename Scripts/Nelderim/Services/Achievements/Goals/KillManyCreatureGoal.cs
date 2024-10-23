using Server;
using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nelderim.Achievements
{
	public class KillManyCreatureGoal : ManyPlayerStatisticGoal
	{
		protected override Dictionary<Type, long> GoalStatistic(PlayerMobile pm) => pm.Statistics.CreaturesKilled;

		public KillManyCreatureGoal(int amount, params Type[] mobileTypes) : base(amount, mobileTypes)
		{
			EventSink.CreatureDeath += Check;
		}

		private void Check(CreatureDeathEventArgs e)
		{
			if (e.Killer is PlayerMobile pm && Types.Contains(e.Creature.GetType()))
			{
				InternalCheck(pm);
			}
		}
	}
}
