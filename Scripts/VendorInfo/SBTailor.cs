using Server.Items;
using System.Collections.Generic;

namespace Server.Mobiles
{
	public class SBTailor : SBInfo
	{
		private readonly List<IBuyItemInfo> m_BuyInfo = new InternalBuyInfo();
		private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();

		public override IShopSellInfo SellInfo => m_SellInfo;
		public override List<IBuyItemInfo> BuyInfo => m_BuyInfo;

		public class InternalBuyInfo : List<IBuyItemInfo>
		{
			public InternalBuyInfo()
			{
				Add(new GenericBuyInfo(typeof(SewingKit), 15, 5, 0xF9D, 0));
				Add(new GenericBuyInfo(typeof(Scissors), 11, 5, 0xF9F, 0));
				Add(new GenericBuyInfo(typeof(DyeTub), 8, 5, 0xFAB, 0));
				Add(new GenericBuyInfo(typeof(Dyes), 8, 2, 0xFA9, 0));

				Add(new GenericBuyInfo(typeof(Shirt), 12, 5, 0x1517, 0));
				Add(new GenericBuyInfo(typeof(ShortPants), 11, 5, 0x152E, 0));
				Add(new GenericBuyInfo(typeof(FancyShirt), 21, 5, 0x1EFD, 0));
				Add(new GenericBuyInfo(typeof(LongPants), 14, 5, 0x1539, 0));
				Add(new GenericBuyInfo(typeof(FancyDress), 26, 5, 0x1EFF, 0));
				Add(new GenericBuyInfo(typeof(PlainDress), 18, 5, 0x1F01, 0));
				Add(new GenericBuyInfo(typeof(Kilt), 14, 5, 0x1537, 0));
				Add(new GenericBuyInfo(typeof(Kilt), 14, 5, 0x1537, Utility.RandomDyedHue()));
				Add(new GenericBuyInfo(typeof(HalfApron), 10, 5, 0x153b, 0));
				Add(new GenericBuyInfo(typeof(Robe), 26, 5, 0x1F03, 0));
				Add(new GenericBuyInfo(typeof(Cloak), 23, 5, 0x1515, 0));
				Add(new GenericBuyInfo(typeof(Cloak), 23, 5, 0x1515, 0));
				Add(new GenericBuyInfo(typeof(Doublet), 13, 5, 0x1F7B, 0));
				Add(new GenericBuyInfo(typeof(Tunic), 18, 5, 0x1FA1, 0));
				Add(new GenericBuyInfo(typeof(JesterSuit), 38, 5, 0x1F9F, 0));

				Add(new GenericBuyInfo(typeof(JesterHat), 24, 5, 0x171C, 0));
				Add(new GenericBuyInfo(typeof(FloppyHat), 18, 5, 0x1713, Utility.RandomDyedHue()));
				Add(new GenericBuyInfo(typeof(WideBrimHat), 20, 5, 0x1714, Utility.RandomDyedHue()));
				Add(new GenericBuyInfo(typeof(Cap), 18, 5, 0x1715, Utility.RandomDyedHue()));
				Add(new GenericBuyInfo(typeof(TallStrawHat), 21, 5, 0x1716, Utility.RandomDyedHue()));
				Add(new GenericBuyInfo(typeof(StrawHat), 17, 5, 0x1717, Utility.RandomDyedHue()));
				Add(new GenericBuyInfo(typeof(WizardsHat), 24, 5, 0x1718, Utility.RandomDyedHue()));
				Add(new GenericBuyInfo(typeof(LeatherCap), 20, 5, 0x1DB9, Utility.RandomDyedHue()));
				Add(new GenericBuyInfo(typeof(FeatheredHat), 20, 5, 0x171A, Utility.RandomDyedHue()));
				Add(new GenericBuyInfo(typeof(TricorneHat), 20, 5, 0x171B, Utility.RandomDyedHue()));
				Add(new GenericBuyInfo(typeof(Bandana), 6, 5, 0x1540, Utility.RandomDyedHue()));
				Add(new GenericBuyInfo(typeof(SkullCap), 7, 5, 0x1544, Utility.RandomDyedHue()));

				Add(new GenericBuyInfo(typeof(BoltOfCloth), 100, 5, 0xf95, Utility.RandomDyedHue()));

				Add(new GenericBuyInfo(typeof(Cloth), 2, 5, 0x1766, Utility.RandomDyedHue()));
				Add(new GenericBuyInfo(typeof(UncutCloth), 2, 5, 0x1767, Utility.RandomDyedHue()));

				Add(new GenericBuyInfo(typeof(Cotton), 102, 5, 0xDF9, 0));
				Add(new GenericBuyInfo(typeof(Wool), 102, 5, 0xDF8, 0));
				Add(new GenericBuyInfo(typeof(Flax), 102, 5, 0x1A9C, 0));
				Add(new GenericBuyInfo(typeof(SpoolOfThread), 102, 10, 0xFA0, 0));

				Add(new GenericBuyInfo(typeof(ThighBoots), 52, 5, 0x1711, Utility.RandomNeutralHue()));
				Add(new GenericBuyInfo(typeof(Shoes), 32, 5, 0x170f, Utility.RandomNeutralHue()));
				Add(new GenericBuyInfo(typeof(Boots), 42, 5, 0x170b, Utility.RandomNeutralHue()));
				Add(new GenericBuyInfo(typeof(Sandals), 22, 5, 0x170d, Utility.RandomNeutralHue()));
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
				Add(typeof(Scissors), 6);
				Add(typeof(SewingKit), 8);
				Add(typeof(Dyes), 4);
				Add(typeof(DyeTub), 4);

				Add(typeof(BoltOfCloth), 50);

				Add(typeof(FancyShirt), 10);
				Add(typeof(Shirt), 6);

				Add(typeof(ShortPants), 3);
				Add(typeof(LongPants), 5);

				Add(typeof(Cloak), 4);
				Add(typeof(FancyDress), 12);
				Add(typeof(Robe), 9);
				Add(typeof(PlainDress), 7);

				Add(typeof(Skirt), 5);
				Add(typeof(Kilt), 5);

				Add(typeof(Doublet), 7);
				Add(typeof(Tunic), 9);
				Add(typeof(JesterSuit), 13);

				Add(typeof(FullApron), 5);
				Add(typeof(HalfApron), 5);

				Add(typeof(JesterHat), 6);
				Add(typeof(FloppyHat), 3);
				Add(typeof(WideBrimHat), 4);
				Add(typeof(Cap), 5);
				Add(typeof(SkullCap), 3);
				Add(typeof(Bandana), 3);
				Add(typeof(TallStrawHat), 4);
				Add(typeof(StrawHat), 4);
				Add(typeof(WizardsHat), 5);
				Add(typeof(Bonnet), 4);
				Add(typeof(FeatheredHat), 5);
				Add(typeof(TricorneHat), 4);

				Add(typeof(Shoes), 4);
				Add(typeof(Boots), 5);
				Add(typeof(ThighBoots), 6);
				Add(typeof(Sandals), 2);

				Add(typeof(SpoolOfThread), 1);

				Add(typeof(Flax), 5);
				Add(typeof(Cotton), 5);
				Add(typeof(Wool), 5);
			}
		}
	}
}
