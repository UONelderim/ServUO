using System.Collections.Generic;

namespace Server.Commands
{
	public class RegionInfo
	{
		public static void Initialize()
		{
			CommandSystem.Register("regionInfo", AccessLevel.Administrator,
				RegionInfo_OnCommand);
		}

		public static void RegionInfo_OnCommand(CommandEventArgs e)
		{
			Mobile from = e.Mobile;

			from.SendMessage("Aktywny region: " + from.Region);
			from.SendMessage("Wszystkie regiony w tym miejscu:");
			SortedSet<Region> regions = [];
			foreach (var region in from.Map.Regions.Values)
			{
				if (region.Contains(from.Location)) regions.Add(region);
			}
			foreach (var region in regions)
			{
				from.SendMessage(region.Name);
			}
		}
	}
}
