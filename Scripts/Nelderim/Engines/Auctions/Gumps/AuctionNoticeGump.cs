using Server.Gumps;
using Server.Network;
using static Arya.Auction.AuctionMessages;

namespace Arya.Auction
{
	public class AuctionNoticeGump : Gump
	{
		private readonly AuctionMessageGump m_Message;

		public AuctionNoticeGump(AuctionMessageGump msg) : base(25, 25)
		{
			m_Message = msg;
			MakeGump();
		}

		private void MakeGump()
		{
			Closable = false;
			Disposable = true;
			Dragable = true;
			Resizable = false;

			AddPage(0);
			AddImageTiled(0, 0, 75, 75, 3004);
			AddImageTiled(1, 1, 73, 73, 2624);
			AddAlphaRegion(1, 1, 73, 73);
			AddButton(7, 7, 5573, 5574, 1, GumpButtonType.Reply, 0);
		}

		public override void OnResponse(NetState sender, RelayInfo info)
		{
			if (!AuctionSystem.Running)
			{
				sender.Mobile.SendMessage(AuctionConfig.MessageHue, AUCTION_SYSTEM_DISABLED);
				return;
			}

			if (info.ButtonID == 1)
			{
				if (m_Message != null)
				{
					m_Message.SendTo(sender.Mobile);
				}
			}
		}
	}
}
