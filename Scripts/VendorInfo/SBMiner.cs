using Server.Items;
using System.Collections.Generic;

namespace Server.Mobiles
{
	public class SBMiner : SBInfo
	{
		private readonly List<IBuyItemInfo> m_BuyInfo = new InternalBuyInfo();
		private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();

		public override IShopSellInfo SellInfo => m_SellInfo;
		public override List<IBuyItemInfo> BuyInfo => m_BuyInfo;

		public class InternalBuyInfo : List<IBuyItemInfo>
		{
			public InternalBuyInfo()
			{
				Add(new GenericBuyInfo(typeof(Bag), 20, 50, 0xE76, 0));
				Add(new GenericBuyInfo(typeof(Candle), 10, 50, 0xA28, 0));
				Add(new GenericBuyInfo(typeof(Torch), 20, 50, 0xF6B, 0));
				Add(new GenericBuyInfo(typeof(Lantern), 30, 50, 0xA25, 0));
				//Add( new GenericBuyInfo( typeof( OilFlask ), 8, 10, 0x####, 0 ) );
				Add(new GenericBuyInfo(typeof(Pickaxe), 30, 50, 0xE86, 0));
				Add(new GenericBuyInfo(typeof(Shovel), 30, 50, 0xF39, 0));
				Add(new GenericBuyInfo(typeof(IronIngot), 5, 50, 0x1BF2, 0));
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
				Add(typeof(Pickaxe), 8);
				Add(typeof(Shovel), 6);
				Add(typeof(Lantern), 1);
				//Add( typeof( OilFlask ), 4 );
				Add(typeof(Torch), 3);
				Add(typeof(Bag), 3);
				Add(typeof(Candle), 3);
				Add(typeof(IronIngot), 1);
			}
		}
	}
}
