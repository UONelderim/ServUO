#region References

using System.Collections.Generic;
using Server.Items;

#endregion

namespace Server.Mobiles
{
	public class SBStable : SBInfo
	{
		public override IShopSellInfo SellInfo { get; } = new InternalSellInfo();

		public override List<IBuyItemInfo> BuyInfo { get; } = new InternalBuyInfo();

		private class InternalBuyInfo : List<IBuyItemInfo>
		{
			public InternalBuyInfo()
			{
				Add(new AnimalBuyInfo(1, typeof(Cat), 100, 5, 201, 0));
				Add(new AnimalBuyInfo(1, typeof(Dog), 200, 5, 217, 0));
				Add(new GenericBuyInfo(typeof(Bandage), 10, 20, 0xE21, 0));
			}
		}

		private sealed class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
				Add(typeof(Bandage), 1);
			}
		}
	}
}
