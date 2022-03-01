namespace Server.SicknessSys.Illnesses
{
	static class IllnessMutationLists
	{
		public static void SetMutation(VirusCell cell)
		{
			IllnessType illness = cell.Illness;

			switch (illness)
			{
				case IllnessType.Cold:

					if (cell.Stage == 1)
					{
						cell.PM.Hue = 5;
					}

					if (cell.Stage == 2 && cell.PM.Hue != 10)
					{
						cell.PM.Hue = 10;

						SicknessAnimate.RunInfectedAnimation(cell.PM);
					}

					if (cell.Stage == 3 && cell.PM.Hue != 15)
					{
						cell.PM.Hue = 15;

						SicknessAnimate.RunInfectedAnimation(cell.PM);
					}

					break;

				case IllnessType.Flu:

					if (cell.Stage == 1)
					{
						cell.PM.Hue = 60;
					}

					if (cell.Stage == 2 && cell.PM.Hue != 65)
					{
						cell.PM.Hue = 65;

						SicknessAnimate.RunInfectedAnimation(cell.PM);
					}

					if (cell.Stage == 3 && cell.PM.Hue != 70)
					{
						cell.PM.Hue = 70;

						SicknessAnimate.RunInfectedAnimation(cell.PM);
					}

					break;

				case IllnessType.Virus:

					if (cell.Stage == 1)
					{
						cell.PM.Hue = 145;
					}

					if (cell.Stage == 2 && cell.PM.Hue != 150)
					{
						cell.PM.Hue = 150;

						SicknessAnimate.RunInfectedAnimation(cell.PM);
					}

					if (cell.Stage == 3 && cell.PM.Hue != 155)
					{
						cell.PM.Hue = 155;

						SicknessAnimate.RunInfectedAnimation(cell.PM);
					}

					break;

				case IllnessType.Vampirism:

					if (SicknessHelper.IsNight(cell.PM))
					{
						if (cell.PM.Hue == 1154 || cell.PM.Hue == 1153 || cell.PM.Hue == 1150)
						{
							if (cell.Level > 99 && cell.PM.Hue != 1175)
							{
								cell.PM.Hue = 1175;
								cell.PM.HairHue = 1153;
								cell.PM.FacialHairHue = 1153;

								SicknessAnimate.RunMutateAnimation(cell.PM);
							}
						}
					}
					else
					{
						if (cell.PM.Hue == 1154 || cell.PM.Hue == 1153 || cell.PM.Hue == 1150 || cell.PM.Hue == 1175 ||
						    cell.PM.Hue == cell.DefaultBodyHue)
						{
							if (cell.Stage == 1 && cell.PM.Hue != 1154)
							{
								cell.PM.Hue = 1154;
								cell.PM.HairHue = 1617;
								cell.PM.FacialHairHue = 1617;

								SicknessAnimate.RunMutateAnimation(cell.PM);
							}

							if (cell.Stage == 2 && cell.PM.Hue != 1150)
							{
								cell.PM.Hue = 1150;
								cell.PM.HairHue = 1172;
								cell.PM.FacialHairHue = 1172;

								SicknessAnimate.RunMutateAnimation(cell.PM);
							}

							if (cell.Stage == 3 && cell.PM.Hue != 1153)
							{
								cell.PM.Hue = 1153;
								cell.PM.HairHue = 1157;
								cell.PM.FacialHairHue = 1157;

								SicknessAnimate.RunMutateAnimation(cell.PM);
							}
						}
					}

					break;

				case IllnessType.Lycanthropia:

					//Item toDisarm1 = cell.PM.FindItemOnLayer(Layer.OneHanded);
					//Item toDisarm2 = cell.PM.FindItemOnLayer(Layer.TwoHanded);

					if (!SicknessHelper.IsNight(cell.PM) || cell.Level > 99 && !SicknessHelper.IsDark(cell.PM))
					{
						if (cell.PM.BodyValue != cell.DefaultBody)
						{
							//    if (toDisarm2 is WereClaws)
							//    {
							//        cell.PM.Backpack.DropItem(toDisarm2);
							//    }

							if (cell.PM.Hue == 1049 || cell.PM.Hue == 1050 || cell.PM.Hue == 1051 ||
							    cell.PM.Hue == 1175)
							{
								SicknessAnimate.RunMutateAnimation(cell.PM);

								cell.PM.BodyValue = cell.DefaultBody;
								cell.PM.Hue = cell.DefaultBodyHue;
							}
						}
					}
					else if (cell.PM.BodyValue == cell.DefaultBody)
					{
						//WereClaws wc = cell.PM.Backpack.FindItemByType(typeof(WereClaws)) as WereClaws;

						//if (toDisarm1 != null || toDisarm2 != null)
						//{
						//    if (toDisarm2 is WereClaws)
						//    {
						//        //do nothing, already equipped
						//    }
						//    else
						//    {
						//        if (toDisarm1 != null)
						//            cell.PM.Backpack.DropItem(toDisarm1);
						//        if (toDisarm2 != null)
						//            cell.PM.Backpack.DropItem(toDisarm2);

						//        if (wc != null)
						//            cell.PM.EquipItem(wc);
						//        else
						//        {
						//            cell.PM.AddToBackpack(new WereClaws(cell.PM));

						//            wc = cell.PM.Backpack.FindItemByType(typeof(WereClaws)) as WereClaws;

						//            cell.PM.EquipItem(wc);
						//        }
						//    }
						//}
						//else
						//{
						//    if (wc != null)
						//        cell.PM.EquipItem(wc);
						//    else
						//    {
						//        cell.PM.AddToBackpack(new WereClaws(cell.PM));

						//        wc = cell.PM.Backpack.FindItemByType(typeof(WereClaws)) as WereClaws;

						//        cell.PM.EquipItem(wc);
						//    }
						//}

						SicknessAnimate.RunMutateAnimation(cell.PM);

						if (cell.Stage == 1)
						{
							cell.PM.BodyValue = 23;
							cell.PM.Hue = 1049;
						}

						if (cell.Stage == 2)
						{
							cell.PM.BodyValue = 98;
							cell.PM.Hue = 1050;
						}

						if (cell.Stage == 3)
						{
							if (cell.Level >= 100)
							{
								cell.PM.BodyValue = 250;
								cell.PM.Hue = 1175;
							}
							else
							{
								cell.PM.BodyValue = 246;
								cell.PM.Hue = 1051;
							}
						}
					}
					else if (SicknessHelper.IsNight(cell.PM))
					{
						int HowlingChance = Utility.RandomMinMax(1, 100);

						if (HowlingChance > 99)
						{
							cell.PM.PlaySound(0x0E6);
						}
					}

					break;

				default:

					cell.PM.Body = cell.DefaultBody;
					cell.PM.Hue = cell.DefaultBodyHue;
					cell.PM.HairHue = cell.DefaultHairHue;
					cell.PM.FacialHairHue = cell.DefaultFacialHue;

					break;
			}
		}
	}
}
