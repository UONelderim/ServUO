using System;

namespace Scripts.Mythik.Systems.Achievements
{
    public abstract class BaseAchievement
    {
        public BaseAchievement(int id, int catid, int itemIcon, bool secret, BaseAchievement prereq, string title, string desc, ushort points, int total, params Type[] rewards)
        {
            ID = id;
            CategoryID = catid;
            Title = title;
            Desc = desc;
            Points = points;
            CompletionTotal = total;
            RewardItems = rewards;
            Secret = secret;
            ItemIcon = itemIcon;
            PreReq = prereq;
        }

        public int ID { get; set; }
        public int CategoryID { get; set; }
        public string Title { get; set; }
        public string Desc { get; set; }
        public int ItemIcon { get; set; }
        public ushort Points { get; set; }
        public Type[] RewardItems { get; set; }
        public int CompletionTotal { get; set; }
        public bool Secret { get; private set; }
        public BaseAchievement PreReq { get; set; }
    }
}
