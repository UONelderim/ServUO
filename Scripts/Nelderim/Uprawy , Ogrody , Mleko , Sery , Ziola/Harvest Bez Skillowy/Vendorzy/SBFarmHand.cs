using System; 
using System.Collections; 
using Server.Items; 
using Server.Items.Crops;

namespace Server.Mobiles 
{ 
	public class SBFarmHand : SBInfo 
	{ 
		private ArrayList m_BuyInfo = new InternalBuyInfo(); 
		private IShopSellInfo m_SellInfo = new InternalSellInfo(); 

		public SBFarmHand() 
		{ 
		} 

		public override IShopSellInfo SellInfo { get { return m_SellInfo; } } 
		public override ArrayList BuyInfo { get { return m_BuyInfo; } } 

		public class InternalBuyInfo : ArrayList 
		{ 
			public InternalBuyInfo() 
			{ 
				//Add( new GenericBuyInfo( typeof( Apple ), 20, 20, 0x9D0, 0 ) );
				//Add( new GenericBuyInfo( typeof( Grapes ), 20, 20, 0x9D1, 0 ) );
				//Add( new GenericBuyInfo( typeof( Watermelon ), 25, 20, 0xC5C, 0 ) );
				//Add( new GenericBuyInfo( typeof( YellowGourd ), 30, 20, 0xC64, 0 ) );
				//Add( new GenericBuyInfo( typeof( Pumpkin ), 40, 20, 0xC6A, 0 ) );
				//Add( new GenericBuyInfo( typeof( Onion ), 20, 20, 0xC6D, 0 ) );
				//Add( new GenericBuyInfo( typeof( Lettuce ), 15, 20, 0xC70, 0 ) );
				//Add( new GenericBuyInfo( typeof( Squash ), 30, 20, 0xC72, 0 ) );
				//Add( new GenericBuyInfo( typeof( HoneydewMelon ), 50, 20, 0xC74, 0 ) );
				//Add( new GenericBuyInfo( typeof( Carrot ), 30, 20, 0xC77, 0 ) );
				//Add( new GenericBuyInfo( typeof( Cantaloupe ), 25, 20, 0xC79, 0 ) );
				//Add( new GenericBuyInfo( typeof( Cabbage ), 20, 20, 0xC7B, 0 ) );
				//Add( new GenericBuyInfo( typeof( EarOfCorn ), 3, 20, XXXXXX, 0 ) );
				//Add( new GenericBuyInfo( typeof( Turnip ), 6, 20, XXXXXX, 0 ) );
				//Add( new GenericBuyInfo( typeof( SheafOfHay ), 2, 20, XXXXXX, 0 ) );
				//Add( new GenericBuyInfo( typeof( Lemon ), 20, 20, 0x1728, 0 ) );
				//Add( new GenericBuyInfo( typeof( Lime ), 20, 20, 0x172A, 0 ) );
				//Add( new GenericBuyInfo( typeof( Peach ), 20, 20, 0x9D2, 0 ) );
				//Add( new GenericBuyInfo( typeof( Pear ), 20, 20, 0x994, 0 ) );
				
                                Add( new GenericBuyInfo( "Nasiona bawelny", typeof( CottonSeed ), 100, 10, 0xF27, 0x5E2 ) );
				Add( new GenericBuyInfo( "Nasiona lnu", typeof( FlaxSeed ), 100, 10, 0xF27, 0x5E2 ) );
				Add( new GenericBuyInfo( "Nasiona pszenicy", typeof( WheatSeed ), 30, 20, 0xF27, 0x5E2 ) );
				Add( new GenericBuyInfo( "Nasiona kukurydzy", typeof( CornSeed ), 15, 10, 0xF27, 0x5E2 ) );
				Add( new GenericBuyInfo( "Nasiona marchwi", typeof( CarrotSeed ), 20, 10, 0xF27, 0x5E2 ) );
				Add( new GenericBuyInfo( "Nasiona cebuli", typeof( OnionSeed ), 20, 10, 0xF27, 0x5E2 ) );
				Add( new GenericBuyInfo( "Nasiona salaty", typeof( LettuceSeed ), 10, 10, 0xF27, 0x5E2 ) );
				Add( new GenericBuyInfo( "Nasiona kapusty", typeof( CabbageSeed ), 10, 10, 0xF27, 0x5E2 ) );
			        Add( new GenericBuyInfo( "Nasiona czosnku", typeof( GarlicSeed ), 25, 10, 0xF27, 0x5E2 ) );

                                ////Ziola////
                                //Add( new GenericBuyInfo( "Ginseng Seed", typeof( GinsengPlant ), 25, 10, 0xF27, 0x5E2 ) );
			        //Add( new GenericBuyInfo( "Mandrake Seed", typeof( MandrakePlant ), 25, 10, 0xF27, 0x5E2 ) );
			        //Add( new GenericBuyInfo( "Nightshade Seed", typeof( NightshadePlant ), 25, 10, 0xF27, 0x5E2 ) );		
                                ////Ziola////
                                
                                //Add( new GenericBuyInfo( "Ziemia Uprawna", typeof( GardenGroundAddonDeed ), 80000, 10, 0xE87, 0xE88 ) );

				Add( new GenericBuyInfo( "Maly Ogrod Domowy", typeof( MalyOgrodAddonDeed ), 20000, 10, 0xE87, 0xE88 ) );
				Add( new GenericBuyInfo( "Maly Ogrod Domowy", typeof( SredniOgrodAddonDeed ), 40000, 10, 0xE87, 0xE88 ) );
				Add( new GenericBuyInfo( "Maly Ogrod Domowy", typeof( WielkiOgrodAddonDeed ), 60000, 10, 0xE87, 0xE88 ) );
                         }
		} 

		public class InternalSellInfo : GenericSellInfo 
		{ 
			public InternalSellInfo() 
			{ 
				Add( typeof( Apple ), 1 );
				Add( typeof( Grapes ), 1 );
				Add( typeof( Watermelon ), 3 );
				Add( typeof( YellowGourd ), 1 );
				Add( typeof( Pumpkin ), 5 );
				Add( typeof( Onion ), 1 );
				Add( typeof( Lettuce ), 2 );
				Add( typeof( Squash ), 1 );
				Add( typeof( Carrot ), 1 );
				Add( typeof( HoneydewMelon ), 3 );
				Add( typeof( Cantaloupe ), 3 );
				Add( typeof( Cabbage ), 2 );
				Add( typeof( Lemon ), 1 );
				Add( typeof( Lime ), 1 );
				Add( typeof( Peach ), 1 );
				Add( typeof( Pear ), 1 );
			} 
		} 
	} 
}