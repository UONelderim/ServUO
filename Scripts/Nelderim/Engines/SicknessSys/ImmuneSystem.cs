#region References

using Server.Mobiles;

#endregion

namespace Server.SicknessSys
{
	static class ImmuneSystem
	{
		public static void UpdateImmuneNegative(VirusCell cell)
		{
			if (cell.Power > 0)
			{
				int GetPowerMod = Utility.RandomMinMax(1, cell.PowerDegenRate);

				if (GetPowerMod == 1)
					cell.Power--;

				UpdateStatsNegative(cell);
			}
			else
			{
				SicknessSpread.SpreadSickness(cell);

				cell.Power = cell.MaxPower;
			}
		}

		public static void UpdateStatsNegative(VirusCell cell)
		{
			if (cell.LastSick > 0)
			{
				cell.LastSick--;
			}
			else
			{
				int rndHelper = Utility.RandomMinMax(1, 100);

				if (SicknessHelper.IsSpecialVirus(cell) && rndHelper > 90)
				{
					if (SicknessHelper.IsNight(cell.PM))
					{
						if (cell.PM.Combatant != null)
						{
							cell.PM.SendMessage("Twoje cialo plonie... CO TO TAKIEGO?!");

							if (cell.PM.InRange(cell.PM.Combatant.Location, 2))
							{
								if (cell.Illness == IllnessType.Vampirism)
									SicknessSkills.BloodDrain(cell.PM, cell);
								else
									SicknessSkills.RageFeed(cell.PM, cell);
							}
						}
					}
				}

				if (SicknessHelper.IsSpecialVirus(cell) && cell.Level > 99)
				{
					if (rndHelper == 1)
						UpdateImmunePositive(cell);
				}
				else
				{
					if (rndHelper == 1 && cell.Power != 99)
						UpdateImmunePositive(cell);

					SetByPowerLevel(cell.PM, cell);
				}
			}
		}

		private static void SetByPowerLevel(PlayerMobile pm, VirusCell cell)
		{
			int powerLevel = 1;

			bool chance = Utility.RandomBool();
			bool rnd = Utility.RandomBool();

			if (chance == rnd)
			{
				if (cell.Level < 100)
				{
					int rndAnim = Utility.RandomMinMax(1, 9);

					SicknessSpread.RunRandomAnimation(cell.PM, rndAnim);
				}

				cell.LastSick = ((30 / cell.Stage) - ((cell.PowerDegenRate + cell.Stage) / cell.Stage));
			}

			if (cell.Power > 0)
				powerLevel = 4;
			if (cell.Power > cell.MaxPower / 4)
				powerLevel = 3;
			if (cell.Power > (cell.MaxPower / 4) * 2)
				powerLevel = 2;
			if (cell.Power > (cell.MaxPower / 4) * 3)
				powerLevel = 1;

			switch (powerLevel)
			{
				case 1:
				{
					if (chance != rnd)
					{
						pm.Damage(GetDamage(cell, 1));
						pm.Mana = pm.Mana - cell.StatDrain * cell.Stage;
						pm.Stam = pm.Stam - cell.StatDrain * cell.Stage;

						pm.SendMessage(53, SicknessMessages.GetSickMessage(pm, cell, cell.Stage, 1));
					}
				}
					break;
				case 2:
				{
					if (chance != rnd)
					{
						pm.Damage(GetDamage(cell, 2));
						pm.Mana = pm.Mana - ((cell.StatDrain * 2) * cell.Stage);
						pm.Stam = pm.Stam - ((cell.StatDrain * 4) * cell.Stage);

						pm.SendMessage(53, SicknessMessages.GetSickMessage(pm, cell, cell.Stage, 2));
					}
				}
					break;
				case 3:
				{
					if (chance != rnd)
					{
						pm.Damage(GetDamage(cell, 3));
						pm.Mana = pm.Mana - ((cell.StatDrain * 3) * cell.Stage);
						pm.Stam = pm.Stam - ((cell.StatDrain * 6) * cell.Stage);

						pm.SendMessage(53, SicknessMessages.GetSickMessage(pm, cell, cell.Stage, 3));
					}
				}
					break;
				case 4:
				{
					if (chance != rnd)
					{
						pm.Damage(GetDamage(cell, 4));
						pm.Mana = pm.Mana - ((cell.StatDrain * 4) * cell.Stage);
						pm.Stam = pm.Stam - ((cell.StatDrain * 8) * cell.Stage);

						pm.SendMessage(53, SicknessMessages.GetSickMessage(pm, cell, cell.Stage, 4));
					}
				}
					break;
			}
		}

		private static int GetDamage(VirusCell cell, int position)
		{
			int rate = 0;

			switch (position)
			{
				case 1:
					rate = cell.Damage * cell.Stage + 1;
					break;
				case 2:
					rate = cell.Damage * cell.Stage + 2;
					break;
				case 3:
					rate = cell.Damage * cell.Stage + 3;
					break;
				case 4:
					rate = cell.Damage * cell.Stage + 4;
					break;
			}

			if (1 > cell.PM.Hits - rate && cell.Level < 100)
				rate = (cell.PM.Hits / 10) * cell.Stage;
			else
				rate = 0;

			cell.LastSick = ((30 / cell.Stage) - ((cell.PowerDegenRate + cell.Stage) / cell.Stage));

			return rate;
		}

		public static void UpdateImmunePositive(VirusCell cell)
		{
			if (cell.Power < cell.MaxPower)
				cell.Power++;

			cell.LastSick = ((30 / cell.Stage) - ((cell.PowerDegenRate + cell.Stage) / cell.Stage));
		}
	}
}
