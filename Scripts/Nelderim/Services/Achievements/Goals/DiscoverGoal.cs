using System;
using Server;
using Server.Mobiles;

namespace Nelderim.Achievements
{
	public class DiscoverGoal : Goal
	{
		private readonly string _Region;

		public DiscoverGoal(string region) : base(1)
		{
			if(!Region.Regions.Exists(r => r.Name == region))
			{
				Console.WriteLine("DiscoverGoal: Region not found: " + region);
			}
			_Region = region;
			EventSink.OnEnterRegion += Check;
		}

		private void Check(OnEnterRegionEventArgs e)
		{
			if (e?.NewRegion?.Name == null || e.From == null)
				return;
			if (e.From is PlayerMobile pm && e.NewRegion.Name.Contains(_Region))
			{
				pm.Complete(Achievement);
			}
		}
		
		public override int GetProgress(PlayerMobile pm)
		{
			return pm.IsCompleted(Achievement) ? 1 : 0;
		}
	}
}
