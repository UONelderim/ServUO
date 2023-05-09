using Server;
using Server.Mobiles;
using System;

namespace Scripts.Mythik.Systems.Achievements
{
	public class ItemCraftedAchievement : BaseAchievement
	{
		private Type m_Item;

		public ItemCraftedAchievement(int id, int catid, int itemIcon, bool secret, BaseAchievement prereq, int total,
			string title, string desc, ushort points, Type item, params Type[] rewards)
			: base(id, catid, itemIcon, secret, prereq, title, desc, points, total, rewards)
		{
			m_Item = item;
			EventSink.CraftSuccess += EventSink_CraftSuccess;
			;
		}

		private void EventSink_CraftSuccess(CraftSuccessEventArgs e)
		{
			if (e.Crafter is PlayerMobile player && e.CraftedItem.GetType() == m_Item)
			{
				player.SetAchievementStatus(this, e.CraftedItem.Amount);
			}
		}
	}
}
