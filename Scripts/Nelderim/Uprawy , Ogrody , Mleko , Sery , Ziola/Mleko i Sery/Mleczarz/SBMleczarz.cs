using System; 
using System.Collections; 
using Server.Items; 

namespace Server.Mobiles 
{ 
	public class SBMleczarz : SBInfo 
	{ 
		private ArrayList m_BuyInfo = new InternalBuyInfo(); 
		private IShopSellInfo m_SellInfo = new InternalSellInfo(); 

		public SBMleczarz() 
		{ 
		} 

		public override IShopSellInfo SellInfo { get { return m_SellInfo; } } 
		public override ArrayList BuyInfo { get { return m_BuyInfo; } } 

		public class InternalBuyInfo : ArrayList 
		{ 
			public InternalBuyInfo() 
			{ 
				Add( new GenericBuyInfo( typeof( BottleCowMilk ), 100, 5, 0x0f09, 0 ) );
				Add( new GenericBuyInfo( typeof( BottleGoatMilk ), 100, 5, 0x0f09, 0 ) );
				Add( new GenericBuyInfo( typeof( BottleSheepMilk ), 100, 5, 0x0f09, 0 ) );
				Add( new GenericBuyInfo( typeof( CheeseWheel ), 200, 5, 0x97E, 0 ) );
				Add( new GenericBuyInfo( typeof( CheeseForm ), 2500, 3, 0x0E78, 0 ) );
			        Add( new GenericBuyInfo( typeof( MilkBucket ), 1500, 3, 0x0FFA, 0 ) );
			} 
		} 

		public class InternalSellInfo : GenericSellInfo 
		{ 
			public InternalSellInfo() 
			{ 
				Add( typeof( CheeseWheel ), 20 );
				Add( typeof( BottleSheepMilk ), 8 );
				Add( typeof( BottleGoatMilk ), 8 );
	                        Add( typeof( BottleCowMilk ), 8 );
			} 
		} 
	} 
}