using Server;
using Server.Mobiles;
using System;

namespace Scripts.Mythik.Systems.Achievements
{
	public class HarvestAchievement : BaseAchievement
	{
		private Type m_Item;

		public HarvestAchievement(int id, int catid, int itemIcon, bool secret, BaseAchievement prereq, int total,
			string title, string desc, ushort points, Type targets, params Type[] rewards)
			: base(id, catid, itemIcon, secret, prereq, title, desc, points, total, rewards)
		{
			m_Item = targets;
			EventSink.ResourceHarvestSuccess += EventSink_ResourceHarvestSuccess;
		}

		private void EventSink_ResourceHarvestSuccess(ResourceHarvestSuccessEventArgs e)
		{
			if (e.Resource.GetType() == m_Item && e.Harvester is PlayerMobile player)
			{
				player.SetAchievementStatus(this, e.Resource.Amount);
			}
		}
	}
}
