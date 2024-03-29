using System.Collections.Generic;

namespace Server.Mobiles
{
    public class SBRancher : SBInfo
    {
        private readonly List<IBuyItemInfo> m_BuyInfo = new InternalBuyInfo();
        private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();

        public override IShopSellInfo SellInfo => m_SellInfo;
        public override List<IBuyItemInfo> BuyInfo => m_BuyInfo;

        public class InternalBuyInfo : List<IBuyItemInfo>
        {
            public InternalBuyInfo()
            {
                Add(new AnimalBuyInfo(1, typeof(PackHorse), 631, 10, 291, 0));
            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
        }
    }
}
