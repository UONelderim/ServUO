using Server.Items;
using System.Collections.Generic;

namespace Server.Mobiles
{
    public class SBKeeperOfNinjitsu : SBInfo
    {
        private readonly List<IBuyItemInfo> m_BuyInfo = new InternalBuyInfo();
        private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();

        public override IShopSellInfo SellInfo => m_SellInfo;
        public override List<IBuyItemInfo> BuyInfo => m_BuyInfo;

        public class InternalBuyInfo : List<IBuyItemInfo>
        {
            public InternalBuyInfo()
            {
                Add(new GenericBuyInfo(typeof(BookOfNinjitsu), 500, 20, 0x23A0, 0));
                AddRange(NinjitsuScrolls);
            }
        }
        
        public static List<IBuyItemInfo> NinjitsuScrolls = new List<IBuyItemInfo>(
	        new []{
		        new GenericBuyInfo(typeof(AnimalFormScroll), 70, 10, 0x1F6F, 1000),
		        new GenericBuyInfo(typeof(MirrorImageScroll), 70, 10, 0x1F70, 1000),
		        new GenericBuyInfo(typeof(FocusAttackScroll), 70, 10, 0x1F6F, 1000),
		        new GenericBuyInfo(typeof(BackstabScroll), 70, 10, 0x1F70, 1000)
	        });

        public class InternalSellInfo : GenericSellInfo
        {
        }
    }
}
