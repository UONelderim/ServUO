using Server;
using System;

namespace Scripts.Mythik.Systems.Achievements.AchieveTypes
{
    class SkillProgressAchievement : BaseAchievement
    {
        private SkillName m_Skill;
        public SkillProgressAchievement(int id, int catid, int itemIcon, bool secret, BaseAchievement prereq, int total, string title, string desc, SkillName skill, ushort points, params Type[] rewards)
            : base(id, catid, itemIcon, secret, prereq, title, desc, points, total, rewards)
        {
            m_Skill = skill;
            //ServUO is missing this event sink, you can create it your self and reenable if you wish.

            //EventSink.SkillGain += EventSink_SkillGain;
        }

        /*private void EventSink_SkillGain(SkillGainEventArgs e)
        {
            if (e.From is PlayerMobile && e.Skill.SkillID == (int)m_Skill)
            {
                if (e.Skill.BaseFixedPoint >= CompletionTotal)
                    AchievmentSystem.SetAchievementStatus(e.From as PlayerMobile, this, e.Skill.BaseFixedPoint);
            }
        }*/


    }
}
