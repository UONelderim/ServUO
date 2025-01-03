using Server.Items;
using System.Collections.Generic;
using Nelderim.Configuration;

namespace Server.Mobiles
{
	public class SBFisherman : SBInfo
	{
		private readonly List<IBuyItemInfo> m_BuyInfo = new InternalBuyInfo();
		private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();

		public override IShopSellInfo SellInfo => m_SellInfo;
		public override List<IBuyItemInfo> BuyInfo => m_BuyInfo;

		public class InternalBuyInfo : List<IBuyItemInfo>
		{
			public InternalBuyInfo()
			{
				Add(new GenericBuyInfo(typeof(RawFishSteak), 3, 20, 0x97A, 0, true));
				Add(new GenericBuyInfo(typeof(SmallFish), 3, 20, 0xDD6, 0));
				Add(new GenericBuyInfo(typeof(SmallFish), 3, 20, 0xDD7, 0));
				Add(new GenericBuyInfo(typeof(Fish), 6, 80, 0x9CC, 0, true));
				Add(new GenericBuyInfo(typeof(Fish), 6, 80, 0x9CD, 0, true));
				Add(new GenericBuyInfo(typeof(Fish), 6, 80, 0x9CE, 0, true));
				Add(new GenericBuyInfo(typeof(Fish), 6, 80, 0x9CF, 0, true));
				Add(new GenericBuyInfo(typeof(FishingPole), 15, 20, 0xDC0, 0));
				Add(new GenericBuyInfo(typeof(AquariumFood), 62, 20, 0xEFC, 0));
				Add(new GenericBuyInfo(typeof(FishBowl), 6312, 20, 0x241C, 0x482));
				Add(new GenericBuyInfo(typeof(VacationWafer), 67, 20, 0x971, 0));
				Add(new GenericBuyInfo(typeof(AquariumNorthDeed), 250002, 20, 0x14F0, 0));
				Add(new GenericBuyInfo(typeof(AquariumEastDeed), 250002, 20, 0x14F0, 0));
				Add(new GenericBuyInfo(typeof(AquariumFishNet), 250, 20, 0xDC8, 0x240));
				Add(new GenericBuyInfo(typeof(NewAquariumBook), 15, 20, 0xFF2, 0));
				if(NConfig.Loot.RecipesEnabled)
				{
					Add(new GenericBuyInfo(typeof(SmallElegantAquariumRecipeScroll), 375000, 500, 0x2831, 0));
					Add(new GenericBuyInfo(typeof(WallMountedAquariumRecipeScroll), 750000, 500, 0x2831, 0));
					Add(new GenericBuyInfo(typeof(LargeElegantAquariumRecipeScroll), 1250000, 500, 0x2831, 0));
				}
				Add(new GenericBuyInfo(typeof(BivalviaNet), 20, 50, 0xDD2, 0));
				Add(new GenericBuyInfo(typeof(BlankMap), 10, 50, 0x14EC, 0));
				Add(new GenericBuyInfo(typeof(MapmakersPen), 30, 50, 0x0FBF, 0));
				Add(new GenericBuyInfo(typeof(BlankScroll), 12, 50, 0xEF3, 0));
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
				Add(typeof(Salt), 2);
				Add(typeof(RawFishSteak), 2);
				Add(typeof(FishSteak), 4);
				Add(typeof(Fish), 2);
				Add(typeof(BigFish), 500);
				Add(typeof(SmallFish), 1);
				Add(typeof(FishingPole), 6);
				Add(typeof(PeculiarFish), 10);
				Add(typeof(PrizedFish), 10);
				Add(typeof(WondrousFish), 10);
				Add(typeof(TrulyRareFish), 10);

				Add(typeof(Amber), 30);
				Add(typeof(Amethyst), 30);
				Add(typeof(Citrine), 30);
				Add(typeof(Diamond), 60);
				Add(typeof(Emerald), 45);
				Add(typeof(Ruby), 45);
				Add(typeof(Sapphire), 45);
				Add(typeof(StarSapphire), 60);
				Add(typeof(Tourmaline), 45);
				Add(typeof(GoldRing), 15);
				Add(typeof(SilverRing), 15);
				Add(typeof(Necklace), 15);
				Add(typeof(GoldNecklace), 15);
				Add(typeof(GoldBeadNecklace), 15);
				Add(typeof(SilverNecklace), 15);
				Add(typeof(SilverBeadNecklace), 15);
				Add(typeof(Beads), 15);
				Add(typeof(GoldBracelet), 15);
				Add(typeof(SilverBracelet), 15);
				Add(typeof(GoldEarrings), 15);
				Add(typeof(SilverEarrings), 15);

				Add(typeof(BlankScroll), 2);
				Add(typeof(MapmakersPen), 8);
				Add(typeof(BlankMap), 2);
				Add(typeof(CityMap), 3);
				Add(typeof(LocalMap), 4);
				Add(typeof(WorldMap), 5);
				Add(typeof(PresetMapEntry), 3);
			}
		}
	}
}
