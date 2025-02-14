using System;
using System.Collections.Generic;
using System.Linq;
using Server;
using Xanthos.Utilities;

namespace Arya.Auction
{
	public class AuctionSearch
	{
		public static List<AuctionItem> Merge(List<AuctionItem> first, List<AuctionItem> second)
		{
			List<AuctionItem> result = first.ToList();

			foreach (AuctionItem item in second)
			{
				if (!result.Contains(item))
					result.Add(item);
			}

			return result;
		}

		public static List<AuctionItem> SearchForText(List<AuctionItem> items, string text)
		{
			string[] split = text.Split(' ');
			List<AuctionItem> result = [];

			foreach (string s in split)
			{
				result = Merge(result, TextSearch(items, s));
			}

			return result;
		}

		private static List<AuctionItem> TextSearch(List<AuctionItem> list, string name)
		{
			List<AuctionItem> results = [];

			if (list == null || name == null)
			{
				return results;
			}

			IEnumerator<AuctionItem> ie = null;

			try
			{
				name = name.ToLower();

				ie = list.GetEnumerator();

				while (ie.MoveNext())
				{
					AuctionItem item = ie.Current;

					if (item != null)
					{
						if (item.ItemName.ToLower().IndexOf(name) > -1)
						{
							results.Add(item);
						}
						else if (item.Description.ToLower().IndexOf(name) > -1)
						{
							results.Add(item);
						}
						else
						{
							// Search individual items
							foreach (AuctionItem.ItemInfo info in item.Items)
							{
								if (info.Name.ToLower().IndexOf(name) > -1)
								{
									results.Add(item);
									break;
								}

								if (info.Properties.ToLower().IndexOf(name) > .1)
								{
									results.Add(item);
									break;
								}
							}
						}
					}
				}
			}
			catch { }
			finally
			{
				IDisposable d = ie as IDisposable;

				if (d != null)
					d.Dispose();
			}

			return results;
		}

		public static List<AuctionItem> ForTypes(List<AuctionItem> list, List<Type> types)
		{
			List<AuctionItem> results = [];

			if (list == null || types == null)
				return results;

			IEnumerator<AuctionItem> ie = null;

			try
			{
				ie = list.GetEnumerator();

				while (ie.MoveNext())
				{
					AuctionItem item = ie.Current;

					if (item == null)
						continue;

					foreach (Type t in types)
					{
						if (MatchesType(item, t))
						{
							results.Add(item);
							break;
						}
					}
				}
			}
			catch { }
			finally
			{
				IDisposable d = ie as IDisposable;

				if (d != null)
					d.Dispose();
			}

			return results;
		}

		private static bool MatchesType(AuctionItem item, Type type)
		{
			foreach (AuctionItem.ItemInfo info in item.Items)
			{
				if (info.Item != null)
				{
					if (type.IsAssignableFrom(info.Item.GetType()))
					{
						return true;
					}
				}
			}

			return false;
		}

		public static List<AuctionItem> ForArtifacts(List<AuctionItem> items)
		{
			List<AuctionItem> results = [];

			foreach (AuctionItem auction in items)
			{
				foreach (AuctionItem.ItemInfo info in auction.Items)
				{
					Item item = info.Item;

					if (Misc.IsArtifact(item))
					{
						results.Add(auction);
						break;
					}
				}
			}

			return results;
		}

		public static List<AuctionItem> ForCommodities(List<AuctionItem> items)
		{
			List<AuctionItem> results = [];

			foreach (AuctionItem auction in items)
			{
				foreach (AuctionItem.ItemInfo info in auction.Items)
				{
					if (info.Item == null)
						continue;

					Type t = info.Item.GetType();

					if (t.GetInterface("ICommodity") != null)
					{
						results.Add(auction);
						break;
					}
				}
			}

			return results;
		}
	}
}
