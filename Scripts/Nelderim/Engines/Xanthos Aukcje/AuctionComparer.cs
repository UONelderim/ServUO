#region AuthorHeader
//
//	Auction version 2.1, by Xanthos and Arya
//
//  Based on original ideas and code by Arya
//
#endregion AuthorHeader
using System;
using System.Collections;

namespace Arya.Auction
{
	/// <summary>
	/// Defines auction sorting methods
	/// </summary>
	public enum AuctionSorting
	{
		/// <summary>
		/// Sorting by item name
		/// </summary>
		Name,
		/// <summary>
		/// Sorting by date of creation
		/// </summary>
		Date,
		/// <summary>
		/// Sorting by time left for the auction
		/// </summary>
		TimeLeft,
		/// <summary>
		/// Sorting by the number of bids
		/// </summary>
		Bids,
		/// <summary>
		/// Sorting by value of minimum bid
		/// </summary>
		MinimumBid,
		/// <summary>
		/// Sorting by value of the higherst bid
		/// </summary>
		HighestBid
	}

	/// <summary>
	/// Provides sorting for auction listings
	/// </summary>
	public class AuctionComparer : IComparer
	{
		private bool m_Ascending;
		private AuctionSorting m_Sorting;

		public AuctionComparer( AuctionSorting sorting, bool ascending )
		{
			m_Ascending = ascending;
			m_Sorting = sorting;
		}

		#region IComparer Members

		public int Compare(object x, object y)
		{
			AuctionItem item1 = null;
			AuctionItem item2 = null;

			if ( m_Ascending )
			{
				item1 = x as AuctionItem;
				item2 = y as AuctionItem;
			}
			else
			{
				// Switch x and y for descending ordering

				item1 = y as AuctionItem;
				item2 = x as AuctionItem;
			}

			if ( item1 == null || item2 == null )
				return 0;

			switch ( m_Sorting )
			{
				case AuctionSorting.Bids:

					return item1.Bids.Count.CompareTo( item2.Bids.Count );

				case AuctionSorting.Date:

					return item1.StartTime.CompareTo( item2.StartTime );

				case AuctionSorting.HighestBid:

					return item1.MinNewBid.CompareTo( item2.MinNewBid );

				case AuctionSorting.MinimumBid:

					return item1.MinBid.CompareTo( item2.MinBid );

				case AuctionSorting.Name:

					return item1.ItemName.CompareTo( item2.ItemName );

				case AuctionSorting.TimeLeft:

					return item1.TimeLeft.CompareTo( item2.TimeLeft );
			}

			return 0;
		}

		#endregion
	}
}