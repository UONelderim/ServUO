#region References

using System.Collections.Generic;

#endregion

namespace Server.SicknessSys.Illnesses
{
	static class IllnessNameLists
	{
		private static readonly List<string> ColdNames = new List<string>();

		public static List<string> GetColdNameList()
		{
			ColdNames.Add("a Common Cold");
			ColdNames.Add("the Seasonal Cold");
			ColdNames.Add("a Spring Cold");
			ColdNames.Add("the Winter Cold");
			ColdNames.Add("the Cold");
			ColdNames.Add("Cooties");
			ColdNames.Add("a Head Cold");
			ColdNames.Add("a Chest Cold");
			ColdNames.Add("Swamp Gas");
			ColdNames.Add("Dungeanitas");

			return ColdNames;
		}

		private static readonly List<string> FluNames = new List<string>();

		public static List<string> GetFluNameList()
		{
			FluNames.Add("the Bird Flu");
			FluNames.Add("the Swine Flu");
			FluNames.Add("the Dragon Flu");
			FluNames.Add("the Yellow Flu");
			FluNames.Add("the Stomach Flu");
			FluNames.Add("Britannia Cat Flu");
			FluNames.Add("Captain Trips");
			FluNames.Add("an Unknown Flu");
			FluNames.Add("Heat Stroke");
			FluNames.Add("Hypothermia");

			return FluNames;
		}

		private static readonly List<string> VirusNames = new List<string>();

		public static List<string> GetVirusNameList()
		{
			VirusNames.Add("a Common Virus");
			VirusNames.Add("a Rare Virus");
			VirusNames.Add("an Unknown Virus");
			VirusNames.Add("Brainpox");
			VirusNames.Add("a Rare Wormword Virus");
			VirusNames.Add("Curse of the Warmbloods");
			VirusNames.Add("Dar-Kosis");
			VirusNames.Add("Death Stench");
			VirusNames.Add("Demon Pox");
			VirusNames.Add("Dragon Pox");
			VirusNames.Add("Dryditch Fever");
			VirusNames.Add("the Hourman Virus");
			VirusNames.Add("the Inferno Virus");
			VirusNames.Add("the Krytos Virus");
			VirusNames.Add("Letumosis");
			VirusNames.Add("Neurodermatitis");
			VirusNames.Add("Ratititis");
			VirusNames.Add("the Red Death");
			VirusNames.Add("Sakutia");
			VirusNames.Add("the Solanum Virus");
			VirusNames.Add("Spattergroit");
			VirusNames.Add("the White Disease");
			VirusNames.Add("the Minoc Lung Virus");
			VirusNames.Add("the Vorpal Bunny Virus");

			return VirusNames;
		}

		private static readonly List<string> VampireNames = new List<string>();

		public static List<string> GetVampireNameList()
		{
			VampireNames.Add("a Common Vampiric Virus");
			VampireNames.Add("the Rare BloodFire Virus");
			VampireNames.Add("an Unknown Vampirism Virus");

			return VampireNames;
		}

		private static readonly List<string> LycanthropiaNames = new List<string>();

		public static List<string> GetLycanthropiaNameList()
		{
			LycanthropiaNames.Add("a Common Lycanthropia Virus");
			LycanthropiaNames.Add("the Rare RageFire Virus");
			LycanthropiaNames.Add("an Unknown Lycanthropia Virus");

			return LycanthropiaNames;
		}
	}
}
