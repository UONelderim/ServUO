using Server;
using Server.Mobiles;
using System;

namespace Scripts.Mythik.Systems.Achievements
{
	public class DiscoveryAchievement : BaseAchievement
	{
		private readonly string _Region;

		public DiscoveryAchievement(int id, int catid, int itemIcon, bool secret, BaseAchievement prereq, string title,
			string desc, ushort points, string region, params Type[] rewards)
			: base(id, catid, itemIcon, secret, prereq, title, desc, points, 1, rewards)
		{
			_Region = region;
			CompletionTotal = 1;
			EventSink.OnEnterRegion += EventSink_OnEnterRegion;
		}

		private void EventSink_OnEnterRegion(OnEnterRegionEventArgs e)
		{
			if (e?.NewRegion?.Name == null || e.From == null)
				return;
			if (e.NewRegion.Name.Contains(_Region) && e.From is PlayerMobile player)
			{
				player.SetAchievementStatus(this, 1);
			}
		}
	}
}
