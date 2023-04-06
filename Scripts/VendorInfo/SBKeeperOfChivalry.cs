using Server.Items;
using System.Collections.Generic;

namespace Server.Mobiles
{
	public class SBKeeperOfChivalry : SBInfo
	{
		private readonly List<IBuyItemInfo> m_BuyInfo = new InternalBuyInfo();
		private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();

		public override IShopSellInfo SellInfo => m_SellInfo;
		public override List<IBuyItemInfo> BuyInfo => m_BuyInfo;

		public class InternalBuyInfo : List<IBuyItemInfo>
		{
			public InternalBuyInfo()
			{
				Add(new GenericBuyInfo(typeof(BookOfChivalry), 500, 20, 0x2252, 0));
				AddRange(ChivalryScrolls);
			}
		}
		
		public static List<IBuyItemInfo> ChivalryScrolls = new List<IBuyItemInfo>(
			new []{
				new GenericBuyInfo(typeof(CloseWoundsScroll), 20, 10, 0x1F6E, 1150),
				new GenericBuyInfo(typeof(RemoveCurseScroll), 20, 10, 0x1F6E, 1150),
				new GenericBuyInfo(typeof(CleanseByFireScroll), 20, 10, 0x1F6D, 1150),
				new GenericBuyInfo(typeof(ConsecrateWeaponScroll), 20, 10, 0x1F6D, 1150),
				new GenericBuyInfo(typeof(DivineFuryScroll), 20, 10, 0x1F6D, 1150)
			});

		public class InternalSellInfo : GenericSellInfo
		{
		}
	}
}
