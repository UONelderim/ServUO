using System;
using System.Collections.Generic;
using Server.Items;

namespace Server.Mobiles
{
	public class SBArtTrader : SBInfo
	{
		public override IShopSellInfo SellInfo => new InternalSellInfo();
		public override List<IBuyItemInfo> BuyInfo => new InternalBuyInfo();
		public SBArtTrader()
		{
		}
		
		public class InternalBuyInfo : List<IBuyItemInfo>
		{
			public InternalBuyInfo()
			{  
				Add( new GenericBuyInfo( typeof( ArtLootBox ), 2000, 5, 0x2DF3, 0 , new object[]{ArtGroup.None}) );
				Add( new GenericBuyInfo( typeof( ArtLootBox ), 3000, 5, 0x2DF3, 0 , new object[]{ArtGroup.Boss}) );
				Add( new GenericBuyInfo( typeof( ArtLootBox ), 3000, 5, 0x2DF3, 0 , new object[]{ArtGroup.Miniboss}) );
				Add( new GenericBuyInfo( typeof( ArtLootBox ), 2500, 5, 0x2DF3, 0 , new object[]{ArtGroup.Paragon}) );
				Add( new GenericBuyInfo( typeof( ArtLootBox ), 4500, 5, 0x2DF3, 0 , new object[]{ArtGroup.Doom}) );
				Add( new GenericBuyInfo( typeof( ArtLootBox ), 3000, 5, 0x2DF3, 0 , new object[]{ArtGroup.Hunter}) );
				Add( new GenericBuyInfo( typeof( ArtLootBox ), 3000, 5, 0x2DF3, 0 , new object[]{ArtGroup.Cartography}) );
				Add( new GenericBuyInfo( typeof( ArtLootBox ), 3000, 5, 0x2DF3, 0 , new object[]{ArtGroup.Fishing}) );
			}
		}

		public class InternalSellInfo : IShopSellInfo
		{
			private Dictionary<Type, int> m_Table = new Dictionary<Type, int>();
			private Type[] m_Types;
			
			public InternalSellInfo()
			{
				foreach (Type type in ArtifactHelper.AllArtifacts) 
				{
					Add(type, 100);
				}
			}
			
			public void Add( Type type, int price )
			{
				m_Table[type] = price;
				m_Types = null;
			}
			
			public string GetNameFor(IEntity e)
			{
				if (e.Name != null)
					return e.Name;

				if (e is Item item)
					return item.LabelNumber.ToString();

				return e.GetType().Name;
			}

			public int GetSellPriceFor(IEntity item)
			{
				var scalar = 1.0;
				m_Table.TryGetValue( item.GetType(), out var price );

				if ( item is IDurability ) {
					IDurability art = (IDurability)item;
					scalar = (double)art.HitPoints / art.InitMaxHits;
				}

				price = (int)(price * scalar);
				if ( price < 1 )
					price = 1;

				return price;
			}

			public int GetSellPriceFor(IEntity item, IVendor vendor)
			{
				return GetSellPriceFor(item);
			}

			public int GetBuyPriceFor(IEntity item)
			{
				return 1;
			}

			public int GetBuyPriceFor(IEntity item, IVendor vendor)
			{
				return GetBuyPriceFor(item);
			}

			public bool IsSellable(IEntity item)
			{
				return m_Table.ContainsKey(item.GetType());
			}

			public Type[] Types
			{
				get
				{
					if (m_Types == null)
					{
						m_Types = new Type[m_Table.Keys.Count];
						m_Table.Keys.CopyTo(m_Types, 0);
					}

					return m_Types;
				}
			}

			public bool IsResellable(IEntity item)
			{
				return false;
			}

			public void OnSold(Mobile seller, IVendor vendor, IEntity item, int amount)
			{
				EventSink.InvokeValidVendorSell(new ValidVendorSellEventArgs(seller, vendor, item, GetSellPriceFor(item, vendor)));
			}
		}
	}
}
