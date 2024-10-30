using Server;
using Server.Mobiles;

namespace Nelderim.Achievements
{
	public class AccountAgeGoal : Goal
	{
		public AccountAgeGoal(int amount) : base(amount)
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
			return (int)pm.Account.Age.TotalDays;
		}
	}
}
