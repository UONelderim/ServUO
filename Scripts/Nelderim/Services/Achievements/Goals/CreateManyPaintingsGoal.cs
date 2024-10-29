using Server;
using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nelderim.Achievements
{
	public class CreateManyPaintingsGoal : ManyPlayerStatisticGoal
	{
		protected override Dictionary<Type, long> GoalStatistic(PlayerMobile pm) => pm.Statistics.PaintingsCreated;

		public CreateManyPaintingsGoal(int amount, params Type[] mobileTypes) : base(amount, mobileTypes)
		{
			EventSink.PaintingCreated += Check;
		}

		private void Check(PaintingCreatedEventArgs e)
		{
			if (e.Artist is PlayerMobile pm && Types.Contains(e.Painting.GetType()))
			{
				InternalCheck(pm);
			}
		}
	}
}
