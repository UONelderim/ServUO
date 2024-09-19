using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Server;
using Server.Mobiles;

namespace Nelderim
{
	public class DifficultyPresets
	{
		private static string JsonPath = Path.Combine(Core.BaseDirectory, "Data", "DifficultyPresets.json");
		private static Dictionary<string, Dictionary<DifficultyLevelValue, double>> _presets = new();
		
		public static void Configure()
		{
			Load();
		}
		
		private static void Load()
		{
			_presets.Clear();
			_presets = JsonSerializer.Deserialize<Dictionary<string, Dictionary<DifficultyLevelValue, double>>>(File.ReadAllText(JsonPath));
			Console.WriteLine("DifficultyPresets: Loaded.");
		}
		
		public static Dictionary<DifficultyLevelValue, double> Get(string preset)
		{
			if (_presets.TryGetValue(preset, out var presetValues))
			{
				return presetValues;
			}
			Console.WriteLine($"DifficultyPreset: Preset not found: {preset}");
			return null;
		}
	}
}
