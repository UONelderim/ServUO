using Server;
using Server.Mobiles;

namespace Nelderim.Achievements
{
	public class BolaKillGoal : Goal
	{
		public BolaKillGoal() : base(1)
		{
			EventSink.BolaThrown += Check;
		}

		public override int GetProgress(PlayerMobile pm)
		{
			return pm.IsCompleted(Achievement) ? 1 : 0;
		}
		
		private void Check(BolaThrownEventArgs e)
		{
			if (e.From is PlayerMobile pm && e.Target is PlayerMobile targetPm && !targetPm.Alive)
			{
				pm.Complete(Achievement);
			}
		}
	}
}
