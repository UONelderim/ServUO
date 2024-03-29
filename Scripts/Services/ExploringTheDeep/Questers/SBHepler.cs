using Server.Items;
using System.Collections.Generic;

namespace Server.Mobiles
{
    public class SBHepler : SBInfo
    {
        private readonly List<IBuyItemInfo> m_BuyInfo = new InternalBuyInfo();
        private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();

        public override IShopSellInfo SellInfo => m_SellInfo;
        public override List<IBuyItemInfo> BuyInfo => m_BuyInfo;

        public class InternalBuyInfo : List<IBuyItemInfo>
        {
            public InternalBuyInfo()
            {
                Add(new GenericBuyInfo("1154215", typeof(SpecialSalvageHook), 1900, 10, 0x14F9, 2654));
            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
        }
    }
}
