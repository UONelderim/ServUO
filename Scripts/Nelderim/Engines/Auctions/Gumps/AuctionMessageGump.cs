//
//	Auction version 2.1, by Xanthos and Arya
//
//  Based on original ideas and code by Arya
//

using System;
using Server;
using Server.Gumps;
using Server.Network;
using Xanthos.Utilities;
using static Arya.Auction.AuctionMessages;

namespace Arya.Auction
{
	/// <summary>
	///     Summary description for AuctionMessageGump.
	/// </summary>
	public class AuctionMessageGump : Gump
	{
		/// <summary>
		///     Sets the message displayed by the gump in the details area
		/// </summary>
		public string Message
		{
			set
			{
				m_HtmlMessage = String.Format("<basefont color=#111111>{0}", value);
			}
		}

		/// <summary>
		///     Sets the text associated with the OK button
		/// </summary>
		public string OkText
		{
			set { m_OkText = value; }
		}

		/// <summary>
		///     Sets the text associated with the Cancel button
		/// </summary>
		public string CancelText
		{
			set { m_CancelText = value; }
		}

		/// <summary>
		///     Specifies whether this gump carries just information. If true, the gump will only have an OK button.
		///     If false the gump will have both OK and Cancel buttons.
		/// </summary>
		public bool InformationMode
		{
			set { m_InformationMode = value; }
		}

		/// <summary>
		///     Gets or sets the auction referenced by this message
		/// </summary>
		public AuctionItem Auction
		{
			get
			{
				return AuctionSystem.Find(m_ID);
			}
			set
			{
				m_ID = value.ID;
			}
		}

		/// <summary>
		///     Specifies if this message is targeted at the auction owner, rather than at bidder
		/// </summary>
		public bool OwnerTarget
		{
			set { m_OwnerTarget = value; }
		}

		/// <summary>
		///     Specifies whether this message should validate the answer with the auction
		/// </summary>
		public bool VerifyAuction
		{
			set { m_VerifyAuction = value; }
		}

		/// <summary>
		///     Specifies whether to show the expiration notice at the bottom of the message
		/// </summary>
		public bool ShowExpiration
		{
			set { m_ShowExpiration = value; }
		}

		private Guid m_ID;
		private string m_HtmlMessage;
		private string m_OkText;
		private string m_CancelText;
		private bool m_InformationMode;
		private bool m_OwnerTarget;
		private bool m_VerifyAuction;
		private bool m_ShowExpiration = true;

		public AuctionMessageGump(AuctionItem auction, bool informationMode, bool ownerTarget, bool verifyAuction) :
			base(50, 50)
		{
			Auction = auction;
			m_InformationMode = informationMode;
			m_OwnerTarget = ownerTarget;
			m_VerifyAuction = verifyAuction;
		}

		public void SendTo(Mobile m)
		{
			MakeGump();
			m.SendGump(this);
		}

		private void MakeGump()
		{
			Closable = false;
			Disposable = true;
			Dragable = false;
			Resizable = false;

			AddPage(0);
			AddImage(0, 0, 9380);
			AddImageTiled(114, 0, 335, 140, 9381);
			AddImage(449, 0, 9382);
			AddImageTiled(0, 140, 115, 153, 9383);
			AddImageTiled(114, 140, 336, 217, 9384);
			AddImageTiled(449, 140, 102, 217, 9385);
			AddImage(0, 290, 9386);
			AddImageTiled(114, 290, 353, 140, 9387);
			AddImage(450, 290, 9388);
			AddLabel(200, 38, 76, MESSAGING_SYSTEM_TITLE);
			AddImageTiled(65, 65, 438, 11, 2091);

			AddLabel(65, 85, 0, AUCTION);

			AuctionItem auction = Auction;

			// BUTTON 0: View auction details
			if (auction != null)
			{
				AddLabel(125, 85, Misc.kRedHue, auction.ItemName);

				AddButton(65, 112, 9762, 9763, 0, GumpButtonType.Reply, 0);
				AddLabel(85, 110, Misc.kLabelHue, VIEW_DETAILS);
			}
			else
			{
				AddLabel(125, 85, Misc.kRedHue, NOT_AVAILABLE);
			}

			AddHtml(75, 170, 413, 120, m_HtmlMessage, true, false);
			AddLabel(75, 150, Misc.kLabelHue, MESSAGE_DETAILS);

			// BUTTON 1: OK
			// BUTTON 2: CANCEL

			if (m_InformationMode)
			{
				// Information mode: show only OK button at bottom with the OK text
				AddButton(45, 345, 1147, 1149, 1, GumpButtonType.Reply, 0);
				AddLabel(125, 355, Misc.kLabelHue, m_OkText);
			}
			else
			{
				AddButton(45, 300, 1147, 1149, 1, GumpButtonType.Reply, 0);
				AddLabel(125, 310, Misc.kLabelHue, m_OkText);
				AddButton(45, 345, 1144, 1146, 2, GumpButtonType.Reply, 0);
				AddLabel(125, 355, Misc.kLabelHue, m_CancelText);
			}

			if (m_ShowExpiration && auction != null)
			{
				AddLabel(55, 405, Misc.kRedHue,
					String.Format(PENDING_TIME_LEFT,
						auction.PendingTimeLeft.Days, auction.PendingTimeLeft.Hours));
			}
		}

		private void ResendMessage(Mobile m)
		{
			m.SendGump(this);
		}

		public override void OnResponse(NetState sender, RelayInfo info)
		{
			if (!AuctionSystem.Running)
			{
				sender.Mobile.SendMessage(AuctionConfig.MessageHue, AUCTION_SYSTEM_DISABLED);
				return;
			}

			AuctionItem auction = Auction;

			if (auction == null)
			{
				sender.Mobile.SendMessage(AuctionConfig.MessageHue, AUCTION_NO_LONGER_EXISTS);
				return;
			}

			switch (info.ButtonID)
			{
				case 0: // View auction details

					if (m_InformationMode && !m_VerifyAuction)
						// This is an outbid message, no need to return after visiting the auction
						sender.Mobile.SendGump(new AuctionViewGump(sender.Mobile, Auction));
					else
						sender.Mobile.SendGump(new AuctionViewGump(sender.Mobile, Auction, ResendMessage));
					break;

				case 1: // OK

					if (m_InformationMode)
					{
						if (m_VerifyAuction)
						{
							auction.ConfirmInformationMessage(m_OwnerTarget);
						}
					}
					else
					{
						auction.ConfirmResponseMessage(m_OwnerTarget, true);
					}

					break;

				case 2: // Cancel

					auction.ConfirmResponseMessage(m_OwnerTarget, false);
					break;
			}
		}
	}
}
