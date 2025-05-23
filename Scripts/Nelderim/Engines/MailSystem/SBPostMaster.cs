using System.Collections.Generic;
using Server.Items;

namespace Server.Mobiles
{
	public class SBPostMaster : SBInfo
	{
		private InternalBuyInfo _BuyInfo = new();
		private InternalSellInfo _SellInfo = new();

		public override IShopSellInfo SellInfo => _SellInfo;
		public override List<IBuyItemInfo> BuyInfo => _BuyInfo;


		public class InternalBuyInfo : List<IBuyItemInfo>
		{
			public InternalBuyInfo()
			{
				Add(new GenericBuyInfo(typeof(Letter), 20, 20, 0x0E34, 0));
				Add(new GenericBuyInfo(typeof(Parcel), 200, 20, 0x14F0, 0));
				Add(new GenericBuyInfo(typeof(Beeswax), 3, 50, 0x1422, 0));
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
				//Nothing for players to sell
			}
		}
	}
}
