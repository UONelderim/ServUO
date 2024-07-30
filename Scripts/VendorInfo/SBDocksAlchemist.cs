using Server.Items;
using System.Collections.Generic;

namespace Server.Mobiles
{
	public class SBDocksAlchemist : SBInfo
	{
		private readonly List<IBuyItemInfo> m_BuyInfo = new InternalBuyInfo();
		private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();

		public override IShopSellInfo SellInfo => m_SellInfo;
		public override List<IBuyItemInfo> BuyInfo => m_BuyInfo;

		public class InternalBuyInfo : List<IBuyItemInfo>
		{
			public InternalBuyInfo()
			{
				Add(new GenericBuyInfo("1116302", typeof(Saltpeter), 167, 20, 16954, 1150));

				Add(new GenericBuyInfo(typeof(RefreshPotion), 15, 100, 0xF0B, 0));
				Add(new GenericBuyInfo(typeof(AgilityPotion), 15, 100, 0xF08, 0));
				Add(new GenericBuyInfo(typeof(NightSightPotion), 15, 100, 0xF06, 0));
				Add(new GenericBuyInfo(typeof(LesserHealPotion), 15, 100, 0xF0C, 0));
				Add(new GenericBuyInfo(typeof(StrengthPotion), 15, 100, 0xF09, 0));
				Add(new GenericBuyInfo(typeof(LesserPoisonPotion), 15, 100, 0xF0A, 0));
				Add(new GenericBuyInfo(typeof(LesserCurePotion), 15, 100, 0xF07, 0));
				Add(new GenericBuyInfo(typeof(LesserExplosionPotion), 21, 100, 0xF0D, 0));
				Add(new GenericBuyInfo(typeof(MortarPestle), 30, 100, 0xE9B, 0));

				Add(new GenericBuyInfo(typeof(BlackPearl), 7, 100, 0xF7A, 0));
				Add(new GenericBuyInfo(typeof(Bloodmoss), SBHerbalist.GlobalHerbsPriceBuy, 100, 0xF7B, 0));
				Add(new GenericBuyInfo(typeof(Garlic), SBHerbalist.GlobalHerbsPriceBuy, 100, 0xF84, 0));
				Add(new GenericBuyInfo(typeof(Ginseng), SBHerbalist.GlobalHerbsPriceBuy, 100, 0xF85, 0));
				Add(new GenericBuyInfo(typeof(MandrakeRoot), SBHerbalist.GlobalHerbsPriceBuy, 100, 0xF86, 0));
				Add(new GenericBuyInfo(typeof(Nightshade), SBHerbalist.GlobalHerbsPriceBuy, 100, 0xF88, 0));
				Add(new GenericBuyInfo(typeof(SpidersSilk), 5, 100, 0xF8D, 0));
				Add(new GenericBuyInfo(typeof(SulfurousAsh), 5, 100, 0xF8C, 0));

				Add(new GenericBuyInfo(typeof(Bottle), 5, 100, 0xF0E, 0));
				Add(new GenericBuyInfo(typeof(HeatingStand), 50, 100, 0x1849, 0));

				Add(new GenericBuyInfo("1041060", typeof(HairDye), 1000, 100, 0xEFF, 0));

				Add(new GenericBuyInfo(typeof(HeatingStand), 2, 100, 0x1849, 0)); // This is on OSI :-P
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
				Add(typeof(Saltpeter), 10);

				Add(typeof(BlackPearl), 2);
				Add(typeof(Bloodmoss), SBHerbalist.GlobalHerbsPriceSell);
				Add(typeof(MandrakeRoot), SBHerbalist.GlobalHerbsPriceSell);
				Add(typeof(Garlic), SBHerbalist.GlobalHerbsPriceSell);
				Add(typeof(Ginseng), SBHerbalist.GlobalHerbsPriceSell);
				Add(typeof(Nightshade), SBHerbalist.GlobalHerbsPriceSell);
				Add(typeof(SpidersSilk), 2);
				Add(typeof(SulfurousAsh), 2);
				Add(typeof(Bottle), 2);
				Add(typeof(MortarPestle), 4);
				Add(typeof(HairDye), 200);

				Add(typeof(NightSightPotion), 7);
				Add(typeof(AgilityPotion), 7);
				Add(typeof(StrengthPotion), 7);
				Add(typeof(RefreshPotion), 7);
				Add(typeof(LesserCurePotion), 7);
				Add(typeof(LesserHealPotion), 7);
				Add(typeof(LesserPoisonPotion), 7);
				Add(typeof(LesserExplosionPotion), 10);

				Add(typeof(Blowpipe), 8);
			}
		}
	}
}
