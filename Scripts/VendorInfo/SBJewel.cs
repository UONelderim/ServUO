using Server.Items;
using System.Collections.Generic;

namespace Server.Mobiles
{
	public class SBJewel : SBInfo
	{
		private readonly List<IBuyItemInfo> m_BuyInfo = new InternalBuyInfo();
		private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();

		public override IShopSellInfo SellInfo => m_SellInfo;
		public override List<IBuyItemInfo> BuyInfo => m_BuyInfo;

		public class InternalBuyInfo : List<IBuyItemInfo>
		{
			public InternalBuyInfo()
			{
				Add(new GenericBuyInfo(typeof(GoldRing), 27, 50, 0x108A, 0));
				Add(new GenericBuyInfo(typeof(Necklace), 26, 50, 0x1085, 0));
				Add(new GenericBuyInfo(typeof(GoldNecklace), 27, 50, 0x1088, 0));
				Add(new GenericBuyInfo(typeof(GoldBeadNecklace), 27, 50, 0x1089, 0));
				Add(new GenericBuyInfo(typeof(Beads), 27, 50, 0x108B, 0), true);
				Add(new GenericBuyInfo(typeof(GoldBracelet), 27, 50, 0x1086, 0));
				Add(new GenericBuyInfo(typeof(GoldEarrings), 27, 50, 0x1087, 0));

				Add(new GenericBuyInfo("1060740", typeof(BroadcastCrystal), 68, 50, 0x1ED0, 0,
					new object[] { 500 })); // 500 charges
				Add(new GenericBuyInfo("1060740", typeof(BroadcastCrystal), 131, 50, 0x1ED0, 0,
					new object[] { 1000 })); // 1000 charges
				Add(new GenericBuyInfo("1060740", typeof(BroadcastCrystal), 256, 50, 0x1ED0, 0,
					new object[] { 2000 })); // 2000 charges

				Add(new GenericBuyInfo("1060740", typeof(ReceiverCrystal), 6, 50, 0x1ED0, 0));

				Add(new GenericBuyInfo(typeof(StarSapphire), 200, 50, 0xF21, 0, true));
				Add(new GenericBuyInfo(typeof(Emerald), 140, 50, 0xF10, 0, true));
				Add(new GenericBuyInfo(typeof(Sapphire), 150, 50, 0xF19, 0, true));
				Add(new GenericBuyInfo(typeof(Ruby), 140, 50, 0xF13, 0, true));
				Add(new GenericBuyInfo(typeof(Citrine), 100, 50, 0xF15, 0, true));
				Add(new GenericBuyInfo(typeof(Amethyst), 100, 50, 0xF16, 0, true));
				Add(new GenericBuyInfo(typeof(Tourmaline), 80, 50, 0xF2D, 0, true));
				Add(new GenericBuyInfo(typeof(Amber), 50, 50, 0xF25, 0, true));
				Add(new GenericBuyInfo(typeof(Diamond), 300, 50, 0xF26, 0, true));
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
				Add(typeof(Amber), 25);
				Add(typeof(Amethyst), 50);
				Add(typeof(Citrine), 50);
				Add(typeof(Diamond), 150);
				Add(typeof(Emerald), 70);
				Add(typeof(Ruby), 70);
				Add(typeof(Sapphire), 70);
				Add(typeof(StarSapphire), 100);
				Add(typeof(Tourmaline), 40);
				Add(typeof(GoldRing), 13);
				Add(typeof(SilverRing), 10);
				Add(typeof(Necklace), 13);
				Add(typeof(GoldNecklace), 13);
				Add(typeof(GoldBeadNecklace), 13);
				Add(typeof(SilverNecklace), 10);
				Add(typeof(SilverBeadNecklace), 10);
				Add(typeof(Beads), 13);
				Add(typeof(GoldBracelet), 13);
				Add(typeof(SilverBracelet), 10);
				Add(typeof(GoldEarrings), 13);
				Add(typeof(SilverEarrings), 10);
			}
		}
	}
}
