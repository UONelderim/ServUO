using Server.Items;
using Server.Items.Crops;
using System.Collections.Generic;

namespace Server.Mobiles
{
	public class SBHerbalist : SBInfo
	{
		public static int GlobalHerbsSeedlingPriceBuy => 100;   // koszt kupna szczepek ziol u npc
		public static int GlobalHerbsPriceBuy => 20;    // koszt kupna ziol u npc
		public static int GlobalHerbsPriceBuyFence => (int)(1.4 * GlobalHerbsPriceBuy);
		public static int GlobalHerbsPriceBuyDouble => 2 * GlobalHerbsPriceBuy;

		public static int GlobalHerbsSeedlingPriceSell => 10;   // zysk za szczepke sprzedana do npc
		public static int GlobalHerbsPriceSell => 2;    // cena za ziolo sprzedane do npc
		public static int GlobalHerbsPriceSellFence => 1;
		public static int GlobalHerbsPriceSellHalf => 1;

		private readonly List<IBuyItemInfo> m_BuyInfo = new InternalBuyInfo();
		private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();

		public override IShopSellInfo SellInfo => m_SellInfo;
		public override List<IBuyItemInfo> BuyInfo => m_BuyInfo;

		public class InternalBuyInfo : List<IBuyItemInfo>
		{
			public InternalBuyInfo()
			{
				Add(new GenericBuyInfo(typeof(Bloodmoss), SBHerbalist.GlobalHerbsPriceBuy, 200, 0xF7B, 0));
				Add(new GenericBuyInfo(typeof(MandrakeRoot), SBHerbalist.GlobalHerbsPriceBuy, 200, 0xF86, 0));
				Add(new GenericBuyInfo(typeof(Garlic), SBHerbalist.GlobalHerbsPriceBuy, 200, 0xF84, 0));
				Add(new GenericBuyInfo(typeof(Ginseng), SBHerbalist.GlobalHerbsPriceBuy, 200, 0xF85, 0));
				Add(new GenericBuyInfo(typeof(Nightshade), SBHerbalist.GlobalHerbsPriceBuy, 200, 0xF88, 0));
				Add(new GenericBuyInfo(typeof(Bottle), 5, 200, 0xF0E, 0));
				Add(new GenericBuyInfo(typeof(MortarPestle), 30, 20, 0xE9B, 0));
				Add(new GenericBuyInfo(typeof(DestroyingAngel), 8, 50, 0xE1F, 0));
				Add(new GenericBuyInfo(typeof(PetrafiedWood), 8, 50, 0x97A, 0));
				Add(new GenericBuyInfo(typeof(SpringWater), 8, 50, 0xE24, 0));
				Add(new GenericBuyInfo("Szufla do lajna", typeof(DungShovel), 30, 50, 0xF39, DungShovel.DefaultHue));
				Add(new GenericBuyInfo("Wiadro na nawoz", typeof(DungBucket), 2000, 5, DungBucket.GraphicsEmpty, DungBucket.HueEmpty));
				Add(new GenericBuyInfo(typeof(SzczepkaCzosnek), SBHerbalist.GlobalHerbsSeedlingPriceBuy, 50, 0x18E3, 178));
				Add(new GenericBuyInfo(typeof(SzczepkaZenszen), SBHerbalist.GlobalHerbsSeedlingPriceBuy, 50, 0x18EB, 0));
				Add(new GenericBuyInfo(typeof(SzczepkaMandragora), SBHerbalist.GlobalHerbsSeedlingPriceBuy, 50, 0x18DD, 0));
				Add(new GenericBuyInfo(typeof(SzczepkaKrwawyMech), SBHerbalist.GlobalHerbsSeedlingPriceBuy, 50, 0x0DCD, 438));
				Add(new GenericBuyInfo(typeof(SzczepkaWilczaJagoda), SBHerbalist.GlobalHerbsSeedlingPriceBuy, 50, 0x18E7, 0));
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
				Add(typeof(Bloodmoss), SBHerbalist.GlobalHerbsPriceSell);
				Add(typeof(MandrakeRoot), SBHerbalist.GlobalHerbsPriceSell);
				Add(typeof(Garlic), SBHerbalist.GlobalHerbsPriceSell);
				Add(typeof(Ginseng), SBHerbalist.GlobalHerbsPriceSell);
				Add(typeof(Nightshade), SBHerbalist.GlobalHerbsPriceSell);
				Add(typeof(Bottle), 3);
				Add(typeof(MortarPestle), 4);
				Add(typeof(DungShovel), 6);
				Add(typeof(DungBucket), 8);
				Add(typeof(SzczepkaCzosnek), SBHerbalist.GlobalHerbsSeedlingPriceSell);
				Add(typeof(SzczepkaZenszen), SBHerbalist.GlobalHerbsSeedlingPriceSell);
				Add(typeof(SzczepkaMandragora), SBHerbalist.GlobalHerbsSeedlingPriceSell);
				Add(typeof(SzczepkaKrwawyMech), SBHerbalist.GlobalHerbsSeedlingPriceSell);
				Add(typeof(SzczepkaWilczaJagoda), SBHerbalist.GlobalHerbsSeedlingPriceSell);
			}
		}
	}
}
