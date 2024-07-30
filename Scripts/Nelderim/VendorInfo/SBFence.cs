#region References

using System.Collections.Generic;
using Server.Items;

#endregion

namespace Server.Mobiles
{
	public class SBFence : SBInfo
	{
		public override IShopSellInfo SellInfo { get; } = new InternalSellInfo();

		public override List<IBuyItemInfo> BuyInfo { get; } = new InternalBuyInfo();

		private class InternalBuyInfo : List<IBuyItemInfo>
		{
			public InternalBuyInfo()
			{
				Add(new GenericBuyInfo(typeof(Bandage), 8, 200, 0xE21, 0));
				Add(new GenericBuyInfo(typeof(Bottle), 10, 50, 0xF0E, 0));
				Add(new GenericBuyInfo(typeof(SpidersSilk), 8, 50, 0xF8D, 0));
				Add(new GenericBuyInfo(typeof(SulfurousAsh), 8, 50, 0xF8C, 0));
				Add(new GenericBuyInfo(typeof(Nightshade), SBHerbalist.GlobalHerbsPriceBuyFence, 50, 0xF88, 0));
				Add(new GenericBuyInfo(typeof(MandrakeRoot), SBHerbalist.GlobalHerbsPriceBuyFence, 50, 0xF86, 0));
				Add(new GenericBuyInfo(typeof(Ginseng), SBHerbalist.GlobalHerbsPriceBuyFence, 50, 0xF85, 0));
				Add(new GenericBuyInfo(typeof(Garlic), SBHerbalist.GlobalHerbsPriceBuyFence, 50, 0xF84, 0));
				Add(new GenericBuyInfo(typeof(Bloodmoss), SBHerbalist.GlobalHerbsPriceBuyFence, 50, 0xF7B, 0));
				Add(new GenericBuyInfo(typeof(BlackPearl), 9, 50, 0xF7A, 0));

				Add(new GenericBuyInfo(typeof(GraveDust), 12, 50, 0xF8F, 0));
				Add(new GenericBuyInfo(typeof(NoxCrystal), 12, 50, 0xF8E, 0));
				Add(new GenericBuyInfo(typeof(PigIron), 12, 50, 0xF8A, 0));
				Add(new GenericBuyInfo(typeof(DaemonBlood), 12, 50, 0xF7D, 0));
				Add(new GenericBuyInfo(typeof(BatWing), 12, 50, 0xF78, 0));
			}
		}

		private class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
				Add(typeof(Arrow), 1);
				Add(typeof(Bolt), 1);
				Add(typeof(Backpack), 2);
				Add(typeof(Pouch), 2);
				Add(typeof(Bag), 2);
				Add(typeof(Candle), 2);
				Add(typeof(Torch), 2);
				Add(typeof(Lantern), 1);
				Add(typeof(Lockpick), 1);
				Add(typeof(FloppyHat), 1);
				Add(typeof(WideBrimHat), 4);
				Add(typeof(Cap), 5);
				Add(typeof(TallStrawHat), 4);
				Add(typeof(StrawHat), 3);
				Add(typeof(WizardsHat), 5);
				Add(typeof(LeatherCap), 5);
				Add(typeof(FeatheredHat), 5);
				Add(typeof(TricorneHat), 4);
				Add(typeof(Bandana), 3);
				Add(typeof(SkullCap), 3);
				Add(typeof(Bottle), 3);
				Add(typeof(RedBook), 7);
				Add(typeof(BlueBook), 7);
				Add(typeof(TanBook), 7);
				Add(typeof(WoodenBox), 7);
				//Add( typeof( Kindling ), 1 );
				// Add( typeof( HairDye ), 200 );
				Add(typeof(Chessboard), 1);
				Add(typeof(CheckerBoard), 1);
				Add(typeof(Backgammon), 1);
				Add(typeof(Dices), 1);

				Add(typeof(Beeswax), 1);

				Add(typeof(Amber), 20);
				Add(typeof(Amethyst), 20);
				Add(typeof(Citrine), 20);
				Add(typeof(Diamond), 40);
				Add(typeof(Emerald), 30);
				Add(typeof(Ruby), 30);
				Add(typeof(Sapphire), 30);
				Add(typeof(StarSapphire), 40);
				Add(typeof(Tourmaline), 30);
				Add(typeof(GoldRing), 10);
				Add(typeof(SilverRing), 10);
				Add(typeof(Necklace), 10);
				Add(typeof(GoldNecklace), 10);
				Add(typeof(GoldBeadNecklace), 10);
				Add(typeof(SilverNecklace), 10);
				Add(typeof(SilverBeadNecklace), 10);
				Add(typeof(Beads), 10);
				Add(typeof(GoldBracelet), 10);
				Add(typeof(SilverBracelet), 10);
				Add(typeof(GoldEarrings), 10);
				Add(typeof(SilverEarrings), 10);
			}
		}
	}
}
