using Server;
using Server.Mobiles;

namespace Nelderim.Achievements
{
	class TotalSkillProgressGoal : Goal
	{
		public TotalSkillProgressGoal(int amount): base(amount)
		{
			EventSink.SkillGain += Progress;
		}

		private void Progress(SkillGainEventArgs e)
		{
		    if (e.From is PlayerMobile pm && GetProgress(pm) >= Amount)
		    {
			    pm.Complete(Achievement);
		    }
		}

		public override int GetProgress(PlayerMobile pm)
		{
			return pm.SkillsTotal;
		}
	}
}
