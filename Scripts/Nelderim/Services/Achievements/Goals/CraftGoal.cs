using Server;
using Server.Mobiles;
using System;

namespace Nelderim.Achievements
{
	public class CraftGoal : Goal
	{
		private readonly Type _CraftedType;

		public CraftGoal(Type craftedType, int amount): base(amount)
		{
			_CraftedType = craftedType;
			EventSink.CraftSuccess += Progress;
		}

		private void Progress(CraftSuccessEventArgs e)
		{
			if (e.Crafter is PlayerMobile pm && e.CraftedItem.GetType() == _CraftedType)
			{
				pm.SetAchievementProgress(Achievement, pm.GetAchivementProgress(Achievement) + e.CraftedItem.Amount);
			}
		}
	}
}
