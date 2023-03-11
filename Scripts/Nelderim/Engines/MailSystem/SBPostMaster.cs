/* 
	Mail System - Version 1.0
	
	Newly Modified On 15/11/2016 
	
	By Veldian 
	Dragon's Legacy Uo Shard 
*/

using System; 
using System.Collections;
using System.Collections.Generic;
using Server.Items; 

namespace Server.Mobiles
{
	public class SBPostMaster: SBInfo
	{
		private List<IBuyItemInfo> m_BuyInfo = new List<IBuyItemInfo>(); 
		private IShopSellInfo m_SellInfo = new InternalSellInfo(); 

        public SBPostMaster()
		{
		}

        public override IShopSellInfo SellInfo { get { return m_SellInfo; } } 
        public override List<IBuyItemInfo> BuyInfo { get { return m_BuyInfo; } } 


        public class InternalBuyInfo : ArrayList 
		{
			public InternalBuyInfo()
			{
               
				Add( new GenericBuyInfo( typeof( BlankScroll ), 5, 20, 0x0E34, 0 ) );
                Add( new GenericBuyInfo( typeof( AddressBook ), 50, 20, 0x2254, 0 ) );
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
				Add( typeof( SealingWax ), 1 );
				Add( typeof( BlankScroll ), 3 );
			}
		}
	}
}
