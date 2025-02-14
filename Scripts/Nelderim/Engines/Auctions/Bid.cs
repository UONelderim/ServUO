//
//	Auction version 2.1, by Xanthos and Arya
//
//  Based on original ideas and code by Arya
//

using System;
using System.IO;
using Server;
using Server.Accounting;
using Server.Mobiles;
using static Arya.Auction.AuctionMessages;

namespace Arya.Auction
{
	/// <summary>
	///     Defines a bid entry
	/// </summary>
	public class Bid
	{
		[CommandProperty(AccessLevel.Administrator)]
		/// <summary>
		/// Gets the mobile who placed the bid
		/// </summary>
		public Mobile Mobile { get; private set; }

		[CommandProperty(AccessLevel.Administrator)]
		/// <summary>
		/// Gets the value of the bid
		/// </summary>
		public int Amount { get; private set; }

		[CommandProperty(AccessLevel.Administrator)]
		/// <summary>
		/// Gets the time the bid has been placed at
		/// </summary>
		public DateTime Time { get; private set; }

		/// <summary>
		///     Creates a new bid
		/// </summary>
		/// <param name="m">The Mobile placing the bid</param>
		/// <param name="amount">The amount of the bid</param>
		public Bid(Mobile m, int amount)
		{
			Time = DateTime.UtcNow;
			Mobile = m;
			Amount = amount;
		}

		/// <summary>
		///     Creates a new bid. Checks if the mobile has enough money to bid
		///     and if so removes the money from the player
		/// </summary>
		/// <param name="from">The mobile bidding</param>
		/// <param name="amount">The amount bid</param>
		/// <returns>A bid object if the mobile has enough money</returns>
		public static Bid CreateBid(Mobile from, int amount)
		{
			if (Banker.Withdraw(from, amount))
			{
				return new Bid(from, amount);
			}

			from.SendMessage(0x40, ERR_NOT_ENOUGH_MONEY);
			return null;
		}

		private Bid()
		{
		}

		public void Serialize(GenericWriter writer)
		{
			// Version 1
			// Version 0
			writer.Write(Mobile);
			writer.Write(Amount);
			writer.Write(Time);
		}

		public static Bid Deserialize(GenericReader reader, int version)
		{
			Bid bid = new Bid();

			switch (version)
			{
				case 1:
				case 0:
					bid.Mobile = reader.ReadMobile();
					bid.Amount = reader.ReadInt();
					bid.Time = reader.ReadDateTime();
					break;
			}

			return bid;
		}

		/// <summary>
		///     Returns the bid money to the highest bidder because they have been outbid
		/// </summary>
		/// <param name="auction">The auction the bid belongs to</param>
		public void Outbid(AuctionItem auction)
		{
			if (Mobile == null || Mobile.Account == null)
				return;

			AuctionCheck check = new AuctionGoldCheck(auction, AuctionResult.Outbid);
			if (!this.Mobile.Backpack.TryDropItem(Mobile, check, false))
			{
				Mobile.BankBox.DropItem(check);
			}

			// Send notice
			AuctionMessaging.SendOutbidMessage(auction, Amount, Mobile);
		}

		/// <summary>
		///     Returns the bid money to the bidder because the auction has been canceled
		/// </summary>
		/// <param name="auction">The auction the bid belongs to</param>
		public void AuctionCanceled(AuctionItem auction)
		{
			if (Mobile == null)
				return;

			AuctionCheck check = new AuctionGoldCheck(auction, AuctionResult.SystemStopped);

			if (Mobile.Backpack == null || !Mobile.Backpack.TryDropItem(Mobile, check, false))
			{
				if (Mobile.BankBox != null)
					Mobile.BankBox.DropItem(check);
				else
					check.Delete();
			}
		}

		/// <summary>
		///     Outputs bid information
		/// </summary>
		/// <param name="writer"></param>
		public void Profile(StreamWriter writer)
		{
			string owner = null;

			if (Mobile != null && Mobile.Account != null)
				owner = String.Format("{0} [ Account : {1} - Serial {2} ]", Mobile.Name,
					(Mobile.Account as Account).Username, Mobile.Serial);
			else
				owner = "None";

			writer.WriteLine("- {0}\t{1}", Amount, owner);
		}
	}
}
