using System;
using Server;
using Server.Gumps;
using Server.Network;
using Xanthos.Utilities;

namespace Arya.Auction
{
	public class AuctionAdminGump : Gump
	{
		public AuctionAdminGump(Mobile m) : base(100, 100)
		{
			m.CloseGump(typeof(AuctionAdminGump));
			MakeGump();
		}

		private void MakeGump()
		{
			Closable = true;
			Disposable = true;
			Dragable = true;
			Resizable = false;

			AddPage(0);
			AddBackground(0, 0, 270, 270, 9300);
			AddAlphaRegion(0, 0, 270, 270);
			AddLabel(36, 5, Misc.kRedHue, @"Auction System Administration");
			AddImageTiled(16, 30, 238, 1, 9274);

			AddLabel(15, 65, Misc.kLabelHue,
				String.Format(@"Deadline: {0} at {1}", AuctionScheduler.Deadline.ToShortDateString(),
					AuctionScheduler.Deadline.ToShortTimeString()));
			AddLabel(15, 40, Misc.kGreenHue,
				String.Format("{0} Auctions, {1} Pending", AuctionSystem.Auctions.Count, AuctionSystem.Pending.Count));

			AddButton(15, 100, 4005, 4006, 1, GumpButtonType.Reply, 0);
			AddLabel(55, 100, Misc.kLabelHue, "Force Verification");

			AddButton(15, 130, 4005, 4006, 2, GumpButtonType.Reply, 0);
			AddLabel(55, 130, Misc.kLabelHue, "Profile the System");

			AddButton(15, 160, 4005, 4006, 3, GumpButtonType.Reply, 0);
			AddLabel(55, 160, Misc.kLabelHue, "Temporarily Shut Down");

			AddButton(15, 190, 4005, 4006, 4, GumpButtonType.Reply, 0);
			AddLabel(55, 190, Misc.kLabelHue, "Permanently Shut Down");

			AddButton(15, 230, 4023, 4024, 0, GumpButtonType.Reply, 0);
			AddLabel(55, 230, Misc.kLabelHue, "Exit");
		}

		public override void OnResponse(NetState sender, RelayInfo info)
		{
			switch (info.ButtonID)
			{
				case 1: // Validate

					AuctionSystem.VerifyAuctions();
					AuctionSystem.VerifyPendencies();

					sender.Mobile.SendGump(new AuctionAdminGump(sender.Mobile));
					break;

				case 2: // Profile

					AuctionSystem.ProfileAuctions();

					sender.Mobile.SendGump(new AuctionAdminGump(sender.Mobile));
					break;

				case 3: // Disable

					AuctionSystem.Disable();
					sender.Mobile.SendMessage(AuctionConfig.MessageHue,
						"The system has been stopped. It will be restored with the next reboot.");
					break;

				case 4: // Delete

					sender.Mobile.SendGump(new DeleteAuctionGump(sender.Mobile));
					break;
			}
		}
	}
}
