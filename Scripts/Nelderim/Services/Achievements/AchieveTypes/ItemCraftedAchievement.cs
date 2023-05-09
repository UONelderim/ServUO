using Server;
using Server.Mobiles;
using System;

namespace Scripts.Mythik.Systems.Achievements
{

    public class ItemCraftedAchievement : BaseAchievement
    {
        private Type m_Item;
        public ItemCraftedAchievement(int id, int catid, int itemIcon, bool hiddenTillComplete, BaseAchievement prereq, int total, string title, string desc, short RewardPoints, Type item, params Type[] rewards)
            : base(id, catid, itemIcon, hiddenTillComplete, prereq, title, desc, RewardPoints, total, rewards)
        {
            m_Item = item;
            EventSink.CraftSuccess += EventSink_CraftSuccess; ;
        }

        private void EventSink_CraftSuccess(CraftSuccessEventArgs e)
        {
            if(e.Crafter is PlayerMobile && e.CraftedItem.GetType() == m_Item)
            {
                AchievementSystem.SetAchievementStatus(e.Crafter as PlayerMobile, this, e.CraftedItem.Amount);
            }
        }

       
    }
}
