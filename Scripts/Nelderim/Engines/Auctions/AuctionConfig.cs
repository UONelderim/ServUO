using System;
using Server;
using Server.Items;
using Xanthos.Utilities;

namespace Arya.Auction
{
	public class AuctionConfig
	{
		public static int MessageHue = 0x40;

		public static Type[] ForbiddenTypes =
		{
			typeof(Gold), typeof(BankCheck), typeof(DeathRobe), typeof(AuctionGoldCheck), typeof(AuctionItemCheck)
		};

		/// <summary>
		///     This is the number of days the system will wait for the buyer or seller to decide on an ambiguous situation.
		///     This can occur whether the highest bid didn't match the auction reserve. The owner will have then X days to
		///     accept or refuse the auction. Another case is when one or more items is deleted due to a wipe or serialization
		///     error.
		///     The buyer will have to decide in this case.
		/// </summary>
		public static int DaysForConfirmation = 5;

		/// <summary>
		///     This value specifies how higher the reserve can be with respect to the starting bid. This factor should limit
		///     any possible abuse of the reserve and prevent players from using very high values to be absolutely sure they will
		///     have
		///     to sell only if they're happy with the outcome.
		/// </summary>
		public static double MaxReserveMultiplier = 3.0d;

		/// <summary>
		///     This is the hue used to simulate the black hue because hue #1 isn't displayed
		///     correctly on gumps. If your shard is using custom hues, you might want to
		///     double check this value and verify it corresponds to a pure black hue.
		/// </summary>
		public static int BlackHue = 2000;

		/// <summary>
		///     This variable controls whether the system will sell pets as well
		/// </summary>
		public static bool AllowPetsAuction = true;

		/// <summary>
		///     This is the Access Level required to admin an auction through its
		///     view gump. This will allow to see the props and to delete it.
		/// </summary>
		public static AccessLevel AuctionAdminAcessLevel = AccessLevel.Administrator;

		/// <summary>
		///     Set this to false if you don't want to the system to produce a log file in \Logs\Auction.txt
		/// </summary>
		public static bool EnableLogging = true;

		/// <summary>
		///     When a bid is placed within 5 minutes from the auction's ending, the auction duration will be
		///     extended by this value.
		/// </summary>
		public static TimeSpan LateBidExtention = TimeSpan.FromMinutes(0.0);

		/// <summary>
		///     This value specifies how much a player will have to pay to auction an item:
		///     - 0.0 means that auctioning is free
		///     - A value less or equal than 1.0 represents a percentage from 0 to 100%. This percentage will be applied on
		///     the max value between the starting bid and the reserve.
		///     - A value higher than 1.0 represents a fixed cost for the service (rounded).
		/// </summary>
		public static double CostOfAuction;

		/// <summary>
		///     Savings Account configuration for daily interest paid
		/// </summary>
		public static double GoldInterestRate = .04; // Percentage paid each day for gold

		public static double TokensInterestRate = .04; // Percentage paid each day for tokens

		public static bool EnableTokens; // Enable/disable them

		public static Type TokenType;
		public static Type TokenCheckType;

		public static int InterestHour = 20;
		public static double MaximumCheckValue = 25000000;
	}
}
