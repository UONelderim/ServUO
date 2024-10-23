
using Server;
using Server.Mobiles;

namespace Nelderim.Achievements
{
	public abstract class PlayerStatisticGoal(int amount) : Goal(amount)
	{
		protected void InternalCheck(PlayerMobile pm)
		{
			//We do it on a timer to let statistics update first
			Timer.DelayCall(() =>
				{
					if (pm != null && GetProgress(pm) >= Amount)
					{
						pm.Complete(Achievement);
					}
				}
			);
		}
	}
}
