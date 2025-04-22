using System.Collections.Generic;
using Server.Items.Crops;
using static Server.Mobiles.SBHerbalist;

namespace Server.Mobiles 
{ 
	public class SBHerbalistUndershadow : SBInfo 
	{
		private readonly List<IBuyItemInfo> m_BuyInfo = new InternalBuyInfo();
		private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();

		public override IShopSellInfo SellInfo => m_SellInfo;
		public override List<IBuyItemInfo> BuyInfo => m_BuyInfo;

		public class InternalBuyInfo : List<IBuyItemInfo> 
		{ 
			public InternalBuyInfo() 
			{ 
				Add(new GenericBuyInfo(typeof(SzczepkaBoczniak), GlobalHerbsSeedlingPriceBuy, 50, 0x0F23, 872));
				Add(new GenericBuyInfo(typeof(SzczepkaZagwica), GlobalHerbsSeedlingPriceBuy, 50, 0x0F23, 1236));
				Add(new GenericBuyInfo(typeof(SzczepkaLysiczka), GlobalHerbsSeedlingPriceBuy, 50, 0x0F23, 798));
				Add(new GenericBuyInfo(typeof(SzczepkaKrwawyMech), GlobalHerbsSeedlingPriceBuy, 50, 0x0DCD, 438));
				Add(new GenericBuyInfo(typeof(SzczepkaMuchomor), GlobalHerbsSeedlingPriceBuy, 50, 0x0F23, 1509));
			}
		} 

		public class InternalSellInfo : GenericSellInfo 
		{ 
			public InternalSellInfo() 
			{ 
				Add(typeof(SzczepkaBoczniak), GlobalHerbsSeedlingPriceSell);
				Add(typeof(SzczepkaZagwica), GlobalHerbsSeedlingPriceSell);
				Add(typeof(SzczepkaLysiczka), GlobalHerbsSeedlingPriceSell);
				Add(typeof(SzczepkaKrwawyMech), GlobalHerbsSeedlingPriceSell);
				Add(typeof(SzczepkaMuchomor), GlobalHerbsSeedlingPriceSell);
			}
		}
	} 
}
