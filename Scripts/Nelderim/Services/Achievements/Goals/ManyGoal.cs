using Server;
using Server.Mobiles;
using System;

namespace Nelderim.Achievements
{
	public abstract class ManyGoal : Goal
	{
		protected readonly Type[] Types;

		public ManyGoal(params Type[] types) : base((int)Math.Pow(types.Length + 1, 2))
		{
			Types = types;
		}

		protected void Progress(PlayerMobile pm, Type type)
		{
			var index = Array.IndexOf(Types, type);
			if (pm != null && index != -1)
			{
				pm.SetAchievementProgress(Achievement, pm.GetAchivementProgress(Achievement) & (1 << index));
			}
		}

		public override int GetProgress(AchievementStatus status)
		{
			var value = status.Progress;
			int count = 0;
			while (value != 0)
			{
				count++;
				value &= value - 1;
			}
			return count;
		}
	}
}
