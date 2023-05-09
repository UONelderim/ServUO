using System;

namespace Scripts.Mythik.Systems.Achievements
{
    public abstract class BaseAchievement
    {
        public BaseAchievement(int id, int catid, int itemIcon, bool hiddenTillComplete, BaseAchievement prereq, string title, string desc, short rewardPoints, int total, params Type[] rewards)
        {
            ID = id;
            CategoryID = catid;
            Title = title;
            Desc = desc;
            RewardPoints = rewardPoints;
            CompletionTotal = total;
            RewardItems = rewards;
            HiddenTillComplete = hiddenTillComplete;
            ItemIcon = itemIcon;
        }

        public int ID { get; set; }
        public int CategoryID { get; set; }
        public string Title { get; set; }
        public string Desc { get; set; }
        public int ItemIcon { get; set; }
        public short RewardPoints { get; set; }
        public Type[] RewardItems { get; set; }
        public int CompletionTotal { get; set; }
        public bool HiddenTillComplete { get; private set; }
        public BaseAchievement PreReq { get; set; }
    }
}
