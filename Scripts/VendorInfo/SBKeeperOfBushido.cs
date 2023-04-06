using Server.Items;
using System.Collections.Generic;

namespace Server.Mobiles
{
	public class SBKeeperOfBushido : SBInfo
	{
		private readonly List<IBuyItemInfo> m_BuyInfo = new InternalBuyInfo();
		private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();

		public override IShopSellInfo SellInfo => m_SellInfo;
		public override List<IBuyItemInfo> BuyInfo => m_BuyInfo;

		public class InternalBuyInfo : List<IBuyItemInfo>
		{
			public InternalBuyInfo()
			{
				Add(new GenericBuyInfo(typeof(BookOfBushido), 500, 20, 0x238C, 0));
				AddRange(BushidoScrolls);
			}
		}
		
		public static List<IBuyItemInfo> BushidoScrolls = new List<IBuyItemInfo>(
			new []{
				new GenericBuyInfo(typeof(HonorableExecutionScroll), 60, 10, 0x1F71, 137),
				new GenericBuyInfo(typeof(ConfidenceScroll), 60, 10, 0x1F72, 137),
				new GenericBuyInfo(typeof(CounterAttackScroll), 60, 10, 0x1F72, 137)
			});

		public class InternalSellInfo : GenericSellInfo
		{
		}
	}
}
