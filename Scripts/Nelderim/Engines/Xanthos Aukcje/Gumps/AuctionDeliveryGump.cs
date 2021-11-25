#region AuthorHeader
//
//	Auction version 2.1, by Xanthos and Arya
//
//  Based on original ideas and code by Arya
//
#endregion AuthorHeader
using System;
using System.Collections;

using Server;
using Server.Gumps;
using Xanthos.Utilities;

namespace Arya.Auction
{
	/// <summary>
	/// This gump is used to deliver the auction checks
	/// </summary>
	public class AuctionDeliveryGump : Gump
	{
		private AuctionCheck m_Check;

		public AuctionDeliveryGump( AuctionCheck check ) : base( 100, 100 )
		{
			m_Check = check;

			MakeGump();
		}

		private void MakeGump()
		{
			Closable = true;
			Disposable = true;
			Dragable = true;
			Resizable = false;

			AddPage(0);
			AddImage(0, 0, 2080);
			AddImageTiled(18, 37, 263, 245, 2081);
			AddImage(20, 280, 2083);
			AddLabel(75, 5, 210, AuctionSystem.ST[ 0 ] );
			AddLabel(45, 35, 0, AuctionSystem.ST[ 1 ] );

			int goldHue = 0;
			int itemHue = 0;

			if ( m_Check is AuctionGoldCheck )
			{
				// Delivering gold
				goldHue = 143;
				itemHue = 730;
				AddImage(200, 39, 2530);
				AddLabel(70, 220, Misc.kLabelHue, AuctionSystem.ST[ 2 ] );
			}
			else
			{
				// Delivering an item
				goldHue = 730;
				itemHue = 143;
				AddImage(135, 39, 2530);
				AddLabel(70, 220, Misc.kLabelHue, AuctionSystem.ST[ 3 ] );
			}

			AddLabel(145, 35, itemHue, AuctionSystem.ST[ 4 ] );
			AddLabel(210, 35, goldHue, AuctionSystem.ST[ 5 ] );

			AddImage(45, 60, 2091);
			AddImage(45, 100, 2091);

			// Item name
			AddLabelCropped( 55, 75, 200, 20, Misc.kLabelHue, m_Check.ItemName );

			AddHtml( 45, 115, 215, 100, m_Check.HtmlDetails, (bool)false, (bool)false);

			// Button 1 : Place in bank
			AddButton(45, 223, 5601, 5605, 1, GumpButtonType.Reply, 0);

			// Button 2 : View Auction
			if ( m_Check.Auction != null )
			{
				AddButton(45, 243, 5601, 5605, 2, GumpButtonType.Reply, 0);
				AddLabel(70, 240, Misc.kLabelHue, AuctionSystem.ST[ 6 ] );
			}

			// Button 0 : Close
			AddButton(45, 263, 5601, 5605, 0, GumpButtonType.Reply, 0);
			AddLabel(70, 260, Misc.kLabelHue, AuctionSystem.ST[ 7 ] );

			AddImage(225, 240, 9004);
		}

		public override void OnResponse(Server.Network.NetState sender, RelayInfo info)
		{
			switch ( info.ButtonID )
			{
				case 1: // Place in bank

					if ( !m_Check.Deliver( sender.Mobile ))
					{
						sender.Mobile.SendGump( new AuctionDeliveryGump( m_Check ));
					}
					break;

				case 2: // View auction

					if ( m_Check.Auction != null )
					{
						sender.Mobile.SendGump( new AuctionViewGump( sender.Mobile, m_Check.Auction, null ));
					}
					else
					{
						sender.Mobile.SendGump( new AuctionDeliveryGump( m_Check ));
					}
					break;
			}
		}
	}
}