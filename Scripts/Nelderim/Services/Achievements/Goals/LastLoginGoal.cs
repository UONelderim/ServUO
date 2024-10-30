using System;
using Server;
using Server.Mobiles;

namespace Nelderim.Achievements
{
	public class LastLoginGoal : Goal
	{
		public LastLoginGoal(int days) : base(days)
		{
			EventSink.Login += Check;
		}

		private void Check(LoginEventArgs e)
		{
			if (e.Mobile is PlayerMobile pm && GetProgress(pm) >= Amount)
			{
				pm.Complete(Achievement);
			}
		}

		public override int GetProgress(PlayerMobile pm)
		{
			return (int)(DateTime.UtcNow - pm.Account.LastLogin).TotalDays;
		}
	}
}
