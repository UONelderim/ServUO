//
//	Auction version 2.1, by Xanthos and Arya
//
//  Based on original ideas and code by Arya
//

using System;
using System.IO;
using Server;
using Server.Accounting;

namespace Arya.Auction
{
	/// <summary>
	///     Summary description for AuctionLog.
	/// </summary>
	public class AuctionLog
	{
		private static StreamWriter m_Writer;
		private static bool m_Enabled;

		public static void Initialize()
		{
			if (AuctionSystem.Running && AuctionConfig.EnableLogging)
			{
				// Create the log writer
				try
				{
					string folder = Path.Combine(Core.BaseDirectory, @"Logs\Aukcje");

					if (!Directory.Exists(folder))
						Directory.CreateDirectory(folder);

					string name = String.Format("{0}.txt", DateTime.UtcNow.ToLongDateString());
					string file = Path.Combine(folder, name);

					m_Writer = new StreamWriter(file, true);
					m_Writer.AutoFlush = true;

					m_Writer.WriteLine("###############################");
					m_Writer.WriteLine("# {0} - {1}", DateTime.UtcNow.ToShortDateString(),
						DateTime.UtcNow.ToShortTimeString());
					m_Writer.WriteLine();

					m_Enabled = true;
				}
				catch (Exception err)
				{
					Console.WriteLine("Couldn't initialize auction system log. Reason:");
					Console.WriteLine(err.ToString());
					m_Enabled = false;
				}
			}
		}

		/// <summary>
		///     Records the creation of a new auction item
		/// </summary>
		/// <param name="auction">The new auction</param>
		public static void WriteNewAuction(AuctionItem auction)
		{
			if (!m_Enabled || m_Writer == null)
				return;

			try
			{
				m_Writer.WriteLine("## New Auction : {0}", auction.ID);
				m_Writer.WriteLine("# {0}", auction.ItemName);
				m_Writer.WriteLine("# Created on {0} at {1}", DateTime.UtcNow.ToShortDateString(),
					DateTime.UtcNow.ToShortTimeString());
				m_Writer.WriteLine("# Owner : {0} [{1}] Account: {2}", auction.Owner.Name,
					auction.Owner.Serial.ToString(), auction.Account.Username);
				m_Writer.WriteLine("# Expires on {0} at {1}", auction.EndTime.ToShortDateString(),
					auction.EndTime.ToShortTimeString());
				m_Writer.WriteLine("# Starting Bid: {0}. Reserve: {1}. Buy Now: {2}",
					auction.MinBid, auction.Reserve, auction.AllowBuyNow ? auction.BuyNow.ToString() : "Disabled");
				m_Writer.WriteLine("# Owner Description : {0}", auction.Description);
				m_Writer.WriteLine("# Web Link : {0}", auction.WebLink != null ? auction.WebLink : "N/A");

				if (auction.Creature)
				{
					// Selling a pet
					m_Writer.WriteLine("#### Selling 1 Creature");
					m_Writer.WriteLine("# Type : {0}. Serial : {1}. Name : {2} Hue : {3}", auction.Pet.GetType().Name,
						auction.Pet.Serial.ToString(), auction.Pet.Name != null ? auction.Pet.Name : "Unnamed",
						auction.Pet.Hue);
					m_Writer.WriteLine("# Statuette Serial : {0}", auction.Item.Serial.ToString());
					m_Writer.WriteLine("# Properties: {0}", auction.Items[0].Properties);
				}
				else
				{
					// Selling items
					m_Writer.WriteLine("#### Selling {0} Items", auction.ItemCount);

					for (int i = 0; i < auction.ItemCount; i++)
					{
						AuctionItem.ItemInfo info = auction.Items[i];
						m_Writer.WriteLine("# {0}. {1} [{2}] Type {3} Hue {4}", i, info.Name, info.Item.Serial,
							info.Item.GetType().Name, info.Item.Hue);
						m_Writer.WriteLine("Properties: {0}", info.Properties);
					}
				}

				m_Writer.WriteLine();
			}
			catch { }
		}

		/// <summary>
		///     Writes the current highest bid in an auction
		/// </summary>
		/// <param name="auction">The auction corresponding to the bid</param>
		public static void WriteBid(AuctionItem auction)
		{
			if (!m_Enabled || m_Writer == null)
				return;

			try
			{
				m_Writer.WriteLine("> [{0}] Bid Amount : {1}, Mobile : {2} [{3}] Account : {4}",
					auction.ID.ToString(),
					auction.HighestBidValue.ToString("#,0"),
					auction.HighestBidder.Name,
					auction.HighestBidder.Serial.ToString(),
					(auction.HighestBidder.Account as Account).Username);
			}
			catch { }
		}

		/// <summary>
		///     Changes the
		/// </summary>
		/// <param name="auction">The auction switching to pending</param>
		/// <param name="reason">The reason why the auction is set to pending</param>
		public static void WritePending(AuctionItem auction, string reason)
		{
			if (!m_Enabled || m_Writer == null)
				return;

			try
			{
				m_Writer.WriteLine("] [{0}] Becoming Pending on {1} at {2}. Reason : {3}",
					auction.ID.ToString(),
					DateTime.UtcNow.ToShortDateString(),
					DateTime.UtcNow.ToShortTimeString(),
					reason);
			}
			catch { }
		}

		/// <summary>
		///     Writes the end of the auction to the log
		/// </summary>
		/// <param name="auction">The auction ending</param>
		/// <param name="reason">The AuctionResult stating why the auction is ending</param>
		/// <param name="m">The Mobile forcing the end of the auction (can be null)</param>
		/// <param name="comments">Additional comments on the ending (can be null)</param>
		public static void WriteEnd(AuctionItem auction, AuctionResult reason, Mobile m, string comments)
		{
			if (!m_Enabled || m_Writer == null)
				return;

			try
			{
				m_Writer.WriteLine("## Ending Auction {0}", auction.ID.ToString());
				m_Writer.WriteLine("# Status : {0}", reason.ToString());

				if (m != null)
					m_Writer.WriteLine("# Ended by {0} [{1}], {2}, Account : {3}",
						m.Name, m.Serial.ToString(), m.AccessLevel.ToString(), (m.Account as Account).Username);

				if (comments != null)
					m_Writer.WriteLine("# Comments : {0}", comments);

				m_Writer.WriteLine();
			}
			catch { }
		}

		/// <summary>
		///     Records a staff member viewing an item
		/// </summary>
		/// <param name="auction">The auction item</param>
		/// <param name="m">The mobile viewing the item</param>
		public static void WriteViewItem(AuctionItem auction, Mobile m)
		{
			if (!m_Enabled || m_Writer == null)
				return;

			try
			{
				m_Writer.WriteLine("} Vieweing item [{0}] Mobile: {1} [2], {3}, Account : {4}",
					auction.ID.ToString(),
					m.Name,
					m.Serial.ToString(),
					m.AccessLevel.ToString(),
					(m.Account as Account).Username);
			}
			catch { }
		}

		/// <summary>
		///     Records a staff member returning an item
		/// </summary>
		/// <param name="auction">The auction</param>
		/// <param name="m">The mobile returning the item</param>
		public static void WriteReturnItem(AuctionItem auction, Mobile m)
		{
			if (!m_Enabled || m_Writer == null)
				return;

			try
			{
				m_Writer.WriteLine("} Returning item [{0}] Mobile: {1} [2], {3}, Account : {4}",
					auction.ID.ToString(),
					m.Name,
					m.Serial.ToString(),
					m.AccessLevel.ToString(),
					(m.Account as Account).Username);
			}
			catch { }
		}
	}
}
