// **********
// ServUO - LootConfig.cs
// **********

using static Server.Config;

namespace Nelderim.Configuration
{
	public class Loot
	{
		internal Loot(){}
		
		public bool Enabled => Get("NelderimLoot.Enabled", true);
		public bool SALootEnabled => Get("NelderimLoot.SALootEnabled", false);
		public double GoldModifier => Get("NelderimLoot.GoldModifier", 1.0);
		public double ItemsCountModifier => Get("NelderimLoot.ItemsCountModifier", 1.0);
		public double PropsCountModifier => Get("NelderimLoot.PropsCountModifier", 1.0);
		public double MinIntensityModifier => Get("NelderimLoot.MinIntensityModifier", 1.0);
		public double MaxIntensityModifier => Get("NelderimLoot.MaxIntensityModifier", 1.0);
		public bool LootBudget => Get("NelderimLoot.LootBudget", true); 
	}
}
