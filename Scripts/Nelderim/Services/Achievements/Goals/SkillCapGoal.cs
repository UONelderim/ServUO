using Server;
using System.Linq;
using Server.Mobiles;

namespace Nelderim.Achievements
{
	class SkillCapGoal : Goal
	{
		private int _Cap;

		public SkillCapGoal(int cap, int amount): base(amount)
		{
			_Cap = cap;

			EventSink.SkillCapChange += Progress;
		}

		private void Progress(SkillCapChangeEventArgs e)
		{
		    if (e.Mobile is PlayerMobile pm && GetProgress(pm) >= Amount)
		    {
			    pm.Complete(Achievement);
		    }
		}

		public override int GetProgress(PlayerMobile pm)
		{
			return pm.Skills.Count(s => s.CapFixedPoint >= _Cap);
		}
	}
}
