using Server;
using System;
using Server.Mobiles;

namespace Nelderim.Achievements
{
	class SkillProgressGoal : Goal
	{
		private SkillName _Skill;

		public SkillProgressGoal(SkillName skill, int amount): base(amount)
		{
			_Skill = skill;
			EventSink.SkillGain += Progress;
		}

		private void Progress(SkillGainEventArgs e)
		{
		    if (e.From is PlayerMobile pm && e.Skill.SkillID == (int)_Skill)
		    {
			    pm.SetAchievementProgress(Achievement, e.Skill.BaseFixedPoint);
		    }
		}
	}
}
