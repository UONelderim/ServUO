#region AuthorHeader

//
//	Auction version 2.1, by Xanthos and Arya
//
//  Based on original ideas and code by Arya
//

#endregion AuthorHeader

#region References

using Server;
using Server.Commands;

#endregion

namespace Arya.Auction
{
	/// <summary>
	///     Summary description for Commands.
	/// </summary>
	public class AuctionCommands
	{
		public static void Initialize()
		{
			CommandHandlers.Register("InitAuction", AccessLevel.Administrator, OnInitAuction);
			CommandHandlers.Register("MyAuction", AccessLevel.Player, OnMyAuction);
			CommandHandlers.Register("Auction", AccessLevel.GameMaster, OnAuction);
			CommandHandlers.Register("AuctionAdmin", AccessLevel.Administrator, OnAuctionAdmin);
		}

		#region Placing the control stone

		[Usage("InitAuction"),
		 Description(
			 "Initializes the auction system by bringing up a target for the creation of the auction control stone. If the system is already running this command will bring the user to the stone's location")]
		private static void OnInitAuction(CommandEventArgs e)
		{
			if (AuctionSystem.Running)
			{
				e.Mobile.SendMessage(AuctionConfig.MessageHue,
					"The Auction System is already running. You have been teleported to the control stone location");
				e.Mobile.Location = AuctionSystem.ControlStone.Location;
				e.Mobile.Map = AuctionSystem.ControlStone.Map;
			}
			else
			{
				e.Mobile.SendMessage(AuctionConfig.MessageHue, "Where do you with to place the Auction control stone?");
				e.Mobile.Target = new AuctionTarget(PlaceStoneCallback, -1, true);
			}
		}

		private static void PlaceStoneCallback(Mobile from, object targeted)
		{
			IPoint3D location = targeted as IPoint3D;

			if (location != null)
			{
				AuctionControl stone = new AuctionControl();

				stone.MoveToWorld(new Point3D(location), from.Map);
				AuctionSystem.ControlStone = stone;
			}
			else
			{
				from.SendMessage(AuctionConfig.MessageHue, "Invalid location");
			}
		}

		#endregion

		#region MyAuction

		[Usage("MyAuction"),
		 Description(
			 "Displays all the auctions a player has created or has bid on. This command can't be used to access the full system, therefore it cannot be used to create new auctions.")]
		private static void OnMyAuction(CommandEventArgs e)
		{
			if (AuctionSystem.Running)
			{
				e.Mobile.SendGump(new MyAuctionGump(e.Mobile, null));
			}
			else
			{
				e.Mobile.SendMessage(AuctionConfig.MessageHue, AuctionSystem.ST[200]);
			}
		}

		#endregion

		#region Auction

		[Usage("Auction"), Description("Displays the main auction system gump")]
		private static void OnAuction(CommandEventArgs e)
		{
			if (AuctionSystem.Running)
			{
				e.Mobile.SendGump(new AuctionGump(e.Mobile));
			}
			else
			{
				e.Mobile.SendMessage(AuctionConfig.MessageHue, "The auction system is currently stopped");
			}
		}

		#endregion

		#region Auction Admin

		[Usage("AuctionAdmin"), Description("Invokes the auction system administration gump")]
		private static void OnAuctionAdmin(CommandEventArgs e)
		{
			if (AuctionSystem.Running)
				e.Mobile.SendGump(new AuctionAdminGump(e.Mobile));
			else
				e.Mobile.SendMessage(AuctionConfig.MessageHue, "The auction system is now stopped");
		}

		#endregion
	}
}
