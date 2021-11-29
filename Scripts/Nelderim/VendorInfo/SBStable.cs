using System.Collections.Generic;
using Server.Items;

namespace Server.Mobiles
{
	public class SBStable : SBInfo
	{
		private List<IBuyItemInfo> m_BuyInfo = new InternalBuyInfo();
		private IShopSellInfo m_SellInfo = new InternalSellInfo();

		public override IShopSellInfo SellInfo => m_SellInfo;
		public override List<IBuyItemInfo> BuyInfo => m_BuyInfo;

		private class InternalBuyInfo : List<IBuyItemInfo>
        {
			public InternalBuyInfo()
			{  
				Add( new AnimalBuyInfo( 1, typeof( Cat ), 100, 5, 201, 0 ) );
				Add( new AnimalBuyInfo( 1, typeof( Dog ), 200, 5, 217, 0 ) );
				Add( new GenericBuyInfo( typeof( Bandage ), 10, 20, 0xE21, 0 ) ); 	
			}
		}

		private sealed class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
				Add( typeof( Bandage ), 1 );
			}
		}
	}
}
