using System.Collections.Generic;
using Server;
using Server.Network;
using static Arya.Auction.AuctionMessages;

namespace Arya.Auction
{
	public class AuctionControl : Item
	{
		private bool m_Delete;
		public List<AuctionItem> Auctions { get; set; }

		public List<AuctionItem> Pending { get; set; }

		[CommandProperty(AccessLevel.GameMaster, AccessLevel.Administrator)]
		public int MaxAuctionsParAccount { get; set; } = 5;

		[CommandProperty(AccessLevel.GameMaster, AccessLevel.Administrator)]
		public int MinAuctionDays { get; set; } = 1;

		[CommandProperty(AccessLevel.GameMaster, AccessLevel.Administrator)]
		public int MaxAuctionDays { get; set; } = 14;

		public AuctionControl() : base(4484)
		{
			Name = "Auction System";
			Visible = false;
			Movable = false;
			Auctions = [];
			Pending = [];

			AuctionSystem.ControlStone = this;
		}

		public AuctionControl(Serial serial) : base(serial)
		{
			Auctions = [];
			Pending = [];
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(1); // Version

			writer.Write(MaxAuctionsParAccount);
			writer.Write(MinAuctionDays);
			writer.Write(MaxAuctionDays);

			writer.Write(Auctions.Count);

			foreach (AuctionItem auction in Auctions)
			{
				auction.Serialize(writer);
			}

			writer.Write(Pending.Count);

			foreach (AuctionItem auction in Pending)
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
						Auctions.Add(AuctionItem.Deserialize(reader, version));
					}

					count = reader.ReadInt();

					for (int i = 0; i < count; i++)
					{
						Pending.Add(AuctionItem.Deserialize(reader, version));
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
				newStone.Auctions.AddRange(this.Auctions);
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
