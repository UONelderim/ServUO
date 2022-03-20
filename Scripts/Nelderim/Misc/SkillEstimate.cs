using System;
using System.Globalization;
using Server;
using Server.Commands;
using Server.Engines.Craft;
using Server.Misc;

namespace Nelderim
{
	public class SkillEstimate
	{
		public static void Initialize()
		{
			CommandSystem.Register("skillEstimate", AccessLevel.Administrator, SkillEstimate_OnCommand);
		}

		private static void SkillEstimate_OnCommand(CommandEventArgs e)
		{
			if (Enum.TryParse(e.Arguments[0], true, out SkillName skillName))
			{
				int start, end;
				if (e.Length == 2)
				{
					start = e.Mobile.Skills[skillName].BaseFixedPoint;
					end = Int32.Parse(e.Arguments[1]);
				}
				else
				{
					start = Int32.Parse(e.Arguments[1]);
					end = Int32.Parse(e.Arguments[2]);
				}

				Mobile fakeMob = new Mobile();
				foreach (var fromSkill in e.Mobile.Skills)
				{
					fakeMob.Skills[fromSkill.SkillID].Base = fromSkill.Base;
					fakeMob.Skills[fromSkill.SkillID].Cap = fromSkill.Cap;
				}

				try
				{
					e.Mobile.SendMessage(
						IntegralTrapezoidRule(x => AverageTimeToGain(fakeMob, fakeMob.Skills[skillName], x), start,
							end, end - start).ToString(CultureInfo.CurrentCulture));
				}
				finally
				{
					fakeMob.Delete();
				}
			}
		}

		public static double AverageTimeToGain(Mobile from, Skill skill, double value)
		{
			int oldValue = skill.BaseFixedPoint;
			from.Skills[skill.SkillID].BaseFixedPoint = (int)value;
			var chance = GetOptimalSuccessChance(from, skill);
			var successChance = SkillCheck.GetGainChance(from, skill, chance, true);
			var failedChance = SkillCheck.GetGainChance(from, skill, chance, false);
			var avgChance = successChance * chance + failedChance * (1 - chance);

			from.Skills[skill.SkillID].BaseFixedPoint = oldValue;
			return 1/avgChance * GetSkillDelay(skill);
		}

		private static double GetOptimalSuccessChance(Mobile from, Skill skill)
		{
			switch (skill.SkillName)
			{
				case SkillName.Alchemy: return GetOptimalCraftSuccessChance(from, DefAlchemy.CraftSystem);
				case SkillName.Anatomy: return 0;
				case SkillName.AnimalLore: return 0;
				case SkillName.ItemID: return 0;
				case SkillName.ArmsLore: return Math.Min(100, skill.Base) / 100;
				case SkillName.Parry: return 0;
				case SkillName.Begging: return 0;
				case SkillName.Blacksmith: return GetOptimalCraftSuccessChance(from, DefBlacksmithy.CraftSystem);
				case SkillName.Fletching: return GetOptimalCraftSuccessChance(from, DefBowFletching.CraftSystem);
				case SkillName.Peacemaking: return 0;
				case SkillName.Camping: return 0;
				case SkillName.Carpentry: return GetOptimalCraftSuccessChance(from, DefCarpentry.CraftSystem);
				case SkillName.Cartography: return GetOptimalCraftSuccessChance(from, DefCartography.CraftSystem);
				case SkillName.Cooking: return GetOptimalCraftSuccessChance(from, DefCooking.CraftSystem);
				case SkillName.DetectHidden: return 0;
				case SkillName.Discordance: return 0;
				case SkillName.EvalInt: return 0;
				case SkillName.Healing: return 0;
				case SkillName.Fishing: return 0;
				case SkillName.Herbalism: return 0;
				case SkillName.Herding: return 0;
				case SkillName.Hiding: return 0;
				case SkillName.Provocation: return 0;
				case SkillName.Inscribe: return GetOptimalCraftSuccessChance(from, DefInscription.CraftSystem);
				case SkillName.Lockpicking: return 0;
				case SkillName.Magery: return 0;
				case SkillName.MagicResist: return 0;
				case SkillName.Tactics: return 0;
				case SkillName.Snooping: return 0;
				case SkillName.Musicianship: return 0;
				case SkillName.Poisoning: return 0;
				case SkillName.Archery: return 0;
				case SkillName.SpiritSpeak: return 0;
				case SkillName.Stealing: return 0;
				case SkillName.Tailoring: return GetOptimalCraftSuccessChance(from, DefTailoring.CraftSystem);
				case SkillName.AnimalTaming: return 0;
				case SkillName.TasteID: return 0;
				case SkillName.Tinkering: return GetOptimalCraftSuccessChance(from, DefTinkering.CraftSystem);
				case SkillName.Tracking: return 0;
				case SkillName.Veterinary: return 0;
				case SkillName.Swords: return 0;
				case SkillName.Macing: return 0;
				case SkillName.Fencing: return 0;
				case SkillName.Wrestling: return 0;
				case SkillName.Lumberjacking: return 0;
				case SkillName.Mining: return 0;
				case SkillName.Meditation: return 0;
				case SkillName.Stealth: return 0;
				case SkillName.RemoveTrap: return 0;
				case SkillName.Necromancy: return 0;
				case SkillName.Focus: return 0;
				case SkillName.Chivalry: return 0;
				case SkillName.Bushido: return 0;
				case SkillName.Ninjitsu: return 0;
				case SkillName.Spellweaving: return 0;
				case SkillName.Mysticism: return 0;
				case SkillName.Imbuing: return 0;
				case SkillName.Throwing: return 0;
				default: throw new NotImplementedException();
			}
		}

		private static double GetOptimalCraftSuccessChance(Mobile from, CraftSystem system)
		{
			double bestChance = 0.0;
			double bestChanceDiff = 1;
			CraftItem debugItem = system.CraftItems.GetAt(0);
			for (var i = 0; i < system.CraftItems.Count; i++)
			{
				var craftItem = system.CraftItems.GetAt(i);
				bool allRequiredSkills = true;
				var chance = craftItem.GetSuccessChance(from, null, system, false, ref allRequiredSkills);
				var chanceDiff = Math.Abs(chance - 0.5);
				if (chanceDiff < bestChanceDiff)
				{
					debugItem = craftItem;
					bestChance = chance;
					bestChanceDiff = chanceDiff;
				}
			}
			Console.WriteLine($"{from.Skills[system.MainSkill].Base} {debugItem.ItemType.Name} {debugItem.Skills.GetAt(0).MinSkill} {debugItem.Skills.GetAt(0).MaxSkill} {bestChance}");
			return bestChance;
		}

		private static double GetSkillDelay(Skill skill)
		{
			switch (skill.SkillName)
			{
				case SkillName.Alchemy: return DefAlchemy.CraftSystem.Delay;
				case SkillName.Anatomy: return 0;
				case SkillName.AnimalLore: return 0;
				case SkillName.ItemID: return 0;
				case SkillName.ArmsLore: return 1;
				case SkillName.Parry: return 0;
				case SkillName.Begging: return 0;
				case SkillName.Blacksmith: return DefBlacksmithy.CraftSystem.Delay;
				case SkillName.Fletching: return DefBowFletching.CraftSystem.Delay;
				case SkillName.Peacemaking: return 0;
				case SkillName.Camping: return 0;
				case SkillName.Carpentry: return DefCarpentry.CraftSystem.Delay;
				case SkillName.Cartography: return DefCartography.CraftSystem.Delay;
				case SkillName.Cooking: return DefCooking.CraftSystem.Delay;
				case SkillName.DetectHidden: return 0;
				case SkillName.Discordance: return 0;
				case SkillName.EvalInt: return 0;
				case SkillName.Healing: return 0;
				case SkillName.Fishing: return 0;
				case SkillName.Herbalism: return 0;
				case SkillName.Herding: return 0;
				case SkillName.Hiding: return 0;
				case SkillName.Provocation: return 0;
				case SkillName.Inscribe: return DefInscription.CraftSystem.Delay;
				case SkillName.Lockpicking: return 0;
				case SkillName.Magery: return 0;
				case SkillName.MagicResist: return 0;
				case SkillName.Tactics: return 0;
				case SkillName.Snooping: return 0;
				case SkillName.Musicianship: return 0;
				case SkillName.Poisoning: return 0;
				case SkillName.Archery: return 0;
				case SkillName.SpiritSpeak: return 0;
				case SkillName.Stealing: return 0;
				case SkillName.Tailoring: return DefTailoring.CraftSystem.Delay;
				case SkillName.AnimalTaming: return 0;
				case SkillName.TasteID: return 0;
				case SkillName.Tinkering: return DefTinkering.CraftSystem.Delay;
				case SkillName.Tracking: return 0;
				case SkillName.Veterinary: return 0;
				case SkillName.Swords: return 0;
				case SkillName.Macing: return 0;
				case SkillName.Fencing: return 0;
				case SkillName.Wrestling: return 0;
				case SkillName.Lumberjacking: return 0;
				case SkillName.Mining: return 0;
				case SkillName.Meditation: return 0;
				case SkillName.Stealth: return 0;
				case SkillName.RemoveTrap: return 0;
				case SkillName.Necromancy: return 0;
				case SkillName.Focus: return 0;
				case SkillName.Chivalry: return 0;
				case SkillName.Bushido: return 0;
				case SkillName.Ninjitsu: return 0;
				case SkillName.Spellweaving: return 0;
				case SkillName.Mysticism: return 0;
				case SkillName.Imbuing: return 0;
				case SkillName.Throwing: return 0;
				default: throw new NotImplementedException();
			}
		}


		static double IntegralTrapezoidRule(Func<double, double> func, double a, double b, int iterations)
		{
			double area = 0;
			double step = (b - a) / iterations;
			area += (func(a) + func(b)) * 0.5;
			for (int i = 1; i < iterations; i++)
				area += func(a + step * i);
			return area * step;
		}
	}
}
