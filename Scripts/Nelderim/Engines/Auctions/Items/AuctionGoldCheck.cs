//
//	Auction version 2.1, by Xanthos and Arya
//
//  Based on original ideas and code by Arya
//

using System;
using Arya.Savings;
using Server;
using Server.Mobiles;
using static Arya.Auction.AuctionMessages;

namespace Arya.Auction
{
	/// <summary>
	///     Summary description for AuctionGoldCheck.
	/// </summary>
	public class AuctionGoldCheck : AuctionCheck
	{
		private static readonly int OutbidHue = 2107;
		private static readonly int SoldHue = 2125;

		private int m_GoldAmount;

		/// <summary>
		///     Creates a check delivering gold for the auction system
		/// </summary>
		/// <param name="auction">The auction originating this check</param>
		/// <param name="result">Specifies the reason for the creation of this check</param>
		public AuctionGoldCheck(AuctionItem auction, AuctionResult result)
		{
			Name = GOLD_CHECK_TITLE;
			m_Auction = auction.ID;
			m_ItemName = auction.ItemName;

			if (result != AuctionResult.BuyNow)
				m_GoldAmount = auction.HighestBid.Amount;
			else
				m_GoldAmount = auction.BuyNow;

			switch (result)
			{
				case AuctionResult.Outbid:
				case AuctionResult.SystemStopped:
				case AuctionResult.PendingRefused:
				case AuctionResult.ReserveNotMet:
				case AuctionResult.PendingTimedOut:
				case AuctionResult.StaffRemoved:
				case AuctionResult.ItemDeleted:

					m_Owner = auction.HighestBid.Mobile;
					Hue = OutbidHue;

					switch (result)
					{
						case AuctionResult.Outbid:
							m_Message = String.Format(RESULT_BID_OUTBID_FMT, m_ItemName, m_GoldAmount.ToString("#,0"));
							break;

						case AuctionResult.SystemStopped:
							m_Message = String.Format(RESULT_BID_SYSTEM_STOPPED_FMT, m_ItemName, m_GoldAmount.ToString("#,0"));
							break;

						case AuctionResult.PendingRefused:
							m_Message = String.Format(RESULT_BID_AUCTION_CANCELED_FMT, m_ItemName);
							break;

						case AuctionResult.ReserveNotMet:
							m_Message = String.Format(RESULT_BID_RESERVE_NOT_MET_FMT, m_GoldAmount.ToString("#,0"), m_ItemName);
							break;

						case AuctionResult.PendingTimedOut:
							m_Message = RESULT_PENDING_TIMEOUT;
							break;

						case AuctionResult.ItemDeleted:
							m_Message = RESULT_BID_ITEM_REMOVED;
							break;

						case AuctionResult.StaffRemoved:
							m_Message = RESULT_BID_STAFF_REMOVED;
							break;
					}

					break;

				case AuctionResult.PendingAccepted:
				case AuctionResult.Succesful:
				case AuctionResult.BuyNow:

					m_Owner = auction.Owner;
					Hue = SoldHue;
					m_Message = String.Format(RESULT_BID_SUCCESS_FMT, m_ItemName, m_GoldAmount.ToString("#,0"));
					break;

				default:
					throw new Exception(String.Format(INVALID_GOLD_CHECK_REASON_FMT, result.ToString()));
			}
		}

		public AuctionGoldCheck(Serial serial) : base(serial)
		{
		}

		public override string ItemName
		{
			get
			{
				return String.Format("{0} Gold Coins", m_GoldAmount.ToString("#,0"));
			}
		}

		public override void GetProperties(ObjectPropertyList list)
		{
			base.GetProperties(list);

			list.Add(1060659, "Gold\t{0}", m_GoldAmount.ToString("#,0"));
		}

		public override bool Deliver(Mobile to)
		{
			if (Delivered)
				return true;

			if (!SavingsAccount.DepositGold(m_Owner, m_GoldAmount) && !Banker.Deposit(m_Owner, m_GoldAmount))
			{
				m_Owner.SendMessage(AuctionConfig.MessageHue, ERR_BANKBOX_FULL);
				return false;
			}

			DeliveryComplete();
			Delete();
			m_Owner.SendMessage(AuctionConfig.MessageHue, ITEM_DELIVERED);
			return true;
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0); // Version

			writer.Write(m_GoldAmount);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			m_GoldAmount = reader.ReadInt();
		}
	}
}
