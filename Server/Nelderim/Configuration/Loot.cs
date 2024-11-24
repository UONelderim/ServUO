// **********
// ServUO - LootConfig.cs
// **********

namespace Nelderim.Configuration
{
	public class Loot
	{
		internal Loot(){}
		
		public bool Enabled => Get("Enabled", true);
		public bool SALootEnabled => Get("SALootEnabled", false);
		public bool ReforgedNamesEnabled => Get("ReforgedNamesEnabled", false);
		public double GoldModifier => Get("GoldModifier", 1.0);
		public double ItemsCountModifier => Get("ItemsCountModifier", 1.0);
		public double PropsCountModifier => Get("PropsCountModifier", 1.0);
		public double MinIntensityModifier => Get("MinIntensityModifier", 1.0);
		public double MaxIntensityModifier => Get("MaxIntensityModifier", 1.0);
		public bool LootBudget => Get("LootBudget", true);
		public bool DebugDifficulty => Get("DebugDifficulty", false);
		public bool ScaleLeechEnabled => Get("ScaleLeechEnabled", false);

		private static T Get<T>(string key, T defaultValue)
		{
			return Server.Config.Get($"NelderimLoot.{key}", defaultValue);
		}
	}
}
