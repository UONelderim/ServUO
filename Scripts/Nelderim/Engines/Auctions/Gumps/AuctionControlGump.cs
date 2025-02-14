//
//	Auction version 2.1, by Xanthos and Arya
//
//  Based on original ideas and code by Arya
//

using System;
using Server;
using Server.Accounting;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;
using Xanthos.Utilities;
using static Arya.Auction.AuctionMessages;

namespace Arya.Auction
{
	/// <summary>
	///     Staff control gump
	/// </summary>
	public class AuctionControlGump : Gump
	{
		readonly Mobile m_User;
		readonly AuctionItem m_Auction;

		public AuctionControlGump(Mobile user, AuctionItem auction, AuctionViewGump view) : base(50, 50)
		{
			m_User = user;
			m_Auction = auction;

			m_User.CloseGump(typeof(AuctionControlGump));

			view.X = 400;
			m_User.SendGump(view);

			MakeGump();
		}

		private void MakeGump()
		{
			Closable = true;
			Disposable = true;
			Dragable = true;
			Resizable = false;

			AddPage(0);
			AddBackground(0, 0, 300, 385, 9270);
			AddAlphaRegion(10, 10, 280, 365);

			AddLabel(99, 15, Misc.kGreenHue, ADMIN_TITLE);
			AddImageTiled(15, 35, 270, 1, 9304);
			AddImageTiled(30, 55, 1, 65, 9304);

			// Auction owner
			bool owner = m_Auction.Owner != null;
			AddLabel(15, 35, Misc.kRedHue,
				String.Format(ADMIN_OWNER_FMT, owner ? m_Auction.Owner.Name : NA));

			// Owner Props : 1
			if (owner)
			{
				AddButton(35, 58, 9702, 9703, 1, GumpButtonType.Reply, 0);
			}

			AddLabel(55, 55, Misc.kLabelHue, ADMIN_PROPERTIES);
			;

			// Owner Account: 2
			if (owner && m_Auction.Owner.Account != null && m_User.AccessLevel == AccessLevel.Administrator)
			{
				AddButton(35, 78, 9702, 9703, 2, GumpButtonType.Reply, 0);
			}

			AddLabel(55, 75, Misc.kLabelHue,
				String.Format(ADMIN_ACCOUNT_FMT,
					(owner && m_Auction.Owner.Account != null)
						? (m_Auction.Owner.Account as Account).Username
						: NA));

			bool online = m_Auction.Owner != null && m_Auction.Owner.NetState != null;

			// Client button : 3
			if (online)
			{
				AddButton(35, 98, 9702, 9703, 3, GumpButtonType.Reply, 0);
			}

			AddLabel(55, 95, online ? Misc.kGreenHue : Misc.kRedHue,
				online ? ADMIN_ONLINE : ADMIN_OFFLINE);

			AddImageTiled(15, 120, 270, 1, 9304);
			AddLabel(20, 125, Misc.kRedHue, ADMIN_AUCTIONED_ITEM);
			AddImageTiled(30, 145, 1, 65, 9304);

			// Item Properties : 4
			AddButton(35, 150, 9702, 9703, 4, GumpButtonType.Reply, 0);
			AddLabel(55, 147, Misc.kLabelHue, ADMIN_PROPERTIES);

			bool item = m_Auction.Item != null;

			// Get item : 5
			AddButton(35, 170, 9702, 9703, 5, GumpButtonType.Reply, 0);
			AddLabel(55, 167, Misc.kLabelHue, ADMIN_BRING);

			// Return item: 6
			AddButton(35, 190, 9702, 9703, 6, GumpButtonType.Reply, 0);
			AddLabel(55, 187, Misc.kLabelHue, ADMIN_PUT_BACK);

			AddImageTiled(15, 210, 270, 1, 9304);
			AddLabel(20, 215, Misc.kRedHue, ADMIN_AUCTION);
			AddImageTiled(30, 235, 1, 110, 9304);

			// Auction Properties: 7
			AddButton(35, 240, 9702, 9703, 7, GumpButtonType.Reply, 0);
			AddLabel(55, 237, Misc.kLabelHue, ADMIN_PROPERTIES);

			// End auction now: 8
			AddButton(35, 260, 9702, 9703, 8, GumpButtonType.Reply, 0);
			AddLabel(55, 257, Misc.kLabelHue, ADMIN_END_AUCTION);

			// Close and return: 9
			AddButton(35, 280, 9702, 9703, 9, GumpButtonType.Reply, 0);
			AddLabel(55, 277, Misc.kLabelHue, ADMIN_CLOSE_RETURN);

			// Close and get: 10
			AddButton(35, 300, 9702, 9703, 10, GumpButtonType.Reply, 0);
			AddLabel(55, 297, Misc.kLabelHue, ADMIN_CLOSE_BRING);

			// Close and delete: 11
			AddButton(35, 320, 9702, 9703, 11, GumpButtonType.Reply, 0);
			AddLabel(55, 317, Misc.kLabelHue, ADMIN_CLOSE_DELETE);

			AddImageTiled(15, 345, 270, 1, 9304);

			AddButton(15, 350, 9702, 9703, 0, GumpButtonType.Reply, 0);
			AddLabel(35, 348, Misc.kLabelHue, EXIT);
		}

		public override void OnResponse(NetState sender, RelayInfo info)
		{
			switch (info.ButtonID)
			{
				case 0: // Exit
					break;

				case 1: // Owner Props
					m_User.SendGump(this);
					m_User.SendGump(new PropertiesGump(m_User, m_Auction.Owner));
					break;

				case 2: // Owner Account
					m_User.SendGump(this);
					m_User.SendGump(new AdminGump(m_User, AdminGumpPage.AccountDetails_Information, 0, null,
						"Request from the auction system", m_Auction.Account));
					break;

				case 3: // Owner Client
					m_User.SendGump(this);

					if (m_Auction.Owner.NetState != null)
					{
						m_User.SendGump(new ClientGump(m_User, m_Auction.Owner.NetState));
					}

					break;

				case 4: // Item Props
					m_User.SendGump(this);
					m_User.SendGump(new PropertiesGump(m_User, m_Auction.Item));
					break;

				case 5: // Get Item
					m_User.SendGump(this);

					if (m_Auction.Creature && m_Auction.Pet != null)
					{
						m_User.SendMessage("That cannot be used on pets");
					}
					else
					{
						m_User.Backpack.AddItem(m_Auction.Item);
					}

					AuctionLog.WriteViewItem(m_Auction, m_User);
					break;

				case 6: // Return Item
					if (m_Auction.Creature)
					{
						m_Auction.Pet.SetControlMaster(null);
						m_Auction.Pet.ControlOrder = OrderType.Stay;
						m_Auction.Pet.Internalize();
					}
					else
					{
						Item item = m_Auction.Item;

						if (item != null)
						{
							if (item.Parent is Mobile)
							{
								((Mobile)item.Parent).RemoveItem(item);
							}
							else if (item.Parent is Item)
							{
								((Item)item.Parent).RemoveItem(item);
							}
						}

						AuctionSystem.ControlStone.AddItem(m_Auction.Item);
						m_Auction.Item.Parent = AuctionSystem.ControlStone;
					}

					AuctionLog.WriteReturnItem(m_Auction, m_User);
					m_User.SendGump(this);
					break;

				case 7: // Auction Props
					m_User.SendGump(this);
					m_User.SendGump(new PropertiesGump(m_User, m_Auction));
					break;

				case 8: // End Auction
					m_Auction.End(m_User);
					goto case 0;

				case 9: // End and Return
					m_Auction.StaffDelete(m_User, ItemFate.ReturnToOwner);
					goto case 0;

				case 10: // End and Get
					m_Auction.StaffDelete(m_User, ItemFate.ReturnToStaff);
					goto case 0;

				case 11: // End and Delete
					m_Auction.StaffDelete(m_User, ItemFate.Delete);
					goto case 0;
			}
		}
	}
}
