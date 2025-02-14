using System.Collections.Generic;

namespace Arya.Auction
{
	public enum AuctionSorting
	{
		Name,
		Date,
		TimeLeft,
		Bids,
		MinimumBid,
		HighestBid
	}

	public class AuctionComparer : IComparer<AuctionItem>
	{
		private readonly bool m_Ascending;
		private readonly AuctionSorting m_Sorting;

		public AuctionComparer(AuctionSorting sorting, bool ascending)
		{
			m_Ascending = ascending;
			m_Sorting = sorting;
		}

		public int Compare(AuctionItem x, AuctionItem y)
		{
			AuctionItem item1;
			AuctionItem item2;

			if (m_Ascending)
			{
				item1 = x;
				item2 = y;
			}
			else
			{
				// Switch x and y for descending ordering

				item1 = y;
				item2 = x;
			}

			if (item1 == null || item2 == null)
				return 0;

			switch (m_Sorting)
			{
				case AuctionSorting.Bids:

					return item1.Bids.Count.CompareTo(item2.Bids.Count);

				case AuctionSorting.Date:

					return item1.StartTime.CompareTo(item2.StartTime);

				case AuctionSorting.HighestBid:

					return item1.MinNewBid.CompareTo(item2.MinNewBid);

				case AuctionSorting.MinimumBid:

					return item1.MinBid.CompareTo(item2.MinBid);

				case AuctionSorting.Name:

					return item1.ItemName.CompareTo(item2.ItemName);

				case AuctionSorting.TimeLeft:

					return item1.TimeLeft.CompareTo(item2.TimeLeft);
			}

			return 0;
		}
	}
}
