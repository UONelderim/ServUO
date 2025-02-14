using System;
using System.Collections.Generic;
using System.Linq;
using Server;
using Server.Gumps;
using Server.Network;
using Xanthos.Utilities;
using static Arya.Auction.AuctionMessages;

namespace Arya.Auction
{
	public class BidViewGump : Gump
	{
		private readonly AuctionGumpCallback m_Callback;
		private readonly int m_Page;
		private readonly List<Bid> m_Bids;

		public BidViewGump(Mobile m, List<Bid> bids, AuctionGumpCallback callback) : this(m, bids, callback, 0)
		{
		}

		public BidViewGump(Mobile m, List<Bid> bids, AuctionGumpCallback callback, int page) : base(100, 100)
		{
			m.CloseGump(typeof(BidViewGump));
			m_Callback = callback;
			m_Page = page;
			m_Bids = bids.ToList();
			MakeGump();
		}

		private void MakeGump()
		{
			Closable = true;
			Disposable = true;
			Dragable = true;
			Resizable = false;

			int numOfPages = (m_Bids.Count - 1) / 10 + 1;

			if (m_Bids.Count == 0)
				numOfPages = 0;

			AddPage(0);
			AddImageTiled(0, 0, 297, 282, 5174);
			AddImageTiled(1, 1, 295, 280, 2702);
			AddAlphaRegion(1, 1, 295, 280);
			AddLabel(12, 5, Misc.kRedHue, BID_HISTORY);

			AddLabel(160, 5, Misc.kGreenHue, String.Format(PAGE_FMT, m_Page + 1, numOfPages));
			AddImageTiled(10, 30, 277, 221, 5174);
			AddImageTiled(11, 31, 39, 19, 9274);
			AddAlphaRegion(11, 31, 39, 19);
			AddImageTiled(51, 31, 104, 19, 9274);
			AddAlphaRegion(51, 31, 104, 19);
			AddLabel(55, 30, Misc.kGreenHue, WHO);
			AddImageTiled(156, 31, 129, 19, 9274);
			AddAlphaRegion(156, 31, 129, 19);
			AddLabel(160, 30, Misc.kGreenHue, AMOUNT);

			for (int i = 0; i < 10; i++)
			{
				AddImageTiled(11, 51 + i * 20, 39, 19, 9264);
				AddAlphaRegion(11, 51 + i * 20, 39, 19);
				AddImageTiled(51, 51 + i * 20, 104, 19, 9264);
				AddAlphaRegion(51, 51 + i * 20, 104, 19);
				AddImageTiled(156, 51 + i * 20, 129, 19, 9264);
				AddAlphaRegion(156, 51 + i * 20, 129, 19);

				if (m_Page * 10 + i < m_Bids.Count)
				{
					Bid bid = m_Bids[m_Page * 10 + i] as Bid;
					AddLabel(15, 50 + i * 20, Misc.kLabelHue, (m_Page * 10 + i + 1).ToString());
					AddLabelCropped(55, 50 + i * 20, 100, 19, Misc.kLabelHue,
						bid.Mobile != null ? bid.Mobile.Name : NA);
					AddLabel(160, 50 + i * 20, Misc.kLabelHue, bid.Amount.ToString("#,0"));
				}
			}

			AddButton(10, 255, 4011, 4012, 0, GumpButtonType.Reply, 0);
			AddLabel(48, 257, Misc.kLabelHue, RETURN_TO_AUCTION);

			// PREV PAGE: 1
			if (m_Page > 0)
			{
				AddButton(250, 8, 9706, 9707, 1, GumpButtonType.Reply, 0);
			}

			// NEXT PAGE: 2
			if (m_Page < numOfPages - 1)
			{
				AddButton(270, 8, 9702, 9703, 2, GumpButtonType.Reply, 0);
			}
		}

		public override void OnResponse(NetState sender, RelayInfo info)
		{
			if (!AuctionSystem.Running)
			{
				sender.Mobile.SendMessage(AuctionConfig.MessageHue, AUCTION_SYSTEM_DISABLED);
				return;
			}

			switch (info.ButtonID)
			{
				case 0:

					if (m_Callback != null)
					{
						try { m_Callback.DynamicInvoke(sender.Mobile); }
						catch { }
					}

					break;

				case 1:

					sender.Mobile.SendGump(new BidViewGump(sender.Mobile, m_Bids, m_Callback, m_Page - 1));
					break;

				case 2:

					sender.Mobile.SendGump(new BidViewGump(sender.Mobile, m_Bids, m_Callback, m_Page + 1));
					break;
			}
		}
	}
}
