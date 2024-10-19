using Server;
using Server.Mobiles;
using System;
using Server.Engines.BulkOrders;

namespace Nelderim.Achievements
{
	public class BulkOrderGoal : Goal
	{
		private readonly BODType _BodType;

		public BulkOrderGoal(BODType bodType, int amount) : base(amount)
		{
			_BodType = bodType;
			EventSink.BODCompleted += Progress;
		}

		private void Progress(BODCompletedEventArgs e)
		{
			if (e.User is PlayerMobile pm && e.BOD is IBOD bod && bod.BODType == _BodType)
			{
				pm.SetAchievementProgress(Achievement, pm.GetAchivementProgress(Achievement) + 1);
			}
		}
	}
}
