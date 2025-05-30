using System;
using System.Linq;
using Server.Commands;
using Server.Engines.BulkOrders;

namespace Server.Nelderim
{
	public class FixFletchingBODs
	{
		public static Type[] FletchingBODTypes = SmallBulkEntry.FletchingSmalls
			.Concat(SmallBulkEntry.FletchingSmallsRegular).Select(x => x.Type).ToArray();
		
		public static void Initialize()
		{
			CommandSystem.Register("checkfletchingbods", AccessLevel.GameMaster, OnCommand);
		}

		public static void FixType(BOBLargeEntry entry, ref BODType type)
		{
			if(FletchingBODTypes.Contains(entry.Entries[0].ItemType))
			{
				type = BODType.Fletching;
			}
		}

		private static void OnCommand(CommandEventArgs e)
		{
			var types = SmallBulkEntry.FletchingSmalls.Concat(SmallBulkEntry.FletchingSmallsRegular).Select(bulkEntry => bulkEntry.Type).ToArray();
			var count = 0;
			var sbods = World.Items.Values.OfType<SmallBOD>();
			foreach (var smallBod in sbods)
			{
				if (smallBod is not SmallFletchingBOD && types.Contains(smallBod.Type))
				{
					e.Mobile.SendMessage($"Corrupted sbod: {smallBod.Serial}");
					count++;
				}
			}
			var lbods = World.Items.Values.OfType<LargeBOD>();
			foreach (var lbod in lbods)
			{
				if (lbod is not LargeFletchingBOD)
				{
					foreach (var entry in lbod.Entries)
					{
						if (types.Contains(entry.Details.Type))
						{
							e.Mobile.SendMessage($"Corrupted lbod: {lbod.Serial}");
							count++;
						}
					}
				}
			}
			var bobs = World.Items.Values.OfType<BulkOrderBook>();
			foreach (var bob in bobs)
			{
				foreach (var bobEntry in bob.Entries)
				{
					if (bobEntry is BOBLargeEntry largeEntry)
					{
						if(largeEntry.DeedType == BODType.Fletching)
							continue;
						
						foreach (var bobLargeSubEntry in largeEntry.Entries)
						{
							if (types.Contains(bobLargeSubEntry.ItemType))
							{
								e.Mobile.SendMessage($"Corrupted lbod in book: {bob.Serial}:{largeEntry.DeedType}:{bobLargeSubEntry.ItemType}");
								count++;
								break;
							}
						}
					}
			
					if (bobEntry is BOBSmallEntry smallEntry)
					{
						if(smallEntry.DeedType == BODType.Fletching)
							continue;
						if (types.Contains(smallEntry.ItemType))
						{
							e.Mobile.SendMessage($"Corrupted sbod in book: {bob.Serial}");
							count++;
						}
					}
				}
			}
			e.Mobile.SendMessage($"Found {count} corrupted items.");
		}
	}
}
