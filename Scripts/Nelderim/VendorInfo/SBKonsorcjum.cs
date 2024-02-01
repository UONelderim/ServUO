#region References

using System.Collections.Generic;
using Nelderim.Engines.ChaosChest;
using Server.Items;

#endregion

namespace Server.Mobiles
{
	public class SBKonsorcjum : SBInfo
	{
		public override IShopSellInfo SellInfo { get; } = new GenericSellInfo();

		public override List<IBuyItemInfo> BuyInfo { get; } = new InternalBuyInfo();

		private class InternalBuyInfo : List<IBuyItemInfo>
		{
			public InternalBuyInfo()
			{
				// Add(new GenericBuyInfo(typeof(Silver), 100, 1000, 0xEF0, 0));
				Add(new GenericBuyInfo(typeof(ChaosChest), 100000, 10, 0x1445, 0));
			}
		}
	}
}
