using System;
using Server;
using static Arya.Auction.AuctionMessages;

namespace Arya.Auction
{
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

		public static void SendOutbidMessage(AuctionItem auction, int amount, Mobile to)
		{
			if (to == null || to.Account == null || to.NetState == null)
				return;

			AuctionMessageGump gump = new AuctionMessageGump(auction, true, false, false);
			gump.Message = String.Format(ERR_OUTBID_FMT, amount.ToString("#,0"));
			gump.OkText = MSG_BUTTON_CLOSE;
			gump.ShowExpiration = false;

			to.SendGump(new AuctionNoticeGump(gump));
		}

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
