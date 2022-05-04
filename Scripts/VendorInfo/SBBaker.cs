using Server.Items;
using System.Collections.Generic;

namespace Server.Mobiles
{
    public class SBBaker : SBInfo
    {
        private readonly List<IBuyItemInfo> m_BuyInfo = new InternalBuyInfo();
        private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();

        public override IShopSellInfo SellInfo => m_SellInfo;
        public override List<IBuyItemInfo> BuyInfo => m_BuyInfo;

        public class InternalBuyInfo : List<IBuyItemInfo>
        {
            public InternalBuyInfo()
            {
				Add( new GenericBuyInfo( typeof( BreadLoaf ), 7, 50, 0x103B, 0 ) ); 
				Add( new GenericBuyInfo( typeof( CheeseWheel ), 4, 50, 0x97E, 0 ) ); 
				Add( new GenericBuyInfo( typeof( FrenchBread ), 3, 50, 0x98C, 0 ) ); 
				Add( new GenericBuyInfo( typeof( FriedEggs ), 8, 50, 0x9B6, 0 ) ); 
				Add( new GenericBuyInfo( typeof( Cake ), 11, 50, 0x9E9, 0 ) ); 
				Add( new GenericBuyInfo( typeof( Cookies ), 6, 50, 0x160b, 0 ) ); 
				Add( new GenericBuyInfo( typeof( Muffins ), 5, 50, 0x9eb, 0 ) ); 
				Add( new GenericBuyInfo( typeof( CheesePizza ), 14, 50, 0x1040, 0 ) ); 
             
				Add( new GenericBuyInfo( typeof( ApplePie ), 10, 50, 0x1041, 0 ) ); 

				Add( new GenericBuyInfo( typeof( PeachCobbler ), 10, 50, 0x1041, 0 ) ); 

				Add( new GenericBuyInfo( typeof( Quiche ), 25, 50, 0x1041, 0 ) ); 
				Add( new GenericBuyInfo( typeof( Dough ), 8, 50, 0x103d, 0 ) ); 
				Add( new GenericBuyInfo( typeof( JarHoney ), 3, 50, 0x9ec, 0 ) ); 
				Add( new BeverageBuyInfo( typeof( Pitcher ), BeverageType.Water, 11, 50, 0x1F9D, 0 ) );
				Add( new GenericBuyInfo( typeof( SackFlour ), 3, 50, 0x1039, 0 ) ); 
				Add( new GenericBuyInfo( typeof( Eggs ), 3, 50, 0x9B5, 0 ) ); 
            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
            public InternalSellInfo()
            {
				Add( typeof( BreadLoaf ), 3 ); 
				Add( typeof( CheeseWheel ), 2 ); 
				Add( typeof( FrenchBread ), 1 ); 
				Add( typeof( FriedEggs ), 4 ); 
				Add( typeof( Cake ), 5 ); 
				Add( typeof( Cookies ), 3 ); 
				Add( typeof( Muffins ), 2 ); 
				Add( typeof( CheesePizza ), 7 ); 
				Add( typeof( ApplePie ), 5 ); 
				Add( typeof( PeachCobbler ), 5 ); 
				Add( typeof( Quiche ), 12 ); 
				Add( typeof( Dough ), 4 ); 
				Add( typeof( JarHoney ), 1 ); 
				Add( typeof( Pitcher ), 5 );
				Add( typeof( SackFlour ), 1 ); 
				Add( typeof( Eggs ), 1 ); 
            }
        }
    }
}
