using Server;
using Server.Mobiles;
using System;

namespace Nelderim.Achievements
{
	public class CraftManyGoal : ManyGoal
	{
		public CraftManyGoal(params Type[] craftTypes) : base(craftTypes)
		{
			EventSink.CraftSuccess += e => Progress(e.Crafter as PlayerMobile, e.CraftedItem.GetType());
		}
	}
}
