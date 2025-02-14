//
//	Auction version 2.1, by Xanthos and Arya
//
//  Based on original ideas and code by Arya
//

using System.Collections;
using Server;
using Server.Network;
using static Arya.Auction.AuctionMessages;

namespace Arya.Auction
{
	/// <summary>
	///     This is the auction control stone. This item should NOT be deleted
	/// </summary>
	public class AuctionControl : Item
	{
		/// <summary>
		///     This item holds all the current auctions
		/// </summary>
		private ArrayList m_Auctions;

		/// <summary>
		///     This lists all auctions whose reserve hasn't been met
		/// </summary>
		private ArrayList m_Pending;

		/// <summary>
		///     Flag used to force the deletion of the system
		/// </summary>
		private bool m_Delete;

		/// <summary>
		///     Gets or sets the list of current auction entries
		/// </summary>
		public ArrayList Auctions
		{
			get { return m_Auctions; }
			set { m_Auctions = value; }
		}

		/// <summary>
		///     Gets or sets the pending auction entries
		/// </summary>
		public ArrayList Pending
		{
			get { return m_Pending; }
			set { m_Pending = value; }
		}

		/// <summary>
		///     The max number of concurrent auctions for each account
		/// </summary>
		[CommandProperty(AccessLevel.GameMaster, AccessLevel.Administrator)]
		/// <summary>
		/// Gets or sets the max number of auctions a single account can have
		/// </summary>
		public int MaxAuctionsParAccount { get; set; } = 5;

		/// <summary>
		///     The minimum number of days an auction must last
		/// </summary>
		[CommandProperty(AccessLevel.GameMaster, AccessLevel.Administrator)]
		/// <summary>
		/// Gets or sets the minimum days an auction must last
		/// </summary>
		public int MinAuctionDays { get; set; } = 1;

		/// <summary>
		///     The max number of days an auction can last
		/// </summary>
		[CommandProperty(AccessLevel.GameMaster, AccessLevel.Administrator)]
		/// <summary>
		/// Gets or sets the max number of days an auction can last
		/// </summary>
		public int MaxAuctionDays { get; set; } = 14;

		public AuctionControl() : base(4484)
		{
			Name = "Auction System";
			Visible = false;
			Movable = false;
			m_Auctions = new ArrayList();
			m_Pending = new ArrayList();

			AuctionSystem.ControlStone = this;
		}

		public AuctionControl(Serial serial) : base(serial)
		{
			m_Auctions = new ArrayList();
			m_Pending = new ArrayList();
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(1); // Version

			// Version 1 : changes in AuctionItem
			// Version 0
			writer.Write(MaxAuctionsParAccount);
			writer.Write(MinAuctionDays);
			writer.Write(MaxAuctionDays);

			writer.Write(m_Auctions.Count);

			foreach (AuctionItem auction in m_Auctions)
			{
				auction.Serialize(writer);
			}

			writer.Write(m_Pending.Count);

			foreach (AuctionItem auction in m_Pending)
			{
				auction.Serialize(writer);
			}
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			switch (version)
			{
				case 1:
				case 0:
					MaxAuctionsParAccount = reader.ReadInt();
					MinAuctionDays = reader.ReadInt();
					MaxAuctionDays = reader.ReadInt();

					int count = reader.ReadInt();

					for (int i = 0; i < count; i++)
					{
						m_Auctions.Add(AuctionItem.Deserialize(reader, version));
					}

					count = reader.ReadInt();

					for (int i = 0; i < count; i++)
					{
						m_Pending.Add(AuctionItem.Deserialize(reader, version));
					}

					break;
			}

			AuctionSystem.ControlStone = this;
		}

		public override void OnDelete()
		{
			// Don't allow users to delete this item unless it's done through the control gump
			if (!m_Delete)
			{
				AuctionControl newStone = new AuctionControl();
				newStone.m_Auctions.AddRange(this.m_Auctions);
				newStone.MoveToWorld(this.Location, this.Map);

				newStone.Items.AddRange(Items);
				Items.Clear();
				foreach (Item item in newStone.Items)
				{
					item.Parent = newStone;
				}

				newStone.PublicOverheadMessage(MessageType.Regular, 0x40, false, AUCTION_CONTROL_MOVE_WARNING);
			}

			base.OnDelete();
		}

		/// <summary>
		///     Deletes the item from the world without triggering the auto-recreation
		///     This function also closes all current auctions
		/// </summary>
		public void ForceDelete()
		{
			m_Delete = true;
			Delete();
		}

		public override void GetProperties(ObjectPropertyList list)
		{
			base.GetProperties(list);

			list.Add(AuctionSystem.Running ? 3005117 : 3005118); // [Active] - [Inactive]
		}
	}
}
