using Server.Items;
using System.Collections.Generic;

namespace Server.Mobiles
{
	public class SBBanker : SBInfo
	{
		private readonly List<IBuyItemInfo> m_BuyInfo = new InternalBuyInfo();
		private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();

		public override IShopSellInfo SellInfo => m_SellInfo;
		public override List<IBuyItemInfo> BuyInfo => m_BuyInfo;

		public class InternalBuyInfo : List<IBuyItemInfo>
		{
			public InternalBuyInfo()
			{
				Add(new GenericBuyInfo("1041243", typeof(ContractOfEmployment), 5000, 20, 0x14F0, 0));
				Add(new GenericBuyInfo("1062332", typeof(VendorRentalContract), 20000, 20, 0x14F0, 0x672));
				// Add(new GenericBuyInfo("1159156", typeof(CommissionContractOfEmployment), 25000, 20, 0x14F0, 0));
				Add(new GenericBuyInfo("1047016", typeof(CommodityDeed), 5, 20, 0x14F0, 0x47));
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
		}
	}
}
