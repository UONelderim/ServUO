using System.Collections.Generic;
using Server.Items.Crops;
using static Server.Mobiles.SBHerbalist;

namespace Server.Mobiles 
{ 
	public class SBHerbalistOverworld : SBInfo 
	{
		private readonly List<IBuyItemInfo> m_BuyInfo = new InternalBuyInfo();
		private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();

		public override IShopSellInfo SellInfo => m_SellInfo;
		public override List<IBuyItemInfo> BuyInfo => m_BuyInfo;

		public class InternalBuyInfo : List<IBuyItemInfo> 
		{ 
			public InternalBuyInfo() 
			{ 
				
				Add(new GenericBuyInfo(typeof(SzczepkaCzosnek), GlobalHerbsSeedlingPriceBuy, 50, 0x18E3, 178));
				Add(new GenericBuyInfo(typeof(SzczepkaZenszen), GlobalHerbsSeedlingPriceBuy, 50, 0x18EB, 0));
				Add(new GenericBuyInfo(typeof(SzczepkaMandragora), GlobalHerbsSeedlingPriceBuy, 50, 0x18DD, 0));
				Add(new GenericBuyInfo(typeof(SzczepkaKrwawyMech), GlobalHerbsSeedlingPriceBuy, 50, 0x0DCD, 438));
				Add(new GenericBuyInfo(typeof(SzczepkaWilczaJagoda), GlobalHerbsSeedlingPriceBuy, 50, 0x18E7, 0));
			}
		} 

		public class InternalSellInfo : GenericSellInfo 
		{ 
			public InternalSellInfo() 
			{ 
				Add(typeof(SzczepkaCzosnek), GlobalHerbsSeedlingPriceSell);
				Add(typeof(SzczepkaZenszen), GlobalHerbsSeedlingPriceSell);
				Add(typeof(SzczepkaMandragora), GlobalHerbsSeedlingPriceSell);
				Add(typeof(SzczepkaKrwawyMech), GlobalHerbsSeedlingPriceSell);
				Add(typeof(SzczepkaWilczaJagoda), GlobalHerbsSeedlingPriceSell);
			}
		}
	} 
}
