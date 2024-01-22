using System.Collections.Generic;

namespace Server.Mobiles
{
	public class SBDrowAnimalTrainer: SBInfo
	{
		public override IShopSellInfo SellInfo { get; } = new GenericSellInfo();

		public override List<IBuyItemInfo> BuyInfo { get; } = new InternalBuyInfo();
		
		private class InternalBuyInfo : List<IBuyItemInfo>
		{
			public InternalBuyInfo()
			{
				Add(new AnimalBuyInfo(1, typeof(JaskiniowyJaszczur), 550, 50, 0xDB, 0));
				Add(new AnimalBuyInfo(1, typeof(JaskiniowyZukJuczny), 631, 50, 0x317, 0));
			}
		}
	}
}
