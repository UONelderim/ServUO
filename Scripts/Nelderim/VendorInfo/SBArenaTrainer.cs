using System.Collections.Generic;
using Server.Items;

namespace Server.Mobiles
{
    public class SBArenaTrainer : SBInfo
    {
        private List<IBuyItemInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new GenericSellInfo();

        public override IShopSellInfo SellInfo => m_SellInfo;
        public override List<IBuyItemInfo> BuyInfo => m_BuyInfo;

        private class InternalBuyInfo : List<IBuyItemInfo>
        {
            public InternalBuyInfo()
            {
                Add( new GenericBuyInfo( typeof( Bandage ), 5, 200, 0xE21, 0 ) );
            }
        }
    }
}
