using Server.Items;
using System.Collections.Generic;

namespace Server.Mobiles
{
	public class SBMapmaker : SBInfo
	{
		private readonly List<IBuyItemInfo> m_BuyInfo = new InternalBuyInfo();
		private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();

		public override IShopSellInfo SellInfo => m_SellInfo;
		public override List<IBuyItemInfo> BuyInfo => m_BuyInfo;

		public class InternalBuyInfo : List<IBuyItemInfo>
		{
			public InternalBuyInfo()
			{
				Add(new GenericBuyInfo(typeof(BlankScroll), 10, 50, 0xEF3, 0));
				Add(new GenericBuyInfo(typeof(MapmakersPen), 30, 50, 0x0FBF, 0));
				Add(new GenericBuyInfo(typeof(BlankMap), 10, 50, 0x14EC, 0));

			//	for (int i = 0; i < PresetMapEntry.Table.Length; ++i)
			//		Add(new PresetMapBuyInfo(PresetMapEntry.Table[i], Utility.RandomMinMax(7, 10), 20));
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
				Add(typeof(BlankScroll), 3);
				Add(typeof(MapmakersPen), 8);
				Add(typeof(BlankMap), 3);
				Add(typeof(LocalMap), 4);
				Add(typeof(CityMap), 5);
				Add(typeof(SeaChart), 7);
				Add(typeof(WorldMap), 9);
				//TODO: Buy back maps that the mapmaker sells!!!
			}
		}
	}
}
