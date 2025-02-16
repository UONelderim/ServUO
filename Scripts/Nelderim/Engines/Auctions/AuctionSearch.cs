using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Server;
using Xanthos.Utilities;

namespace Arya.Auction
{
	public class AuctionSearch
	{
		public static List<AuctionItem> Merge(List<AuctionItem> first, List<AuctionItem> second)
		{
			return first.Union(second).ToList();
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

		private static List<AuctionItem> TextSearch(List<AuctionItem> list, string text)
		{
			if (list == null || text == null)
			{
				return [];
			}
			
			List<AuctionItem> results = [];
			try
			{
				foreach (var auctionItem in list)
				{
					if (auctionItem == null) continue;
					
					if (auctionItem.ItemName.Contains(text, StringComparison.OrdinalIgnoreCase))
					{
						results.Add(auctionItem);
					}
					else if (auctionItem.Description.Contains(text, StringComparison.OrdinalIgnoreCase))
					{
						results.Add(auctionItem);
					}
					else
					{
						// Search individual items
						foreach (AuctionItem.ItemInfo info in auctionItem.Items)
						{
							if (info.Name.Contains(text, StringComparison.OrdinalIgnoreCase))
							{
								results.Add(auctionItem);
								break;
							}
							if (info.Properties.Contains(text, StringComparison.OrdinalIgnoreCase))
							{
								results.Add(auctionItem);
								break;
							}
						}
					}
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}

			return results;
		}

		public static List<AuctionItem> ForTypes(List<AuctionItem> list, List<Type> types)
		{
			if (list == null || types == null)
				return [];
			
			try
			{
				return list.Where(auctionItem =>
						auctionItem != null &&
						types.Any(type => MatchesType(auctionItem, type)))
					.ToList();
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}

			return [];
		}

		private static bool MatchesType(AuctionItem item, Type type)
		{
			return item.Items.Where(info => info.Item != null).Any(info => type.IsAssignableFrom(info.Item.GetType()));
		}

		public static List<AuctionItem> ForArtifacts(List<AuctionItem> items)
		{
			return items.Where(auction => 
				auction
					.Items
					.Select(info => info.Item)
					.Any(IsArtifact))
				.ToList();
		}
		
		private static bool IsArtifact(Item item)
		{
			if (null == item)
				return false;

			Type t = item.GetType();
			PropertyInfo prop = null;

			try { prop = t.GetProperty("ArtifactRarity"); }
			catch { }

			if (null == prop || (int)(prop.GetValue(item, null)) <= 0)
				return false;

			return true;
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
