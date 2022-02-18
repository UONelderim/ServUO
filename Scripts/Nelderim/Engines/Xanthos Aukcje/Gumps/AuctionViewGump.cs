#region AuthorHeader

//
//	Auction version 2.1, by Xanthos and Arya
//
//  Based on original ideas and code by Arya
//

#endregion AuthorHeader

#region References

using System;
using System.Globalization;
using Server;
using Server.Gumps;
using Server.Network;
using Xanthos.Utilities;

#endregion

namespace Arya.Auction
{
	/// <summary>
	///     This gump displays the general information about an auction
	/// </summary>
	public class AuctionViewGump : Gump
	{
		private const int kHueExampleID = 7107;
		private const int kBeigeBorderOuter = 2524;
		private const int kBeigeBorderInner = 2624;

		private readonly Mobile m_User;
		private readonly AuctionItem m_Auction;
		private readonly int m_Page;
		private readonly AuctionGumpCallback m_Callback;

		public AuctionViewGump(Mobile user, AuctionItem auction) : this(user, auction, null)
		{
		}

		public AuctionViewGump(Mobile user, AuctionItem auction, AuctionGumpCallback callback) : this(user, auction,
			callback, 0)
		{
		}

		public AuctionViewGump(Mobile user, AuctionItem auction, AuctionGumpCallback callback, int page) : base(50, 50)
		{
			m_Page = page;
			m_User = user;
			m_Auction = auction;
			m_Callback = callback;

			MakeGump();
		}

		/// <summary>
		///     Gets the item hue
		/// </summary>
		/// <param name="item">The item to get the hue of</param>
		/// <returns>A positive hue value</returns>
		private int GetItemHue(Item item)
		{
			if (null == item)
				return 0;

			int hue = item.Hue == 1 ? AuctionConfig.BlackHue : item.Hue;

			hue &= 0x7FFF; // Some hues are | 0x8000 for some reason, but it leads to the same hue

			// Validate in case the hue was shifted by some other value

			return (hue < 0 || hue >= 3000) ? 0 : hue;
		}

		private void MakeGump()
		{
			AuctionItem.ItemInfo item = m_Auction[m_Page];

			if (item == null)
				return;

			int itemHue = GetItemHue(item.Item);

			Closable = true;
			Disposable = true;
			Dragable = true;
			Resizable = false;

			AddPage(0);

			// The page and background
			AddBackground(0, 0, 502, 370, 9270);
			AddImageTiled(4, 4, 492, 362, kBeigeBorderOuter);
			AddImageTiled(5, 5, 490, 360, kBeigeBorderInner);
			AddAlphaRegion(5, 5, 490, 360);

			//
			// The item display area
			//
			AddImageTiled(4, 4, 156, 170, kBeigeBorderOuter);
			AddImageTiled(5, 5, 154, 168, kBeigeBorderInner);
			AddAlphaRegion(5, 5, 154, 168);

			// Item image goes here
			if (item.Item != null)
			{
				NewAuctionGump.AddItemCentered(5, 5, 155, 140, item.Item.ItemID, item.Item.Hue, this);
				//AddItemProperty( item.Item.Serial);
			}

			// Hue preview image goes here if the item has a hue
			if (item.Item != null && 0 != itemHue)
			{
				AddImageTiled(30, 140, 107, 24, 3004);
				AddImageTiled(31, 141, 105, 22, kBeigeBorderInner);
				AddAlphaRegion(31, 141, 105, 22);
				AddLabel(37, 142, Misc.kLabelHue, AuctionSystem.ST[82]);
				AddItem(90, 141, kHueExampleID, itemHue);
			}

			//
			// The Auction info area
			//
			AddImageTiled(4, 169, 156, 196, kBeigeBorderOuter);
			AddImageTiled(5, 170, 154, 195, kBeigeBorderInner);
			AddAlphaRegion(5, 170, 154, 195);

			// Reserve and bids
			AddLabel(10, 175, Misc.kLabelHue, AuctionSystem.ST[68]);
			AddLabel(45, 190, Misc.kGreenHue, m_Auction.MinBid.ToString("#,0"));

			AddLabel(10, 280, Misc.kLabelHue, AuctionSystem.ST[69]);
			AddLabel(45, 295, m_Auction.ReserveMet ? Misc.kGreenHue : Misc.kRedHue,
				m_Auction.ReserveMet ? "Met" : "Not Met");

			AddLabel(10, 210, Misc.kLabelHue, AuctionSystem.ST[70]);

			if (m_Auction.HasBids)
				AddLabel(45, 225, m_Auction.ReserveMet ? Misc.kGreenHue : Misc.kRedHue,
					m_Auction.HighestBid.Amount.ToString("#,0"));
			else
				AddLabel(45, 225, Misc.kRedHue, AuctionSystem.ST[71]);

			// Time remaining
			string timeleft = null;

			AddLabel(10, 245, Misc.kLabelHue, AuctionSystem.ST[56]);

			if (!m_Auction.Expired)
			{
				if (m_Auction.TimeLeft >= TimeSpan.FromDays(1))
					timeleft = String.Format(AuctionSystem.ST[73], m_Auction.TimeLeft.Days, m_Auction.TimeLeft.Hours);
				else if (m_Auction.TimeLeft >= TimeSpan.FromMinutes(60))
					timeleft = String.Format(AuctionSystem.ST[74], m_Auction.TimeLeft.Hours);
				else if (m_Auction.TimeLeft >= TimeSpan.FromSeconds(60))
					timeleft = String.Format(AuctionSystem.ST[75], m_Auction.TimeLeft.Minutes);
				else
					timeleft = String.Format(AuctionSystem.ST[76], m_Auction.TimeLeft.Seconds);
			}
			else if (m_Auction.Pending)
			{
				timeleft = AuctionSystem.ST[77];
			}
			else
			{
				timeleft = AuctionSystem.ST[78];
			}

			AddLabel(45, 260, Misc.kGreenHue, timeleft);

			// Bidding
			if (m_Auction.CanBid(m_User) && !m_Auction.Expired)
			{
				AddLabel(10, 318, Misc.kLabelHue, AuctionSystem.ST[79]);
				AddImageTiled(9, 338, 112, 22, kBeigeBorderOuter);
				AddImageTiled(10, 339, 110, 20, kBeigeBorderInner);
				AddAlphaRegion(10, 339, 110, 20);

				// Bid text: 0
				AddTextEntry(10, 339, 110, 20, Misc.kGreenHue, 0, @"");

				// Bid button: 4
				AddButton(125, 338, 4011, 4012, 4, GumpButtonType.Reply, 0);
			}
			else if (m_Auction.IsOwner(m_User))
			{
				// View bids: button 5
				AddLabel(10, 338, Misc.kLabelHue, AuctionSystem.ST[80]);
				AddButton(125, 338, 4011, 4012, 5, GumpButtonType.Reply, 0);
			}

			//
			// Item properties area
			//
			AddImageTiled(169, 29, 317, 142, kBeigeBorderOuter);
			AddImageTiled(170, 30, 315, 140, kBeigeBorderInner);
			AddAlphaRegion(170, 30, 315, 140);

			// If it is a container make room for the arrows to navigate to each of the items
			if (m_Auction.ItemCount > 1)
			{
				AddLabel(170, 10, Misc.kGreenHue, String.Format(AuctionSystem.ST[231], m_Auction.ItemName));

				AddImageTiled(169, 29, 317, 27, kBeigeBorderOuter);
				AddImageTiled(170, 30, 315, 25, kBeigeBorderInner);
				AddAlphaRegion(170, 30, 315, 25);
				AddLabel(185, 35, Misc.kGreenHue, String.Format(AuctionSystem.ST[67], m_Page + 1, m_Auction.ItemCount));

				// Prev Item button: 1
				if (m_Page > 0)
				{
					AddButton(415, 31, 4014, 4015, 1, GumpButtonType.Reply, 0);
				}

				// Next Item button: 2
				if (m_Page < m_Auction.ItemCount - 1)
				{
					AddButton(450, 31, 4005, 4006, 2, GumpButtonType.Reply, 0);
				}

				//AddHtml( 173, 56, 312, 114, m_Auction[ m_Page ].Properties, (bool)false, (bool)true );
				AddHtml(173, 56, 312, 114, "Hover your mouse over the item to the left to see this item's properties.",
					false, true);
			}
			else
			{
				AddLabel(170, 10, Misc.kGreenHue, m_Auction.ItemName);
				//AddHtml( 173, 30, 312, 140, m_Auction[ m_Page ].Properties, (bool)false, (bool)true );
				AddHtml(173, 30, 312, 140, "Hover your mouse over the item to the left to see this item's properties.",
					false, true);
			}

			//
			// Owner description area
			//
			AddImageTiled(169, 194, 317, 112, kBeigeBorderOuter);
			AddLabel(170, 175, Misc.kLabelHue, AuctionSystem.ST[81]);
			AddImageTiled(170, 195, 315, 110, kBeigeBorderInner);
			AddAlphaRegion(170, 195, 315, 110);
			AddHtml(173, 195, 312, 110, String.Format("<basefont color=#FFFFFF>{0}", m_Auction.Description), false,
				true);

			// Web link button: 3
			if (m_Auction.WebLink != null && m_Auction.WebLink.Length > 0)
			{
				AddLabel(350, 175, Misc.kLabelHue, AuctionSystem.ST[72]);
				AddButton(415, 177, 5601, 5605, 3, GumpButtonType.Reply, 0);
			}

			//
			// Auction controls
			//

			// Buy now : Button 8
			if (m_Auction.AllowBuyNow && m_Auction.CanBid(m_User) && !m_Auction.Expired)
			{
				AddButton(170, 310, 4029, 4030, 8, GumpButtonType.Reply, 0);
				AddLabel(205, 312, Misc.kGreenHue,
					String.Format(AuctionSystem.ST[210], m_Auction.BuyNow.ToString("#,0")));
			}

			// Button 6 : Admin Auction Panel
			if (m_User.AccessLevel >= AuctionConfig.AuctionAdminAcessLevel)
			{
				AddButton(170, 338, 4011, 4012, 6, GumpButtonType.Reply, 0);
				AddLabel(205, 340, Misc.kLabelHue, AuctionSystem.ST[227]);
			}

			// Close button: 0
			AddButton(455, 338, 4017, 4018, 0, GumpButtonType.Reply, 0);
			AddLabel(415, 340, Misc.kLabelHue, AuctionSystem.ST[7]);
		}

		public override void OnResponse(NetState sender, RelayInfo info)
		{
			if (!AuctionSystem.Running)
			{
				sender.Mobile.SendMessage(AuctionConfig.MessageHue, AuctionSystem.ST[15]);
				return;
			}

			if (!AuctionSystem.Auctions.Contains(m_Auction))
			{
				sender.Mobile.SendMessage(AuctionConfig.MessageHue, AuctionSystem.ST[207]);

				if (m_Callback != null)
				{
					try { m_Callback.DynamicInvoke(m_User); }
					catch { }
				}

				return;
			}

			switch (info.ButtonID)
			{
				case 0: // Close

					if (m_Callback != null)
					{
						try { m_Callback.DynamicInvoke(m_User); }
						catch { }
					}

					break;

				case 1: // Prev item

					m_User.SendGump(new AuctionViewGump(m_User, m_Auction, m_Callback, m_Page - 1));
					break;

				case 2: // Next item

					m_User.SendGump(new AuctionViewGump(m_User, m_Auction, m_Callback, m_Page + 1));
					break;

				case 3: // Web link

					m_User.SendGump(new AuctionViewGump(m_User, m_Auction, m_Callback, m_Page));
					m_Auction.SendLinkTo(m_User);
					break;

				case 4: // Bid

					uint bid = 0;

					try { bid = UInt32.Parse(info.TextEntries[0].Text, NumberStyles.AllowThousands); }
					catch { }

					if (m_Auction.Expired)
					{
						m_User.SendMessage(AuctionConfig.MessageHue, AuctionSystem.ST[84]);
					}
					else if (bid == 0)
					{
						m_User.SendMessage(AuctionConfig.MessageHue, AuctionSystem.ST[85]);
					}
					else
					{
						if (m_Auction.AllowBuyNow && bid >= m_Auction.BuyNow)
						{
							// Do buy now instead
							goto case 8;
						}

						m_Auction.PlaceBid(m_User, (int)bid);
					}

					m_User.SendGump(new AuctionViewGump(m_User, m_Auction, m_Callback, m_Page));
					break;

				case 5: // View bids

					m_User.SendGump(new BidViewGump(m_User, m_Auction.Bids, BidViewCallback));
					break;

				case 6: // Staff Panel

					m_User.SendGump(new AuctionControlGump(m_User, m_Auction, this));
					break;

				case 8: // Buy Now

					if (m_Auction.DoBuyNow(sender.Mobile))
					{
						goto case 0; // Close the gump
					}
					else
					{
						sender.Mobile.SendGump(new AuctionViewGump(sender.Mobile, m_Auction, m_Callback, m_Page));
					}

					break;
			}
		}

		private void BidViewCallback(Mobile m)
		{
			m.SendGump(new AuctionViewGump(m, m_Auction, m_Callback, m_Page));
		}
	}
}
