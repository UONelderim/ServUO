using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using Server;
using Server.Accounting;
using Server.Items;
using Server.Mobiles;
using static Arya.Auction.AuctionMessages;

namespace Arya.Auction
{
	public enum AuctionPendency : byte
	{
		Pending = 0,
		Accepted = 1,
		NotAccepted = 2
	}

	public enum ItemFate
	{
		ReturnToOwner,
		ReturnToStaff,
		Delete
	}

	public enum AuctionMessage : byte
	{
		None = 0,
		Information = 1,
		Response = 2
	}

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

				if (item.Amount > 1)
				{
					m_Name = $"{item.Amount:#,0} {m_Name}";
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

			public void VeirfyIntegrity()
			{
				if (Item is MobileStatuette shrinkItem && shrinkItem.ShrunkenPet == null)
				{
					Item.Delete();
					Item = null; // This will make this item invalid
				}
			}
		}

		private static string GetItemProperties(Item item)
		{
			if (item == null || item.PropertyList == null)
			{
				return NA;
			}

			ObjectPropertyList plist = new ObjectPropertyList(item);
			item.GetProperties(plist);

			byte[] data = plist.Stream.ToArray();
			List<string> list = [];

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
		public bool AllowBuyNow => BuyNow > 0;

		[CommandProperty(AccessLevel.Administrator)]
		public int BuyNow { get; set; }

		[CommandProperty(AccessLevel.Administrator)]
		public DateTime PendingEnd { get; private set; }

		[CommandProperty(AccessLevel.Administrator)]
		public Item Item { get; private set; }

		[CommandProperty(AccessLevel.Administrator)]
		public Mobile Owner { get; private set; }

		[CommandProperty(AccessLevel.Administrator)]
		public DateTime StartTime => m_StartTime;

		[CommandProperty(AccessLevel.Administrator)]
		public DateTime EndTime => m_EndTime;

		public TimeSpan Duration
		{
			get => m_Duration;
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
		public TimeSpan TimeLeft => m_EndTime - DateTime.UtcNow;

		public int MinBid { get; set; } = 1000;

		public int Reserve { get; set; } = 2000;

		[CommandProperty(AccessLevel.Administrator)]
		public string Description { get; set; } = "";

		[CommandProperty(AccessLevel.Administrator)]
		public string WebLink
		{
			get => m_WebLink;
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

		public List<Bid> Bids { get; set; }

		[CommandProperty(AccessLevel.Administrator)]
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
		public string ItemName { get; set; }

		[CommandProperty(AccessLevel.Administrator)]
		public bool Pending { get; private set; }
		public ItemInfo[] Items { get; private set; }

		[CommandProperty(AccessLevel.Administrator)]
		public int ItemCount => Items.Length;

		[CommandProperty(AccessLevel.Administrator)]
		public Guid ID => m_ID;

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
		public bool HasBids => Bids.Count > 0;

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
		public bool ReserveMet => HighestBid != null && HighestBid.Amount >= Reserve;

		[CommandProperty(AccessLevel.Administrator)]
		public bool Expired => DateTime.UtcNow > m_EndTime;

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
		public bool PendingExpired => DateTime.UtcNow >= PendingEnd;

		[CommandProperty(AccessLevel.Administrator)]
		public TimeSpan PendingTimeLeft => PendingEnd - DateTime.UtcNow;

		[CommandProperty(AccessLevel.Administrator)]
		public bool Creature => Item is MobileStatuette;

		[CommandProperty(AccessLevel.Administrator)]
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

		public AuctionItem(Item item, Mobile owner)
		{
			m_ID = Guid.NewGuid();
			Item = item;
			Owner = owner;
			Bids = [];

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

		private AuctionItem()
		{
			Bids = [];
		}

		[IndexerName("SoldItem")]
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

		public void SendLinkTo(Mobile m)
		{
			if (m != null && m.NetState != null)
			{
				if (m_WebLink != null && m_WebLink.Length > 0)
				{
					m.LaunchBrowser($"http://{m_WebLink}");
				}
			}
		}

		public bool CanBid(Mobile m)
		{
			if (m.AccessLevel > AccessLevel.Player)
				return false; // Staff shoudln't bid. This will also give the bids view to staff members.

			if (Account == (m.Account as Account)) // Same account as auctioneer
				return false;

			if (Creature)
			{
				return (Pet != null && Pet.CanBeControlledBy(m));
			}

			return true;
		}

		public bool IsOwner(Mobile m)
		{
			return (Account == (m.Account as Account));
		}

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
		}

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
					comments = LOG_ITEM_DELETED;
					break;

				case ItemFate.ReturnToOwner:

					GiveItemTo(Owner, check);
					comments = LOG_ITEM_RETURN_OWNER;
					break;

				case ItemFate.ReturnToStaff:

					GiveItemTo(m, check);
					comments = LOG_ITEM_RETURN_STAFF;
					break;
			}

			AuctionLog.WriteEnd(this, AuctionResult.StaffRemoved, m, comments);
		}

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

		public void SendMessage(Mobile to)
		{
			if (!Pending || to == null)
				return;

			if (to == Owner || (Owner != null && to.Account.Equals(Owner.Account)))
			{
				// This is the owner loggin in
				if (m_OwnerMessage != AuctionMessage.None)
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

		public void ConfirmInformationMessage(bool owner)
		{
			if (owner)
			{
				m_OwnerMessage = AuctionMessage.None; // Don't resent
			}
			else
			{
				m_BuyerMessage = AuctionMessage.None;
			}
		}

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

		public bool MobileHasBids(Mobile m)
		{
			foreach (Bid bid in Bids)
			{
				if (bid.Mobile == m)
					return true;
			}

			return false;
		}

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

			AuctionLog.WriteEnd(this, AuctionResult.BuyNow, m, null);

			return true;
		}
		
		public void VeirfyIntergrity()
		{
			foreach (var ii in Items)
				ii.VeirfyIntegrity();
		}
	}
}
