using Server.Items;
using System.Collections.Generic;

namespace Server.Mobiles
{
    public class SBSpearForkWeapon : SBInfo
    {
        private readonly List<IBuyItemInfo> m_BuyInfo = new InternalBuyInfo();
        private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();

        public override IShopSellInfo SellInfo => m_SellInfo;
        public override List<IBuyItemInfo> BuyInfo => m_BuyInfo;

        public class InternalBuyInfo : List<IBuyItemInfo>
        {
            public InternalBuyInfo()
            {
                Add(new GenericBuyInfo(typeof(Pitchfork), 19, 20, 0xE87, 0));
                Add(new GenericBuyInfo(typeof(ShortSpear), 23, 20, 0x1403, 0));
                Add(new GenericBuyInfo(typeof(Spear), 31, 20, 0xF62, 0));
            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
            public InternalSellInfo()
            {
                Add(typeof(Spear), 15);
                Add(typeof(Pitchfork), 9);
                Add(typeof(ShortSpear), 11);
            }
        }
    }
}
