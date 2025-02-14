using System.Collections;
using Server;
using Server.Gumps;
using Server.Network;
using Xanthos.Utilities;
using static Arya.Auction.AuctionMessages;

namespace Arya.Auction
{
	public class AuctionSortGump : Gump
	{
		private readonly bool m_Search;
		private readonly bool m_ReturnToAuction;
		private readonly ArrayList m_List;

		public AuctionSortGump(Mobile m, ArrayList items, bool returnToAuction, bool search) : base(50, 50)
		{
			m.CloseGump(typeof(AuctionSortGump));

			m_List = items;
			m_ReturnToAuction = returnToAuction;
			m_Search = search;

			MakeGump();
		}

		private void MakeGump()
		{
			Closable = true;
			Disposable = true;
			Dragable = true;
			Resizable = false;

			AddPage(0);

			AddImageTiled(49, 34, 402, 312, 3004);
			AddImageTiled(50, 35, 400, 310, 2624);
			AddAlphaRegion(50, 35, 400, 310);

			AddImage(165, 65, 10452);
			AddImage(0, 20, 10400);
			AddImage(0, 280, 10402);
			AddImage(35, 20, 10420);
			AddImage(421, 20, 10410);
			AddImage(410, 20, 10430);
			AddImageTiled(90, 32, 323, 16, 10254);
			AddLabel(160, 45, Misc.kGreenHue, SORT_SYSTEM_TITLE);

			AddLabel(95, 125, Misc.kRedHue, ITEM_NAME);
			AddImage(75, 125, 2511);

			AddButton(110, 144, 9702, 9703, 1, GumpButtonType.Reply, 0);
			AddLabel(135, 141, Misc.kLabelHue, SORT_NAME_ASC);

			AddButton(110, 163, 9702, 9703, 2, GumpButtonType.Reply, 0);
			AddLabel(135, 160, Misc.kLabelHue, SORT_NAME_DESC);

			AddImage(420, 280, 10412);
			AddLabel(95, 185, Misc.kRedHue, SORT_DATE);
			AddImage(75, 185, 2511);

			AddButton(110, 204, 9702, 9703, 3, GumpButtonType.Reply, 0);
			AddLabel(135, 201, Misc.kLabelHue, SORT_DATE_ASC);

			AddButton(110, 223, 9702, 9703, 4, GumpButtonType.Reply, 0);
			AddLabel(135, 220, Misc.kLabelHue, SORT_DATE_DESC);

			AddLabel(95, 245, Misc.kRedHue, TIME_LEFT);
			AddImage(75, 245, 2511);

			AddButton(110, 264, 9702, 9703, 5, GumpButtonType.Reply, 0);
			AddLabel(135, 261, Misc.kLabelHue, SORT_TIME_LEFT_ASC);

			AddButton(110, 283, 9702, 9703, 6, GumpButtonType.Reply, 0);
			AddLabel(135, 280, Misc.kLabelHue, SORT_TIME_LEFT_DESC);

			AddLabel(290, 125, Misc.kRedHue, SORT_BID_COUNT);
			AddImage(270, 125, 2511);

			AddButton(305, 144, 9702, 9703, 7, GumpButtonType.Reply, 0);
			AddLabel(330, 141, Misc.kLabelHue, SORT_BID_COUNT_ASC);

			AddButton(305, 163, 9702, 9703, 8, GumpButtonType.Reply, 0);
			AddLabel(330, 160, Misc.kLabelHue, SORT_BID_COUNT_DESC);

			AddLabel(290, 185, Misc.kRedHue, SORT_MIN_BID);
			AddImage(270, 185, 2511);

			AddButton(305, 204, 9702, 9703, 9, GumpButtonType.Reply, 0);
			AddLabel(330, 201, Misc.kLabelHue, SORT_BID_ASC);

			AddButton(305, 223, 9702, 9703, 10, GumpButtonType.Reply, 0);
			AddLabel(330, 220, Misc.kLabelHue, SORT_BID_DESC);

			AddLabel(290, 245, Misc.kRedHue, SORT_MAX_BID);
			AddImage(270, 245, 2511);

			AddButton(305, 264, 9702, 9703, 11, GumpButtonType.Reply, 0);
			AddLabel(330, 261, Misc.kLabelHue, SORT_BID_ASC);

			AddButton(305, 283, 9702, 9703, 12, GumpButtonType.Reply, 0);
			AddLabel(330, 280, Misc.kLabelHue, SORT_BID_DESC);

			AddLabel(120, 315, Misc.kLabelHue, SORT_CANCEL);
			AddButton(80, 315, 4017, 4018, 0, GumpButtonType.Reply, 0);
		}

		public override void OnResponse(NetState sender, RelayInfo info)
		{
			if (!AuctionSystem.Running)
			{
				sender.Mobile.SendMessage(AuctionConfig.MessageHue, AUCTION_SYSTEM_DISABLED);
				return;
			}

			AuctionComparer cmp = null;

			switch (info.ButtonID)
			{
				case 1: // Name

					cmp = new AuctionComparer(AuctionSorting.Name, true);
					break;

				case 2:

					cmp = new AuctionComparer(AuctionSorting.Name, false);
					break;

				case 3:

					cmp = new AuctionComparer(AuctionSorting.Date, true);
					break;

				case 4:

					cmp = new AuctionComparer(AuctionSorting.Date, false);
					break;

				case 5:

					cmp = new AuctionComparer(AuctionSorting.TimeLeft, true);
					break;

				case 6:

					cmp = new AuctionComparer(AuctionSorting.TimeLeft, false);
					break;

				case 7:

					cmp = new AuctionComparer(AuctionSorting.Bids, true);
					break;

				case 8:

					cmp = new AuctionComparer(AuctionSorting.Bids, false);
					break;

				case 9:

					cmp = new AuctionComparer(AuctionSorting.MinimumBid, true);
					break;

				case 10:

					cmp = new AuctionComparer(AuctionSorting.MinimumBid, false);
					break;

				case 11:

					cmp = new AuctionComparer(AuctionSorting.HighestBid, true);
					break;

				case 12:

					cmp = new AuctionComparer(AuctionSorting.HighestBid, false);
					break;
			}

			if (cmp != null)
			{
				m_List.Sort(cmp);
			}

			sender.Mobile.SendGump(new AuctionListing(sender.Mobile, m_List, m_Search, m_ReturnToAuction));
		}
	}
}
