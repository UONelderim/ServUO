using System.Collections.Generic;
using Server.Items;

namespace Server.Mobiles
{
	public class SBRedBarkeeper : SBInfo
	{
		private List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
		private IShopSellInfo m_SellInfo = new InternalSellInfo();

		public SBRedBarkeeper()
		{
		}

		public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
		public override List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }

		public class InternalBuyInfo : List<GenericBuyInfo>
        {
			public InternalBuyInfo()
			{  
				Add( new GenericBuyInfo( "1062332", typeof( VendorRentalContract ), 2000, 2, 0x14F0, 0x672 ) );
				Add( new GenericBuyInfo( "1041280", typeof( InteriorDecorator ), 10000, 20, 0xFC1, 0 ) );
				Add( new GenericBuyInfo( "a barkeep contract", typeof( BarkeepContract ), 2000, 2, 0x14F0, 0 ) );
				Add( new GenericBuyInfo( "1041243", typeof( ContractOfEmployment ), 2000, 2, 0x14F0, 0 ) );
				Add( new GenericBuyInfo( typeof( Dices ), 3, 2, 0xFA7, 0 ) );
				Add( new GenericBuyInfo( typeof( Backgammon ), 3, 2, 0xE1C, 0 ) );
				Add( new GenericBuyInfo( "1016449", typeof( CheckerBoard ), 3, 2, 0xFA6, 0 ) );
				Add( new GenericBuyInfo( "1016450", typeof( Chessboard ), 3, 2, 0xFA6, 0 ) );
				Add( new GenericBuyInfo( typeof( LambLeg ), 12, 5, 0x160A, 0 ) );
				Add( new GenericBuyInfo( typeof( CookedBird ), 25, 5, 0x9B7, 0 ) );
				Add( new GenericBuyInfo( typeof( CheeseWheel ), 5, 5, 0x97E, 0 ) );
				Add( new GenericBuyInfo( typeof( BreadLoaf ), 10, 5, 0x103B, 0 ) );
				Add( new BeverageBuyInfo( typeof( Pitcher ), BeverageType.Water, 15, 5, 0x1F9D, 0 ) );
				Add( new BeverageBuyInfo( typeof( Pitcher ), BeverageType.Wine, 15, 5, 0x1F9B, 0 ) );
				Add( new BeverageBuyInfo( typeof( Pitcher ), BeverageType.Milk, 10, 5, 0x9F0, 0 ) );
				Add( new BeverageBuyInfo( typeof( Pitcher ), BeverageType.Liquor, 16, 5, 0x1F99, 0 ) );
				Add( new GenericBuyInfo( typeof( Pitcher ), 10, 5, 0xFF6, 0 ) );
				Add( new BeverageBuyInfo( typeof( Pitcher ), BeverageType.Cider, 15, 5, 0x1F97, 0 ) );
				Add( new BeverageBuyInfo( typeof( Pitcher ), BeverageType.Ale, 15, 5, 0x1F95, 0 ) );
				Add( new BeverageBuyInfo( typeof( Jug ), BeverageType.Cider, 20, 5, 0x9C8, 0 ) );
				Add( new BeverageBuyInfo( typeof( BeverageBottle ), BeverageType.Liquor, 10, 5, 0x99B, 0 ) );
				Add( new BeverageBuyInfo( typeof( BeverageBottle ), BeverageType.Wine, 10, 5, 0x9C7, 0 ) );
				Add( new BeverageBuyInfo( typeof( BeverageBottle ), BeverageType.Ale, 10, 5, 0x99F, 0 ) );
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
				Add( typeof( WoodenBowlOfCarrots ), 1 );
				Add( typeof( WoodenBowlOfCorn ), 1 );
				Add( typeof( WoodenBowlOfLettuce ), 1 );
				Add( typeof( WoodenBowlOfPeas ), 1 );
				Add( typeof( EmptyPewterBowl ), 1 );
				Add( typeof( PewterBowlOfCorn ), 1 );
				Add( typeof( PewterBowlOfLettuce ), 1 );
				Add( typeof( PewterBowlOfPeas ), 1 );
				Add( typeof( PewterBowlOfPotatos ), 1 );
				Add( typeof( WoodenBowlOfStew ), 1 );
				Add( typeof( WoodenBowlOfTomatoSoup ), 1 );
				Add( typeof( BeverageBottle ), 3 );
				Add( typeof( Jug ), 6 );
				Add( typeof( Pitcher ), 5 );
				Add( typeof( GlassMug ), 1 );
				Add( typeof( BreadLoaf ), 3 );
				Add( typeof( CheeseWheel ), 2 );
				Add( typeof( Ribs ), 2 );
				Add( typeof( Peach ), 1 );
				Add( typeof( Pear ), 1 );
				Add( typeof( Grapes ), 1 );
				Add( typeof( Apple ), 1 );
				Add( typeof( Banana ), 1 );
				Add( typeof( Candle ), 3 );
				Add( typeof( Chessboard ), 1 );
				Add( typeof( CheckerBoard ), 1 );
				Add( typeof( Backgammon ), 1 );
				Add( typeof( Dices ), 1 );
				Add( typeof( ContractOfEmployment ), 200 );
			}
		}
	}
}
