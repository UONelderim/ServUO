#region References

using System;
using System.Collections.Generic;
using System.IO;
using Server.Commands;
using Server.Misc;
using Server.Mobiles;

#endregion

namespace Server.Scripts.Commands
{
	public class TestSkill
	{
		public static void Initialize()
		{
			CommandSystem.Register("testSkill", AccessLevel.GameMaster, TestSkilll_OnCommand);
			CommandSystem.Register("testStat", AccessLevel.GameMaster, TestStat_OnCommand);
			CommandSystem.Register("testTemplate", AccessLevel.GameMaster, TestTemplate_OnCommand);
		}

		private static void testStat(Mobile from, SkillName skillName, int targetStr, int targetDex, int targetInt,
			double targetSkillValue)
		{
			PlayerMobile pm = (PlayerMobile)from;
			bool debugEnabled = pm.GainsDebugEnabled;

			if (debugEnabled)
				pm.GainsDebugEnabled = false;

			double successChance = 0.5; // ASSUMING OPTIMUM

			Skill skill = from.Skills[skillName];
			double delay = SkillCheck.SkillGains[skill.SkillID, 0];
			if (targetSkillValue > 115.0)
				skill.Cap = 120.0;
			else if (targetSkillValue > 110.0)
				skill.Cap = 115.0;
			else if (targetSkillValue > 105.0)
				skill.Cap = 110.0;
			else if (targetSkillValue > 100.0)
				skill.Cap = 105.0;
			from.SkillsCap = 7200;

			List<SkillName> manaReq = new List<SkillName>();
			manaReq.Add(SkillName.Bushido);
			manaReq.Add(SkillName.Chivalry);
			manaReq.Add(SkillName.Inscribe);
			manaReq.Add(SkillName.Magery);
			manaReq.Add(SkillName.Necromancy);
			manaReq.Add(SkillName.SpiritSpeak);
			bool mana = manaReq.Contains(skillName);

			int startStr = from.RawStr;
			int startDex = from.RawDex;
			int startInt = from.RawInt;

			double startSkill = from.Skills[skillName].Base;

			string logsDirectory = "Logs/Gains";
			if (!Directory.Exists(logsDirectory))
				Directory.CreateDirectory(logsDirectory);

			DateTime now = DateTime.Now;
			string fileName = String.Format("{0}_{1}-{2}-{3} {4}-{5}-{6}", from.Account, now.Year, now.Month, now.Day,
				now.Hour, now.Minute, now.Second);
			fileName = Path.Combine(Core.BaseDirectory, logsDirectory + "/" + fileName + ".log");

			FileLogger logger = new FileLogger(fileName, true);
			logger.WriteLine("\tSkillName\tSkillValue\tUses\tskillDelay\tTotalHours");

			double previous = 0;
			int previousStr = 0;
			int previousDex = 0;
			int previousInt = 0;
			int uses;
			for (uses = 0;; ++uses)
			{
				if (from.CheckSkill(skillName, successChance))
				{
					if (mana)
						from.CheckSkill(SkillName.Meditation, 0, 100);
				}

				if (uses % 6 == 0 && false)
					logger.WriteLine("\t" + skill.Name + "\t" + skill.Base + "\t" + (uses + 1) + "\t" + delay + "\t" +
					                 (delay * (uses + 1) / 3600));

				// to avoid infinite loop:
				if (uses % 20100 == 0)
				{
					if (skill.Base == previous && from.RawStr == previousStr && from.RawDex == previousDex &&
					    from.RawInt == previousInt)
					{
						SkillDebug(from, "przerywamy petle!!!!!");
						break;
					}

					previous = skill.Base;
					previousStr = from.RawStr;
					previousDex = from.RawDex;
					previousInt = from.RawInt;
				}

				// blokuj przyrost statow po osiagnieciu docelowej wartosci
				if (from.RawStr >= targetStr && targetStr > 0) from.StrLock = StatLockType.Locked;
				if (from.RawDex >= targetDex && targetDex > 0) from.DexLock = StatLockType.Locked;
				if (from.RawInt >= targetInt && targetInt > 0) from.IntLock = StatLockType.Locked;

				if (skill.Base >= targetSkillValue && targetSkillValue > 0)
				{
					SkillDebug(from, "Skill skoksany - przerywamy!");
					break;
					//skill.SetLockNoRelay(SkillLock.Locked);
				}

				// przerwij koks jesli wszystkie staty/skill osiagnely docelowe wartosci
				if ( //trainStats &&
				    from.RawStr >= targetStr
				    && from.RawDex >= targetDex
				    && from.RawInt >= targetInt
				    && skill.Base >= targetSkillValue
				   )
				{
					SkillDebug(from, "Skoksane!");
					break;
				}

				// przerwij trening gdy skill skoksany - w przypadku braku warunkow na staty docelowe
				//if ( ! trainStats
				//  && (skill.Base == skill.Cap || skill.Base >= targetSkillValue) )
				//    break;
			}

			logger.Close();


			double time = uses * delay;
			int hours = (int)(time / 3600);
			int minutes = (int)(time / 60);
			int seconds = (int)(time % 60);


			SkillDebug(from, "----------------------------");
			SkillDebug(from, "Test statystyk: {0}", skill);
			SkillDebug(from, "----------------------------");
			SkillDebug(from, "Czas trwania: {0}h {1}m {2}s, lacznie {3} sekund", hours, minutes, seconds, time);
			SkillDebug(from, "Skill delay: {0}s", delay);
			SkillDebug(from, "Liczba uzyc: {0}", uses);
			SkillDebug(from, "Wartosci poczatkowe: ");
			SkillDebug(from, "{0}/{1}/{2} ({3}) | {4}: {5}% ({6}% cap)", startStr, startDex, startInt,
				startStr + startDex + startInt, skillName, startSkill, from.Skills[skillName].Cap);
			SkillDebug(from, "Wartosci aktualne: ");
			SkillDebug(from, "{0}/{1}/{2} ({3}) | {4}: {5}% ({6}% cap)", from.RawStr, from.RawDex, from.RawInt,
				from.RawStatTotal, skillName, from.Skills[skillName].Base, from.Skills[skillName].Cap);
			SkillDebug(from, "Przyrosty: ");
			SkillDebug(from, "STR: +{0} | DEX: +{1} INT: +{2} | {3}: +{4}%", from.RawStr - startStr,
				from.RawDex - startDex, from.RawInt - startInt, skillName, from.Skills[skillName].Base - startSkill);

			if (debugEnabled)
				pm.GainsDebugEnabled = true;
		}

		[Usage("testTemplate")]
		[Description("Testuje przyrost statystyk podczas koksu danego template")]
		private static void TestTemplate_OnCommand(CommandEventArgs arg)
		{
			Mobile from = arg.Mobile;
			try
			{
				int template = 0;

				if (template == 0) // Mag-Nekro-Zielarz-Anat
				{
					int Str = 80;
					int Dex = 20;
					int Int = 125;

					foreach (SkillName sk in (SkillName[])Enum.GetValues(typeof(SkillName)))
						from.Skills[sk].Base = 0.0;
					from.Skills[SkillName.Magery].Base = 50.0;
					from.Skills[SkillName.Necromancy].Base = 50.0;
					from.Skills[SkillName.Herbalism].Base = 50.0;

					testStat(from, SkillName.Anatomy, Str, Dex, Int, 60);
					testStat(from, SkillName.Herbalism, Str, Dex, Int, 100);
					testStat(from, SkillName.Mining, Str, Dex, Int, 0);
					from.Skills[SkillName.Mining].SetLockNoRelay(SkillLock.Down);
					testStat(from, SkillName.Necromancy, Str, Dex, Int, 100);
					testStat(from, SkillName.Magery, Str, Dex, Int, 120);
					testStat(from, SkillName.SpiritSpeak, Str, Dex, Int, 100);
					testStat(from, SkillName.EvalInt, Str, Dex, Int, 120);
					testStat(from, SkillName.Meditation, Str, Dex, Int, 100);
				}
				else if (template == 1) // Wojek-Bushi-Parry
				{
					int Str = 80;
					int Dex = 100;
					int Int = 45;

					foreach (SkillName sk in (SkillName[])Enum.GetValues(typeof(SkillName)))
						from.Skills[sk].Base = 0.0;
					from.Skills[SkillName.Bushido].Base = 50.0;
					from.Skills[SkillName.Healing].Base = 50.0;
					from.Skills[SkillName.Swords].Base = 50.0;

					testStat(from, SkillName.Parry, Str, Dex, Int, 120);
					testStat(from, SkillName.Healing, Str, Dex, Int, 10);
					testStat(from, SkillName.Anatomy, Str, Dex, Int, 100);
					testStat(from, SkillName.Swords, Str, Dex, Int, 120);
					testStat(from, SkillName.Tactics, Str, Dex, Int, 100);
					testStat(from, SkillName.Bushido, Str, Dex, Int, 120);
					testStat(from, SkillName.MagicResist, Str, Dex, Int, 60);
				}
				else if (template == 2) // Bard
				{
					int Str = 80;
					int Dex = 20;
					int Int = 125;

					foreach (SkillName sk in (SkillName[])Enum.GetValues(typeof(SkillName)))
						from.Skills[sk].Base = 0.0;
					from.Skills[SkillName.Musicianship].Base = 50.0;
					from.Skills[SkillName.Peacemaking].Base = 50.0;
					from.Skills[SkillName.Magery].Base = 50.0;

					testStat(from, SkillName.Mining, Str, Dex, Int, 0);
					from.Skills[SkillName.Mining].SetLockNoRelay(SkillLock.Down);

					testStat(from, SkillName.Musicianship, Str, Dex, Int, 120);
					testStat(from, SkillName.Peacemaking, Str, Dex, Int, 120);
					testStat(from, SkillName.Discordance, Str, Dex, Int, 120);
					testStat(from, SkillName.Magery, Str, Dex, Int, 120);
					testStat(from, SkillName.EvalInt, Str, Dex, Int, 120);
					testStat(from, SkillName.Meditation, Str, Dex, Int, 100);
				}
			}
			catch (Exception e)
			{
				arg.Mobile.SendMessage("Nieprawidlowy format: " + e.Message);
			}
		}

		[Usage("startSkill2")]
		[Description("Testuje przyrost statystyk podczas koksu danego skilla.")]
		private static void TestStat_OnCommand(CommandEventArgs arg)
		{
			try
			{
				if (arg.Length < 1)
					arg.Mobile.SendMessage(38,
						"TestSkill2 <Skill> [<TargetStr>] [<TargetDex>] [<TargetInt>] [<TargetSkillValue>]");
				else
				{
					SkillName skillName = (SkillName)Enum.Parse(typeof(SkillName), arg.GetString(0), true);
					int targetStr = (arg.Length > 1) ? arg.GetInt32(1) : -1;
					int targetDex = (arg.Length > 2) ? arg.GetInt32(2) : -1;
					int targetInt = (arg.Length > 3) ? arg.GetInt32(3) : -1;
					double targetSkillValue =
						(arg.Length > 4) ? arg.GetDouble(4) : -1.0; // from.Skills[ skillName ].Base;

					testStat(arg.Mobile, skillName, targetStr, targetDex, targetInt, targetSkillValue);
				}
			}
			catch (Exception e)
			{
				arg.Mobile.SendMessage("Nieprawidlowy format: " + e.Message);
			}
		}

		[Usage("startSkill")]
		[Description("Testuje dany skill przez okreslony czas.")]
		private static void TestSkilll_OnCommand(CommandEventArgs arg)
		{
			Mobile from = arg.Mobile;

			try
			{
				if (arg.Length < 7)
					from.SendMessage(38,
						"TestSkill <SkillName> <SkillDelay> <SkillStartValue> <SuccessChance> <Hours> <Minutes> <Seconds>");
				else
				{
					PlayerMobile pm = (PlayerMobile)from;
					bool debugEnabled = pm.GainsDebugEnabled;

					if (debugEnabled)
						pm.GainsDebugEnabled = false;

					SkillName skillName = (SkillName)Enum.Parse(typeof(SkillName), arg.GetString(0), true);
					double skillDelay = arg.GetDouble(1);
					double skillStartValue = arg.GetDouble(2);
					double successChance = arg.GetDouble(3);
					int hours = arg.GetInt32(4);
					int minutes = arg.GetInt32(5);
					int seconds = arg.GetInt32(6);

					int totalSeconds = hours * 3600 + minutes * 60 + seconds;

					from.Skills[skillName].Base = skillStartValue;
					double startSkill = from.Skills[skillName].Base;
					int startStr = from.RawStr;
					int startDex = from.RawDex;
					int startInt = from.RawInt;

					int uses = (int)Math.Floor(totalSeconds / skillDelay);
					Skill skill = from.Skills[skillName];

					string logsDirectory = "Logs/Gains";
					if (!Directory.Exists(logsDirectory))
						Directory.CreateDirectory(logsDirectory);

					DateTime now = DateTime.Now;
					string fileName = String.Format("{0}_{1}-{2}-{3} {4}-{5}-{6}", from.Account, now.Year, now.Month,
						now.Day, now.Hour, now.Minute, now.Second);
					fileName = Path.Combine(Core.BaseDirectory, logsDirectory + "/" + fileName + ".log");

					FileLogger logger = new FileLogger(fileName, true);
					logger.WriteLine("\tSkillName\tSkillValue\tUses\tskillDelay\tTotalHours");

					for (int i = 0; i < uses; i++)
					{
						from.CheckSkill(skillName, successChance);
						if (i % 6 == 0)
							logger.WriteLine("\t" + skill.Name + "\t" + skill.Base + "\t" + (i + 1) + "\t" +
							                 skillDelay + "\t" + (skillDelay * (i + 1) / 3600));

						if (skill.Base == skill.Cap)
							break;
					}

					logger.Close();

					SkillDebug(from, "----------------------------");
					SkillDebug(from, "Test umiejetnosci: {0}", skill);
					SkillDebug(from, "----------------------------");
					SkillDebug(from, "Czas trwania: {0}h {1}m {2}s, lacznie {3} sekund", hours, minutes, seconds,
						totalSeconds);
					SkillDebug(from, "Skill delay: {0}s", skillDelay);
					SkillDebug(from, "Liczba uzyc: {0}", uses);
					SkillDebug(from, "Wartosci poczatkowe: ");
					SkillDebug(from, "{0}/{1}/{2} ({3}) | {4}: {5}% ({6}% cap)", startStr, startDex, startInt,
						startStr + startDex + startInt, skillName, startSkill, from.Skills[skillName].Cap);
					SkillDebug(from, "Wartosci aktualne: ");
					SkillDebug(from, "{0}/{1}/{2} ({3}) | {4}: {5}% ({6}% cap)", from.RawStr, from.RawDex, from.RawInt,
						from.RawStatTotal, skillName, from.Skills[skillName].Base, from.Skills[skillName].Cap);
					SkillDebug(from, "Przyrosty: ");
					SkillDebug(from, "STR: +{0} | DEX: +{1} INT: +{2} | {3}: +{4}%", from.RawStr - startStr,
						from.RawDex - startDex, from.RawInt - startInt, skillName,
						from.Skills[skillName].Base - startSkill);

					if (debugEnabled)
						pm.GainsDebugEnabled = true;
				}
			}
			catch (Exception e)
			{
				from.SendMessage("Nieprawidlowy format: " + e.Message);
			}
		}

		public static void SkillDebug(Mobile from, string msg, params object[] args)
		{
			if (args.Length > 0)
				msg = String.Format(msg, args);

			from.SendMessage(0x501, msg);
			//Console.WriteLine( msg );
		}
	}
}
