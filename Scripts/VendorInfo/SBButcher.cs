using Server.Items;
using System.Collections.Generic;

namespace Server.Mobiles
{
	public class SBButcher : SBInfo
	{
		private readonly List<IBuyItemInfo> m_BuyInfo = new InternalBuyInfo();
		private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();

		public override IShopSellInfo SellInfo => m_SellInfo;
		public override List<IBuyItemInfo> BuyInfo => m_BuyInfo;

		public class InternalBuyInfo : List<IBuyItemInfo>
		{
			public InternalBuyInfo()
			{
				Add(new GenericBuyInfo(typeof(Bacon), 7, 50, 0x979, 0));
				Add(new GenericBuyInfo(typeof(Ham), 26, 50, 0x9C9, 0));
				Add(new GenericBuyInfo(typeof(Sausage), 18, 50, 0x9C0, 0));
				Add(new GenericBuyInfo(typeof(RawChickenLeg), 6, 50, 0x1607, 0));
				Add(new GenericBuyInfo(typeof(RawBird), 9, 50, 0x9B9, 0));
				Add(new GenericBuyInfo(typeof(RawLambLeg), 9, 50, 0x1609, 0));
				Add(new GenericBuyInfo(typeof(RawRibs), 16, 50, 0x9F1, 0));
				Add(new GenericBuyInfo(typeof(Gut), 8, 50, 7407, 0));
				Add(new GenericBuyInfo(typeof(ButcherKnife), 13, 50, 0x13F6, 0));
				Add(new GenericBuyInfo(typeof(Cleaver), 13, 50, 0xEC3, 0));
				Add(new GenericBuyInfo(typeof(SkinningKnife), 13, 50, 0xEC4, 0));
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
				Add(typeof(RawRibs), 2);
				Add(typeof(RawLambLeg), 2);
				Add(typeof(RawChickenLeg), 2);
				Add(typeof(RawBird), 2);
				Add(typeof(Bacon), 2);
				Add(typeof(Sausage), 2);
				Add(typeof(Ham), 2);
				Add(typeof(Gut), 1);
				Add(typeof(ButcherKnife), 3);
				Add(typeof(Cleaver), 3);
				Add(typeof(SkinningKnife), 3);
			}
		}
	}
}
