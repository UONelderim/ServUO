using Server;
using System;
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
		    if (e.From is PlayerMobile pm)
		    {
			    pm.SetAchievementProgress(Achievement, e.From.SkillsTotal);
		    }
		}
	}
}
