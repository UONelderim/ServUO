//
// Auction version 2.1, by Xanthos and Arya
//
// Based on original ideas and code by Arya
//

using System;
using System.Collections;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using Server;
using Server.Accounting;
using Server.Items;
using Server.Mobiles;
using Xanthos.Interfaces;
using static Arya.Auction.AuctionMessages;

namespace Arya.Auction
{
	/// <summary>
	///     Defines situations for pending situations
	/// </summary>
	public enum AuctionPendency : byte
	{
		/// <summary>
		///     The user still has to make a decision
		/// </summary>
		Pending = 0,

		/// <summary>
		///     The user OKs the auction
		/// </summary>
		Accepted = 1,

		/// <summary>
		///     The user didn't accept the auction
		/// </summary>
		NotAccepted = 2
	}

	/// <summary>
	///     Defines what happens with the auction item if the auction is ended by the staff
	/// </summary>
	public enum ItemFate
	{
		/// <summary>
		///     The item is returned to the owner
		/// </summary>
		ReturnToOwner,

		/// <summary>
		///     The item is taken by the staff
		/// </summary>
		ReturnToStaff,

		/// <summary>
		///     The item is deleted
		/// </summary>
		Delete
	}

	/// <summary>
	///     Specifies the type of message that should be dispatched for the buyer or the owner
	/// </summary>
	public enum AuctionMessage : byte
	{
		/// <summary>
		///     No message should be dispatched
		/// </summary>
		None = 0,

		/// <summary>
		///     An information message should be dispatched
		/// </summary>
		Information = 1,

		/// <summary>
		///     A feedback message should be dispatched
		/// </summary>
		Response = 2
	}

	/// <summary>
	///     An auction entry, holds all the information about a single auction process
	/// </summary>
	public class AuctionItem
	{
		public class ItemInfo
		{
			private string m_Name;
			private string m_Props;

			public string Name
			{
				get
				{
					if (Item != null)
						return m_Name;
					return "N/A";
				}
			}

			public Item Item { get; private set; }

			public string Properties
			{
				get
				{
					if (Item != null)
						return m_Props;
					return ERR_ITEM_REMOVED;
				}
			}

			public ItemInfo(Item item)
			{
				Item = item;

				if (item.Name != null)
					m_Name = item.Name;
				// else
				// m_Name = m_StringList.Table[ item.LabelNumber ] as string;

				if (item.Amount > 1)
				{
					m_Name = String.Format("{0} {1}", item.Amount.ToString("#,0"), m_Name);
				}

				if (item is MobileStatuette)
				{
					m_Props = GetCreatureProperties((item as MobileStatuette).ShrunkenPet);
				}
				else
				{
					m_Props = GetItemProperties(item);
				}
			}

			private static string GetCreatureProperties(BaseCreature creature)
			{
				StringBuilder sb = new StringBuilder();

				sb.Append("<basefont color=#FFFFFF>");

				if (creature.Name != null)
				{
					sb.AppendFormat("Name : {0}<br", creature.Name);
				}

				sb.AppendFormat(CONTROL_SLOTS_FMT, creature.ControlSlots);
				sb.AppendFormat(BONDABLE_FMT, creature.IsBondable ? "Yes" : "No");
				sb.AppendFormat(STR_FMT, creature.Str);
				sb.AppendFormat(DEX_FMT, creature.Dex);
				sb.AppendFormat(INT_FMT, creature.Int);

				int index = 0;
				Skill skill = null;

				while ((skill = creature.Skills[index++]) != null)
				{
					if (skill.Value > 0)
					{
						sb.AppendFormat("{0} : {1}<br>", skill.Name, skill.Value);
					}
				}

				return sb.ToString();
			}

			private ItemInfo()
			{
			}

			public void Serialize(GenericWriter writer)
			{
				// Version 1
				// Version 0
				writer.Write(m_Name);
				writer.Write(Item);
				writer.Write(m_Props);
			}

			public static ItemInfo Deserialize(GenericReader reader, int version)
			{
				ItemInfo item = new ItemInfo();

				switch (version)
				{
					case 1:
					case 0:
						item.m_Name = reader.ReadString();
						item.Item = reader.ReadItem();
						item.m_Props = reader.ReadString();
						break;
				}

				return item;
			}

			/// <summary>
			///     Verifies if the mobile referenced by this item is still valid
			/// </summary>
			public void VeirfyIntegrity()
			{
				IShrinkItem shrinkItem = Item as IShrinkItem;

				if (null != shrinkItem && null == shrinkItem.ShrunkenPet)
				{
					Item.Delete();
					Item = null; // This will make this item invalid
				}
			}
		}

		private static StringList m_StringList;

		static AuctionItem()
		{
			string clilocFolder = null;

			if (AuctionConfig.ClilocLocation != null)
			{
				clilocFolder = Path.GetDirectoryName(AuctionConfig.ClilocLocation);
				//Ultima.Client.Directories.Insert( 0, clilocFolder );
			}

			m_StringList = new StringList("ENU");

			if (clilocFolder != null)
			{
				//Ultima.Client.Directories.RemoveAt( 0 );
			}
		}

		/// <summary>
		///     Gets an html formatted string with all the properies for the item
		/// </summary>
		/// <returns>A string object containing the html structure corresponding to the item properties</returns>
		private static string GetItemProperties(Item item)
		{
			if (item == null || item.PropertyList == null)
			{
				return NA;
			}

			ObjectPropertyList plist = new ObjectPropertyList(item);
			item.GetProperties(plist);

			byte[] data = plist.Stream.ToArray();
			ArrayList list = new ArrayList();

			int index = 15; // First localization number index

			while (true)
			{
				if (index + 4 >= data.Length)
				{
					break;
				}

				uint number = (uint)(data[index++] << 24 | data[index++] << 16 | data[index++] << 8 | data[index++]);
				ushort length = 0;

				if (index + 2 > data.Length)
				{
					break;
				}

				length = (ushort)(data[index++] << 8 | data[index++]);

				// Read the string
				int end = index + length;

				if (end >= data.Length)
				{
					end = data.Length - 1;
				}

				StringBuilder s = new StringBuilder();
				while (index + 2 <= end + 1)
				{
					short next = (short)(data[index++] | data[index++] << 8);

					if (next == 0)
						break;

					s.Append(Encoding.Unicode.GetString(BitConverter.GetBytes(next)));
				}

				list.Add(ComputeProperty((int)number, s.ToString()));
			}

			StringBuilder sb = new StringBuilder();
			sb.Append("<basefont color=#FFFFFF><p>");

			foreach (string prop in list)
			{
				sb.AppendFormat("{0}<br>", prop);
			}

			return sb.ToString();
		}

		/// <summary>
		///     Capitalizes each word in a string
		/// </summary>
		/// <param name="property">The input string</param>
		/// <returns>The output string </returns>
		private static string Capitalize(string property)
		{
			string[] parts = property.Split(' ');
			StringBuilder sb = new StringBuilder();

			for (int i = 0; i < parts.Length; i++)
			{
				string part = parts[i];

				if (part.Length == 0)
				{
					continue;
				}

				char c = Char.ToUpper(part[0]);

				part = part.Substring(1);
				sb.AppendFormat("{0}{1}", String.Concat(c, part), i < parts.Length - 1 ? " " : "");
			}

			return sb.ToString();
		}

		/// <summary>
		///     Converts a localization number and its arguments to a valid string
		/// </summary>
		/// <param name="number">The localization number of the label</param>
		/// <param name="arguments">The arguments for the label</param>
		/// <returns>The translated string</returns>
		private static string ComputeProperty(int number, string arguments)
		{
			string prop = ""; 

			if (arguments == null || arguments.Length == 0)
			{
				return Capitalize(prop);
			}

			string[] args = arguments.Split('\t');
			Regex reg = new Regex(@"~\d+\w*~", RegexOptions.None);
			MatchCollection matches = reg.Matches(prop, 0);

			if (matches.Count == args.Length)
			{
				// Valid
				for (int i = 0; i < matches.Count; i++)
				{
					if (args[i].StartsWith("#"))
					{
						int loc = -1;

						try
						{
							loc = Int32.Parse(args[i].Substring(1));
						}
						catch { }

						if (loc != -1)
						{
							//	args[ i ] = m_StringList.Table[ loc ] as string;
						}
					}

					Match m = matches[i];

					prop = prop.Replace(m.Value, args[i]);
				}

				return Capitalize(prop);
			}

			return INVALID;
		}

		private DateTime m_StartTime;
		private DateTime m_EndTime;
		private TimeSpan m_Duration = TimeSpan.FromDays(7);
		private string m_WebLink = "";
		private Guid m_ID;
		private AuctionPendency m_OwnerPendency = AuctionPendency.Pending;
		private AuctionPendency m_BuyerPendency = AuctionPendency.Pending;
		private AuctionMessage m_OwnerMessage = AuctionMessage.None;
		private AuctionMessage m_BuyerMessage = AuctionMessage.None;

		[CommandProperty(AccessLevel.Administrator)]
		/// <summary>
		/// States whether this auction allows the buy now feature
		/// </summary>
		public bool AllowBuyNow
		{
			get { return BuyNow > 0; }
		}

		[CommandProperty(AccessLevel.Administrator)]
		/// <summary>
		/// Gets the buy now value
		/// </summary>
		public int BuyNow { get; set; }

		[CommandProperty(AccessLevel.Administrator)]
		/// <summary>
		/// Gets the date and time corrsponding to the moment when the pending situation will automatically end
		/// </summary>
		public DateTime PendingEnd { get; private set; }

		[CommandProperty(AccessLevel.Administrator)]
		/// <summary>
		/// Gets the item being sold at the auction
		/// </summary>
		public Item Item { get; private set; }

		[CommandProperty(AccessLevel.Administrator)]
		/// <summary>
		/// Gets the owner of the item
		/// </summary>
		public Mobile Owner { get; private set; }

		[CommandProperty(AccessLevel.Administrator)]
		/// <summary>
		/// Gets the starting time for this auction
		/// </summary>
		public DateTime StartTime
		{
			get { return m_StartTime; }
		}

		[CommandProperty(AccessLevel.Administrator)]
		/// <summary>
		/// Gets the end time for this auction
		/// </summary>
		public DateTime EndTime
		{
			get { return m_EndTime; }
		}

		/// <summary>
		///     Gets the running length of the auction for this item
		/// </summary>
		public TimeSpan Duration
		{
			get { return m_Duration; }
			set
			{
				try
				{
					m_Duration = value;
				}
				catch
				{
					m_Duration = TimeSpan.Zero;
				}
			}
		}

		[CommandProperty(AccessLevel.Administrator)]
		/// <summary>
		/// Gets the time to live left for this auction
		/// </summary>
		public TimeSpan TimeLeft
		{
			get
			{
				return m_EndTime - DateTime.UtcNow;
			}
		}

		/// <summary>
		///     Gets or sets the minimum bid allowed for this item
		/// </summary>
		public int MinBid { get; set; } = 1000;

		/// <summary>
		///     Gets or sets the reserve price for the item
		/// </summary>
		public int Reserve { get; set; } = 2000;

		[CommandProperty(AccessLevel.Administrator)]
		/// <summary>
		/// Gets or sets the description for this item
		/// </summary>
		public string Description { get; set; } = "";

		[CommandProperty(AccessLevel.Administrator)]
		/// <summary>
		/// A web link associated with this auction item
		/// </summary>
		public string WebLink
		{
			get { return m_WebLink; }
			set
			{
				if (value != null && value.Length > 0)
				{
					if (value.ToLower().StartsWith("http://") && value.Length > 7)
					{
						value = value.Substring(7);
					}
				}

				m_WebLink = value;
			}
		}

		/// <summary>
		///     Gets or sets the list of existing bids
		/// </summary>
		public ArrayList Bids { get; set; }

		[CommandProperty(AccessLevel.Administrator)]
		/// <summary>
		/// Gets the account that's selling this item
		/// </summary>
		public Account Account
		{
			get
			{
				if (Owner != null && Owner.Account != null)
				{
					return Owner.Account as Account;
				}

				return null;
			}
		}

		[CommandProperty(AccessLevel.Administrator)]
		/// <summary>
		/// Gets or sets the name of the item being sold
		/// </summary>
		public string ItemName { get; set; }

		[CommandProperty(AccessLevel.Administrator)]
		/// <summary>
		/// True if the auction is over but the reserve hasn't been met and the owner still haven't decided
		/// if to sell the item or not. This value makes no sense before the auction is over.
		/// </summary>
		public bool Pending { get; private set; }

		/// <summary>
		///     Gets the definitions of the items sold
		/// </summary>
		public ItemInfo[] Items { get; private set; }

		[CommandProperty(AccessLevel.Administrator)]
		/// <summary>
		/// Gets the number of items sold
		/// </summary>
		public int ItemCount
		{
			get { return Items.Length; }
		}

		[CommandProperty(AccessLevel.Administrator)]
		/// <summary>
		/// Gets the unique ID of this auction
		/// </summary>
		public Guid ID
		{
			get { return m_ID; }
		}

		/// <summary>
		///     Saves the auction item into the world file
		/// </summary>
		/// <param name="writer"></param>
		public void Serialize(GenericWriter writer)
		{
			// Version 1
			writer.Write(BuyNow);

			// Version 0
			writer.Write(Owner);
			writer.Write(m_StartTime);
			writer.Write(m_Duration);
			writer.Write(MinBid);
			writer.Write(Reserve);
			writer.Write(m_Duration);
			writer.Write(Description);
			writer.Write(m_WebLink);
			writer.Write(Pending);
			writer.Write(ItemName);
			writer.Write(Item);
			writer.Write(m_ID.ToString());
			writer.WriteDeltaTime(m_EndTime);
			writer.Write((byte)m_OwnerPendency);
			writer.Write((byte)m_BuyerPendency);
			writer.Write((byte)m_OwnerMessage);
			writer.Write((byte)m_BuyerMessage);
			writer.WriteDeltaTime(PendingEnd);

			writer.Write(Items.Length);
			// Items
			foreach (ItemInfo ii in Items)
			{
				ii.Serialize(writer);
			}

			// Bids
			writer.Write(Bids.Count);
			foreach (Bid bid in Bids)
			{
				bid.Serialize(writer);
			}
		}

		/// <summary>
		///     Loads an AuctionItem
		/// </summary>
		/// <returns>An AuctionItem</returns>
		public static AuctionItem Deserialize(GenericReader reader, int version)
		{
			AuctionItem auction = new AuctionItem();

			switch (version)
			{
				case 1:
					auction.BuyNow = reader.ReadInt();
					goto case 0;

				case 0:
					auction.Owner = reader.ReadMobile();
					auction.m_StartTime = reader.ReadDateTime();
					auction.m_Duration = reader.ReadTimeSpan();
					auction.MinBid = reader.ReadInt();
					auction.Reserve = reader.ReadInt();
					auction.m_Duration = reader.ReadTimeSpan();
					auction.Description = reader.ReadString();
					auction.m_WebLink = reader.ReadString();
					auction.Pending = reader.ReadBool();
					auction.ItemName = reader.ReadString();
					auction.Item = reader.ReadItem();
					auction.m_ID = new Guid(reader.ReadString());
					auction.m_EndTime = reader.ReadDeltaTime();
					auction.m_OwnerPendency = (AuctionPendency)reader.ReadByte();
					auction.m_BuyerPendency = (AuctionPendency)reader.ReadByte();
					auction.m_OwnerMessage = (AuctionMessage)reader.ReadByte();
					auction.m_BuyerMessage = (AuctionMessage)reader.ReadByte();
					auction.PendingEnd = reader.ReadDeltaTime();

					int count = reader.ReadInt();
					auction.Items = new ItemInfo[count];

					for (int i = 0; i < count; i++)
					{
						auction.Items[i] = ItemInfo.Deserialize(reader, version);
					}

					count = reader.ReadInt();

					for (int i = 0; i < count; i++)
					{
						auction.Bids.Add(Bid.Deserialize(reader, version));
					}

					break;
			}

			return auction;
		}

		[CommandProperty(AccessLevel.Administrator)]
		/// <summary>
		/// Gets the minimum increment required for the auction
		/// </summary>
		public int BidIncrement
		{
			get
			{
				if (MinBid <= 100)
					return 10;

				if (MinBid <= 500)
					return 20;

				if (MinBid <= 1000)
					return 50;

				if (MinBid <= 5000)
					return 100;

				if (MinBid <= 10000)
					return 200;

				if (MinBid <= 20000)
					return 250;

				if (MinBid <= 50000)
					return 500;

				return 1000;
			}
		}

		[CommandProperty(AccessLevel.Administrator)]
		/// <summary>
		/// States whether an item has at least one bid
		/// </summary>
		public bool HasBids
		{
			get { return Bids.Count > 0; }
		}

		/// <summary>
		///     Gets the highest bid for this item
		/// </summary>
		public Bid HighestBid
		{
			get
			{
				if (Bids.Count > 0)
					return Bids[0] as Bid;
				return null;
			}
		}

		[CommandProperty(AccessLevel.Administrator)]
		public Mobile HighestBidder
		{
			get
			{
				if (Bids.Count > 0)
					return (Bids[0] as Bid).Mobile;
				return null;
			}
		}

		[CommandProperty(AccessLevel.Administrator)]
		public int HighestBidValue
		{
			get
			{
				if (Bids.Count > 0)
					return (Bids[0] as Bid).Amount;
				return 0;
			}
		}

		[CommandProperty(AccessLevel.Administrator)]
		/// <summary>
		/// States whether the reserve has been met for this item
		/// </summary>
		public bool ReserveMet
		{
			get { return HighestBid != null && HighestBid.Amount >= Reserve; }
		}

		[CommandProperty(AccessLevel.Administrator)]
		/// <summary>
		/// States whether this auction has expired
		/// </summary>
		public bool Expired
		{
			get { return DateTime.UtcNow > m_EndTime; }
		}

		/// <summary>
		///     Gets the minimum bid that a player can place
		/// </summary>
		public int MinNewBid
		{
			get
			{
				if (HighestBid != null)
					return HighestBid.Amount;
				return MinBid;
			}
		}

		[CommandProperty(AccessLevel.Administrator)]
		/// <summary>
		/// Gets the next deadline required by this auction
		/// </summary>
		public DateTime Deadline
		{
			get
			{
				if (!Expired)
				{
					return m_EndTime;
				}

				if (Pending)
				{
					return PendingEnd;
				}

				return DateTime.MaxValue;
			}
		}

		[CommandProperty(AccessLevel.Administrator)]
		/// <summary>
		/// Specifies if the pending period has timed out
		/// </summary>
		public bool PendingExpired
		{
			get
			{
				return DateTime.UtcNow >= PendingEnd;
			}
		}

		[CommandProperty(AccessLevel.Administrator)]
		/// <summary>
		/// Gets the time left before the pending period expired
		/// </summary>
		public TimeSpan PendingTimeLeft
		{
			get { return PendingEnd - DateTime.UtcNow; }
		}

		[CommandProperty(AccessLevel.Administrator)]
		/// <summary>
		/// States whether this auction is selling a pet
		/// </summary>
		public bool Creature
		{
			get { return Item is MobileStatuette; }
		}

		[CommandProperty(AccessLevel.Administrator)]
		/// <summary>
		/// Gets the BaseCreature sold through this auction. This will be null when selling an item.
		/// </summary>
		public BaseCreature Pet
		{
			get
			{
				if (Creature)
				{
					return ((MobileStatuette)Item).ShrunkenPet;
				}

				return null;
			}
		}

		/// <summary>
		///     Creates a new AuctionItem
		/// </summary>
		/// <param name="item">The item being sold</param>
		/// <param name="owner">The owner of the item</param>
		public AuctionItem(Item item, Mobile owner)
		{
			m_ID = Guid.NewGuid();
			Item = item;
			Owner = owner;
			Bids = new ArrayList();

			if (!Creature)
			{
				Item.Visible = false;
				Owner.SendMessage(AuctionConfig.MessageHue, ITEM_NOT_FOUND);
			}

			// Item name
			if (Item.Name != null && Item.Name.Length > 0)
			{
				ItemName = Item.Name;
			}

			if (Item.Amount > 1)
			{
				ItemName = String.Format("{0} {1}", Item.Amount.ToString("#,0"), ItemName);
			}
		}

		/// <summary>
		///     Creates an AuctionItem - for use in deserialization
		/// </summary>
		private AuctionItem()
		{
			Bids = new ArrayList();
		}

		[IndexerName("SoldItem")]
		/// <summary>
		/// Gets the item info corresponding to the index value
		/// </summary>
		public ItemInfo this[int index]
		{
			get
			{
				if (index > -1 && index < Items.Length)
				{
					return Items[index];
				}

				return null;
			}
		}

		/// <summary>
		///     Confirms the auction item and adds it into the system
		/// </summary>
		public void Confirm()
		{
			m_StartTime = DateTime.UtcNow;
			m_EndTime = m_StartTime + m_Duration;

			if (Creature && Pet != null)
			{
				Pet.ControlTarget = null;
				Pet.ControlOrder = OrderType.Stay;
				Pet.Internalize();

				Pet.SetControlMaster(null);
				Pet.SummonMaster = null;
			}

			// Calculate all the ItemInfo
			if (Item is Container && Item.Items.Count > 0)
			{
				// Container with items
				Items = new ItemInfo[Item.Items.Count];

				for (int i = 0; i < Items.Length; i++)
				{
					Items[i] = new ItemInfo(Item.Items[i]);
				}
			}
			else
			{
				Items = new ItemInfo[1];

				Items[0] = new ItemInfo(Item);
			}

			AuctionSystem.Add(this);
			AuctionScheduler.UpdateDeadline(m_EndTime);

			AuctionLog.WriteNewAuction(this);
		}

		/// <summary>
		///     Cancels the new auction and returns the item to the owner
		/// </summary>
		public void Cancel()
		{
			if (Creature)
			{
				(Item as MobileStatuette)?.GiveCreatureTo(Owner);
				Owner.SendMessage(AuctionConfig.MessageHue, AUCTION_CANCELED_PET);
			}
			else
			{
				Item.Visible = true;
				Owner.SendMessage(AuctionConfig.MessageHue, AUCTION_CANCELED_ITEM);
			}
		}

		/// <summary>
		///     Sends the associated web link to a mobile
		/// </summary>
		/// <param name="m">The mobile that should receive the web link</param>
		public void SendLinkTo(Mobile m)
		{
			if (m != null && m.NetState != null)
			{
				if (m_WebLink != null && m_WebLink.Length > 0)
				{
					m.LaunchBrowser(String.Format("http://{0}", m_WebLink));
				}
			}
		}

		/// <summary>
		///     Verifies if a mobile can place a bid on this item
		/// </summary>
		/// <param name="m">The Mobile trying to bid</param>
		/// <returns>True if the mobile is allowed to bid</returns>
		public bool CanBid(Mobile m)
		{
			if (m.AccessLevel > AccessLevel.Player)
				return false; // Staff shoudln't bid. This will also give the bids view to staff members.

			if (this.Account == (m.Account as Account)) // Same account as auctioneer
				return false;

			if (Creature)
			{
				return (Pet != null && Pet.CanBeControlledBy(m));
			}

			return true;
		}

		/// <summary>
		///     Verifies if a mobile is the owner of this auction (checks accounts)
		/// </summary>
		/// <param name="m">The mobile being checked</param>
		/// <returns>True if the mobile is the owner of the auction</returns>
		public bool IsOwner(Mobile m)
		{
			return (this.Account == (m.Account as Account));
		}

		/// <summary>
		///     Places a new bid
		/// </summary>
		/// <param name="from">The Mobile bidding</param>
		/// <param name="amount">The bid amount</param>
		/// <returns>True if the bid has been added and accepted</returns>
		public bool PlaceBid(Mobile from, int amount)
		{
			if (!CanBid(from))
				return false;

			if (HighestBid != null)
			{
				if (amount <= HighestBid.Amount)
				{
					from.SendMessage(AuctionConfig.MessageHue, ERR_BID_LOW);
					return false;
				}
			}
			else if (amount <= MinBid)
			{
				from.SendMessage(AuctionConfig.MessageHue, ERR_BID_MIN);
				return false;
			}

			int delta = 0;

			if (HighestBid != null)
				delta = amount - HighestBid.Amount;
			else
				delta = amount - MinBid;

			if (BidIncrement > delta)
			{
				from.SendMessage(AuctionConfig.MessageHue, ERR_BID_INCREMENT_FMT, BidIncrement);
				return false;
			}

			// Ok, do bid
			Bid bid = Bid.CreateBid(from, amount);

			if (bid != null)
			{
				if (HighestBid != null)
				{
					HighestBid.Outbid(this); // Return money to previous highest bidder
				}

				Bids.Insert(0, bid);
				AuctionLog.WriteBid(this);

				// Check for auction extension
				if (AuctionConfig.LateBidExtention > TimeSpan.Zero)
				{
					TimeSpan timeLeft = m_EndTime - DateTime.UtcNow;

					if (timeLeft < TimeSpan.FromMinutes(5.0))
					{
						m_EndTime += AuctionConfig.LateBidExtention;
						bid.Mobile.SendMessage(AuctionConfig.MessageHue, LATE_BID_EXTENSION);
					}
				}
			}

			return bid != null;
		}

		/// <summary>
		///     Forces the end of the auction when the item or creature has been deleted
		/// </summary>
		public void EndInvalid()
		{
			AuctionSystem.Auctions.Remove(this);

			if (HighestBid != null)
			{
				AuctionGoldCheck gold = new AuctionGoldCheck(this, AuctionResult.ItemDeleted);
				GiveItemTo(HighestBid.Mobile, gold);
			}

			// The item has been deleted, no need to return it to the owner.
			// If it's a statuette, delete it
			if (Creature && Item != null)
			{
				Item.Delete();
			}

			AuctionLog.WriteEnd(this, AuctionResult.ItemDeleted, null, null);

			// Over.
		}

		/// <summary>
		///     Forces the end of the auction and removes it from the system
		/// </summary>
		/// <param name="m">The staff member deleting the auction</param>
		/// <param name="itemfate">Specifies what should occur with the item</param>
		public void StaffDelete(Mobile m, ItemFate itemfate)
		{
			if (AuctionSystem.Auctions.Contains(this))
				AuctionSystem.Auctions.Remove(this);
			else if (AuctionSystem.Pending.Contains(this))
				AuctionSystem.Pending.Remove(this);

			if (HighestBid != null)
			{
				AuctionGoldCheck gold = new AuctionGoldCheck(this, AuctionResult.StaffRemoved);
				GiveItemTo(HighestBid.Mobile, gold);
			}

			AuctionItemCheck check = new AuctionItemCheck(this, AuctionResult.StaffRemoved);
			string comments = null;

			switch (itemfate)
			{
				case ItemFate.Delete:

					check.ForceDelete();
					comments = "The item has been deleted";
					break;

				case ItemFate.ReturnToOwner:

					GiveItemTo(Owner, check);
					comments = "The item has been returned to the owner";
					break;

				case ItemFate.ReturnToStaff:

					GiveItemTo(m, check);
					comments = "The item has been claimed by the staff";
					break;
			}

			AuctionLog.WriteEnd(this, AuctionResult.StaffRemoved, m, comments);

			// OVer.
		}

		/// <summary>
		///     Ends the auction.
		///     This function is called by the auction system during its natural flow
		/// </summary>
		/// <param name="m">The Mobile eventually forcing the ending</param>
		public void End(Mobile m)
		{
			AuctionSystem.Auctions.Remove(this);

			if (HighestBid == null)
			{
				// No bids, simply return the item
				AuctionCheck item = new AuctionItemCheck(this, AuctionResult.NoBids);
				GiveItemTo(Owner, item);

				// Over, this auction no longer exists
				AuctionLog.WriteEnd(this, AuctionResult.NoBids, m, null);
			}
			else
			{
				// Verify that all items still exist too, otherwise make it pending
				if (IsValid() && ReserveMet)
				{
					// Auction has been succesful
					AuctionCheck item = new AuctionItemCheck(this, AuctionResult.Succesful);
					GiveItemTo(HighestBid.Mobile, item);

					AuctionCheck gold = new AuctionGoldCheck(this, AuctionResult.Succesful);
					GiveItemTo(Owner, gold);

					// Over, this auction no longer exists
					AuctionLog.WriteEnd(this, AuctionResult.Succesful, m, null);
				}
				else
				{
					// Reserve hasn't been met or auction isn't valid, this auction is pending
					Pending = true;
					PendingEnd = DateTime.UtcNow + TimeSpan.FromDays(AuctionConfig.DaysForConfirmation);
					AuctionSystem.Pending.Add(this);

					DoOwnerMessage();
					DoBuyerMessage();

					Mobile owner = GetOnlineMobile(Owner);
					Mobile buyer = GetOnlineMobile(HighestBid.Mobile);

					SendMessage(owner);
					SendMessage(buyer);

					AuctionScheduler.UpdateDeadline(PendingEnd);

					AuctionLog.WritePending(this, ReserveMet ? "Item deleted" : "Reserve not met");
				}
			}
		}

		/// <summary>
		///     Gets the online mobile belonging to a mobile's account
		/// </summary>
		private Mobile GetOnlineMobile(Mobile m)
		{
			if (m == null || m.Account == null)
				return null;

			if (m.NetState != null)
				return m;

			Account acc = m.Account as Account;

			for (int i = 0; i < 5; i++)
			{
				Mobile mob = acc[i];

				if (mob != null && mob.NetState != null)
				{
					return mob;
				}
			}

			return null;
		}

		/// <summary>
		///     Ends the auction.
		///     This function is called when the system is being disbanded and all auctions must be forced close
		///     The item will be returned to the original owner, and the highest bidder will receive the money back
		/// </summary>
		public void ForceEnd()
		{
			AuctionSystem.Auctions.Remove(this);

			// Turn the item into a deed and give it to the auction owner
			AuctionCheck item = new AuctionItemCheck(this, AuctionResult.SystemStopped);

			if (item != null)
				GiveItemTo(Owner, item); // This in case the item has been wiped or whatever

			if (HighestBid != null)
			{
				HighestBid.AuctionCanceled(this);
			}

			AuctionLog.WriteEnd(this, AuctionResult.SystemStopped, null, null);
		}

		/// <summary>
		///     This function will put an item in a player's backpack, and if full put it inside their bank.
		///     If the mobile is null, this will delete the item.
		/// </summary>
		/// <param name="m">The mobile receiving the item</param>
		/// <param name="item">The item being given</param>
		private static void GiveItemTo(Mobile m, Item item)
		{
			if (m == null || item == null)
			{
				if (item != null)
					item.Delete();

				return;
			}

			if (m.Backpack == null || !m.Backpack.TryDropItem(m, item, false))
			{
				if (m.BankBox != null)
				{
					m.BankBox.AddItem(item);
				}
				else
				{
					item.Delete(); // Sucks to be you
				}
			}
		}

		/// <summary>
		///     Verifies if all the items being sold through this auction still exist
		/// </summary>
		/// <returns>True if all the items still exist</returns>
		public bool IsValid()
		{
			bool valid = true;

			foreach (ItemInfo info in Items)
			{
				if (info.Item == null)
					valid = false;
			}

			return valid;
		}

		/// <summary>
		///     Defines what kind of message the auction owner should receive. Doesn't send any messages.
		/// </summary>
		public void DoOwnerMessage()
		{
			if (Owner == null || Owner.Account == null)
			{
				// If owner deleted the character, accept the auction by default
				m_OwnerPendency = AuctionPendency.Accepted;
			}
			else if (!IsValid() && ReserveMet)
			{
				// Assume the owner will sell even if invalid when reserve is met
				m_OwnerPendency = AuctionPendency.Accepted;
			}
			else if (!ReserveMet)
			{
				m_OwnerPendency = AuctionPendency.Pending;
				m_OwnerMessage = AuctionMessage.Response; // This is always reserve not met for the owner
			}
			else if (!IsValid())
			{
				m_OwnerPendency = AuctionPendency.Accepted;
				m_OwnerMessage = AuctionMessage.Information; // This is always about validty for the owner
			}
		}

		/// <summary>
		///     Defines what kind of message the buyer should receive. Doesn't send any messages.
		/// </summary>
		public void DoBuyerMessage()
		{
			if (HighestBid.Mobile == null || HighestBid.Mobile.Account == null)
			{
				// Buyer deleted the character, accept the auction by default
				m_BuyerPendency = AuctionPendency.Accepted;
			}
			else if (!IsValid())
			{
				// Send the buyer a message about missing items in the auction
				m_BuyerMessage = AuctionMessage.Response;
				m_BuyerPendency = AuctionPendency.Pending;
			}
			else if (!ReserveMet)
			{
				// Assume the buyer will buy even if the reserve hasn't been met
				m_BuyerPendency = AuctionPendency.Accepted;
				// Send the buyer a message to inform them of the reserve issue
				m_BuyerMessage = AuctionMessage.Information;
			}
		}

		/// <summary>
		///     Validates the pending status of the auction. This method should be called whenever a pendency
		///     value is changed. If the auction has been validated, it will finalize items and remove the auction from the system.
		///     This is the only method that should be used to finalize a pending auction.
		/// </summary>
		public void Validate()
		{
			if (!AuctionSystem.Pending.Contains(this))
				return;

			if (m_OwnerPendency == AuctionPendency.Accepted && m_BuyerPendency == AuctionPendency.Accepted)
			{
				// Both parts confirmed the auction
				Pending = false;
				AuctionSystem.Pending.Remove(this);

				AuctionCheck item = new AuctionItemCheck(this, AuctionResult.PendingAccepted);
				AuctionCheck gold = new AuctionGoldCheck(this, AuctionResult.PendingAccepted);

				if (item != null)
				{
					GiveItemTo(HighestBid.Mobile, item); // Item to buyer
				}

				GiveItemTo(Owner, gold); // Gold to owner

				// Over, this auction no longer exists
				AuctionLog.WriteEnd(this, AuctionResult.PendingAccepted, null, null);
			}
			else if (m_OwnerPendency == AuctionPendency.NotAccepted || m_BuyerPendency == AuctionPendency.NotAccepted)
			{
				// At least one part refused
				Pending = false;
				AuctionSystem.Pending.Remove(this);

				AuctionCheck item = new AuctionItemCheck(this, AuctionResult.PendingRefused);
				AuctionCheck gold = new AuctionGoldCheck(this, AuctionResult.PendingRefused);

				if (item != null)
				{
					GiveItemTo(Owner, item); // Give item back to owner
				}

				GiveItemTo(HighestBid.Mobile, gold); // Give gold to highest bidder

				// Over, this auction no longer exists
				AuctionLog.WriteEnd(this, AuctionResult.PendingRefused, null, null);
			}
		}

		/// <summary>
		///     Sends any message this auction might have in store for a given mobile
		/// </summary>
		/// <param name="to">The Mobile logging into the server</param>
		public void SendMessage(Mobile to)
		{
			if (!Pending || to == null)
				return;

			if (to == Owner || (Owner != null && to.Account.Equals(Owner.Account)))
			{
				// This is the owner loggin in
				if (this.m_OwnerMessage != AuctionMessage.None)
				{
					// Owner needs a message
					if (m_OwnerMessage == AuctionMessage.Information)
					{
						// Send information message about validity condition
						AuctionMessaging.SendInvalidMessageToOwner(this);
					}
					else if (m_OwnerMessage == AuctionMessage.Response)
					{
						// Send reserve not met confirmation request
						AuctionMessaging.SendReserveMessageToOwner(this);
					}
				}
			}
			else if (to == HighestBid.Mobile ||
			         (HighestBid.Mobile != null && to.Account.Equals(HighestBid.Mobile.Account)))
			{
				// This is the buyer logging in
				if (m_BuyerMessage != AuctionMessage.None)
				{
					// Buyer should receive a message
					if (m_BuyerMessage == AuctionMessage.Information)
					{
						// Send message about reserve not met condition
						AuctionMessaging.SendReserveMessageToBuyer(this);
					}
					else if (m_BuyerMessage == AuctionMessage.Response)
					{
						// Send request to confirm invalid items auction
						AuctionMessaging.SendInvalidMessageToBuyer(this);
					}
				}
			}
		}

		/// <summary>
		///     Confirms an information message
		/// </summary>
		/// <param name="owner">True if the message was sent to the owner, false if to the buyer</param>
		public void ConfirmInformationMessage(bool owner)
		{
			if (owner)
			{
				// Owner
				m_OwnerMessage = AuctionMessage.None; // Don't resent
			}
			else
			{
				// Buyer
				m_BuyerMessage = AuctionMessage.None;
			}
		}

		/// <summary>
		///     Gives a response to a message
		/// </summary>
		/// <param name="owner">True if the message was sent to the owner, false if to the buyer</param>
		/// <param name="ok">The response to the message</param>
		public void ConfirmResponseMessage(bool owner, bool ok)
		{
			if (owner)
			{
				if (ok)
				{
					m_OwnerPendency = AuctionPendency.Accepted;
				}
				else
				{
					m_OwnerPendency = AuctionPendency.NotAccepted;
				}
			}
			else
			{
				if (ok)
				{
					m_BuyerPendency = AuctionPendency.Accepted;
				}
				else
				{
					m_BuyerPendency = AuctionPendency.NotAccepted;
				}
			}

			Validate();
		}

		/// <summary>
		///     The pending period has timed out and the auction must end unsuccesfully
		/// </summary>
		public void PendingTimeOut()
		{
			AuctionSystem.Pending.Remove(this);

			m_OwnerPendency = AuctionPendency.NotAccepted;
			m_BuyerPendency = AuctionPendency.NotAccepted;
			m_OwnerMessage = AuctionMessage.None;
			m_BuyerMessage = AuctionMessage.None;

			AuctionCheck item = new AuctionItemCheck(this, AuctionResult.PendingTimedOut);
			AuctionCheck gold = new AuctionGoldCheck(this, AuctionResult.PendingTimedOut);

			if (item != null)
				GiveItemTo(Owner, item);
			GiveItemTo(HighestBid.Mobile, gold);

			// Over, this auction no longer exists
			AuctionLog.WriteEnd(this, AuctionResult.PendingTimedOut, null, null);
		}

		/// <summary>
		///     Verifies is a mobile has bid on this auction
		/// </summary>
		public bool MobileHasBids(Mobile m)
		{
			ArrayList bids = new ArrayList(Bids);

			foreach (Bid bid in bids)
			{
				if (bid.Mobile == m)
					return true;
			}

			return false;
		}

		/// <summary>
		///     Outputs relevant information about this auction
		/// </summary>
		public void Profile(StreamWriter writer)
		{
			writer.WriteLine("ID : {0}", m_ID);
			writer.WriteLine("Name : {0}", ItemName);

			if (Owner != null && Owner.Account != null)
				writer.WriteLine("Owner : {0} [ Account {1} - Serial {2} ]",
					Owner.Name,
					(Owner.Account as Account).Username,
					Owner.Serial);
			else
				writer.WriteLine("Owner : no longer existing");

			writer.WriteLine("Starting bid: {0}", MinBid);
			writer.WriteLine("Reserve : {0}", Reserve);

			writer.WriteLine("Created on {0} at {1}", m_StartTime.ToShortDateString(), m_StartTime.ToShortTimeString());
			writer.WriteLine("Duration: {0}", m_Duration.ToString());
			writer.WriteLine("End Time: {0} at {1}", m_EndTime.ToShortDateString(), m_EndTime.ToShortTimeString());

			writer.WriteLine("Expired : {0}", Expired.ToString());
			writer.WriteLine("Pending : {0}", Pending.ToString());
			writer.WriteLine("Next Deadline : {0} at {1}", Deadline.ToShortDateString(), Deadline.ToShortTimeString());

			writer.WriteLine();

			if (Creature)
			{
				writer.WriteLine("** This auction is selling a pet");

				// Pet
				if (Item != null && Pet != null)
				{
					writer.WriteLine("Creature: {0}", Pet.Serial);
					writer.WriteLine("Statuette : {0}", Item.Serial);
					writer.WriteLine("Type : {0}", Item.Name);
				}
				else
				{
					writer.WriteLine("Pet deleted, this auction is invalid");
				}
			}
			else
			{
				// Items
				writer.WriteLine("{0} Items", Items.Length);

				foreach (ItemInfo item in Items)
				{
					writer.Write("- {0}", item.Name);

					if (item.Item != null)
						writer.WriteLine(" [{0}]", item.Item.Serial);
					else
						writer.WriteLine(" [Deleted]");
				}
			}

			writer.WriteLine();
			writer.WriteLine("{0} Bids", Bids.Count);

			foreach (Bid bid in Bids)
			{
				bid.Profile(writer);
			}

			writer.WriteLine();
		}

		/// <summary>
		///     Attempts to buy now
		/// </summary>
		/// <param name="m">The user trying to purchase</param>
		/// <returns>True if the item has been sold</returns>
		public bool DoBuyNow(Mobile m)
		{
			if (!Banker.Withdraw(m, BuyNow))
			{
				m.SendMessage(AuctionConfig.MessageHue, ERR_BUY_NOW_NOT_ENOUGH_MONEY);
				return false;
			}

			AuctionSystem.Auctions.Remove(this);

			if (HighestBid != null)
			{
				HighestBid.Outbid(this);
			}

			// Simulate bid
			Bid bid = new Bid(m, BuyNow);
			Bids.Insert(0, bid);

			AuctionGoldCheck gold = new AuctionGoldCheck(this, AuctionResult.BuyNow);
			AuctionItemCheck item = new AuctionItemCheck(this, AuctionResult.BuyNow);

			GiveItemTo(m, item);
			GiveItemTo(Owner, gold);

			// Over.
			AuctionLog.WriteEnd(this, AuctionResult.BuyNow, m, null);

			return true;
		}

		/// <summary>
		///     Verifies if the eventual pets in this auction are gone
		/// </summary>
		public void VeirfyIntergrity()
		{
			foreach (ItemInfo ii in this.Items)
				ii.VeirfyIntegrity();
		}
	}
}
