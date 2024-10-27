using System.Linq;
using Server;
using Server.Mobiles;

namespace Nelderim.Achievements
{
	public class TamedTypesGoal : PlayerStatisticGoal
	{
		public override int GetProgress(PlayerMobile pm)
		{
			return pm.Statistics.AnimalsTamed.Keys.Count;
		}

		public TamedTypesGoal(int amount): base(amount)
		{
			EventSink.TameCreature += Check;
		}

		private void Check(TameCreatureEventArgs e)
		{
			if (e.Mobile is PlayerMobile pm)
			{
				InternalCheck(pm);
			}
		}
	}
}
