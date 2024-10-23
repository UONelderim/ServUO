using Server;
using Server.Mobiles;
using System.Linq;

namespace Nelderim.Achievements
{
	public class EnemyFactionKillGoal : PlayerStatisticGoal
	{
		public override int GetProgress(PlayerMobile pm)
		{
			return (int)pm.Statistics.PlayerKillsFaction.Where(kvp => pm.Faction.Enemies.Contains(kvp.Key)).Sum(kvp => kvp.Value);
		}

		public EnemyFactionKillGoal(int amount): base(amount)
		{
			EventSink.PlayerDeath += Check;
		}

		private void Check(PlayerDeathEventArgs e)
		{
			if (e.Killer is PlayerMobile killer && 
			    e.Mobile is PlayerMobile killed && 
			    killer.Faction.Enemies.Contains(killed.Faction))
			{
				InternalCheck(killer);
			}
		}
	}
}
