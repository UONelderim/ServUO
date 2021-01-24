using System;
using System.Collections;
using Server;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Prompts;

namespace Server.Engines.BulkOrders
{
	public class HuntingBOBGump : Gump
	{
		private PlayerMobile m_From;
		private HuntingBulkOrderBook m_Book;
		private ArrayList m_List;

		private int m_Page;

		private const int LabelColor = 0x7FFF;

		public Item Reconstruct( object obj )
		{
			Item item = null;

			if ( obj is HuntingBOBLargeEntry )
				item = ((HuntingBOBLargeEntry)obj).Reconstruct();
			else if ( obj is HuntingBOBSmallEntry )
				item = ((HuntingBOBSmallEntry)obj).Reconstruct();

			return item;
		}

		public bool CheckFilter( object obj )
		{
			if ( obj is HuntingBOBLargeEntry )
			{
				HuntingBOBLargeEntry e = (HuntingBOBLargeEntry)obj;

				return CheckFilter( e.AmountMax, true, ( e.Entries.Length > 0 ? e.Entries[0].ItemType : null ), (e.Entries[0]).Level );
			}
			else if ( obj is HuntingBOBSmallEntry )
			{
				HuntingBOBSmallEntry e = (HuntingBOBSmallEntry)obj;

				return CheckFilter( e.AmountMax, false, e.ItemType, e.Level );
			}

			return false;
		}

		public bool CheckFilter( int amountMax, bool isLarge, Type itemType, int level )
		{
			HuntingBOBFilter f = m_Book.Filter;

			if ( f.IsDefault )
				return true;

			//if ( f.Quality == 1 && reqExc )
			//	return false;
			//else if ( f.Quality == 2 && !reqExc )
			//	return false;

			if ( f.Quantity == 1 && amountMax != 10 )
				return false;
			else if ( f.Quantity == 2 && amountMax != 15 )
				return false;
			else if ( f.Quantity == 3 && amountMax != 20 )
				return false;

			if ( f.Type == 1 && isLarge )
				return false;
			else if ( f.Type == 2 && !isLarge )
				return false;

			switch ( f.Class )
			{
				default:
				case  0: return true;
				case  1: return ( level == 1 );
				case  2: return ( level == 2 );
				case  3: return ( level == 3 );
			}
		}

		public int GetIndexForPage( int page )
		{
			int index = 0;

			while ( page-- > 0 )
				index += GetCountForIndex( index );

			return index;
		}

		public int GetCountForIndex( int index )
		{
			int slots = 0;
			int count = 0;

			ArrayList list = m_List;

			for ( int i = index; i >= 0 && i < list.Count; ++i )
			{
				object obj = list[i];

				if ( CheckFilter( obj ) )
				{
					int add;

					if ( obj is HuntingBOBLargeEntry )
						add = ((HuntingBOBLargeEntry)obj).Entries.Length;
					else
						add = 1;

					if ( (slots + add) > 10 )
						break;

					slots += add;
				}

				++count;
			}

			return count;
		}

		public HuntingBOBGump( PlayerMobile from, HuntingBulkOrderBook book ) : this( from, book, 0, null )
		{
		}

		public override void OnResponse( Server.Network.NetState sender, RelayInfo info )
		{
			int index = info.ButtonID;

			switch ( index )
			{
				case 0: // EXIT
				{
					break;
				}
				case 1: // Set Filter
				{
					m_From.SendGump( new HuntingBOBFilterGump( m_From, m_Book ) );

					break;
				}
				case 2: // Previous page
				{
					if ( m_Page > 0 )
						m_From.SendGump( new HuntingBOBGump( m_From, m_Book, m_Page - 1, m_List ) );

					return;
				}
				case 3: // Next page
				{
					if ( GetIndexForPage( m_Page + 1 ) < m_List.Count )
						m_From.SendGump( new HuntingBOBGump( m_From, m_Book, m_Page + 1, m_List ) );

					break;
				}
				default:
				{
					bool canDrop = m_Book.IsChildOf( m_From.Backpack );
					bool canPrice = canDrop || (m_Book.RootParent is PlayerVendor);

					index -= 4;

					int type = index % 2;
					index /= 2;

					if ( index < 0 || index >= m_List.Count )
						break;

					object obj = m_List[index];

					if ( !m_Book.Entries.Contains( obj ) )
					{
						m_From.SendLocalizedMessage( 1062382 ); // The deed selected is not available.
						break;
					}

					if ( type == 0 ) // Drop
					{
						if ( m_Book.IsChildOf( m_From.Backpack ) )
						{
							Item item = Reconstruct( obj );

							if ( item != null )
							{
								m_From.AddToBackpack( item );
								m_From.SendLocalizedMessage( 1045152 ); // The bulk order deed has been placed in your backpack.

								m_Book.Entries.Remove( obj );
								m_Book.InvalidateProperties();

								if ( m_Book.Entries.Count > 0 )
									m_From.SendGump( new HuntingBOBGump( m_From, m_Book, 0, null ) );
								else
									m_From.SendLocalizedMessage( 1062381 ); // The book is empty.
							}
							else
							{
								m_From.SendMessage( "Internal error. The bulk order deed could not be reconstructed." );
							}
						}
					}
					else // Set Price | Buy
					{
						if ( m_Book.IsChildOf( m_From.Backpack ) )
						{
							m_From.Prompt = new SetPricePrompt( m_Book, obj, m_Page, m_List );
							m_From.SendLocalizedMessage( 1062383 ); // Type in a price for the deed:
						}
						else if ( m_Book.RootParent is PlayerVendor )
						{
							PlayerVendor pv = (PlayerVendor)m_Book.RootParent;

							VendorItem vi = pv.GetVendorItem( m_Book );

							int price = 0;

							if ( vi != null && !vi.IsForSale )
							{
								if ( obj is HuntingBOBLargeEntry )
									price = ((HuntingBOBLargeEntry)obj).Price;
								else if ( obj is HuntingBOBSmallEntry )
									price = ((HuntingBOBSmallEntry)obj).Price;
							}

							if ( price == 0 )
								m_From.SendLocalizedMessage( 1062382 ); // The deed selected is not available.
							else
								m_From.SendGump( new HuntingBODBuyGump( m_From, m_Book, obj, price ) );
						}
					}

					break;
				}
			}
		}

		private class SetPricePrompt : Prompt
		{
			private HuntingBulkOrderBook m_Book;
			private object m_Object;
			private int m_Page;
			private ArrayList m_List;

			public SetPricePrompt( HuntingBulkOrderBook book, object obj, int page, ArrayList list )
			{
				m_Book = book;
				m_Object = obj;
				m_Page = page;
				m_List = list;
			}

			public override void OnResponse( Mobile from, string text )
			{
				if ( !m_Book.Entries.Contains( m_Object ) )
				{
					from.SendLocalizedMessage( 1062382 ); // The deed selected is not available.
					return;
				}

				int price = Utility.ToInt32( text );

				if ( price < 0 || price > 250000000 )
				{
					from.SendLocalizedMessage( 1062390 ); // The price you requested is outrageous!
				}
				else if ( m_Object is HuntingBOBLargeEntry )
				{
					((HuntingBOBLargeEntry)m_Object).Price = price;

					from.SendLocalizedMessage( 1062384 ); // Deed price set.

					if ( from is PlayerMobile )
						from.SendGump( new HuntingBOBGump( (PlayerMobile)from, m_Book, m_Page, m_List ) );
				}
				else if ( m_Object is HuntingBOBSmallEntry )
				{
					((HuntingBOBSmallEntry)m_Object).Price = price;

					from.SendLocalizedMessage( 1062384 ); // Deed price set.

					if ( from is PlayerMobile )
						from.SendGump( new HuntingBOBGump( (PlayerMobile)from, m_Book, m_Page, m_List ) );
				}
			}
		}

		public HuntingBOBGump( PlayerMobile from, HuntingBulkOrderBook book, int page, ArrayList list ) : base( 12, 24 )
		{
			from.CloseGump( typeof( HuntingBOBGump ) );
			from.CloseGump( typeof( HuntingBOBFilterGump ) );

			m_From = from;
			m_Book = book;
			m_Page = page;

			if ( list == null )
			{
				list = new ArrayList( book.Entries.Count );

				for ( int i = 0; i < book.Entries.Count; ++i )
				{
					object obj = book.Entries[i];

					if ( CheckFilter( obj ) )
						list.Add( obj );
				}
			}

			m_List = list;

			int index = GetIndexForPage( page );
			int count = GetCountForIndex( index );

			int tableIndex = 0;

			PlayerVendor pv = book.RootParent as PlayerVendor;

			bool canDrop = book.IsChildOf( from.Backpack );
			bool canBuy = ( pv != null );
			bool canPrice = ( canDrop || canBuy );

			if ( canBuy )
			{
				VendorItem vi = pv.GetVendorItem( book );

				canBuy = ( vi != null && !vi.IsForSale );
			}

			int width = 600;

			if ( !canPrice )
				width = 516;

			X = (624 - width) / 2;

			AddPage( 0 );

			AddBackground( 10, 10, width, 439, 5054 );
			AddImageTiled( 18, 20, width - 17, 420, 2624 );

			if ( canPrice )
			{
				AddImageTiled( 573, 64, 24, 352, 200 );
				AddImageTiled( 493, 64, 78, 352, 1416 );
			}

			if ( canDrop )
				AddImageTiled( 24, 64, 32, 352, 1416 );

			AddImageTiled( 58, 64, 36, 352, 200 );
			AddImageTiled( 96, 64, 133, 352, 1416 );
			AddImageTiled( 231, 64, 180, 352, 200 );
			AddImageTiled( 313, 64, 100, 352, 1416 );
			AddImageTiled( 415, 64, 76, 352, 200 );

			for ( int i = index; i < (index + count) && i >= 0 && i < list.Count; ++i )
			{
				object obj = list[i];

				if ( !CheckFilter( obj ) )
					continue;

				AddImageTiled( 24, 94 + (tableIndex * 32), canPrice ? 573 : 489, 2, 2624 );

				if ( obj is HuntingBOBLargeEntry )
					tableIndex += ((HuntingBOBLargeEntry)obj).Entries.Length;
				else if ( obj is HuntingBOBSmallEntry )
					++tableIndex;
			}

			AddAlphaRegion( 18, 20, width - 17, 420 );
			AddImage( 5, 5, 10460 );
			AddImage( width - 15, 5, 10460 );
			AddImage( 5, 424, 10460 );
			AddImage( width - 15, 424, 10460 );

			AddHtmlLocalized( canPrice ? 266 : 224, 32, 200, 32, 1062220, LabelColor, false, false ); // Bulk Order Book
			AddHtmlLocalized( 63, 64, 200, 32, 1062213, LabelColor, false, false ); // Type
			AddHtmlLocalized( 147, 64, 200, 32, 1062214, LabelColor, false, false ); // Item
			//AddHtmlLocalized( 246, 64, 200, 32, 1062215, LabelColor, false, false ); // Quality
			AddHtmlLocalized( 246, 64, 600, 32, 1062217, LabelColor, false, false ); // Amount
            AddHtml( 336, 64, 200, 32, "<basefont color=#FFFFFF>Klasa</basefont>", false, false );
      
			AddButton( 35, 32, 4005, 4007, 1, GumpButtonType.Reply, 0 );
			AddHtmlLocalized( 70, 32, 200, 32, 1062476, LabelColor, false, false ); // Set Filter

			HuntingBOBFilter f = book.Filter;

			if ( f.IsDefault )
				AddHtmlLocalized( canPrice ? 470 : 386, 32, 120, 32, 1062475, 16927, false, false ); // Using No Filter
			else if ( from.UseOwnFilter )
				AddHtmlLocalized( canPrice ? 470 : 386, 32, 120, 32, 1062451, 16927, false, false ); // Using Your Filter
			else
				AddHtmlLocalized( canPrice ? 470 : 386, 32, 120, 32, 1062230, 16927, false, false ); // Using Book Filter

			AddButton( 375, 416, 4017, 4018, 0, GumpButtonType.Reply, 0 );
			AddHtmlLocalized( 410, 416, 120, 20, 1011441, LabelColor, false, false ); // EXIT

			if ( canDrop )
				AddHtmlLocalized( 26, 64, 50, 32, 1062212, LabelColor, false, false ); // Drop

			if ( canPrice )
			{
				AddHtmlLocalized( 516, 64, 200, 32, 1062218, LabelColor, false, false ); // Price

				if ( canBuy )
					AddHtmlLocalized( 576, 64, 200, 32, 1062219, LabelColor, false, false ); // Buy
				else
					AddHtmlLocalized( 576, 64, 200, 32, 1062227, LabelColor, false, false ); // Set
			}

			tableIndex = 0;

			if ( page > 0 )
			{
				AddButton( 75, 416, 4014, 4016, 2, GumpButtonType.Reply, 0 );
				AddHtmlLocalized( 110, 416, 150, 20, 1011067, LabelColor, false, false ); // Previous page
			}

			if ( GetIndexForPage( page + 1 ) < list.Count )
			{
				AddButton( 225, 416, 4005, 4007, 3, GumpButtonType.Reply, 0 );
				AddHtmlLocalized( 260, 416, 150, 20, 1011066, LabelColor, false, false ); // Next page
			}

			for ( int i = index; i < (index + count) && i >= 0 && i < list.Count; ++i )
			{
				object obj = list[i];

				if ( !CheckFilter( obj ) )
					continue;

				if ( obj is HuntingBOBLargeEntry )
				{
					HuntingBOBLargeEntry e = (HuntingBOBLargeEntry)obj;

					int y = 96 + (tableIndex * 32);

					if ( canDrop )
						AddButton( 35, y + 2, 5602, 5606, 4 + (i * 2), GumpButtonType.Reply, 0 );

					if ( canDrop || (canBuy && e.Price > 0) )
					{
						AddButton( 579, y + 2, 2117, 2118, 5 + (i * 2), GumpButtonType.Reply, 0 );
						AddLabel( 495, y, 1152, e.Price.ToString() );
					}

					AddHtmlLocalized( 61, y, 50, 32, 1062225, LabelColor, false, false ); // Large

					for ( int j = 0; j < e.Entries.Length; ++j )
					{
						HuntingBOBLargeSubEntry sub = e.Entries[j];

						AddHtmlLocalized( 103, y, 130, 32, sub.Number, LabelColor, false, false );


						AddLabel( 235, y, 1152, String.Format( "{0} / {1}", sub.AmountCur, e.AmountMax ) );
                        AddLabel(316, y, 1152, sub.Level.ToString());

						++tableIndex;
						y += 32;
					}
				}
				else if ( obj is HuntingBOBSmallEntry )
				{
					HuntingBOBSmallEntry e = (HuntingBOBSmallEntry)obj;

					int y = 96 + (tableIndex++ * 32);

					if ( canDrop )
						AddButton( 35, y + 2, 5602, 5606, 4 + (i * 2), GumpButtonType.Reply, 0 );

					if ( canDrop || (canBuy && e.Price > 0) )
					{
						AddButton( 579, y + 2, 2117, 2118, 5 + (i * 2), GumpButtonType.Reply, 0 );
						AddLabel( 495, y, 1152, e.Price.ToString() );
					}

					AddHtmlLocalized( 61, y, 50, 32, 1062224, LabelColor, false, false ); // Small

					AddHtmlLocalized( 103, y, 130, 32, e.Number, LabelColor, false, false );


					AddLabel( 235, y, 1152, String.Format( "{0} / {1}", e.AmountCur, e.AmountMax ) );
					AddLabel( 316, y, 1152, e.Level.ToString() );
				}
			}
		}
	}
}