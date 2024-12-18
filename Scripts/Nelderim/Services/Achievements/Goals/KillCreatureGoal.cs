﻿using Server;
using Server.Mobiles;
using System;
using System.Collections.Generic;

namespace Nelderim.Achievements
{
	public class KillCreatureGoal : BasicPlayerStatisticGoal
	{
		protected override Dictionary<Type, long> GoalStatistic(PlayerMobile pm) => pm.Statistics.CreaturesKilled;

		public KillCreatureGoal(int amount, Type mobileType): base(amount, mobileType)
		{
			EventSink.CreatureDeath += Check;
		}

		private void Check(CreatureDeathEventArgs e)
		{
			if (e.Killer is PlayerMobile pm && e.Creature.GetType() == Type)
			{
				InternalCheck(pm);
			}
		}
	}
}
