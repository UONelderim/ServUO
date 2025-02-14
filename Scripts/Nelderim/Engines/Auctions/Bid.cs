using System;
using System.IO;
using Server;
using Server.Accounting;
using Server.Mobiles;
using static Arya.Auction.AuctionMessages;

namespace Arya.Auction
{
	public class Bid
	{
		[CommandProperty(AccessLevel.Administrator)]
		public Mobile Mobile { get; private set; }

		[CommandProperty(AccessLevel.Administrator)]
		public int Amount { get; private set; }

		[CommandProperty(AccessLevel.Administrator)]
		public DateTime Time { get; private set; }

		public Bid(Mobile m, int amount)
		{
			Time = DateTime.UtcNow;
			Mobile = m;
			Amount = amount;
		}

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
