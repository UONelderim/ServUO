using Server.Items;
using System.Collections.Generic;

namespace Server.Mobiles
{
    public class SBBeekeeper : SBInfo
    {
        private readonly List<IBuyItemInfo> m_BuyInfo = new InternalBuyInfo();
        private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();

        public override IShopSellInfo SellInfo => m_SellInfo;
        public override List<IBuyItemInfo> BuyInfo => m_BuyInfo;

        public class InternalBuyInfo : List<IBuyItemInfo>
        {
            public InternalBuyInfo()
            {
				Add( new GenericBuyInfo( typeof( JarHoney ), 3, 50, 0x9EC, 0 ) );
				Add( new GenericBuyInfo( typeof( Beeswax ), 3, 50, 0x1422, 0 ) );
				Add( new GenericBuyInfo( typeof( SackFlour ), 10, 50, 0x1039, 0 ) );
				Add( new GenericBuyInfo( typeof( SheafOfHay ), 4, 50, 0xF36, 0 ) );
            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
            public InternalSellInfo()
            {
				Add( typeof( JarHoney ), 1 );
				Add( typeof( Beeswax ), 1 );
				Add( typeof( SackFlour ), 1 );
				Add( typeof( SheafOfHay ), 1 );
			
            }
        }
    }
}
