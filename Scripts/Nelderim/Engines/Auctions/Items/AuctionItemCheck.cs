//
//	Auction version 2.1, by Xanthos and Arya
//
//  Based on original ideas and code by Arya
//

using System;
using Server;
using static Arya.Auction.AuctionMessages;

namespace Arya.Auction
{
	/// <summary>
	///     Summary description for AuctionItemCheck.
	/// </summary>
	public class AuctionItemCheck : AuctionCheck
	{
		private static readonly int ItemSoldHue = 2119;
		private static readonly int ItemReturnedHue = 52;
		private Item m_Item;

		/// <summary>
		///     Creates a check that will deliver an item for the auction system
		/// </summary>
		/// <param name="auction">The auction generating this check</param>
		/// <param name="result">Specifies the reason for the generation of this check</param>
		public AuctionItemCheck(AuctionItem auction, AuctionResult result)
		{
			Name = auction.Creature ? CREATURE_CHECK_TITLE : ITEM_CHECK_TITLE;

			m_Auction = auction.ID;
			m_ItemName = auction.ItemName;
			m_Item = auction.Item;

			if (m_Item != null)
			{
				AuctionSystem.ControlStone.RemoveItem(m_Item);
				m_Item.Parent = this; // This will avoid cleanup
			}

			switch (result)
			{
				// Returning the item to the owner
				case AuctionResult.NoBids:
				case AuctionResult.PendingRefused:
				case AuctionResult.SystemStopped:
				case AuctionResult.PendingTimedOut:
				case AuctionResult.ItemDeleted:
				case AuctionResult.StaffRemoved:

					m_Owner = auction.Owner;
					Hue = ItemReturnedHue;

					switch (result)
					{
						case AuctionResult.NoBids:
							m_Message = String.Format(RESULT_NO_BIDS_FMT, m_ItemName);
							break;

						case AuctionResult.PendingRefused:
							m_Message = String.Format(RESULT_CANCELED_FMT, m_ItemName);
							break;

						case AuctionResult.SystemStopped:
							m_Message = String.Format(RESULT_SYSTEM_STOPPED_FMT, m_ItemName);
							break;

						case AuctionResult.PendingTimedOut:
							m_Message = RESULT_PENDING_TIMEOUT;
							break;

						case AuctionResult.ItemDeleted:
							m_Message = RESULT_ITEM_REMOVED;
							break;
						case AuctionResult.StaffRemoved:
							m_Message = RESULT_STAFF_CLOSED;
							break;
					}

					break;

				case AuctionResult.PendingAccepted:
				case AuctionResult.Succesful:
				case AuctionResult.BuyNow:

					m_Owner = auction.HighestBid.Mobile;
					Hue = ItemSoldHue;
					m_Message = String.Format(RESULT_SUCCESS_FMT, m_ItemName,
						auction.HighestBid.Amount.ToString("#,0"));
					break;

				default:
					throw new Exception(String.Format(INVALID_ITEM_CHECK_REASON_FMT, result.ToString()));
			}
		}

		public override string ItemName
		{
			get
			{
				return m_ItemName;
			}
		}

		public override Item AuctionedItem
		{
			get
			{
				return m_Item;
			}
		}

		public override void GetProperties(ObjectPropertyList list)
		{
			base.GetProperties(list);

			list.Add(1060659, "Item\t{0}", m_ItemName);
		}

		public override bool Deliver(Mobile to)
		{
			if (Delivered)
				return true;

			Item item = AuctionedItem;

			if (null == item)
			{
				to.SendMessage(AuctionConfig.MessageHue, ITEM_NOT_FOUND_ERROR);
				return false;
			}

			if (to.BankBox.TryDropItem(to, item, false))
			{
				item.UpdateTotals();
				DeliveryComplete();
				Delete();
				to.SendMessage(AuctionConfig.MessageHue, ITEM_DELIVERED);
				return true;
			}

			return false;
		}

		public override void OnDelete()
		{
			if (Delivered)
				m_Item = null;
			else
				ForceDelete();

			base.OnDelete();
		}

		public void ForceDelete()
		{
			if (m_Item != null)
			{
				if (m_Item is MobileStatuette)
				{
					(m_Item as MobileStatuette).ForceDelete();
				}
				else
				{
					m_Item.Delete();
				}
			}
		}

		public AuctionItemCheck(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0); // Version

			writer.Write(m_Item);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			m_Item = reader.ReadItem();
		}
	}
}
