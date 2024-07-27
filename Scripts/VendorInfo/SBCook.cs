using Server.Items;
using System.Collections.Generic;

namespace Server.Mobiles
{
	public class SBCook : SBInfo
	{
		private readonly List<IBuyItemInfo> m_BuyInfo = new InternalBuyInfo();
		private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();

		public override IShopSellInfo SellInfo => m_SellInfo;
		public override List<IBuyItemInfo> BuyInfo => m_BuyInfo;

		public class InternalBuyInfo : List<IBuyItemInfo>
		{
			public InternalBuyInfo()
			{
				Add(new GenericBuyInfo(typeof(BreadLoaf), 5, 20, 0x103B, 0, true));
				Add(new GenericBuyInfo(typeof(BreadLoaf), 5, 20, 0x103C, 0, true));
				Add(new GenericBuyInfo(typeof(ApplePie), 7, 20, 0x1041, 0,
					true)); //OSI just has Pie, not Apple/Fruit/Meat
				Add(new GenericBuyInfo(typeof(Cake), 13, 20, 0x9E9, 0, true));
				Add(new GenericBuyInfo(typeof(Muffins), 3, 20, 0x9EA, 0, true));

				Add(new GenericBuyInfo(typeof(CheeseWheel), 21, 10, 0x97E, 0, true));
				Add(new GenericBuyInfo(typeof(CookedBird), 17, 20, 0x9B7, 0, true));
				Add(new GenericBuyInfo(typeof(LambLeg), 8, 20, 0x160A, 0, true));
				Add(new GenericBuyInfo(typeof(ChickenLeg), 5, 20, 0x1608, 0, true));

				Add(new GenericBuyInfo(typeof(WoodenBowlOfCarrots), 3, 50, 0x15F9, 0));
				Add(new GenericBuyInfo(typeof(WoodenBowlOfCorn), 3, 50, 0x15FA, 0));
				Add(new GenericBuyInfo(typeof(WoodenBowlOfLettuce), 3, 50, 0x15FB, 0));
				Add(new GenericBuyInfo(typeof(WoodenBowlOfPeas), 3, 50, 0x15FC, 0));
				Add(new GenericBuyInfo(typeof(EmptyPewterBowl), 2, 50, 0x15FD, 0));
				Add(new GenericBuyInfo(typeof(PewterBowlOfCorn), 3, 50, 0x15FE, 0));
				Add(new GenericBuyInfo(typeof(PewterBowlOfLettuce), 3, 50, 0x15FF, 0));
				Add(new GenericBuyInfo(typeof(PewterBowlOfPeas), 3, 50, 0x1600, 0));
				Add(new GenericBuyInfo(typeof(PewterBowlOfPotatos), 3, 50, 0x1601, 0));
				Add(new GenericBuyInfo(typeof(WoodenBowlOfStew), 3, 50, 0x1604, 0));
				Add(new GenericBuyInfo(typeof(WoodenBowlOfTomatoSoup), 3, 5, 0x1606, 0));


				Add(new GenericBuyInfo(typeof(RoastPig), 106, 50, 0x9BB, 0));
				Add(new GenericBuyInfo(typeof(SackFlour), 3, 50, 0x1039, 0));
				//Add( new GenericBuyInfo( typeof( JarHoney ), 3, 5, 0x9EC, 0 ) );
				Add(new GenericBuyInfo(typeof(RollingPin), 9, 50, 0x1043, 0));
				Add(new GenericBuyInfo(typeof(FlourSifter), 11, 50, 0x103E, 0));
				Add(new GenericBuyInfo("1044567", typeof(Skillet), 15, 50, 0x97F, 0));

				Add(new GenericBuyInfo(typeof(Bacon), 7, 50, 0x979, 0));
				Add(new GenericBuyInfo(typeof(Ham), 26, 50, 0x9C9, 0));
				Add(new GenericBuyInfo(typeof(Sausage), 18, 50, 0x9C0, 0));
				Add(new GenericBuyInfo(typeof(RawChickenLeg), 6, 50, 0x1607, 0));
				Add(new GenericBuyInfo(typeof(RawBird), 9, 50, 0x9B9, 0));
				Add(new GenericBuyInfo(typeof(RawLambLeg), 9, 50, 0x1609, 0));
				Add(new GenericBuyInfo(typeof(RawRibs), 16, 50, 0x9F1, 0));
				Add(new GenericBuyInfo(typeof(Gut), 8, 50, 7407, 0));
				// wyrzucam bronie:
				Add(new GenericBuyInfo(typeof(ButcherKnife), 13, 50, 0x13F6, 0));
				Add(new GenericBuyInfo(typeof(Cleaver), 13, 50, 0xEC3, 0));
				Add(new GenericBuyInfo(typeof(SkinningKnife), 13, 50, 0xEC4, 0));
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
				Add(typeof(CheeseWheel), 2);
				Add(typeof(CookedBird), 2);
				Add(typeof(RoastPig), 3);
				Add(typeof(Cake), 2);
				Add(typeof(Cookies), 2);
				Add(typeof(Muffins), 2);
				Add(typeof(JarHoney), 1);
				Add(typeof(SackFlour), 1);
				Add(typeof(BreadLoaf), 2);
				Add(typeof(FrenchBread), 1);
				Add(typeof(ChickenLeg), 2);
				Add(typeof(LambLeg), 2);
				Add(typeof(Skillet), 1);
				Add(typeof(FlourSifter), 1);
				Add(typeof(RollingPin), 1);
				Add(typeof(ApplePie), 3);
				Add(typeof(CheesePizza), 3);
				Add(typeof(PeachCobbler), 4);
				Add(typeof(Quiche), 6);
				Add(typeof(Dough), 4);
				Add(typeof(Pitcher), 3);
				Add(typeof(Eggs), 1);

				Add(typeof(WoodenBowlOfCarrots), 1);
				Add(typeof(WoodenBowlOfCorn), 1);
				Add(typeof(WoodenBowlOfLettuce), 1);
				Add(typeof(WoodenBowlOfPeas), 1);
				Add(typeof(EmptyPewterBowl), 1);
				Add(typeof(PewterBowlOfCorn), 1);
				Add(typeof(PewterBowlOfLettuce), 1);
				Add(typeof(PewterBowlOfPeas), 1);
				Add(typeof(PewterBowlOfPotatos), 1);
				Add(typeof(WoodenBowlOfStew), 1);
				Add(typeof(WoodenBowlOfTomatoSoup), 1);

				Add(typeof(RawRibs), 2);
				Add(typeof(RawLambLeg), 2);
				Add(typeof(RawChickenLeg), 2);
				Add(typeof(RawBird), 2);
				Add(typeof(Bacon), 2);
				Add(typeof(Sausage), 2);
				Add(typeof(Ham), 2);
				Add(typeof(Gut), 1);

				Add(typeof(UnbakedQuiche), 3);
				Add(typeof(UnbakedMeatPie), 3);
				Add(typeof(UncookedSausagePizza), 3);
				Add(typeof(Quiche), 5);
				Add(typeof(MeatPie), 5);
				Add(typeof(SausagePizza), 5);
				Add(typeof(UncookedCheesePizza), 3);
				Add(typeof(CheesePizza), 5);
				Add(typeof(UnbakedFruitPie), 3);
				Add(typeof(FruitPie), 5);
				Add(typeof(UnbakedPeachCobbler), 3);
				Add(typeof(SweetDough), 3);
				Add(typeof(CakeMix), 3);
				Add(typeof(CookieMix), 3);
				Add(typeof(GreenTea), 3);
				Add(typeof(MisoSoup), 3);
				Add(typeof(RedMisoSoup), 3);
				Add(typeof(WhiteMisoSoup), 3);
			}
		}
	}
}
