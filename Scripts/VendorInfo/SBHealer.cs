using Server.Items;
using System.Collections.Generic;
using System;

namespace Server.Mobiles
{
	public class SBHealer : SBInfo
	{
		private readonly List<IBuyItemInfo> m_BuyInfo = new InternalBuyInfo();
		private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();

		public override IShopSellInfo SellInfo => m_SellInfo;
		public override List<IBuyItemInfo> BuyInfo => m_BuyInfo;

		public class InternalBuyInfo : List<IBuyItemInfo>
		{
			public InternalBuyInfo()

			{
				Add(new GenericBuyInfo(typeof(Bandage), 5, 250, 0xE21, 0));
				Add(new GenericBuyInfo(typeof(LesserHealPotion), 15, 50, 0xF0C, 0));
				Add(new GenericBuyInfo(typeof(LesserCurePotion), 15, 50, 0xF0C, 0));
				Add(new GenericBuyInfo(typeof(Ginseng), SBHerbalist.GlobalHerbsPriceBuy, 50, 0xF85, 0));
				Add(new GenericBuyInfo(typeof(Garlic), SBHerbalist.GlobalHerbsPriceBuy, 50, 0xF84, 0));
				Add(new GenericBuyInfo(typeof(RefreshPotion), 15, 50, 0xF0B, 0));
				Add(new GenericBuyInfo(typeof(Bandage), 5, 200, 0xE21, 0));
				Add(new GenericBuyInfo(typeof(DestroyingAngel), 8, 50, 0xE1F, 0));
				Add(new GenericBuyInfo(typeof(PetrafiedWood), 8, 50, 0x97A, 0));
				Add(new GenericBuyInfo(typeof(SpringWater), 8, 50, 0xE24, 0));
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
				Add(typeof(Bandage), 1);
				Add(typeof(LesserHealPotion), 7);
				Add(typeof(RefreshPotion), 7);
				Add(typeof(LesserHealPotion), 5);
				Add(typeof(RefreshPotion), 5);
				Add(typeof(Garlic), SBHerbalist.GlobalHerbsPriceSellHalf);
				Add(typeof(Ginseng), SBHerbalist.GlobalHerbsPriceSellHalf);
			}
		}
	}
}
