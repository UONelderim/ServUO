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
	public class AuctionListing : Gump
	{
		private readonly bool m_EnableSearch;
		private int m_Page;
		private readonly List<AuctionItem> m_List;
		private readonly bool m_ReturnToAuction;

		public AuctionListing(Mobile m, List<AuctionItem> items, bool searchEnabled, bool returnToAuction, int page) : base(50,
			50)
		{
			m.CloseGump(typeof(AuctionListing));
			m_EnableSearch = searchEnabled;
			m_Page = page;
			m_List = items.ToList();
			m_ReturnToAuction = returnToAuction;
			MakeGump();
		}

		public AuctionListing(Mobile m, List<AuctionItem> items, bool searchEnabled, bool returnToAuction) : this(m, items,
			searchEnabled, returnToAuction, 0)
		{
		}

		private void MakeGump()
		{
			Closable = true;
			Disposable = true;
			Dragable = true;
			Resizable = false;

			AddPage(0);

			AddImageTiled(49, 39, 402, 352, 3004);
			AddImageTiled(50, 40, 400, 350, 2624);
			AddAlphaRegion(50, 40, 400, 350);
			AddImage(165, 65, 10452);
			AddImage(0, 20, 10400);
			AddImage(0, 330, 10402);
			AddImage(35, 20, 10420);
			AddImage(421, 20, 10410);
			AddImage(410, 20, 10430);
			AddImageTiled(90, 32, 323, 16, 10254);
			AddLabel(160, 45, Misc.kGreenHue, AUCTION_SYSTEM_TITLE);
			AddImage(420, 330, 10412);
			AddImage(420, 175, 10411);
			AddImage(0, 175, 10401);

			if (m_EnableSearch)
			{
				AddLabel(305, 120, Misc.kLabelHue, SEARCH);
				AddButton(270, 120, 4005, 4006, 1, GumpButtonType.Reply, 0);
			}

			AddLabel(395, 120, Misc.kLabelHue, SORT);
			AddButton(360, 120, 4005, 4006, 2, GumpButtonType.Reply, 0);

			while (m_Page * 10 >= m_List.Count)
				m_Page--;

			if (m_List.Count > 0)
			{
				// Display the page number
				AddLabel(360, 95, Misc.kRedHue, String.Format(PAGE_FMT, m_Page + 1, (m_List.Count - 1) / 10 + 1));
				AddLabel(70, 120, Misc.kRedHue, String.Format(DISPLAY_COUNT_FMT, m_List.Count));
			}
			else
				AddLabel(70, 120, Misc.kRedHue, DISPLAY_EMPTY);

			// Display items: BUTTONS 10 + i

			int lower = m_Page * 10;

			if (m_List.Count > 0)
			{
				for (int i = 0; i < 10 && (m_Page * 10 + i) < m_List.Count; i++)
				{
					AuctionItem item = m_List[m_Page * 10 + i] as AuctionItem;

					AddButton(115, 153 + i * 20, 5601, 5605, 10 + i, GumpButtonType.Reply, 0);
					AddLabelCropped(140, 150 + i * 20, 260, 20, Misc.kLabelHue, item.ItemName);
				}
			}

			// Next page: BUTTON 3
			if ((m_Page + 1) * 10 < m_List.Count)
			{
				AddLabel(355, 360, Misc.kLabelHue, NEXT_PAGE);
				AddButton(315, 360, 4005, 4006, 3, GumpButtonType.Reply, 0);
			}

			// Previous page: BUTTON 4
			if (m_Page > 0)
			{
				AddLabel(180, 360, Misc.kLabelHue, PREVIOUS_PAGE);
				AddButton(280, 360, 4014, 4015, 4, GumpButtonType.Reply, 0);
			}

			// Close: BUTTON 0
			AddLabel(115, 360, Misc.kLabelHue, CLOSE);
			AddButton(75, 360, 4017, 4018, 0, GumpButtonType.Reply, 0);
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
				case 0: // Exit
					if (m_ReturnToAuction)
						sender.Mobile.SendGump(new AuctionGump(sender.Mobile));
					else
						sender.Mobile.SendGump(new MyAuctionGump(sender.Mobile, null));
					break;

				case 1: // Search
					sender.Mobile.SendGump(new AuctionSearchGump(sender.Mobile, m_List, m_ReturnToAuction));
					break;

				case 2: // Sort
					sender.Mobile.SendGump(
						new AuctionSortGump(sender.Mobile, m_List, m_ReturnToAuction, m_EnableSearch));
					break;

				case 3: // Next page
					sender.Mobile.SendGump(new AuctionListing(sender.Mobile, m_List, m_EnableSearch, m_ReturnToAuction,
						m_Page + 1));
					break;

				case 4: // Previous page
					sender.Mobile.SendGump(new AuctionListing(sender.Mobile, m_List, m_EnableSearch, m_ReturnToAuction,
						m_Page - 1));
					break;

				default:
					int index = m_Page * 10 + info.ButtonID - 10;

					if (index < 0 || index >= m_List.Count)
					{
						// Apparently in some cases this can go out of bounds, investigating.

						sender.Mobile.SendMessage(AuctionConfig.MessageHue, ERROR);

						if (m_ReturnToAuction)
							sender.Mobile.SendGump(new AuctionGump(sender.Mobile));
						else
							sender.Mobile.SendGump(new MyAuctionGump(sender.Mobile, null));

						return;
					}

					AuctionItem item = m_List[index] as AuctionItem;

					if (item != null)
					{
						if ((!item.Expired || item.Pending) && (AuctionSystem.Auctions.Contains(item) ||
						                                        AuctionSystem.Pending.Contains(item)))
						{
							sender.Mobile.SendGump(new AuctionViewGump(sender.Mobile, item, ViewCallback));
						}
						else
						{
							sender.Mobile.SendMessage(AuctionConfig.MessageHue, ITEM_EXPIRED);
							sender.Mobile.SendGump(new AuctionListing(sender.Mobile, m_List, m_EnableSearch,
								m_ReturnToAuction, m_Page));
						}
					}

					break;
			}
		}

		private void ViewCallback(Mobile user)
		{
			user.SendGump(new AuctionListing(user, m_List, m_EnableSearch, m_ReturnToAuction, m_Page));
		}
	}
}
