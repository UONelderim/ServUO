using Server.Items;
using System.Collections.Generic;

namespace Server.Mobiles
{
	public class SBHairStylist : SBInfo
	{
		private readonly List<IBuyItemInfo> m_BuyInfo = new InternalBuyInfo();
		private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();

		public override IShopSellInfo SellInfo => m_SellInfo;
		public override List<IBuyItemInfo> BuyInfo => m_BuyInfo;

		public class InternalBuyInfo : List<IBuyItemInfo>
		{
			public InternalBuyInfo()
			{
				Add(new GenericBuyInfo("specjalna farba do brody", typeof(SpecialBeardDye), 50000, 50, 0xE26, 0));
				Add(new GenericBuyInfo("specjalna farba do wlosow", typeof(SpecialHairDye), 50000, 50, 0xE26, 0));
				Add(new GenericBuyInfo("farba do wlosow", typeof(HairDye), 1000, 50, 0xEFF, 0));
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
				Add(typeof(HairDye), 200);
				Add(typeof(SpecialBeardDye), 15000);
				Add(typeof(SpecialHairDye), 15000);
			}
		}
	}
}
