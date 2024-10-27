
using Server;
using Server.Mobiles;

namespace Nelderim.Achievements
{
	public class ThrowBolaGoal : PlayerStatisticGoal
	{
		public override int GetProgress(PlayerMobile pm)
		{
			return (int)pm.Statistics.BolasThrown;
		}
		
		public ThrowBolaGoal(int amount): base(amount)
		{
			EventSink.BolaThrown += Check;
		}
		
		private void Check(BolaThrownEventArgs e)
		{
			if (e.From is PlayerMobile pm)
			{
				InternalCheck(pm);
			}
		}
	}
}
