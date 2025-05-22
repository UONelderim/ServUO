using System.Collections;
using System.Collections.Generic;
using Server.Items;

namespace Server.Mobiles
{
	public class SBPostMaster : SBInfo
	{
		private List<IBuyItemInfo> m_BuyInfo = new();
		private IShopSellInfo m_SellInfo = new InternalSellInfo();

		public override IShopSellInfo SellInfo => m_SellInfo;
		public override List<IBuyItemInfo> BuyInfo => m_BuyInfo;


		public class InternalBuyInfo : ArrayList
		{
			public InternalBuyInfo()
			{
				Add(new GenericBuyInfo(typeof(BlankScroll), 5, 20, 0x0E34, 0));
				Add(new GenericBuyInfo(typeof(AddressBook), 50, 20, 0x2254, 0));
				Add(new GenericBuyInfo(typeof(HouseMailBoxDeed), 20000, 20, 0x14F0, 0));
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
				Add(typeof(BlankScroll), 3);
			}
		}
	}
}
