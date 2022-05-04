using Server.Engines.Quests;
using Server.Items;
using System.Collections.Generic;

namespace Server.Mobiles
{
    public class SBAlchemist : SBInfo
    {
        private readonly List<IBuyItemInfo> m_BuyInfo;
        private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();

        public SBAlchemist(Mobile m)
        {
            if (m != null)
            {
                m_BuyInfo = new InternalBuyInfo(m);
            }
        }

        public override IShopSellInfo SellInfo => m_SellInfo;
        public override List<IBuyItemInfo> BuyInfo => m_BuyInfo;

        public class InternalBuyInfo : List<IBuyItemInfo>
        {
            public InternalBuyInfo(Mobile m)
            {
				Add( new GenericBuyInfo( typeof( RefreshPotion ), 15, 100, 0xF0B, 0 ) );
				Add( new GenericBuyInfo( typeof( AgilityPotion ), 15, 100, 0xF08, 0 ) );
				Add( new GenericBuyInfo( typeof( NightSightPotion ), 15, 100, 0xF06, 0 ) );
				Add( new GenericBuyInfo( typeof( LesserHealPotion ), 15, 100, 0xF0C, 0 ) );
				Add( new GenericBuyInfo( typeof( StrengthPotion ), 15, 100, 0xF09, 0 ) );
				Add( new GenericBuyInfo( typeof( LesserPoisonPotion ), 15, 100, 0xF0A, 0 ) );
 				Add( new GenericBuyInfo( typeof( LesserCurePotion ), 15, 100, 0xF07, 0 ) );
				Add( new GenericBuyInfo( typeof( LesserExplosionPotion ), 21, 100, 0xF0D, 0 ) );
				Add( new GenericBuyInfo( typeof( MortarPestle ), 30, 100, 0xE9B, 0 ) );

				Add( new GenericBuyInfo( typeof( BlackPearl ), 7, 100, 0xF7A, 0 ) );
				Add( new GenericBuyInfo( typeof( Bloodmoss ), 7, 100, 0xF7B, 0 ) );
				Add( new GenericBuyInfo( typeof( Garlic ), 5, 100, 0xF84, 0 ) );
				Add( new GenericBuyInfo( typeof( Ginseng ), 5, 100, 0xF85, 0 ) );
				Add( new GenericBuyInfo( typeof( MandrakeRoot ), 5, 100, 0xF86, 0 ) );
				Add( new GenericBuyInfo( typeof( Nightshade ), 5, 100, 0xF88, 0 ) );
				Add( new GenericBuyInfo( typeof( SpidersSilk ), 5, 100, 0xF8D, 0 ) );
				Add( new GenericBuyInfo( typeof( SulfurousAsh ), 5, 100, 0xF8C, 0 ) );

				Add( new GenericBuyInfo( typeof( Bottle ), 5, 100, 0xF0E, 0 ) ); 
				Add( new GenericBuyInfo( typeof( HeatingStand ), 50, 100, 0x1849, 0 ) ); 
                Add( new GenericBuyInfo( "1041060", typeof( HairDye ), 1000, 100, 0xEFF, 0 ) );

                if (m.Map != Map.TerMur)
                {
                    Add(new GenericBuyInfo(typeof(HairDye), 37, 10, 0xEFF, 0));
                }
                else if (m is Zosilem)
                {
                  //  Add(new GenericBuyInfo(typeof(GlassblowingBook), 10637, 30, 0xFF4, 0));
                    Add(new GenericBuyInfo(typeof(SandMiningBook), 10637, 30, 0xFF4, 0));
                    Add(new GenericBuyInfo(typeof(Blowpipe), 21, 100, 0xE8A, 0x3B9));
                }
            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
            public InternalSellInfo()
            {
				// juri : BlackPearl 3->2, Bloodmoss 3->2, Bottle 3->2
				Add( typeof( BlackPearl ), 2 ); 
				Add( typeof( Bloodmoss ), 2 ); 
				Add( typeof( MandrakeRoot ), 2 ); 
				Add( typeof( Garlic ), 2 ); 
				Add( typeof( Ginseng ), 2 ); 
				Add( typeof( Nightshade ), 2 ); 
				Add( typeof( SpidersSilk ), 2 ); 
				Add( typeof( SulfurousAsh ), 2 ); 
				Add( typeof( Bottle ), 2 );
				Add( typeof( MortarPestle ), 4 );
				Add( typeof( HairDye ), 200 );
				
				Add( typeof( NightSightPotion ), 7 );
				Add( typeof( AgilityPotion ), 7 );
				Add( typeof( StrengthPotion ), 7 );
				Add( typeof( RefreshPotion ), 7 );
				Add( typeof( LesserCurePotion ), 7 );
				Add( typeof( LesserHealPotion ), 7 );
				Add( typeof( LesserPoisonPotion ), 7 );
				Add( typeof( LesserExplosionPotion ), 10 );

				//Add( typeof( GlassblowingBook ), 3000 );
				Add( typeof( Blowpipe ), 8 );
            }
        }
    }
}