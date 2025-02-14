//
//	Auction version 2.1, by Xanthos and Arya
//
//  Based on original ideas and code by Arya
//

using System;
using Server;
using static Arya.Auction.AuctionMessages;

namespace Arya.Auction
{
	/// <summary>
	///     Manages the delivery of messages between players involved in an auction
	/// </summary>
	public class AuctionMessaging
	{
		public static void Initialize()
		{
			EventSink.Login += OnPlayerLogin;
		}

		private static void OnPlayerLogin(LoginEventArgs e)
		{
			if (!AuctionSystem.Running)
				return;

			foreach (AuctionItem auction in AuctionSystem.Pending)
			{
				auction.SendMessage(e.Mobile);
			}
		}

		/// <summary>
		///     Sends a message to a mobile to notify them that they have been outbid during an auction.
		/// </summary>
		/// <param name="auction">The auction generating the message</param>
		/// <param name="amount">The value of the mobile's bid</param>
		/// <param name="to">The mobile sending to. This can be null or offline. If offline, nothing will be sent.</param>
		public static void SendOutbidMessage(AuctionItem auction, int amount, Mobile to)
		{
			if (to == null || to.Account == null || to.NetState == null)
				return;

			AuctionMessageGump gump = new AuctionMessageGump(auction, true, false, false);
			gump.Message = String.Format(ERR_OUTBID_FMT, amount.ToString("#,0"));
			gump.OkText = "Close this message";
			gump.ShowExpiration = false;

			to.SendGump(new AuctionNoticeGump(gump));
		}

		/// <summary>
		///     Sends the confirmation request for the reserve not met to the auction owner
		/// </summary>
		/// <param name="item">The auction</param>
		public static void SendReserveMessageToOwner(AuctionItem item)
		{
			if (item.Owner == null || item.Owner.Account == null || item.Owner.NetState == null)
				return;

			AuctionMessageGump gump = new AuctionMessageGump(item, false, true, true);
			string msg = String.Format(
				MSG_RESERVE_OWNER_FMT,
				item.HighestBid.Amount, item.Reserve.ToString("#,0"));

			if (!item.IsValid())
			{
				msg += MSG_RESERVE_OWNER_ITEM_INVALID;
			}

			gump.Message = msg;
			gump.OkText = MSG_RESERVE_OWNER_OK;
			gump.CancelText = MSG_RESERVE_OWNER_CANCEL;

			item.Owner.SendGump(new AuctionNoticeGump(gump));
		}

		/// <summary>
		///     Sends the information message about the reserve not met to the buyer
		/// </summary>
		public static void SendReserveMessageToBuyer(AuctionItem item)
		{
			if (item.HighestBid.Mobile == null || item.HighestBid.Mobile.Account == null ||
			    item.HighestBid.Mobile.NetState == null)
				return;

			AuctionMessageGump gump = new AuctionMessageGump(item, true, false, true);
			gump.Message = String.Format(MSG_RESERVE_BUYER_FMT, item.HighestBid.Amount, item.Reserve);

			gump.OkText = MSG_BUTTON_CLOSE;

			item.HighestBid.Mobile.SendGump(new AuctionNoticeGump(gump));
		}

		/// <summary>
		///     Informs the buyer that some of the items auctioned have been deleted.
		/// </summary>
		public static void SendInvalidMessageToBuyer(AuctionItem item)
		{
			Mobile m = item.HighestBid.Mobile;

			if (m == null || m.Account == null || m.NetState == null)
				return;

			AuctionMessageGump gump = new AuctionMessageGump(item, false, false, true);
			string msg = String.Format(MSG_ERROR_BUYER_FMT, item.HighestBid.Amount.ToString("#,0"));

			if (!item.ReserveMet)
			{
				msg += MSG_ERROR_BUYER_RESERVE_NOT_MET;
			}

			gump.Message = msg;
			gump.OkText = MSG_ERROR_BUYER_OK;
			gump.CancelText = MSG_ERROR_BUYER_CANCEL;

			m.SendGump(new AuctionNoticeGump(gump));
		}

		/// <summary>
		///     Sends the invalid message to the owner.
		/// </summary>
		/// <param name="gump"></param>
		public static void SendInvalidMessageToOwner(AuctionItem item)
		{
			Mobile m = item.Owner;

			if (m == null || m.Account == null || m.NetState == null)
				return;

			AuctionMessageGump gump = new AuctionMessageGump(item, true, true, true);
			gump.Message = MSG_ERROR_OWNER;
			gump.OkText = MSG_BUTTON_CLOSE;

			m.SendGump(new AuctionNoticeGump(gump));
		}
	}
}
