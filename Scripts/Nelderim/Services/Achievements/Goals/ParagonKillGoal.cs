using System.Linq;
using Server;
using Server.Mobiles;

namespace Nelderim.Achievements
{
	public class ParagonKillGoal : PlayerStatisticGoal
	{
		public override int GetProgress(PlayerMobile pm)
		{
			return (int)pm.Statistics.ParagonsKilled.Values.Sum();
		}

		public ParagonKillGoal(int amount): base(amount)
		{
			EventSink.CreatureDeath += Check;
		}

		private void Check(CreatureDeathEventArgs e)
		{
			if (e.Killer is PlayerMobile pm && e.Creature is BaseCreature bc && bc.IsParagon)
			{
				InternalCheck(pm);
			}
		}
	}
}
