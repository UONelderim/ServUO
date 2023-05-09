using Server;
using Server.Mobiles;
using System;

namespace Scripts.Mythik.Systems.Achievements
{
    public class HunterAchievement : BaseAchievement
    {
        private Type m_Mobile;
        public HunterAchievement(int id, int catid, int itemIcon, bool secret, BaseAchievement prereq, int total, string title, string desc, ushort points, Type targets, params Type[] rewards)
            : base(id, catid, itemIcon, secret, prereq, title, desc, points, total, rewards)
        {
            m_Mobile = targets;
            EventSink.OnKilledBy += EventSink_OnKilledBy;
        }

        private void EventSink_OnKilledBy(OnKilledByEventArgs e)
        {
	        if (e.KilledBy is PlayerMobile player && e.Killed.GetType() == m_Mobile)
            {
                AchievementSystem.SetAchievementStatus(player, this, 1);
            }
        }
    }
}
