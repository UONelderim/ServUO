using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;

namespace Nelderim.Achievements
{
	class SkillProgressGoal : PlayerStatisticGoal
	{
		private SkillName _Skill;

		public SkillProgressGoal(SkillName skill, int amount): base(amount)
		{
			_Skill = skill;
			EventSink.SkillGain += Check;
		}

		private void Check(SkillGainEventArgs e)
		{
		    if (e.From is PlayerMobile pm && e.Skill.SkillID == (int)_Skill)
		    {
			    InternalCheck(pm);
		    }
		}

		public override int GetProgress(PlayerMobile pm)
		{
			return Math.Min(Amount, (int)pm.Statistics.MaxSkillGained.GetValueOrDefault(_Skill, 0));
		}
	}
}
