// **********
// ServUO - NConfig.cs
// **********

using Nelderim.Config;
using static Server.Config;

namespace Nelderim.Configuration
{
	public static class NConfig
	{
		public static Loot Loot = new Loot();
		public static Durability Durability = new Durability();
		public static bool CustomOnSpeech => Get("Nelderim.CustomOnSpeech", true);
		public static bool CustomFameKarma => Get("Nelderim.CustomFameKarma", true);
		public static bool CustomGainChance => Get("Nelderim.CustomGainChance", true);
		public static double BaseGainFactor => Get("Nelderim.BaseGainFactor", 0.1);
		public static bool TimeSystemEnabled => Get("Nelderim.TimeSystemEnabled", true);
		public static bool FameTitlesEnabled => Get("Nelderim.FameTitlesEnabled", false);
		public static bool NameSystemEnabled => Get("Nelderim.NameSystemEnabled", true);
		public static bool CustomInsuranceCost => Get("Nelderim.CustomInsuranceCost", true);
		public static double BloodyBandageChance => Get("Nelderim.BloodyBandageChance", 0.75);
		public static bool BetterAI => Get("Nelderim.BetterAI", true);
		public static bool ReducedCastDelay => Get("Nelderim.ReducedCastDelay", true);
	}
}
