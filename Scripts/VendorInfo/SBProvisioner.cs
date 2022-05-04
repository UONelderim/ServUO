using Server.Items;
using System.Collections.Generic;

namespace Server.Mobiles
{
    public class SBProvisioner : SBInfo
    {
        private readonly List<IBuyItemInfo> m_BuyInfo = new InternalBuyInfo();
        private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();

        public override IShopSellInfo SellInfo => m_SellInfo;
        public override List<IBuyItemInfo> BuyInfo => m_BuyInfo;

        public class InternalBuyInfo : List<IBuyItemInfo>
        {
            public InternalBuyInfo()
            {
				Add( new GenericBuyInfo( typeof( Dices ), 2, 20, 0xFA7, 0 ) );
				
				if ( Core.AOS )
					Add( new GenericBuyInfo( typeof( Engines.Mahjong.MahjongGame ), 6, 20, 0xFAA, 0 ) );
					
				Add( new GenericBuyInfo( typeof( Backgammon ), 2, 20, 0xE1C, 0 ) );
				Add( new GenericBuyInfo( "1016449", typeof( CheckerBoard ), 2, 20, 0xFA6, 0 ) );
				Add( new GenericBuyInfo( "1016450", typeof( Chessboard ), 2, 20, 0xFA6, 0 ) );

				Add( new GenericBuyInfo( "1041060", typeof( HairDye ), 1000, 20, 0xEFF, 0 ) );
				
				Add( new GenericBuyInfo( typeof( PaintBrush ), 2000, 20, 4033, 0 ) );

				//Add( new GenericBuyInfo( "1041205", typeof( Multis.SmallBoatDeed ), 52500, 20, 0x14F2, 0 ) );

				Add( new GenericBuyInfo( typeof( Kindling ), 2, 20, 0xDE1, 0 ) );
				Add( new GenericBuyInfo( typeof( Bedroll ), 5, 20, 0xA59, 0 ) );

				Add( new GenericBuyInfo( typeof( Key ), 9, 20, 0x100E, 0 ) );
				Add( new GenericBuyInfo( typeof( WoodenBox ), 14, 50, 0xE7D, 0 ) );

				Add( new GenericBuyInfo( typeof( TanBook ), 15, 20, 0xFF0, 0 ) );
				Add( new GenericBuyInfo( typeof( BlueBook ), 15, 20, 0xFF2, 0 ) );
				Add( new GenericBuyInfo( typeof( RedBook ), 15, 20, 0xFF1, 0 ) );

				Add( new GenericBuyInfo( typeof( Bottle ), 5, 20, 0xF0E, 0 ) );

				Add( new GenericBuyInfo( typeof( Ginseng ), 3, 50, 0xF85, 0 ) );
				Add( new GenericBuyInfo( typeof( Garlic ), 3, 50, 0xF84, 0 ) );

				Add( new GenericBuyInfo( typeof( Beeswax ), 1, 20, 0x1422, 0 ) );

				Add( new GenericBuyInfo( typeof( Apple ), 3, 20, 0x9D0, 0 ) );
				Add( new GenericBuyInfo( typeof( Pear ), 3, 20, 0x994, 0 ) );

				Add( new BeverageBuyInfo( typeof( Jug ), BeverageType.Cider, 13, 20, 0x9C8, 0 ) );
				Add( new BeverageBuyInfo( typeof( BeverageBottle ), BeverageType.Liquor, 7, 20, 0x99B, 0 ) );
				Add( new BeverageBuyInfo( typeof( BeverageBottle ), BeverageType.Wine, 7, 20, 0x9C7, 0 ) );
				Add( new BeverageBuyInfo( typeof( BeverageBottle ), BeverageType.Ale, 7, 20, 0x99F, 0 ) );

				Add( new GenericBuyInfo( typeof( CookedBird ), 17, 20, 0x9B7, 0 ) );
				Add( new GenericBuyInfo( typeof( ChickenLeg ), 5, 20, 0x1608, 0 ) );
				Add( new GenericBuyInfo( typeof( LambLeg ), 8, 20, 0x160A, 0 ) );
				Add( new GenericBuyInfo( typeof( BreadLoaf ), 6, 20, 0x103B, 0 ) );

				Add( new GenericBuyInfo( typeof( SkullCap ), 12, 20, 0x1544, 0x31E ) );
				Add( new GenericBuyInfo( typeof( Bandana ), 14, 20, 0x153F, 0x215 ) );
				Add( new GenericBuyInfo( typeof( TricorneHat ), 26, 20, 0x171B, 0x282 ) );
				Add( new GenericBuyInfo( typeof( FeatheredHat ), 27, 20, 0x171A, 0x217 ) );
				Add( new GenericBuyInfo( typeof( LeatherCap ), 27, 20, 0x1DB9, 0x252 ) );
				Add( new GenericBuyInfo( typeof( WizardsHat ), 30, 20, 0x1718, 0x34C ) );			
				Add( new GenericBuyInfo( typeof( StrawHat ), 25, 20, 0x1717, 0x2B5 ) );
				Add( new GenericBuyInfo( typeof( TallStrawHat ), 26, 20, 0x1716, 0x282 ) );
				Add( new GenericBuyInfo( typeof( Cap ), 27, 20, 0x1715, 0x2D4 ) );
				Add( new GenericBuyInfo( typeof( WideBrimHat ), 26, 20, 0x1714, 0 ) );
				Add( new GenericBuyInfo( typeof( FloppyHat ), 25, 20, 0x1713, 0x282 ) );

				Add( new GenericBuyInfo( typeof( Lockpick ), 12, 50, 0x14FC, 0 ) );

				//TODO: Oil Flask @ 8GP
				Add( new GenericBuyInfo( typeof( Lantern ), 6, 20, 0xA25, 0 ) );
				Add( new GenericBuyInfo( typeof( Torch ), 8, 20, 0xF6B, 0 ) );
				Add( new GenericBuyInfo( typeof( Candle ), 6, 20, 0xA28, 0 ) );

				Add( new GenericBuyInfo( typeof( Bag ), 27, 50, 0xE76, 0 ) );
				Add( new GenericBuyInfo( typeof( Pouch ), 20, 50, 0xE79, 0 ) );
				Add( new GenericBuyInfo( typeof( Backpack ), 27, 50, 0x9B2, 0 ) );

				Add( new GenericBuyInfo( typeof( Bolt ), 5, 100, 0x1BFB, 0 ) );
				Add( new GenericBuyInfo( typeof( Arrow ), 3, 100, 0xF3F, 0 ) );
				
				Add( new GenericBuyInfo( typeof( PaintBrush ), 5000, 100, 0xF3F, 0 ) );
				

				Add( new GenericBuyInfo( "1060834", typeof( Engines.Plants.PlantBowl ), 2, 20, 0x15FD, 0 ) );

				Add( new GenericBuyInfo( typeof( SeedBox ), 125, 5, 0x0E41, 271 ) );
				
				Add( new GenericBuyInfo( typeof( WaterSprinkler ), 20000, 20, 0xE7A, 0 ) );
				Add( new GenericBuyInfo( typeof( SprinklerContainer ), 200, 20, 3707, 0 ) );
				Add( new GenericBuyInfo( typeof( PoisonSprinkler ), 20000, 20, 0xE7A, 0 ) );
				Add( new GenericBuyInfo( typeof( CureSprinkler ), 20000, 20, 0xE7A, 0 ) );
				Add( new GenericBuyInfo( typeof( StrengthSprinkler ), 20000, 20, 0xE7A, 0 ) );
				Add( new GenericBuyInfo( typeof( HealSprinkler ), 20000, 20, 0xE7A, 0 ) );

                Add(new GenericBuyInfo("1079931", typeof(SalvageBag), 1255, 20, 0xE76, Utility.RandomBlueHue()));
                Add(new GenericBuyInfo("1114770", typeof(SkinTingeingTincture), 1255, 20, 0xEFF, 90));
            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
            public InternalSellInfo()
            {
				Add( typeof( Arrow ), 1 );
				Add( typeof( Bolt ), 2 );
				Add( typeof( Backpack ), 7 );
				Add( typeof( Pouch ), 3 );
				Add( typeof( Bag ), 3 );
				Add( typeof( Candle ), 3 );
				Add( typeof( Torch ), 4 );
				Add( typeof( Lantern ), 1 );
				Add( typeof( Lockpick ), 6 );
				Add( typeof( FloppyHat ), 3 );
				Add( typeof( WideBrimHat ), 4 );
				Add( typeof( Cap ), 5 );
				Add( typeof( TallStrawHat ), 4 );
				Add( typeof( StrawHat ), 3 );
				Add( typeof( WizardsHat ), 5 );
				Add( typeof( LeatherCap ), 5 );
				Add( typeof( FeatheredHat ), 5 );
				Add( typeof( TricorneHat ), 4 );
				Add( typeof( Bandana ), 3 );
				Add( typeof( SkullCap ), 3 );
				Add( typeof( Bottle ), 3 );
				Add( typeof( RedBook ), 7 );
				Add( typeof( BlueBook ), 7 );
				Add( typeof( TanBook ), 7 );
				Add( typeof( WoodenBox ), 7 );
				Add( typeof( Kindling ), 1 );
				Add( typeof( Chessboard ), 1 );
				Add( typeof( CheckerBoard ), 1 );
				Add( typeof( Backgammon ), 1 );
				Add( typeof( Dices ), 1 );
				//Add( typeof( GuildDeed ), 25000 ); // NEEDS TO BE REMOVED WHEN NEW GUILD SYSTEM IS IN PLACE
				Add( typeof( Beeswax ), 1 );
            }
        }
    }
}
