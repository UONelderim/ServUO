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
				default: return "Bob, He is sick, eh?"; //Dumb Canadian Humor - https://youtu.be/OJE3EgTGg9k
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
				default: return "OK, Doug, grab me a beer, eh?"; //Dumb Canadian Humor - https://youtu.be/OJE3EgTGg9k
			}
		}

		private static string GetRandomOneMsg()
		{
			int randomMessage = Utility.RandomMinMax(1, 10);

			switch (randomMessage)
			{
				case 1: return "You are sick!";
				case 2: return "You feel ill!";
				case 3: return "You are not well!";
				case 4: return "You are drained!";
				case 5: return "You feel exhausted!";
				case 6: return "You are not breathing well!";
				case 7: return "You are dying!";
				case 8: return "You feel extremely ill!";
				case 9: return "You are not going to live!";
				default: return "You are not doing good!";
			}
		}

		private static string GetRandomTwoMsg()
		{
			int randomMessage = Utility.RandomMinMax(1, 10);

			switch (randomMessage)
			{
				case 1: return "You are in pain!";
				case 2: return "You feel weak!";
				case 3: return "Your head hurts!";
				case 4: return "You are very sick!";
				case 5: return "You feel deeply ill!";
				case 6: return "Your stomach aches!";
				case 7: return "You are in a lot of pain!";
				case 8: return "You feel exceedingly sick!";
				case 9: return "Your life is fading!";
				default: return "You have a headache!";
			}
		}

		private static string GetRandomThreeMsg()
		{
			int randomMessage = Utility.RandomMinMax(1, 10);

			switch (randomMessage)
			{
				case 1: return "You are dizzy!";
				case 2: return "You feel light headed!";
				case 3: return "You are not doing good!";
				case 4: return "You are miserable!";
				case 5: return "You feel like dying!";
				case 6: return "Your body aches!";
				case 7: return "You are to sick to think straight!";
				case 8: return "You feel like you are on deaths door!";
				case 9: return "You are on your last breath!";
				default: return "You have trouble breathing!";
			}
		}

		private static string GetRandomFourMsg()
		{
			int randomMessage = Utility.RandomMinMax(1, 10);

			switch (randomMessage)
			{
				case 1: return "You are in a lot of pain!";
				case 2: return "You feel like crap!";
				case 3: return "You are not healthy!";
				case 4: return "You are horribly sick!";
				case 5: return "You feel like death!";
				case 6: return "You are losing life!";
				case 7: return "You are about to die!";
				case 8: return "You want to die!";
				case 9: return "Your veins are bleeding!";
				default: return "You are very close to death!";
			}
		}

		private static string GetRandomDeathMsg(PlayerMobile pm)
		{
			int randomMessage = Utility.RandomMinMax(1, 10);

			switch (randomMessage)
			{
				case 1: return "Your chances of dying are increasing!";
				case 2: return "You feel very close to death!";
				case 3: return "The Reaper is coming for " + pm.Name;
				case 4: return "You are extremely sick!";
				case 5: return "You feel death drawing close!";
				case 6: return "Your life will be lost!";
				case 7: return "You are about to die!";
				case 8: return "You want to die, it hurts so bad!";
				case 9: return "Your veins are burning!";
				default: return "You will die soon!";
			}
		}
	}
}
