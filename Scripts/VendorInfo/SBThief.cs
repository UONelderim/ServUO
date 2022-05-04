using Server.Items;
using System.Collections.Generic;

namespace Server.Mobiles
{
    public class SBThief : SBInfo
    {
        private readonly List<IBuyItemInfo> m_BuyInfo = new InternalBuyInfo();
        private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();

        public override IShopSellInfo SellInfo => m_SellInfo;
        public override List<IBuyItemInfo> BuyInfo => m_BuyInfo;

        public class InternalBuyInfo : List<IBuyItemInfo>
        {
            public InternalBuyInfo()
            {
				Add( new GenericBuyInfo( typeof( Backpack ), 28, 5, 0x9B2, 0 ) );
				Add( new GenericBuyInfo( typeof( Pouch ), 20, 5, 0xE79, 0 ) );
				Add( new GenericBuyInfo( typeof( Torch ), 8, 5, 0xF6B, 0 ) );
				Add( new GenericBuyInfo( typeof( Lantern ), 8, 5, 0xA25, 0 ) );
				//Add( new GenericBuyInfo( typeof( OilFlask ), 8, 20, 0x####, 0 ) );
				Add( new GenericBuyInfo( typeof( Lockpick ), 12, 10, 0x14FC, 0 ) );
				Add( new GenericBuyInfo( typeof( WoodenBox ), 14, 5, 0x9AA, 0 ) );
				Add( new GenericBuyInfo( typeof( Key ), 11, 5, 0x100E, 0 ) );
				Add( new GenericBuyInfo( typeof( HairDye ), 200, 5, 0xEFF, 0 ) );
            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
            public InternalSellInfo()
            {
				Add( typeof( Backpack ), 4 );
				Add( typeof( Pouch ), 3 );
				Add( typeof( Torch ), 3 );
				Add( typeof( Lantern ), 1 );
				//Add( typeof( OilFlask ), 4 );
				Add( typeof( Lockpick ), 3 );
				Add( typeof( WoodenBox ), 7 );
				Add( typeof( HairDye ), 200 );
            }
        }
    }
}
