#region References

using System;
using System.Collections.Generic;
using System.Linq;
using Server.Mobiles;
using Server.OneTime.Events;
using Server.SicknessSys.Gumps;
using Server.SicknessSys.Illnesses;
using Server.SicknessSys.Items;
using Server.SicknessSys.Mobiles;

#endregion

namespace Server.SicknessSys
{
	public static class SicknessCore
	{
		/// <summary>
		///     Main Core Counter
		/// </summary>
		private static int MainCounter { get; set; }

		/// <summary>
		///     Check for first set up pass
		/// </summary>
		private static bool SetUpCheck { get; set; }

		/// <summary>
		///     Counter for Special Virus Weakness's
		/// </summary>
		private static int WeakCounter { get; set; }

		/// <summary>
		///     VirusCell has been counted?
		/// </summary>
		private static bool VirusCounted { get; set; }

		/// <summary>
		///     Main VirusCell List
		/// </summary>
		public static List<VirusCell> VirusCellList { get; set; }

		/// <summary>
		///     Initialize on Server Start, Set Up Counters, Load Known VirusCells, starts Event Handle
		/// </summary>
		public static void Initialize()
		{
			MainCounter = 0;
			WeakCounter = 25;

			VirusCounted = false;
			SetUpCheck = false;

			if (GetVirusCells())
				OneTimeSecEvent.SecTimerTick += UpdateSickness;
		}

		/// <summary>
		///     Fetches all known VirusCells in the World
		/// </summary>
		/// <returns>True if successful fetch, false for a failed attempt</returns>
		private static bool GetVirusCells()
		{
			if (!VirusCounted)
			{
				List<VirusCell> GetCells = new List<VirusCell>();

				IEnumerable<VirusCell> result = from c in World.Items.Values
					where c is VirusCell
					select c as VirusCell;

				if (result != null)
				{
					foreach (VirusCell cell in result)
					{
						GetCells.Add(cell);
					}

					VirusCellList = GetCells;
				}

				VirusCounted = true;
			}

			return VirusCounted;
		}

		/// <summary>
		///     Core System Events
		/// </summary>
		/// <param name="sender">Timer</param>
		/// <param name="e">Empty</param>
		private static void UpdateSickness(object sender, EventArgs e)
		{
			MainCounter++;

			if (WeakCounter > 0)
				WeakCounter--;
			else
				WeakCounter = 25;

			if (MainCounter > 3600)
			{
				if (VirusCellList.Count > 0)
				{
					if (VirusCellList.Count > 1)
						World.Broadcast(0x35, true,
							"Licznik zakazen = [" + VirusCellList.Count + "] zarazonych graczy");
					else
						World.Broadcast(0x35, true, "Licznik zakazen = 1 zarazony gracz");
				}

				MainCounter = 0;
			}

			if (WeakCounter == 20 || !SetUpCheck)
			{
				//future check slot
			}

			if (WeakCounter == 15 || !SetUpCheck)
			{
				CheckWorldSpawn();
			}

			if (WeakCounter == 10 || !SetUpCheck)
			{
				SicknessInfect.OutBreak(VirusCellList);
			}

			if (WeakCounter == 5 || !SetUpCheck)
			{
				foreach (VirusCell cell in VirusCellList)
				{
					SicknessCure.SelfCureIllness(cell);
				}
			}

			if (!SetUpCheck)
				SetUpCheck = true;

			if (VirusCellList.Count > 0)
				SicknessSequence();
		}

		/// <summary>
		///     Main Sequence to call each VirusCell
		/// </summary>
		private static void SicknessSequence()
		{
			for (int i = 0; i < VirusCellList.Count; i++)
			{
				VirusCell cell = VirusCellList[i];

				if (cell.PM != null)
				{
					if (cell.PM.Alive)
					{
						if (cell.LastSkill > 0)
							cell.LastSkill--;

						if (SicknessHelper.IsSpecialVirus(cell) && WeakCounter < 1)
						{
							if (cell.Illness == IllnessType.Vampirism)
								Vampirism.VampireWeakness(cell);
							else
								Lycanthropia.LycanthropiaWeakness(cell);
						}

						cell.WorldInfectionLevel = VirusCellList.Count;

						SicknessHelper.SendHeartGump(cell);
					}
				}
			}

			//if (VirusCellList.Count > 0)
			//{
			//    foreach (VirusCell cell in VirusCellList)
			//    {
			//        if (cell.PM != null)
			//        {
			//            if (cell.PM.Alive)
			//            {
			//                if (cell.LastSkill > 0)
			//                    cell.LastSkill--;

			//                if (SicknessHelper.IsSpecialVirus(cell) && WeakCounter < 1)
			//                {
			//                    if (cell.Illness == IllnessType.Vampirism)
			//                        Illnesses.Vampirism.VampireWeakness(cell);
			//                    else
			//                        Illnesses.Lycanthropia.LycanthropiaWeakness(cell);
			//                }

			//                cell.WorldInfectionLevel = VirusCellList.Count;

			//                SicknessHelper.SendHeartGump(cell);
			//            }
			//        }
			//    }
			//}
		}

		/// <summary>
		///     Special Virus Spawner
		/// </summary>
		private static void CheckWorldSpawn()
		{
			try
			{
				int numbat = 0;
				int numwolf = 0;

				foreach (Mobile mobile in World.Mobiles.Values)
				{
					if (mobile is InfectedBat)
					{
						numbat++;

						if (numbat > 1)
							mobile.Delete();
					}

					if (mobile is InfectedWolf)
					{
						numwolf++;

						if (numbat > 1)
							mobile.Delete();
					}

					if (mobile is PlayerMobile)
					{
						PlayerMobile pm = mobile as PlayerMobile;

						WhiteCell whitecell = pm.Backpack.FindItemByType(typeof(WhiteCell)) as WhiteCell;
						VirusCell viruscell = pm.Backpack.FindItemByType(typeof(VirusCell)) as VirusCell;

						if (whitecell != null && viruscell == null)
						{
							if (whitecell.DefaultBodyHue != pm.Hue)
							{
								pm.BodyValue = whitecell.DefaultBody;
								pm.Hue = whitecell.DefaultBodyHue;
								pm.HairHue = whitecell.DefaultHairHue;
								pm.FaceHue = whitecell.DefaultFacialHue;

								if (pm.HasGump(typeof(PowerGump)))
									pm.CloseGump(typeof(PowerGump));
							}
						}
					}
				}

				if (numbat < 1 || numbat < 1)
				{
					foreach (Mobile mobile in World.Mobiles.Values)
					{
						if (mobile is VampireBat && numbat < 1)
						{
							numbat++;
							InfectedBat infectedBat = new InfectedBat();
							infectedBat.MoveToWorld(mobile.Location, mobile.Map);
						}

						if (numwolf < 1)
						{
							if (mobile is DireWolf ||
							    mobile is HellHound ||
							    mobile is TimberWolf ||
							    mobile is TsukiWolf ||
							    mobile is WhiteWolf)
							{
								numwolf++;
								PickRandomWolf(mobile);
							}
						}
					}
				}
			}
			catch //Failed Spawning Infected Vampire or Werewolf
			{
			}
		}

		/// <summary>
		///     Spawn Random Infected Wolf to World
		/// </summary>
		/// <param name="mobile"></param>
		private static void PickRandomWolf(Mobile mobile)
		{
			int rnd = Utility.RandomMinMax(1, 5);

			switch (rnd)
			{
				case 1:
				{
					IDireWolf infectedWolf = new IDireWolf();
					infectedWolf.MoveToWorld(mobile.Location, mobile.Map);
					break;
				}
				case 2:
				{
					IHellHound infectedWolf = new IHellHound();
					infectedWolf.MoveToWorld(mobile.Location, mobile.Map);
					break;
				}
				case 3:
				{
					ITimberWolf infectedWolf = new ITimberWolf();
					infectedWolf.MoveToWorld(mobile.Location, mobile.Map);
					break;
				}
				case 4:
				{
					ITsukiWolf infectedWolf = new ITsukiWolf();
					infectedWolf.MoveToWorld(mobile.Location, mobile.Map);
					break;
				}
				case 5:
				{
					IWhiteWolf infectedWolf = new IWhiteWolf();
					infectedWolf.MoveToWorld(mobile.Location, mobile.Map);
					break;
				}
			}
		}
	}
}
