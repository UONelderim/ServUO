using System;
using Server;
using Server.Items;
using Server.Mobiles;
using System.Collections;

/*
** XmlPointsRewards
** ArteGordon
** updated 11/08/04
**
** this class lets you specify rewards that can be purchased for XmlPoints kill Credits.
** The items will be displayed in the PointsRewardGump that is opened by the PointsRewardStone
*/

namespace Server.Engines.XmlSpawner2
{
    public class XmlPointsRewards
    {
        public int Cost;       // cost of the reward in credits
        public Type  RewardType;   // this will be used to create an instance of the reward
        public string Name;         // used to describe the reward in the gump
        public int ItemID;     // used for display purposes
        public object [] RewardArgs; // arguments passed to the reward constructor
        public int MinPoints;   // the minimum points requirement for the reward

        private static ArrayList    PointsRewardList = new ArrayList();
        
        public static ArrayList RewardsList { get { return PointsRewardList; } }
        
        public XmlPointsRewards(int minpoints, Type reward, string name, int cost, int id, object[] args)
        {
            RewardType = reward;
            Cost = cost;
            ItemID = id;
            Name = name;
            RewardArgs = args;
            MinPoints = minpoints;
        }
        
        public static void Initialize()
        {
            // these are items as rewards. Note that the args list must match a constructor for the reward type specified.
            PointsRewardList.Add( new XmlPointsRewards( 200, typeof(RewardScroll), "Zwoj nagrody klasy 10", 200, 0x14F0, new object[] { 10 }));
            PointsRewardList.Add( new XmlPointsRewards( 400, typeof(RewardScroll), "Zwoj nagrody klasy 9", 400, 0x14F0, new object[] { 9 }));
            PointsRewardList.Add( new XmlPointsRewards( 600, typeof(RewardScroll), "Zwoj nagrody klasy 8", 600, 0x14F0, new object[] { 8 }));
            PointsRewardList.Add( new XmlPointsRewards( 1000, typeof(RewardScroll), "Zwoj nagrody klasy 7", 1000, 0x14F0, new object[] { 7 }));
            PointsRewardList.Add( new XmlPointsRewards( 3000, typeof(RewardScroll), "Zwoj nagrody klasy 6", 3000, 0x14F0, new object[] { 6 }));
            PointsRewardList.Add( new XmlPointsRewards( 8000, typeof(RewardScroll), "Zwoj nagrody klasy 5", 8000, 0x14F0, new object[] { 5 }));
            PointsRewardList.Add( new XmlPointsRewards( 250, typeof(Silver), "Srebrne monety", 300, 0xEF0, new object[] { 500 }));


            // this is an example of adding a mobile as a reward
           // PointsRewardList.Add( new XmlPointsRewards( 0, typeof(RidableLlama),"Ridable Llama", 1, 0x20f6, null));

            // this is an example of adding an attachment as a reward
           // PointsRewardList.Add( new XmlPointsRewards( 0, typeof(XmlEnemyMastery), "+200% Balron Mastery for 1 day", 2, 0, new object[] { "Balron", 50, 200, 1440.0 }));
           // PointsRewardList.Add( new XmlPointsRewards( 0, typeof(XmlStr), "+20 Strength for 1 day", 10, 0, new object[] { 20, 86400.0 }));
        }

    }
}
