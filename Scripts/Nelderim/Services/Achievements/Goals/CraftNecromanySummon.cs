using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;

namespace Nelderim.Achievements
{
	public class CraftNecromancySummon : BasicPlayerStatisticGoal
	{
		protected override Dictionary<Type, long> GoalStatistic(PlayerMobile pm) => pm.Statistics.NecromancySummonsCrafted;

		public CraftNecromancySummon(int amount, Type summonType): base(amount, summonType)
		{
			EventSink.NecromancySummonCrafted += Check;
		}

		private void Check(NecromancySummonCraftedEventArgs e)
		{
			if (e.Crafter is PlayerMobile pm && e.Summon.GetType() == Type)
			{
				InternalCheck(pm);
			}
		}
	}
}
