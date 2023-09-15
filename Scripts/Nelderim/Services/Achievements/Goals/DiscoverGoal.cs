using Server;
using Server.Mobiles;
using System;

namespace Nelderim.Achievements
{
	public class DiscoverGoal : Goal
	{
		private readonly string _Region;

		public DiscoverGoal(string region) : base(1)
		{
			_Region = region;
			EventSink.OnEnterRegion += Progress;
		}

		private void Progress(OnEnterRegionEventArgs e)
		{
			if (e?.NewRegion?.Name == null || e.From == null)
				return;
			if (e.NewRegion.Name.Contains(_Region) && e.From is PlayerMobile pm)
			{
				pm.SetAchievementProgress(Achievement, 1);
			}
		}
	}
}
