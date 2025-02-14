//
//	Auction version 2.1, by Xanthos and Arya
//
//  Based on original ideas and code by Arya
//

using Server.Gumps;
using Server.Network;
using Xanthos.Utilities;

namespace Arya.Auction
{
	/// <summary>
	///     This gump is used to deliver a creature
	/// </summary>
	public class CreatureDeliveryGump : Gump
	{
		private readonly AuctionCheck m_Check;

		public CreatureDeliveryGump(AuctionCheck check) : base(100, 100)
		{
			m_Check = check;
			MakeGump();
		}

		private void MakeGump()
		{
			Closable = true;
			Disposable = true;
			Dragable = true;
			Resizable = false;

			AddPage(0);
			AddImage(0, 0, 2080);
			AddImageTiled(18, 37, 263, 310, 2081);
			AddImage(21, 347, 2083);
			AddLabel(75, 5, 210, AuctionSystem.ST[0]);
			AddLabel(90, 35, 210, AuctionSystem.ST[90]);

			AddImage(45, 60, 2091);
			AddImage(45, 100, 2091);

			AddLabelCropped(45, 75, 210, 20, Misc.kLabelHue, m_Check.ItemName);

			AddHtml(45, 115, 215, 100, m_Check.HtmlDetails, false, false);

			// Button 1 : Stable
			AddButton(50, 300, 5601, 5605, 1, GumpButtonType.Reply, 0);
			AddLabel(70, 298, Misc.kLabelHue, AuctionSystem.ST[91]);

			// Button 0: Close
			AddButton(50, 325, 5601, 5605, 0, GumpButtonType.Reply, 0);
			AddImage(230, 315, 9004);
			AddLabel(70, 323, Misc.kLabelHue, AuctionSystem.ST[7]);
			AddLabel(45, 220, Misc.kLabelHue, AuctionSystem.ST[92]);
			AddLabel(45, 240, Misc.kLabelHue, AuctionSystem.ST[93]);
			AddLabel(45, 255, Misc.kLabelHue, AuctionSystem.ST[94]);
			AddLabel(45, 275, Misc.kLabelHue, AuctionSystem.ST[95]);
		}

		public override void OnResponse(NetState sender, RelayInfo info)
		{
			if (info.ButtonID == 1)
			{
				MobileStatuette ms = m_Check.AuctionedItem as MobileStatuette;

				if (ms != null)
				{
					ms.Stable(sender.Mobile);
					m_Check.DeliveryComplete();
					m_Check.Delete();
				}
			}
		}
	}
}
