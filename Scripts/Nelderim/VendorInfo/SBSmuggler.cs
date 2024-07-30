#region References

using System.Collections.Generic;
using Server.Engines.Plants;
using Server.Items;

#endregion

namespace Server.Mobiles
{
	public class SBSmuggler : SBInfo
	{
		public override IShopSellInfo SellInfo { get; } = new InternalSellInfo();

		public override List<IBuyItemInfo> BuyInfo { get; } = new InternalBuyInfo();

		private class InternalBuyInfo : List<IBuyItemInfo>
		{
			public InternalBuyInfo()
			{
				Add(new GenericBuyInfo(typeof(Bandage), 7, 20, 0xE21, 0));

				Add(new GenericBuyInfo(typeof(HeatingStand), 15, 5, 0x1849, 0));
				Add(new GenericBuyInfo(typeof(Bottle), 7, 20, 0xF0E, 0));
				Add(new GenericBuyInfo(typeof(SpidersSilk), 5, 25, 0xF8D, 0));
				Add(new GenericBuyInfo(typeof(SulfurousAsh), 5, 25, 0xF8C, 0));
				Add(new GenericBuyInfo(typeof(Nightshade), SBHerbalist.GlobalHerbsPriceBuy, 25, 0xF88, 0));
				Add(new GenericBuyInfo(typeof(MandrakeRoot), SBHerbalist.GlobalHerbsPriceBuy, 25, 0xF86, 0));
				Add(new GenericBuyInfo(typeof(Ginseng), SBHerbalist.GlobalHerbsPriceBuy, 25, 0xF85, 0));
				Add(new GenericBuyInfo(typeof(Garlic), SBHerbalist.GlobalHerbsPriceBuy, 25, 0xF84, 0));
				Add(new GenericBuyInfo(typeof(Bloodmoss), SBHerbalist.GlobalHerbsPriceBuy, 25, 0xF7B, 0));
				Add(new GenericBuyInfo(typeof(BlackPearl), 7, 25, 0xF7A, 0));
				Add(new GenericBuyInfo(typeof(MortarPestle), 12, 5, 0xE9B, 0));

				Add(new GenericBuyInfo(typeof(GraveDust), 6, 25, 0xF8F, 0));
				Add(new GenericBuyInfo(typeof(NoxCrystal), 12, 25, 0xF8E, 0));
				Add(new GenericBuyInfo(typeof(PigIron), 10, 25, 0xF8A, 0));
				Add(new GenericBuyInfo(typeof(DaemonBlood), 12, 25, 0xF7D, 0));
				Add(new GenericBuyInfo(typeof(BatWing), 6, 225, 0xF78, 0));

				//Add( new GenericBuyInfo( typeof( RecallRune ), 50, 10, 0x1F14, 0 ) );

				Add(new GenericBuyInfo(typeof(BlankScroll), 10, 10, 0x0E34, 0));
				Add(new GenericBuyInfo(typeof(ScribesPen), 16, 5, 0xFBF, 0));

				/*
				Add( new GenericBuyInfo( typeof( Spellbook ), 100, 1, 0xEFA, 0 ) );
				Add( new GenericBuyInfo( typeof( BookOfChivalry ), 1000, 20, 0x2252, 0 ) );
				Add( new GenericBuyInfo( typeof( NecromancerSpellbook ), 1000, 10, 0x2253, 0 ) );
				Add( new GenericBuyInfo( typeof( BookOfBushido ), 1000,  20, 0x238C, 0 ) );
				Add( new GenericBuyInfo( typeof( BookOfNinjitsu ), 1000, 20, 0x23A0, 0 ) );
				*/
				Add(new GenericBuyInfo(typeof(Bedroll), 7, 5, 0xA59, 0));

				Add(new GenericBuyInfo(typeof(Key), 11, 5, 0x100E, 0));
				Add(new GenericBuyInfo(typeof(WoodenBox), 21, 5, 0xE7D, 0));

				Add(new GenericBuyInfo(typeof(Lockpick), 18, 5, 0x14FC, 0));

				Add(new GenericBuyInfo(typeof(Lantern), 8, 5, 0xA25, 0));
				Add(new GenericBuyInfo(typeof(Torch), 12, 5, 0xF6B, 0));
				Add(new GenericBuyInfo(typeof(Candle), 9, 5, 0xA28, 0));

				Add(new GenericBuyInfo(typeof(Bag), 27, 5, 0xE76, 0));
				Add(new GenericBuyInfo(typeof(Pouch), 26, 5, 0xE79, 0));
				Add(new GenericBuyInfo(typeof(Backpack), 29, 5, 0x9B2, 0));

				Add(new GenericBuyInfo(typeof(Bolt), 5, 25, 0x1BFB, 0));
				Add(new GenericBuyInfo(typeof(Arrow), 5, 25, 0xF3F, 0));

				Add(new GenericBuyInfo("1060834", typeof(PlantBowl), 3, 5, 0x15FD, 0));
			}
		}

		private sealed class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
				Add(typeof(Bottle), 1);
			}
		}
	}
}
