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
	///     Summary description for DeleteSystemGump.
	/// </summary>
	public class DeleteAuctionGump : Gump
	{
		public DeleteAuctionGump(Mobile m) : base(100, 100)
		{
			m.CloseGump(typeof(DeleteAuctionGump));
			MakeGump();
		}

		private void MakeGump()
		{
			Closable = true;
			Disposable = true;
			Dragable = true;
			Resizable = false;

			AddPage(0);
			AddImageTiled(0, 0, 350, 250, 5174);
			AddImageTiled(1, 1, 348, 248, 2702);
			AddAlphaRegion(1, 1, 348, 248);
			AddLabel(70, 15, Misc.kRedHue, TERMINATION_TITLE);
			AddHtml(30, 45, 285, 130, TERMINATION_WARNING, false, false);

			// Close: Button 1
			AddButton(30, 185, 4017, 4017, 1, GumpButtonType.Reply, 0);
			AddLabel(70, 185, Misc.kRedHue, TERMINATION_CONFIRM);
			AddButton(30, 215, 4014, 4015, 0, GumpButtonType.Reply, 0);
			AddLabel(70, 215, Misc.kGreenHue, TERMINATION_CANCEL);
		}

		public override void OnResponse(NetState sender, RelayInfo info)
		{
			if (info.ButtonID == 1)
			{
				AuctionSystem.ForceDelete(sender.Mobile);
			}
		}
	}
}
