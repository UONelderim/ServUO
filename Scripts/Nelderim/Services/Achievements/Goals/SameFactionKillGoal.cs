using System.Collections.Generic;
using Server;
using Server.Mobiles;

namespace Nelderim.Achievements
{
	public class SameFactionKillGoal : PlayerStatisticGoal
	{
		public override int GetProgress(PlayerMobile pm)
		{
			return (int)pm.Statistics.PlayerKillsFaction.GetValueOrDefault(pm.Faction, 0);
		}

		public SameFactionKillGoal(int amount): base(amount)
		{
			EventSink.PlayerDeath += Check;
		}

		private void Check(PlayerDeathEventArgs e)
		{
			if (e.Killer is PlayerMobile killer && 
			    e.Mobile is PlayerMobile killed && 
			    killer.Faction == killed.Faction)
			{
				InternalCheck(killer);
			}
		}
	}
}
