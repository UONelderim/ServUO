using System.Collections.Generic;
using Nelderim.Engines.ChaosChest;
using Server.Items;

namespace Server.Mobiles
{
	public class SBKonsorcjum : SBInfo
	{
		private List<IBuyItemInfo> m_BuyInfo = new InternalBuyInfo();
		private IShopSellInfo m_SellInfo = new GenericSellInfo();

		public override IShopSellInfo SellInfo => m_SellInfo;
		public override List<IBuyItemInfo> BuyInfo => m_BuyInfo;

		private class InternalBuyInfo : List<IBuyItemInfo>
		{
			public InternalBuyInfo()
			{
				Add(new GenericBuyInfo(typeof(Silver), 100, 1000, 0xEF0, 0));
				Add(new GenericBuyInfo(typeof(ChaosChest), 100000, 10, 0x1445, 0));
			}
		}
	}
}
