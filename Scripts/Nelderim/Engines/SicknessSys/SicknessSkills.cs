#region References

using Server.Mobiles;

#endregion

namespace Server.SicknessSys
{
	public static class SicknessSkills
	{
		//Vampirism
		public static void BloodDrain(PlayerMobile pm, VirusCell cell) //Auto
		{
			int damage = cell.Damage;

			if (cell.MaxPower > cell.Power + damage / 2)
			{
				cell.Power = cell.Power + damage / 2;

				pm.Combatant.Damage(damage / 2, pm);

				SicknessAnimate.RunBloodDrainAnimation(pm);
			}
			else
			{
				pm.Say("*nie trafiles*");
				pm.SendMessage("Masz maksymalna moc!");
			}
		}

		public static void BloodBurn(PlayerMobile pm, VirusCell cell) //Attack
		{
			int damage = cell.Damage;

			if (0 < cell.Power - damage * 2)
			{
				cell.Power = cell.Power - damage * 2;

				pm.Combatant.Damage(damage, pm);

				SicknessAnimate.RunBloodBurnAnimation(pm);
			}
			else
			{
				pm.Say("*nie trafiles*");
				pm.SendMessage("Brak Ci mocy, by uzyc umiejetnosci Palaca Krew");
			}
		}

		public static void BloodBath(PlayerMobile pm, VirusCell cell) //Defense
		{
			int manaGain = pm.ManaMax - pm.Mana;

			if (cell.Power > manaGain)
			{
				pm.SpeechHue = 47;

				pm.Say("Mana + " + manaGain);

				pm.Mana = cell.PM.ManaMax;

				pm.SendMessage("Twoj umysl oczyszcza sie!");

				SicknessAnimate.RunBloodBathAnimation(pm);
			}
			else
			{
				pm.Say("*rozprasza sie*");
				pm.SendMessage("Nie masz wystarczajacej mocy, by wykonac umiejetnosc Kapiel w Krwii");
			}
		}

		//Lycanthropia
		public static void RageFeed(PlayerMobile pm, VirusCell cell) //Auto
		{
			int damage = cell.Damage;

			if (cell.MaxPower < cell.Power + damage / 2)
			{
				cell.Power = cell.Power + damage / 2;

				pm.Combatant.Damage(damage / 2, pm);
			}
			else
			{
				pm.Say("*nie trafiles*");
				pm.SendMessage("Jestes pelny i nie chce Ci sie jesc!");
			}
		}

		public static void RageStrike(PlayerMobile pm, VirusCell cell) //Attack
		{
			int damage = cell.Damage;

			if (0 < cell.Power - damage * 2)
			{
				cell.Power = cell.Power - damage * 2;

				pm.Combatant.Damage(damage, pm);
			}
			else
			{
				pm.Say("*nie trafiles*");
				pm.SendMessage("Nie masz wystarczajacej mocy, by wykonac umiejetnosc Gniewne Uderzenie");
			}
		}

		public static void RagePush(PlayerMobile pm, VirusCell cell) //Defense
		{
			int hitsGain = pm.HitsMax - pm.Hits;

			if (cell.Power > hitsGain)
			{
				pm.SpeechHue = 47;

				pm.Say("Zycie + " + hitsGain);

				pm.Hits = cell.PM.HitsMax;

				pm.SendMessage("Twoje cialo goi sie!");
			}
			else
			{
				pm.Say("*rozprasza sie*");
				pm.SendMessage("Nie masz wystarczajacej mocy, by wykonac umiejetnosc Wsciekle Odepchniecie");
			}
		}
	}
}
