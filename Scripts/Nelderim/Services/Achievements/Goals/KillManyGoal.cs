using Server;
using Server.Mobiles;
using System;

namespace Nelderim.Achievements
{
	public class KillManyGoal : ManyGoal
	{
		public KillManyGoal(params Type[] mobileTypes) : base(mobileTypes)
		{
			EventSink.OnKilledBy += e => Progress(e.KilledBy as PlayerMobile, e.Killed.GetType());
		}
	}
}
