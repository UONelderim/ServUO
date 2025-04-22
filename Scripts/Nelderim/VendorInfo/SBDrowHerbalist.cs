using System; 
using System.Collections;
using System.Collections.Generic;
using Server.Items;
using Server.Items.Crops;

namespace Server.Mobiles 
{ 
	public class SBDrowHerbalist : SBInfo 
	{
		public static int GlobalHerbsSeedlingPriceBuy => 100;   // koszt kupna szczepek ziol u npc
		public static int GlobalHerbsPriceBuy => 20;    // koszt kupna ziol u npc
		public static int GlobalHerbsPriceBuyFence => (int)(1.4 * GlobalHerbsPriceBuy);
		public static int GlobalHerbsPriceBuyDouble => 2 * GlobalHerbsPriceBuy;

		public static int GlobalHerbsSeedlingPriceSell => 10;   // zysk za szczepke sprzedana do npc
		public static int GlobalHerbsPriceSell => 2;    // cena za ziolo sprzedane do npc
		public static int GlobalHerbsPriceSellFence => 1;
		public static int GlobalHerbsPriceSellHalf => 1;

		private readonly InternalBuyInfo m_BuyInfo = new InternalBuyInfo();
		private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();

		public class InternalBuyInfo : ArrayList 
		{ 
			public InternalBuyInfo() 
			{ 
				Add( new GenericBuyInfo( typeof( Bloodmoss ), SBHerbalist.GlobalHerbsPriceBuy, 200, 0xF7B, 0 ) ); 
				Add( new GenericBuyInfo( typeof( MandrakeRoot ), SBHerbalist.GlobalHerbsPriceBuy, 200, 0xF86, 0 ) ); 
				Add( new GenericBuyInfo( typeof( Garlic ), SBHerbalist.GlobalHerbsPriceBuy, 200, 0xF84, 0 ) ); 
				Add( new GenericBuyInfo( typeof( Ginseng ), SBHerbalist.GlobalHerbsPriceBuy, 200, 0xF85, 0 ) ); 
				Add( new GenericBuyInfo( typeof( Nightshade ), SBHerbalist.GlobalHerbsPriceBuy, 200, 0xF88, 0 ) ); 
				Add( new GenericBuyInfo( typeof( Bottle ), 5, 200, 0xF0E, 0 ) ); 
				Add( new GenericBuyInfo( typeof( MortarPestle ), 30, 20, 0xE9B, 0 ) );
				Add( new GenericBuyInfo( typeof( DestroyingAngel ), 8, 50, 0xE1F, 0 ) );
				Add( new GenericBuyInfo( typeof( PetrafiedWood ), 8, 50, 0x97A, 0 ) );
				Add( new GenericBuyInfo( typeof( SpringWater ), 8, 50, 0xE24, 0 ) );
				Add(new GenericBuyInfo("Szufla do lajna", typeof(DungShovel), 30, 50, 0xF39, DungShovel.DefaultHue));
				Add(new GenericBuyInfo("Wiadro na nawoz", typeof(DungBucket), 2000, 5, DungBucket.GraphicsEmpty, DungBucket.HueEmpty));
				Add(new GenericBuyInfo(typeof(SzczepkaBoczniak), SBHerbalist.GlobalHerbsSeedlingPriceBuy, 50, 0x0F23, 872));
				Add(new GenericBuyInfo(typeof(SzczepkaZagwica), SBHerbalist.GlobalHerbsSeedlingPriceBuy, 50, 0x0F23, 1236));
				Add(new GenericBuyInfo(typeof(SzczepkaLysiczka), SBHerbalist.GlobalHerbsSeedlingPriceBuy, 50, 0x0F23, 798));
				Add(new GenericBuyInfo(typeof(SzczepkaKrwawyMech), SBHerbalist.GlobalHerbsSeedlingPriceBuy, 50, 0x0DCD, 438));
				Add(new GenericBuyInfo(typeof(SzczepkaMuchomor), SBHerbalist.GlobalHerbsSeedlingPriceBuy, 50, 0x0F23, 1509));
			}
		} 

		public class InternalSellInfo : GenericSellInfo 
		{ 
			public InternalSellInfo() 
			{ 
				Add( typeof( Bloodmoss ), SBHerbalist.GlobalHerbsPriceSell);
				Add( typeof( MandrakeRoot ), SBHerbalist.GlobalHerbsPriceSell);
				Add( typeof( Garlic ), SBHerbalist.GlobalHerbsPriceSell);
				Add( typeof( Ginseng ), SBHerbalist.GlobalHerbsPriceSell);
				Add( typeof( Nightshade ), SBHerbalist.GlobalHerbsPriceSell);
				Add( typeof( Bottle ), 3 ); 
				Add( typeof( MortarPestle ), 4 );
				Add(typeof(DungShovel), 6);
				Add(typeof(DungBucket), 8);
				Add(typeof(SzczepkaBoczniak), SBHerbalist.GlobalHerbsSeedlingPriceSell);
				Add(typeof(SzczepkaZagwica), SBHerbalist.GlobalHerbsSeedlingPriceSell);
				Add(typeof(SzczepkaLysiczka), SBHerbalist.GlobalHerbsSeedlingPriceSell);
				Add(typeof(SzczepkaKrwawyMech), SBHerbalist.GlobalHerbsSeedlingPriceSell);
				Add(typeof(SzczepkaMuchomor), SBHerbalist.GlobalHerbsSeedlingPriceSell);
			}
		}

		public override IShopSellInfo SellInfo { get; }
		public override List<IBuyItemInfo> BuyInfo { get; }
	} 
}
