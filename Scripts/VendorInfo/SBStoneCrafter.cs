using Server.Items;
using System.Collections.Generic;

namespace Server.Mobiles
{
	public class SBStoneCrafter : SBInfo
	{
		private readonly List<IBuyItemInfo> m_BuyInfo = new InternalBuyInfo();
		private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();

		public override IShopSellInfo SellInfo => m_SellInfo;
		public override List<IBuyItemInfo> BuyInfo => m_BuyInfo;

		public class InternalBuyInfo : List<IBuyItemInfo>
		{
			public InternalBuyInfo()
			{
				Add(new GenericBuyInfo(typeof(Nails), 15, 20, 0x102E, 0));
				Add(new GenericBuyInfo(typeof(Axle), 2, 20, 0x105B, 0, true));
				Add(new GenericBuyInfo(typeof(DrawKnife), 15, 20, 0x10E4, 0));
				Add(new GenericBuyInfo(typeof(MouldingPlane), 15, 20, 0x102C, 0));
				Add(new GenericBuyInfo(typeof(SmoothingPlane), 15, 20, 0x1032, 0));
				Add(new GenericBuyInfo(typeof(JointingPlane), 15, 20, 0x1030, 0));

				Add(new GenericBuyInfo("Making Valuables With Stonecrafting", typeof(MasonryBook), 10625, 10, 0xFBE,
					0));
				Add(new GenericBuyInfo("Mining For Quality Stone", typeof(StoneMiningBook), 10625, 10, 0xFBE, 0));
				Add(new GenericBuyInfo("1044515", typeof(MalletAndChisel), 100, 50, 0x12B3, 0));
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
				Add(typeof(MasonryBook), 1000);
				Add(typeof(StoneMiningBook), 1000);
				Add(typeof(MalletAndChisel), 1);
			}
		}
	}
}
