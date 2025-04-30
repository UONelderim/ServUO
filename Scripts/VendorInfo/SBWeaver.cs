using Server.Items;
using System.Collections.Generic;

namespace Server.Mobiles
{
	public class SBWeaver : SBInfo
	{
		private readonly List<IBuyItemInfo> m_BuyInfo = new InternalBuyInfo();
		private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();

		public override IShopSellInfo SellInfo => m_SellInfo;
		public override List<IBuyItemInfo> BuyInfo => m_BuyInfo;

		public class InternalBuyInfo : List<IBuyItemInfo>
		{
			public InternalBuyInfo()
			{
				Add(new GenericBuyInfo(typeof(Scissors), 13, 20, 0xF9F, 0));
				Add(new GenericBuyInfo(typeof(Dyes), 8, 20, 0xFA9, 0));
				Add(new GenericBuyInfo(typeof(DyeTub), 9, 20, 0xFAB, 0));
				Add(new GenericBuyInfo(typeof(BoltOfCloth), 100, 20, 0xf95, 0));
				Add(new GenericBuyInfo("1023614", typeof(LightYarnUnraveled), 18, 20, 0xE1F, 0));
				Add(new GenericBuyInfo("1023614", typeof(LightYarn), 18, 20, 0xE1E, 0));
				Add(new GenericBuyInfo("1023615", typeof(DarkYarn), 18, 20, 0xE1D, 0));

				Add(new GenericBuyInfo("1154003", typeof(LeatherBraid), 50, 500, 5152, 2968));
			}

			private void PurchaseCloth(UncutCloth cloth, GenericBuyInfo info)
			{
				if (cloth != null)
				{
					cloth.ItemID = info.ItemID;
				}
			}

			private void PurchaseBolt(BoltOfCloth cloth, GenericBuyInfo info)
			{
				if (cloth != null)
				{
					cloth.ItemID = info.ItemID;
				}
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
				Add(typeof(Scissors), 6);
				Add(typeof(Dyes), 4);
				Add(typeof(DyeTub), 4);
				Add(typeof(BoltOfCloth), 50);
				Add(typeof(LightYarnUnraveled), 9);
				Add(typeof(LightYarn), 9);
				Add(typeof(DarkYarn), 9);
			}
		}
	}
}
