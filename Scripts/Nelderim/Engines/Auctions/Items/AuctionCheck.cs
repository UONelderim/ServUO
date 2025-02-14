using System;
using Server;

namespace Arya.Auction
{
	public enum AuctionResult
	{
		Succesful,
		NoBids,
		ReserveNotMet,
		Outbid,
		PendingRefused,
		PendingAccepted,
		PendingTimedOut,
		SystemStopped,
		ItemDeleted,
		StaffRemoved,
		BuyNow
	}

	public abstract class AuctionCheck : Item
	{
		protected Guid m_Auction;
		protected string m_Message;
		protected string m_ItemName;
		protected Mobile m_Owner;

		public string Message => m_Message;

		public AuctionItem Auction => AuctionSystem.Find(m_Auction);

		public bool Delivered { get; private set; }

		public string HtmlDetails => $"<basefont color=#FFFFFF>{m_Message}";

		public abstract string ItemName
		{
			get;
		}

		public AuctionCheck() : base(5360)
		{
			LootType = LootType.Blessed;
			Delivered = false;
		}

		public AuctionCheck(Serial serial) : base(serial)
		{
		}

		public virtual Item AuctionedItem => null;

		public override void OnDoubleClick(Mobile from)
		{
			if (!IsChildOf(from.Backpack))
			{
				from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
			}
			else if (AuctionedItem is MobileStatuette)
			{
				from.CloseGump(typeof(CreatureDeliveryGump));
				from.SendGump(new CreatureDeliveryGump(this));
			}
			else
			{
				from.CloseGump(typeof(AuctionDeliveryGump));
				from.SendGump(new AuctionDeliveryGump(this));
			}
		}

		public override void GetProperties(ObjectPropertyList list)
		{
			base.GetProperties(list);

			list.Add(1060658, "Message\t{0}", m_Message); // ~1_val~: ~2_val~
		}

		public abstract bool Deliver(Mobile to);

		public void DeliveryComplete()
		{
			Delivered = true;
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0); // Version

			writer.Write(m_Auction.ToString());
			writer.Write(m_Message);
			writer.Write(m_ItemName);
			writer.Write(m_Owner);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			m_Auction = new Guid(reader.ReadString());
			m_Message = reader.ReadString();
			m_ItemName = reader.ReadString();
			m_Owner = reader.ReadMobile();
		}
	}
}
