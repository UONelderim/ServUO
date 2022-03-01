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
			ColdNames.Add("Przeziebienie");
			ColdNames.Add("Sezonowe Przeziebienie");
			ColdNames.Add("Katar");
			ColdNames.Add("Kaszel");
			ColdNames.Add("Garlanska Grypa");
			ColdNames.Add("Bol Gardla");
			ColdNames.Add("Bol GLowy");
			ColdNames.Add("Tasandorska Grypa");
			ColdNames.Add("Niestrawnosc");
			ColdNames.Add("Dur Brzuszny");

			return ColdNames;
		}

		private static readonly List<string> FluNames = new List<string>();

		public static List<string> GetFluNameList()
		{
			FluNames.Add("Ptasia Grypa");
			FluNames.Add("Swinska Grypa");
			FluNames.Add("Smocza Grypa");
			FluNames.Add("Zielona Grypa");
			FluNames.Add("Rozwolnienie");
			FluNames.Add("Tasandorska Szczurza Grypa");
			FluNames.Add("Kac");
			FluNames.Add("Nieznana Choroba");
			FluNames.Add("Zawal serca");
			FluNames.Add("Hipotermia");

			return FluNames;
		}

		private static readonly List<string> VirusNames = new List<string>();

		public static List<string> GetVirusNameList()
		{
			VirusNames.Add("Zwyczajny wirus");
			VirusNames.Add("Rzadki Wirus");
			VirusNames.Add("Nieznany wirus");
			VirusNames.Add("Tasandorosis");
			VirusNames.Add("Wirus Orkopochodny");
			VirusNames.Add("Klatwa Krwii Moreny Nekromantki");
			VirusNames.Add("Dar-Kosis");
			VirusNames.Add("Cuchnaca Smierc");
			VirusNames.Add("Wyprysk Demona");
			VirusNames.Add("Wyprysk Smoka");
			VirusNames.Add("Goraczka z Garlan");
			VirusNames.Add("Letumosis");
			VirusNames.Add("Neurodermatitis");
			VirusNames.Add("Ratititis");


			return VirusNames;
		}

		private static readonly List<string> VampireNames = new List<string>();

		public static List<string> GetVampireNameList()
		{
			VampireNames.Add("Zwyczajny Wirus Wampiryzmu");
			VampireNames.Add("Rzadki wirus plonacej krwii");
			VampireNames.Add("nieznany wirus wampiryzmu");

			return VampireNames;
		}

		private static readonly List<string> LycanthropiaNames = new List<string>();

		public static List<string> GetLycanthropiaNameList()
		{
			LycanthropiaNames.Add("zwyczajny wirus likanotropii");
			LycanthropiaNames.Add("rzadki wirus wilczego gniewu");
			LycanthropiaNames.Add("nieznany wirus likanotropii");

			return LycanthropiaNames;
		}
	}
}
