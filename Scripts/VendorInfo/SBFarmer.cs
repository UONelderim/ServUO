using System;
using Server.Items;
using System.Collections.Generic;
using Server.Engines.Apiculture;
using Server.Items.Crops;

namespace Server.Mobiles
{
	public class SBFarmer : SBInfo
	{
		private readonly List<IBuyItemInfo> m_BuyInfo = new InternalBuyInfo();
		private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();

		public override IShopSellInfo SellInfo => m_SellInfo;
		public override List<IBuyItemInfo> BuyInfo => m_BuyInfo;

		public class InternalBuyInfo : List<IBuyItemInfo>
		{
			public InternalBuyInfo()
			{
				Add(new GenericBuyInfo(typeof(JarHoney), 3, 50, 0x9EC, 0));
				Add(new GenericBuyInfo(typeof(Beeswax), 1, 50, 0x1422, 0));
				Add(new GenericBuyInfo(typeof(SackFlour), 5, 50, 0x1039, 0));
				Add(new GenericBuyInfo(typeof(apiBeeHiveDeed), 4000, 50, 2330, 0));
				Add(new GenericBuyInfo(typeof(HiveTool), 10, 50, 2549, 0));
				Add(new GenericBuyInfo(typeof(apiSmallWaxPot), 500, 50, 2532, 0));
				Add(new GenericBuyInfo(typeof(apiLargeWaxPot), 800, 50, 2541, 0));
				Add(new GenericBuyInfo(typeof(Cabbage), 5, 50, 0xC7B, 0));
				Add(new GenericBuyInfo(typeof(Cantaloupe), 6, 50, 0xC79, 0));
				Add(new GenericBuyInfo(typeof(Carrot), 3, 50, 0xC78, 0));
				Add(new GenericBuyInfo(typeof(HoneydewMelon), 7, 50, 0xC74, 0));
				Add(new GenericBuyInfo(typeof(Squash), 3, 50, 0xC72, 0));
				Add(new GenericBuyInfo(typeof(Lettuce), 5, 50, 0xC70, 0));
				Add(new GenericBuyInfo(typeof(Onion), 3, 50, 0xC6D, 0));
				Add(new GenericBuyInfo(typeof(Pumpkin), 11, 50, 0xC6A, 0));
				Add(new GenericBuyInfo(typeof(GreenGourd), 3, 50, 0xC66, 0));
				Add(new GenericBuyInfo(typeof(YellowGourd), 3, 50, 0xC64, 0));
				//Add( new GenericBuyInfo( typeof( Turnip ), 6, 20, XXXXXX, 0 ) );
				Add(new GenericBuyInfo(typeof(Watermelon), 7, 50, 0xC5C, 0));
				//Add( new GenericBuyInfo( typeof( EarOfCorn ), 3, 20, XXXXXX, 0 ) );
				Add(new GenericBuyInfo(typeof(Eggs), 3, 50, 0x9B5, 0));
				Add(new BeverageBuyInfo(typeof(Pitcher), BeverageType.Milk, 7, 50, 0x9AD, 0));
				Add(new GenericBuyInfo(typeof(Peach), 3, 50, 0x9D2, 0));
				Add(new GenericBuyInfo(typeof(Pear), 3, 50, 0x994, 0));
				Add(new GenericBuyInfo(typeof(Lemon), 3, 50, 0x1728, 0));
				Add(new GenericBuyInfo(typeof(Lime), 3, 50, 0x172A, 0));
				Add(new GenericBuyInfo(typeof(Grapes), 3, 50, 0x9D1, 0));
				Add(new GenericBuyInfo(typeof(Apple), 3, 50, 0x9D0, 0));
				Add(new GenericBuyInfo(typeof(SheafOfHay), 2, 50, 0xF36, 0));
				
				Add(new GenericBuyInfo("Nasiona bawelny", typeof(SzczepkaBawelna), 100, 10, 0xF27, 0x5E2));
				Add(new GenericBuyInfo("Nasiona lnu", typeof(SzczepkaLen), 100, 10, 0xF27, 0x5E2));
				Add(new GenericBuyInfo("Nasiona konopii", typeof(SzczepkaKonopia), 100, 10, 0xF27, 0x5E2));
				Add(new GenericBuyInfo("Nasiona rosliny dla jedwabnika", typeof(SzczepkaJedwab), 100, 10, 0xF27, 0x5E2));
				Add(new GenericBuyInfo("Nasiona pszenicy", typeof(SzczepkaPszenica), 30, 20, 0xF27, 0x5E2));
				Add(new GenericBuyInfo("Nasiona kukurydzy", typeof(SzczepkaKukurydza), 15, 10, 0xF27, 0x5E2));
				Add(new GenericBuyInfo("Nasiona marchwi", typeof(SzczepkaMarchew), 20, 10, 0xF27, 0x5E2));
				Add(new GenericBuyInfo("Nasiona cebuli", typeof(SzczepkaCebula), 20, 10, 0xF27, 0x5E2));
				Add(new GenericBuyInfo("Nasiona salaty", typeof(SzczepkaSalata), 10, 10, 0xF27, 0x5E2));
				Add(new GenericBuyInfo("Nasiona kapusty", typeof(SzczepkaKapusta), 10, 10, 0xF27, 0x5E2));
				
				Add( new GenericBuyInfo( "Maly Ogrod Domowy", typeof( MalyOgrodAddonDeed ), 20000, 10, 0xE87, 0xE88 ) );
				Add( new GenericBuyInfo( "Maly Ogrod Domowy", typeof( SredniOgrodAddonDeed ), 40000, 10, 0xE87, 0xE88 ) );
				Add( new GenericBuyInfo( "Maly Ogrod Domowy", typeof( WielkiOgrodAddonDeed ), 60000, 10, 0xE87, 0xE88 ) );

				Add(new GenericBuyInfo("Szufla do lajna", typeof(DungShovel), 30, 50, 0xF39, DungShovel.DefaultHue));
				Add(new GenericBuyInfo("Wiadro na nawoz", typeof(DungBucket), 2000, 5, DungBucket.GraphicsEmpty, DungBucket.HueEmpty));
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
				Add(typeof(JarHoney), 1);
				Add(typeof(Beeswax), 1);
				Add(typeof(SackFlour), 1);

				Add(typeof(apiBeeHiveDeed), 1000);
				Add(typeof(HiveTool), 5);
				Add(typeof(apiSmallWaxPot), 125);
				Add(typeof(apiLargeWaxPot), 200);

				Add(typeof(Pitcher), 5);
				Add(typeof(Eggs), 1);
				Add(typeof(Apple), 1);
				Add(typeof(Grapes), 1);
				Add(typeof(Watermelon), 3);
				Add(typeof(YellowGourd), 1);
				Add(typeof(GreenGourd), 1);
				Add(typeof(Pumpkin), 5);
				Add(typeof(Onion), 1);
				Add(typeof(Lettuce), 2);
				Add(typeof(Squash), 1);
				Add(typeof(Carrot), 1);
				Add(typeof(HoneydewMelon), 3);
				Add(typeof(Cantaloupe), 3);
				Add(typeof(Cabbage), 2);
				Add(typeof(Lemon), 1);
				Add(typeof(Lime), 1);
				Add(typeof(Peach), 1);
				Add(typeof(Pear), 1);
				Add(typeof(SheafOfHay), 1);
			}
		}
	}
}
