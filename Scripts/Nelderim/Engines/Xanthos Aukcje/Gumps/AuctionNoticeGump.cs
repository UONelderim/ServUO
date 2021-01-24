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

namespace Arya.Auction
{
	/// <summary>
	/// Provides the message notice for messages from the auction system
	/// </summary>
	public class AuctionNoticeGump : Gump
	{
		private AuctionMessageGump m_Message;

		public AuctionNoticeGump( AuctionMessageGump msg ) : base ( 25, 25 )
		{
			m_Message = msg;
			MakeGump();
		}

		private void MakeGump()
		{
			Closable = false;
			Disposable = true;
			Dragable = true;
			Resizable = false;

			AddPage(0);
			AddImageTiled(0, 0, 75, 75, 3004);
			AddImageTiled(1, 1, 73, 73, 2624);
			AddAlphaRegion(1, 1, 73, 73);
			AddButton(7, 7, 5573, 5574, 1, GumpButtonType.Reply, 0);
		}

		public override void OnResponse(Server.Network.NetState sender, RelayInfo info)
		{
			if ( ! AuctionSystem.Running )
			{
				sender.Mobile.SendMessage( AuctionConfig.MessageHue, AuctionSystem.ST[ 15 ] );
				return;
			}

			if ( info.ButtonID == 1 )
			{
				if ( m_Message != null )
				{
					m_Message.SendTo( sender.Mobile );
				}
			}
		}
	}
}
