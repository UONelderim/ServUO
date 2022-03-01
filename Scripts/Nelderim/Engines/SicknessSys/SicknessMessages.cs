#region References

using Server.Mobiles;

#endregion

namespace Server.SicknessSys
{
	public static class SicknessMessages
	{
		public static string GetSickMessage(PlayerMobile pm, VirusCell cell, int stage, int power)
		{
			if (cell.Level == 100 && !SicknessHelper.IsSpecialVirus(cell))
			{
				return GetRandomDeathMsg(pm);
			}

			switch (stage)
			{
				case 1: return GetRandomString(power);
				case 2: return GetRandomString(power);
				case 3: return GetRandomString(power);
				default: return "On jest chory, he?"; //Dumb Canadian Humor - https://youtu.be/OJE3EgTGg9k
			}
		}

		private static string GetRandomString(int power)
		{
			switch (power)
			{
				case 1: return GetRandomOneMsg();
				case 2: return GetRandomTwoMsg();
				case 3: return GetRandomThreeMsg();
				case 4: return GetRandomFourMsg();
				default: return "Cholera!?"; //Dumb Canadian Humor - https://youtu.be/OJE3EgTGg9k
			}
		}

		private static string GetRandomOneMsg()
		{
			int randomMessage = Utility.RandomMinMax(1, 10);

			switch (randomMessage)
			{
				case 1: return "Czujesz sie chory!";
				case 2: return "Czujesz sie bardzo chory!";
				case 3: return "Czujesz sie bardzo bardzo chory!";
				case 4: return "Czujesz sie wycienczony!";
				case 5: return "Czujesz, ze padasz na twarz!";
				case 6: return "Twoj oddech jest plytki!";
				case 7: return "Prawie umierasz!";
				case 8: return "Jestes ekstremalnie chory!";
				case 9: return "Chyba tego nie przezyjesz!";
				default: return "Nie miewasz sie dobrze!";
			}
		}

		private static string GetRandomTwoMsg()
		{
			int randomMessage = Utility.RandomMinMax(1, 10);

			switch (randomMessage)
			{
				case 1: return "Czujesz bol";
				case 2: return "Czujesz sie slabo!";
				case 3: return "Boli Cie glowa!";
				case 4: return "Czujesz sie bardzo chory!";
				case 5: return "Czujesz sie bardzo bardzo chory!";
				case 6: return "Brzuch Cie boli!";
				case 7: return "Odczuwasz ogromny bol!";
				case 8: return "Jestes ekstremalnie chory!";
				case 9: return "Chyba tego nie przezyjesz!";
				default: return "Nie miewasz sie dobrze!";
			}
		}

		private static string GetRandomThreeMsg()
		{
			int randomMessage = Utility.RandomMinMax(1, 10);

			switch (randomMessage)
			{
				case 1: return "Czujesz sie chory!";
				case 2: return "Czujesz sie bardzo chory!";
				case 3: return "Czujesz sie bardzo bardzo chory!";
				case 4: return "Czujesz sie wycienczony!";
				case 5: return "Czujesz, ze padasz na twarz!";
				case 6: return "Twoj oddech jest plytki!";
				case 7: return "Prawie umierasz!";
				case 8: return "Jestes ekstremalnie chory!";
				case 9: return "Chyba tego nie przezyjesz!";
				default: return "Nie miewasz sie dobrze!";
			}
		}

		private static string GetRandomFourMsg()
		{
			int randomMessage = Utility.RandomMinMax(1, 10);

			switch (randomMessage)
			{
				case 1: return "Czujesz sie chory!";
				case 2: return "Czujesz sie bardzo chory!";
				case 3: return "Czujesz sie bardzo bardzo chory!";
				case 4: return "Czujesz sie wycienczony!";
				case 5: return "Czujesz, ze padasz na twarz!";
				case 6: return "Twoj oddech jest plytki!";
				case 7: return "Prawie umierasz!";
				case 8: return "Jestes ekstremalnie chory!";
				case 9: return "Chyba tego nie przezyjesz!";
				default: return "Nie miewasz sie dobrze!";
			}
		}

		private static string GetRandomDeathMsg(PlayerMobile pm)
		{
			int randomMessage = Utility.RandomMinMax(1, 10);

			switch (randomMessage)
			{
				case 1: return "Czujesz sie chory!";
				case 2: return "Czujesz sie bardzo chory!";
				case 3: return "Czujesz sie bardzo bardzo chory!";
				case 4: return "Czujesz sie wycienczony!";
				case 5: return "Czujesz, ze padasz na twarz!";
				case 6: return "Twoj oddech jest plytki!";
				case 7: return "Prawie umierasz!";
				case 8: return "Jestes ekstremalnie chory!";
				case 9: return "Chyba tego nie przezyjesz!";
				default: return "Nie miewasz sie dobrze!";
			}
		}
	}
}
