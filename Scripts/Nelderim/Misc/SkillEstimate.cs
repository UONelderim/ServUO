using System;
using System.Globalization;
using System.Linq;
using Server;
using Server.Commands;
using Server.Engines.Craft;
using Server.Engines.Harvest;
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
			var successGainChance = SkillCheck.GetGainChance(from, skill, chance, true);
			var failedGainChance = SkillCheck.GetGainChance(from, skill, chance, false);
			var avgChance = ChanceBasedAverage(chance, successGainChance, failedGainChance);

			from.Skills[skill.SkillID].BaseFixedPoint = oldValue;
			return 1/avgChance * GetSkillDelay(from, skill);
		}

		private static double GetOptimalSuccessChance(Mobile from, Skill skill)
		{
			switch (skill.SkillName)
			{
				case SkillName.Alchemy: return OptimalCraftSuccessChance(from, DefAlchemy.CraftSystem);
				case SkillName.Anatomy: return 0;
				case SkillName.AnimalLore: return 0;
				case SkillName.ItemID: return 0;
				case SkillName.ArmsLore: return MinMaxSuccessChance(skill, 0, 100);
				case SkillName.Parry: return 0;
				case SkillName.Begging: return 0;
				case SkillName.Blacksmith: return OptimalCraftSuccessChance(from, DefBlacksmithy.CraftSystem);
				case SkillName.Fletching: return OptimalCraftSuccessChance(from, DefBowFletching.CraftSystem);
				case SkillName.Peacemaking: return 0.5; //optimal
				case SkillName.Camping: return 0;
				case SkillName.Carpentry: return OptimalCraftSuccessChance(from, DefCarpentry.CraftSystem);
				case SkillName.Cartography: return OptimalCraftSuccessChance(from, DefCartography.CraftSystem);
				case SkillName.Cooking: return OptimalCraftSuccessChance(from, DefCooking.CraftSystem);
				case SkillName.DetectHidden: return 0;
				case SkillName.Discordance: return 0.5; //optimal
				case SkillName.EvalInt: return MinMaxSuccessChance(skill, 0, skill.Cap); //EvalInt assumes only direct skill use, without casting spells
				case SkillName.Healing: return 0;
				case SkillName.Fishing: return HarvestSuccessChance(from, skill, Fishing.System, Fishing.System.Definition);
				case SkillName.Herbalism: return 0;
				case SkillName.Herding: return 0;
				case SkillName.Hiding: return 0;
				case SkillName.Provocation: return 0.5; //optimal
				case SkillName.Inscribe: return OptimalCraftSuccessChance(from, DefInscription.CraftSystem);
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
				case SkillName.Tailoring: return OptimalCraftSuccessChance(from, DefTailoring.CraftSystem);
				case SkillName.AnimalTaming: return 0.5; //optimal
				case SkillName.TasteID: return 0;
				case SkillName.Tinkering: return OptimalCraftSuccessChance(from, DefTinkering.CraftSystem);
				case SkillName.Tracking: return 0;
				case SkillName.Veterinary: return 0;
				case SkillName.Swords: return 0;
				case SkillName.Macing: return 0;
				case SkillName.Fencing: return 0;
				case SkillName.Wrestling: return 0;
				case SkillName.Lumberjacking: return HarvestSuccessChance(from, skill, Lumberjacking.System, Lumberjacking.System.Definition);
				case SkillName.Mining: return HarvestSuccessChance(from, skill, Mining.System, Mining.System.OreAndStone);
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

		private static double MinMaxSuccessChance(Skill skill, double minSkill, double maxSkill)
		{
			return (skill.Base - minSkill) / (maxSkill - minSkill);
		}

		private static double HarvestSuccessChance(Mobile from, Skill skill, HarvestSystem system, HarvestDefinition def)
		{
			var weightedVeins = def.Veins
				.ToDictionary(harvestVein => harvestVein, harvestVein => (int)(harvestVein.VeinChance * 10));

			HarvestVein vein = Utility.RandomWeigthed(weightedVeins);
			HarvestResource res = system.MutateResource(from, null, def, null, Point3D.Zero, vein, vein.PrimaryResource,
				vein.FallbackResource);

			return MinMaxSuccessChance(skill, res.MinSkill, res.MaxSkill);
		}

		private static double OptimalCraftSuccessChance(Mobile from, CraftSystem system)
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

		private static double GetSkillDelay(Mobile from, Skill skill)
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
				case SkillName.Peacemaking: return ChanceBasedAverage(GetOptimalSuccessChance(from, skill), 10, 5);
				case SkillName.Camping: return 0;
				case SkillName.Carpentry: return DefCarpentry.CraftSystem.Delay;
				case SkillName.Cartography: return DefCartography.CraftSystem.Delay;
				case SkillName.Cooking: return DefCooking.CraftSystem.Delay;
				case SkillName.DetectHidden: return 0;
				case SkillName.Discordance: return ChanceBasedAverage(GetOptimalSuccessChance(from, skill), 8, 5);;
				case SkillName.EvalInt: return 1;
				case SkillName.Healing: return 0;
				case SkillName.Fishing: return 0;
				case SkillName.Herbalism: return 0;
				case SkillName.Herding: return 0;
				case SkillName.Hiding: return 0;
				case SkillName.Provocation: return 10;
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
				case SkillName.AnimalTaming: return 4 * 3; //
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

		private static double ChanceBasedAverage(double chance, double successValue, double failValue)
		{
			return successValue * chance + failValue * (1 - chance); 
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
