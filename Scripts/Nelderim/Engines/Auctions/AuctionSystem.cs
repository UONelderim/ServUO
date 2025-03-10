using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Server;
using Server.Accounting;
using Server.Items;
using Server.Mobiles;
using static Arya.Auction.AuctionMessages;

namespace Arya.Auction
{
	public delegate void AuctionGumpCallback(Mobile user);
	
	public class AuctionSystem
	{
		private static AuctionControl m_ControlStone;

		public static AuctionControl ControlStone
		{
			get => m_ControlStone;
			set => m_ControlStone = value;
		}

		public static List<AuctionItem> Auctions => m_ControlStone?.Auctions;

		public static List<AuctionItem> Pending => m_ControlStone?.Pending;

		private static int MaxAuctions
		{
			get
			{
				if (m_ControlStone == null)
					return 0;
				return m_ControlStone.MaxAuctionsParAccount;
			}
		}

		public static int MinAuctionDays => m_ControlStone.MinAuctionDays;

		public static int MaxAuctionDays => m_ControlStone.MaxAuctionDays;

		public static bool Running => m_ControlStone != null;

		public static void Add(AuctionItem auction)
		{
			// Put the item into the control stone
			auction.Item.Internalize();
			m_ControlStone.AddItem(auction.Item);
			auction.Item.Parent = m_ControlStone;
			auction.Item.Visible = true;

			Auctions.Add(auction);

			m_ControlStone.InvalidateProperties();
		}

		public static void AuctionRequest(Mobile mobile)
		{
			if (!CanAuction(mobile))
			{
				mobile.SendMessage(AuctionConfig.MessageHue, ERR_TOO_MANY_AUCTIONS_FMT, MaxAuctions);
				mobile.SendGump(new AuctionGump(mobile));
				return;
			}
			if (!IsAuctionerNearby(mobile))
			{
				mobile.SendMessage(AuctionConfig.MessageHue, ERR_AUCTIONER_NOT_FOUND);
				return;
			}
			mobile.SendMessage(AuctionConfig.MessageHue, NEW_AUCTION_PROMPT);
			mobile.CloseAllGumps();
			mobile.Target = new AuctionTarget(OnNewAuctionTarget, -1, false);
		}

		private static void OnCreatureAuction(Mobile from, BaseCreature creature)
		{
			MobileStatuette ms = MobileStatuette.Create(from, creature);

			if (ms == null)
			{
				from.Target = new AuctionTarget(OnNewAuctionTarget, -1, false);
			}

			/*
			 * Pets are auctioned within an item (MobileStatuette)
			 * 
			 * The item's name is the type of the pet, the hue corresponds to the pet
			 * and the item id is retrieved from the shrink table.
			 * 
			 */

			AuctionItem auction = new AuctionItem(ms, from);
			from.SendGump(new NewAuctionGump(from, auction));
		}

		private static void OnNewAuctionTarget(Mobile from, object targeted)
		{
			Item item = targeted as Item;
			BaseCreature bc = targeted as BaseCreature;
			if (!IsAuctionerNearby(from))
			{
				from.SendMessage(AuctionConfig.MessageHue, ERR_AUCTIONER_NOT_FOUND);
				return;
			}

			if (item == null && !AuctionConfig.AllowPetsAuction)
			{
				// Can't auction pets and target it invalid
				from.SendMessage(AuctionConfig.MessageHue, ERR_NOT_ITEM);
				from.Target = new AuctionTarget(OnNewAuctionTarget, -1, false);
				return;
			}

			if (bc != null)
			{
				// Auctioning a pet
				OnCreatureAuction(from, bc);
				return;
			}

			if (!CheckItem(item))
			{
				from.SendMessage(AuctionConfig.MessageHue, ERR_INVALID_ITEM);
				from.Target = new AuctionTarget(OnNewAuctionTarget, -1, false);
				return;
			}

			if (!item.Movable)
			{
				from.SendMessage(AuctionConfig.MessageHue, ERR_FROZEN_ITEM);
				from.Target = new AuctionTarget(OnNewAuctionTarget, -1, false);
				return;
			}

			bool ok = true;

			if (item is Container)
			{
				foreach (Item sub in item.Items)
				{
					if (!CheckItem(sub))
					{
						from.SendMessage(AuctionConfig.MessageHue, ERR_CONTAINER_INVALID_ITEM);
						ok = false;
						break;
					}

					if (!sub.Movable)
					{
						from.SendMessage(AuctionConfig.MessageHue, ERR_FROZEN_ITEM);
						ok = false;
						break;
					}

					if (sub is Container && sub.Items.Count > 0)
					{
						ok = false;
						from.SendMessage(AuctionConfig.MessageHue, ERR_CONTAINER_NESTED);
						break;
					}
				}
			}

			if (!(item.IsChildOf(from.Backpack) || item.IsChildOf(from.BankBox)))
			{
				from.SendMessage(AuctionConfig.MessageHue, ERR_INVALID_PARENT);
				ok = false;
			}

			if (!ok)
			{
				from.Target = new AuctionTarget(OnNewAuctionTarget, -1, false);
			}
			else
			{
				// Item ok, start auction creation
				AuctionItem auction = new AuctionItem(item, from);

				from.SendGump(new NewAuctionGump(from, auction));
			}
		}

		private static bool CheckItem(Item item)
		{
			return AuctionConfig.ForbiddenTypes.All(t => t != item.GetType());
		}

		public static void ForceDelete(Mobile m)
		{
			Console.WriteLine("Auction system terminated on {0} at {1} by {2} ({3}, Account: {4})",
				DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString(), m.Name, m.Serial,
				(m.Account as Account).Username);

			while (Auctions.Count > 0 || Pending.Count > 0)
			{
				while (Auctions.Count > 0)
				{
					(Auctions[0]).ForceEnd();
				}

				while (Pending.Count > 0)
				{
					(Pending[0]).ForceEnd();
				}
			}

			ControlStone.ForceDelete();
			ControlStone = null;
		}

		public static AuctionItem Find(Guid id)
		{
			if (!Running)
				return null;

			var found = Pending.Find(item => item.ID == id);
			return found ?? Auctions.Find(item => item.ID == id);
		}

		public static List<AuctionItem> GetAuctions(Mobile m)
		{
			try
			{
				return Auctions
					.Where(auction => auction.Owner == m || (auction.Owner != null && m.Account.Equals(auction.Owner.Account)))
					.ToList();
			}
			catch(Exception e)
			{
				Console.WriteLine(e);
			}

			return [];
		}

		public static List<AuctionItem> GetBids(Mobile m)
		{

			try
			{
				return Auctions.Where(auction => auction.MobileHasBids(m)).ToList();
			}
			catch(Exception e)
			{
				Console.WriteLine(e);
			}

			return [];
		}

		public static List<AuctionItem> GetPendencies(Mobile m)
		{
			try
			{
				return Pending
					.Where(auction =>
						auction.Owner == m ||
						(auction.Owner != null && m.Account.Equals(auction.Owner.Account)) ||
						auction.HighestBid.Mobile == m ||
						(auction.HighestBid.Mobile != null && m.Account.Equals(auction.HighestBid.Mobile.Account)))
					.ToList();
			}
			catch(Exception e)
			{
				Console.WriteLine(e);
			}

			return [];
		}

		public static bool CanAuction(Mobile m)
		{
			if (m.IsStaff())
				return true;

			var count = Auctions.Count(auction => auction.Account == (m.Account as Account));

			return count < MaxAuctions;
		}

		public static bool IsAuctionerNearby(Mobile from)
		{
			if (from.IsStaff())
				return true;
			
			var eable = from.GetMobilesInRange(4);
			var found = eable.OfType<Auctioner>().Any(from.CanSee);
			eable.Free();
			return found;
		}

		public static void Initialize()
		{
			try
			{
				if (Running)
				{
					VerifyIntegrity();
					VerifyAuctions();
					VerifyPendencies();
				}
			}
			catch (Exception err)
			{
				m_ControlStone = null;

				Console.WriteLine(
					"An error occurred when initializing the Auction System. The system has been temporarily disabled.");
				Console.WriteLine("Error details: {0}", err);
			}
		}

		public static void OnDeadlineReached()
		{
			if (!Running)
				return;

			VerifyAuctions();
			VerifyPendencies();
		}

		private static void VerifyIntegrity()
		{
			foreach (var auction in Auctions)
				auction.VeirfyIntergrity();
		}

		public static void VerifyAuctions()
		{
			if (!Running)
				return;
			
			foreach (var invalid in Auctions.Where(auction => auction.Item == null || (auction.Creature && auction.Pet == null)).ToArray())
			{
				invalid.EndInvalid();
			}

			foreach (var expired in Auctions.Where(auction => auction.Expired).ToArray())
			{
				expired.End(null);
			}
		}

		public static void VerifyPendencies()
		{
			if (!Running)
				return;
			
			foreach (var expired in Pending.Where(auction => auction.PendingExpired).ToArray()) 
				expired.PendingTimeOut();
		}

		public static void Disable()
		{
			m_ControlStone = null;
			AuctionScheduler.Stop();
		}

		public static void ProfileAuctions()
		{
			string file = Path.Combine(Core.BaseDirectory, "AuctionProfile.txt");

			try
			{
				StreamWriter sw = new StreamWriter(file, false);

				sw.WriteLine("Auction System Profile");
				sw.WriteLine("{0}", DateTime.Now.ToLongDateString());
				sw.WriteLine("{0}", DateTime.Now.ToShortTimeString());
				sw.WriteLine("{0} Running Auctions", Auctions.Count);
				sw.WriteLine("{0} Pending Auctions", Pending.Count);
				sw.WriteLine("Next Deadline : {0} at {1}", AuctionScheduler.Deadline.ToShortDateString(),
					AuctionScheduler.Deadline.ToShortTimeString());

				sw.WriteLine();
				sw.WriteLine("Auctions List");
				sw.WriteLine();


				foreach (AuctionItem a in Auctions)
				{
					a.Profile(sw);
				}

				sw.WriteLine("Pending Auctions List");
				sw.WriteLine();

				foreach (AuctionItem p in Pending)
				{
					p.Profile(sw);
				}

				sw.WriteLine("End of profile");
				sw.Close();
			}
			catch (Exception err)
			{
				Console.WriteLine("Couldn't output auction profile. Error: {0}", err);
			}
		}
	}
}
