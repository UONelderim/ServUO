using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Server;
using Server.Commands;
using Server.Items;
using Server.Mobiles;

namespace Nelderim
{
	public class RegionDifficulty
	{
		private static string BasePath = Path.Combine(Core.BaseDirectory, "Data", "Difficulty");
		private static string PresetsPath = Path.Combine(BasePath, "DifficultyPresets.json");
		private static string RegionsPath = Path.Combine(BasePath, "DifficultyRegions.json");
		private static Dictionary<DifficultyLevelValue, double> _default = new() {{DifficultyLevelValue.Normal, 100}};

		private static Dictionary<string, Dictionary<DifficultyLevelValue, double>> _presets = new();
		private static Dictionary<string, string> _regions = new();
		
		public static void Initialize()
		{
			Load();
			CommandSystem.Register("RegionDifficultyLoad", AccessLevel.Administrator, e => Load());
			CommandSystem.Register("RDLoad", AccessLevel.Administrator, e => Load());
			EventSink.MobileCreated += OnMobileCreated;
		}
		
		private static void Load()
		{
			_presets.Clear();
			_presets = JsonSerializer.Deserialize<Dictionary<string, Dictionary<DifficultyLevelValue, double>>>(File.ReadAllText(PresetsPath));
			_regions.Clear();
			_regions = JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(RegionsPath));
			foreach (var regionName in _regions.Keys)
			{
				if(Region.Regions.All(r => r.Name != regionName))
				{
					Console.WriteLine("[WARN] RegionDifficulty: Invalid region name: " + regionName);
				}
			}
			Console.WriteLine("RegionDifficulty: Loaded.");
		}
		
		private static void OnMobileCreated(MobileCreatedEventArgs e)
		{
			var m = e.Mobile;

			if (m is BaseCreature { Tamable: false } bc && !ArtifactHelper.IsBoss(bc) && m.Region is { Name: not null })
			{
				var difficulty = Get(m.Region.Name);
				if (difficulty != null && difficulty.Count != 0 && bc.DifficultyLevel == DifficultyLevelValue.Normal)
				{
					bc.DifficultyLevel = Utility.RandomWeigthed(difficulty);
				}
			}
		}
		
		public static Dictionary<DifficultyLevelValue, double> Get(string regionName)
		{
			if (_regions.TryGetValue(regionName, out var preset))
			{
				return GetPreset(preset);
			}
			return null;
		}
		
		private static Dictionary<DifficultyLevelValue, double> GetPreset(string preset)
		{
			if (_presets.TryGetValue(preset, out var presetValues))
			{
				return presetValues;
			}
			Console.WriteLine($"DifficultyPreset: Preset not found: {preset}");
			return _default;
		}
	}
}
