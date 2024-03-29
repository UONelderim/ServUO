using Server.Items;
using System.Collections.Generic;

namespace Server.Mobiles
{
    public class SBSABlacksmith : SBInfo
    {
        private readonly List<IBuyItemInfo> m_BuyInfo = new InternalBuyInfo();
        private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();

        public override IShopSellInfo SellInfo => m_SellInfo;
        public override List<IBuyItemInfo> BuyInfo => m_BuyInfo;

        public class InternalBuyInfo : List<IBuyItemInfo>
        {
            public InternalBuyInfo()
            {
                Add(new GenericBuyInfo(typeof(IronIngot), 9, 16, 0x1BF2, 0));
                Add(new GenericBuyInfo(typeof(Tongs), 13, 14, 0xFBB, 0));
                Add(new GenericBuyInfo(typeof(GemMiningBook), 10625, 20, 0xFBE, 0));

            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
            public InternalSellInfo()
            {
                Add(typeof(IronIngot), 4);
                Add(typeof(Tongs), 7);
            }
        }
    }
}
