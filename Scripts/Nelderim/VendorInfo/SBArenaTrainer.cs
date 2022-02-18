#region References

using System.Collections.Generic;
using Server.Items;

#endregion

namespace Server.Mobiles
{
	public class SBArenaTrainer : SBInfo
	{
		public override IShopSellInfo SellInfo { get; } = new GenericSellInfo();

		public override List<IBuyItemInfo> BuyInfo { get; } = new InternalBuyInfo();

		private class InternalBuyInfo : List<IBuyItemInfo>
		{
			public InternalBuyInfo()
			{
				Add(new GenericBuyInfo(typeof(Bandage), 5, 200, 0xE21, 0));
			}
		}
	}
}
