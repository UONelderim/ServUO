#region AuthorHeader

//
//	Auction version 2.1, by Xanthos and Arya
//
//  Based on original ideas and code by Arya
//

#endregion AuthorHeader

#region References

using Server;
using Server.Gumps;
using Server.Network;
using Xanthos.Utilities;

#endregion

namespace Arya.Auction
{
	/// <summary>
	///     Summary description for MyAuctionGump.
	/// </summary>
	public class MyAuctionGump : Gump
	{
		private readonly AuctionGumpCallback m_Callback;

		public MyAuctionGump(Mobile m, AuctionGumpCallback callback) : base(50, 50)
		{
			m_Callback = callback;
			m.CloseGump(typeof(MyAuctionGump));
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
			AddImage(0, 20, 10400);
			AddImage(0, 185, 10402);
			AddImage(35, 20, 10420);
			AddImage(421, 20, 10410);
			AddImage(410, 20, 10430);
			AddImageTiled(90, 32, 323, 16, 10254);
			AddLabel(160, 45, Misc.kGreenHue, AuctionSystem.ST[8]);
			AddLabel(100, 130, Misc.kLabelHue, AuctionSystem.ST[11]);
			AddLabel(285, 130, Misc.kLabelHue, AuctionSystem.ST[12]);
			AddLabel(100, 165, Misc.kLabelHue, AuctionSystem.ST[13]);

			AddButton(60, 130, 4005, 4006, 1, GumpButtonType.Reply, 0);
			AddButton(245, 130, 4005, 4006, 2, GumpButtonType.Reply, 0);
			AddButton(60, 165, 4005, 4006, 3, GumpButtonType.Reply, 0);
			AddButton(60, 205, 4017, 4018, 0, GumpButtonType.Reply, 0);
			AddLabel(100, 205, Misc.kLabelHue, AuctionSystem.ST[14]);
			AddImage(420, 185, 10412);
		}

		public override void OnResponse(NetState sender, RelayInfo info)
		{
			if (!AuctionSystem.Running)
			{
				sender.Mobile.SendMessage(AuctionConfig.MessageHue, AuctionSystem.ST[15]);
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

				case 1: // View your auctions

					sender.Mobile.SendGump(new AuctionListing(sender.Mobile, AuctionSystem.GetAuctions(sender.Mobile),
						false, false));
					break;

				case 2: // View your bids

					sender.Mobile.SendGump(new AuctionListing(sender.Mobile, AuctionSystem.GetBids(sender.Mobile),
						false, false));
					break;

				case 3: // View your pendencies

					sender.Mobile.SendGump(new AuctionListing(sender.Mobile, AuctionSystem.GetPendencies(sender.Mobile),
						false, false));
					break;
			}
		}
	}
}
