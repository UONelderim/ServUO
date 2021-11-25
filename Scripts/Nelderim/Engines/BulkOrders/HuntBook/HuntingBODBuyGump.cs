using System;
using Server;
using Server.Items;
using Server.Gumps;
using Server.Mobiles;

namespace Server.Engines.BulkOrders
{
	public class HuntingBODBuyGump : Gump
	{
		private PlayerMobile m_From;
		private HuntingBulkOrderBook m_Book;
		private object m_Object;
		private int m_Price;

		public override void OnResponse( Server.Network.NetState sender, RelayInfo info )
		{
			if ( info.ButtonID == 2 )
			{
				PlayerVendor pv = m_Book.RootParent as PlayerVendor;

				if ( m_Book.Entries.Contains( m_Object ) && pv != null )
				{
					int price = 0;

					VendorItem vi = pv.GetVendorItem( m_Book );

					if ( vi != null && !vi.IsForSale )
					{
						if ( m_Object is HuntingBOBLargeEntry )
							price = ((HuntingBOBLargeEntry)m_Object).Price;
						else if ( m_Object is HuntingBOBSmallEntry )
							price = ((HuntingBOBSmallEntry)m_Object).Price;
					}

					if ( price != m_Price )
					{
						pv.SayTo( m_From, "The price has been been changed. If you like, you may offer to purchase the item again." );
					}
					else if ( price == 0 )
					{
						pv.SayTo( m_From, 1062382 ); // The deed selected is not available.
					}
					else
					{
						Item item = null;

						if ( m_Object is HuntingBOBLargeEntry )
							item = ((HuntingBOBLargeEntry)m_Object).Reconstruct();
						else if ( m_Object is HuntingBOBSmallEntry )
							item = ((HuntingBOBSmallEntry)m_Object).Reconstruct();

						if ( item == null )
						{
							m_From.SendMessage( "Internal error. The bulk order deed could not be reconstructed." );
						}
						else
						{
							pv.Say( m_From.Name );

							Container pack = m_From.Backpack;

							if ( (pack != null && pack.ConsumeTotal( typeof( Gold ), price )) || Banker.Withdraw( m_From, price ) )
							{
								m_Book.Entries.Remove( m_Object );
								m_Book.InvalidateProperties();

								pv.HoldGold += price;

								if ( m_From.AddToBackpack( item ) )
									m_From.SendLocalizedMessage( 1045152 ); // The bulk order deed has been placed in your backpack.
								else
									pv.SayTo( m_From, 503204 ); // You do not have room in your backpack for this.

								if ( m_Book.Entries.Count > 0 )
									m_From.SendGump( new HuntingBOBGump( m_From, m_Book ) );
								else
									m_From.SendLocalizedMessage( 1062381 ); // The book is empty.
							}
							else
							{
								pv.SayTo( m_From, 503205 ); // You cannot afford this item.
								item.Delete();
							}
						}
					}
				}
				else
				{
					if ( pv == null )
						m_From.SendLocalizedMessage( 1062382 ); // The deed selected is not available.
					else
						pv.SayTo( m_From, 1062382 ); // The deed selected is not available.
				}
			}
			else
			{
				m_From.SendLocalizedMessage( 503207 ); // Cancelled purchase.
			}
		}

		public HuntingBODBuyGump( PlayerMobile from, HuntingBulkOrderBook book, object obj, int price ) : base( 100, 200 )
		{
			m_From = from;
			m_Book = book;
			m_Object = obj;
			m_Price = price;

			AddPage( 0 );

			AddBackground( 100, 10, 300, 150, 5054 );

			AddHtmlLocalized( 125, 20, 250, 24, 1019070, false, false ); // You have agreed to purchase:
			AddHtmlLocalized( 125, 45, 250, 24, 1045151, false, false ); // a bulk order deed

			AddHtmlLocalized( 125, 70, 250, 24, 1019071, false, false ); // for the amount of:
			AddLabel( 125, 95, 0, price.ToString() );

			AddButton( 250, 130, 4005, 4007, 1, GumpButtonType.Reply, 0 );
			AddHtmlLocalized( 282, 130, 100, 24, 1011012, false, false ); // CANCEL

			AddButton( 120, 130, 4005, 4007, 2, GumpButtonType.Reply, 0 );
			AddHtmlLocalized( 152, 130, 100, 24, 1011036, false, false ); // OKAY
		}
	}
}
