//
//	Auction version 2.1, by Xanthos and Arya
//
//  Based on original ideas and code by Arya
//

using Server;
using Server.Gumps;
using Server.Network;
using Xanthos.Utilities;
using static Arya.Auction.AuctionMessages;

namespace Arya.Auction
{
	/// <summary>
	///     The main gump for the auction house
	/// </summary>
	public class AuctionGump : Gump
	{
		public AuctionGump(Mobile user) : base(50, 50)
		{
			user.CloseGump(typeof(AuctionGump));
			MakeGump();
		}

		private void MakeGump()
		{
			Closable = true;
			Disposable = true;
			Dragable = true;
			Resizable = false;

			AddPage(0);
			AddImageTiled(49, 39, 402, 197, 3004);
			AddImageTiled(50, 40, 400, 195, 2624);
			AddAlphaRegion(50, 40, 400, 195);
			AddImage(165, 65, 10452);
			AddImage(-1, 20, 10400);
			AddImage(-1, 185, 10402);
			AddImage(35, 20, 10420);
			AddImage(421, 20, 10410);
			AddImage(410, 20, 10430);
			AddImageTiled(90, 32, 323, 16, 10254);
			AddImage(420, 185, 10412);

			AddLabel(160, 45, 151, AUCTION_SYSTEM_TITLE);

			// Create new auction: B1
			AddLabel(100, 130, Misc.kLabelHue, CREATE_AUCTION);
			AddButton(60, 130, 4005, 4006, 1, GumpButtonType.Reply, 0);

			// View all auctions: B2
			AddLabel(285, 130, Misc.kLabelHue, VIEW_ALL_AUCTIONS);
			AddButton(245, 130, 4005, 4006, 2, GumpButtonType.Reply, 0);

			// View your auctions: B3
			AddLabel(100, 165, Misc.kLabelHue, VIEW_YOUR_AUCTIONS);
			AddButton(60, 165, 4005, 4006, 3, GumpButtonType.Reply, 0);

			// View your bids: B4
			AddLabel(285, 165, Misc.kLabelHue, VIEW_YOUR_BIDS);
			AddButton(245, 165, 4005, 4006, 4, GumpButtonType.Reply, 0);

			// View pendencies: B5
			AddButton(60, 200, 4005, 4006, 5, GumpButtonType.Reply, 0);
			AddLabel(100, 200, Misc.kLabelHue, VIEW_PENDENCIES);

			// Exit: B0
			AddLabel(285, 205, Misc.kLabelHue, EXIT);
			AddButton(245, 200, 4017, 4018, 0, GumpButtonType.Reply, 0);
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
					break;

				case 1: // Create auction
					AuctionSystem.AuctionRequest(sender.Mobile);
					break;

				case 2: // View all auctions
					sender.Mobile.SendGump(new AuctionListing(sender.Mobile, AuctionSystem.Auctions, true, true));
					break;

				case 3: // View your auctions

					sender.Mobile.SendGump(new AuctionListing(sender.Mobile, AuctionSystem.GetAuctions(sender.Mobile),
						true, true));
					break;

				case 4: // View your bids

					sender.Mobile.SendGump(new AuctionListing(sender.Mobile, AuctionSystem.GetBids(sender.Mobile), true,
						true));
					break;

				case 5: // View pendencies

					sender.Mobile.SendGump(new AuctionListing(sender.Mobile, AuctionSystem.GetPendencies(sender.Mobile),
						true, true));
					break;
			}
		}
	}
}
