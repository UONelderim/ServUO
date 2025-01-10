using System;
using System.Collections.Generic;
using System.Linq;
using Server;
using Server.Commands;
using Server.Engines.Craft;
using Server.Engines.Harvest;
using Server.Items;
using Server.Misc;
using Server.Spells;
using Server.Spells.Bushido;

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
					e.Mobile.SendMessage($"Trening {skillName} od {start/10} do {end/10} zajmie około " + TimeSpan.FromSeconds(
						IntegralTrapezoidRule(x => AverageTimeToGain(e.Mobile, fakeMob, fakeMob.Skills[skillName], x), 
							start, end, end - start)).ToString(@"c" ));
				}
				finally
				{
					fakeMob.Delete();
				}
			}
		}

		private static double AverageTimeToGain(Mobile from, Mobile fakeMob, Skill skill, double value)
		{
			skill.BaseFixedPoint = (int)value;
			var chance = GetOptimalSuccessChance(from, fakeMob, skill);
			var successGainChance = SkillCheck.GetGainChance(fakeMob, skill, chance, true);
			var failedGainChance = SkillCheck.GetGainChance(fakeMob, skill, chance, false);
			var avgChance = ChanceBasedAverage(chance, successGainChance, failedGainChance);

			var skillDelay = GetSkillDelay(from, fakeMob, skill);
			var result = 1 / avgChance * skillDelay;
			return result;
		}

		private static double GetOptimalSuccessChance(Mobile from, Mobile fakeMob, Skill skill)
		{
			switch (skill.SkillName)
			{
				case SkillName.Alchemy: return OptimalCraftSuccessChance(fakeMob, DefAlchemy.CraftSystem);
				case SkillName.Anatomy: return MinMaxSuccessChance(skill);
				case SkillName.AnimalLore: return MinMaxSuccessChance(skill);
				case SkillName.ItemID: return 0; // Not relevant
				case SkillName.ArmsLore: return MinMaxSuccessChance(skill);
				case SkillName.Parry: return 0.5; ///At average should be around 50% if we train correctly
				case SkillName.Begging: return 0; // Not relevant
				case SkillName.Blacksmith: return OptimalCraftSuccessChance(fakeMob, DefBlacksmithy.CraftSystem);
				case SkillName.Fletching: return OptimalCraftSuccessChance(fakeMob, DefBowFletching.CraftSystem);
				case SkillName.Peacemaking: return 0.5; //At average should be around 50% if we train correctly
				case SkillName.Camping: return MinMaxSuccessChance(skill);
				case SkillName.Carpentry: return OptimalCraftSuccessChance(fakeMob, DefCarpentry.CraftSystem);
				case SkillName.Cartography: return OptimalCraftSuccessChance(fakeMob, DefCartography.CraftSystem);
				case SkillName.Cooking: return OptimalCraftSuccessChance(fakeMob, DefCooking.CraftSystem);
				case SkillName.DetectHidden: return MinMaxSuccessChance(skill);
				case SkillName.Discordance: return 0.5; //At average should be around 50% if we train correctly
				case SkillName.EvalInt: return MinMaxSuccessChance(skill); //EvalInt assumes only direct skill use, without casting spells
				case SkillName.Healing: return MinMaxSuccessChance(skill, 0, 120);
				case SkillName.Fishing: return HarvestSuccessChance(fakeMob, skill, Fishing.System, Fishing.System.Definition);
				case SkillName.Herbalism: return skill.Value < 85.0 ? MinMaxSuccessChance(skill, 0, 90) : MinMaxSuccessChance(skill, 80, 100);
				case SkillName.Herding: return 0.5;
				case SkillName.Hiding: return MinMaxSuccessChance(skill);
				case SkillName.Provocation: return 0.5; ///At average should be around 50% if we train correctly
				case SkillName.Inscribe: return OptimalCraftSuccessChance(fakeMob, DefInscription.CraftSystem);
				case SkillName.Lockpicking: return 0.5; ///At average should be around 50% if we train correctly
				case SkillName.Magery: return MagicalSuccessChance(fakeMob, skill);
				case SkillName.MagicResist: return MinMaxSuccessChance(skill, 0, 120);
				case SkillName.Tactics: return MinMaxSuccessChance(skill);
				case SkillName.Snooping: return MinMaxSuccessChance(skill);
				case SkillName.Musicianship: return MinMaxSuccessChance(skill, 0, 120);
				case SkillName.Poisoning: return 0.5; ///At average should be around 50% if we train correctly
				case SkillName.Archery: return 0.5; ///At average should be around 50% if we train correctly
				case SkillName.SpiritSpeak: return MinMaxSuccessChance(skill, 0, 120);
				case SkillName.Stealing: return 0.5; //At average should be around 50% if we train correctly
				case SkillName.Tailoring: return OptimalCraftSuccessChance(fakeMob, DefTailoring.CraftSystem);
				case SkillName.AnimalTaming: return 0.5; //At average should be around 50% if we train correctly
				case SkillName.TasteID: return 0; // Not relevant
				case SkillName.Tinkering: return OptimalCraftSuccessChance(fakeMob, DefTinkering.CraftSystem);
				case SkillName.Tracking: return MinMaxSuccessChance(skill); //It's not as simple as that, but it's good enough
				case SkillName.Veterinary: return MinMaxSuccessChance(skill, 0, 120);
				case SkillName.Swords: return 0.5; ///At average should be around 50% if we train correctly
				case SkillName.Macing: return 0.5; ///At average should be around 50% if we train correctly
				case SkillName.Fencing: return 0.5; ///At average should be around 50% if we train correctly
				case SkillName.Wrestling: return 0.5; ///At average should be around 50% if we train correctly
				case SkillName.Lumberjacking: return HarvestSuccessChance(fakeMob, skill, Lumberjacking.System, Lumberjacking.System.Definition);
				case SkillName.Mining: return HarvestSuccessChance(fakeMob, skill, Mining.System, Mining.System.OreAndStone);
				case SkillName.Meditation: return 0; // Not relevant, trains itself
				case SkillName.Stealth: return 0.5; //At average should be around 50% if we train correctly
				case SkillName.RemoveTrap: return 0.5; //At average should be around 50% if we train correctly
				case SkillName.Necromancy: return MagicalSuccessChance(fakeMob, skill);
				case SkillName.Focus: return 0; // Not relevant, trains itself
				case SkillName.Chivalry: return MagicalSuccessChance(fakeMob, skill);
				case SkillName.Bushido: return MagicalSuccessChance(fakeMob, skill);
				case SkillName.Ninjitsu: return MagicalSuccessChance(fakeMob, skill);
				case SkillName.Spellweaving: return MagicalSuccessChance(fakeMob, skill);
				case SkillName.Mysticism: return MagicalSuccessChance(fakeMob, skill);
				case SkillName.Imbuing: return 0.5; //At average should be around 50% if we train correctly
				case SkillName.Throwing: return 0.5; ///At average should be around 50% if we train correctly
				default: throw new NotImplementedException();
			}
		}

		private static double MinMaxSuccessChance(Skill skill)
		{
			return MinMaxSuccessChance(skill, 0, skill.Cap);
		}

		private static double MinMaxSuccessChance(Skill skill, double minSkill, double maxSkill)
		{
			return (skill.Base - minSkill) / (maxSkill - minSkill);
		}

		private static double HarvestSuccessChance(Mobile fakeMob, Skill skill, HarvestSystem system, HarvestDefinition def)
		{
			var weightedVeins = def.Veins
				.ToDictionary(harvestVein => harvestVein, harvestVein => (int)(harvestVein.VeinChance * 10));

			HarvestVein vein = Utility.RandomWeigthed(weightedVeins);
			HarvestResource res = system.MutateResource(fakeMob, null, def, null, Point3D.Zero, vein, vein.PrimaryResource,
				vein.FallbackResource);

			return MinMaxSuccessChance(skill, res.MinSkill, res.MaxSkill);
		}

		private static double OptimalCraftSuccessChance(Mobile fakeMob, CraftSystem system)
		{
			double bestChance = 0.0;
			double bestChanceDiff = 1;
			CraftItem debugItem = system.CraftItems.GetAt(0);
			for (var i = 0; i < system.CraftItems.Count; i++)
			{
				var craftItem = system.CraftItems.GetAt(i);
				bool allRequiredSkills = true;
				var chance = craftItem.GetSuccessChance(fakeMob, null, system, false, ref allRequiredSkills);
				var chanceDiff = Math.Abs(chance - 0.5);
				if (chanceDiff < bestChanceDiff)
				{
					debugItem = craftItem;
					bestChance = chance;
					bestChanceDiff = chanceDiff;
				}
			}
			// Console.WriteLine($"{fakeMob.Skills[system.MainSkill].Base} {debugItem.ItemType.Name} {debugItem.Skills.GetAt(0).MinSkill} {debugItem.Skills.GetAt(0).MaxSkill} {bestChance}");
			return bestChance;
		}

		private static HashSet<Spell> spellCache = new HashSet<Spell>();
		private static HashSet<SpecialMove> moveCache = new HashSet<SpecialMove>();

		private static double MagicalSuccessChance(Mobile fakeMob, Skill skill)
		{
			if (spellCache.Count == 0)
				foreach (var type in SpellRegistry.Types)
					if (type != null)
						if (type.IsSubclassOf(typeof(SpecialMove)))
							moveCache.Add((SpecialMove)Activator.CreateInstance(type));
						else
							spellCache.Add((Spell)Activator.CreateInstance(type, fakeMob, null));

			return BestSpellChance(skill, out _);
		}

		private static double BestSpellChance(Skill skill, out object bestSpell)
		{
			bestSpell = null;
			double bestChance = 0.0;
			double bestChanceDiff = 1;
			double chance;
			if (skill.SkillName == SkillName.Bushido || skill.SkillName == SkillName.Ninjitsu)
			{
				foreach (var move in moveCache)
				{
					if (move.MoveSkill != skill.SkillName) continue;
					if (move is LightningStrike && skill.Value >= 87.5)
						chance = MinMaxSuccessChance(skill, 0, skill.Cap) / 4;
					else if (move is MomentumStrike)
						chance = MinMaxSuccessChance(skill, move.RequiredSkill, 120);
					else
						chance = MinMaxSuccessChance(skill, move.RequiredSkill - 12.5, move.RequiredSkill + 37.5);

					var chanceDiff = Math.Abs(0.65 - chance);
					if (chanceDiff < bestChanceDiff)
					{
						bestSpell = move;
						bestChance = chance;
						bestChanceDiff = chanceDiff;
					}
				}
			}
			else
			{
				foreach (var spell in spellCache)
				{
					if (spell.CastSkill != skill.SkillName) continue;
					spell.GetCastSkills(out var min, out var max);
					chance = MinMaxSuccessChance(skill, min, max);

					var chanceDiff = Math.Abs(0.65 - chance);
					if (chanceDiff < bestChanceDiff)
					{
						bestSpell = spell;
						bestChance = chance;
						bestChanceDiff = chanceDiff;
					}
				}
			}

			return bestChance;
		}

		private static double GetSkillDelay(Mobile from, Mobile fakeMob, Skill skill)
		{
			switch (skill.SkillName)
			{
				case SkillName.Alchemy: return DefAlchemy.CraftSystem.Delay;
				case SkillName.Anatomy: return 1;
				case SkillName.AnimalLore: return 1;
				case SkillName.ItemID: return 1;
				case SkillName.ArmsLore: return 1;
				case SkillName.Parry: return 0;
				case SkillName.Begging: return 10;
				case SkillName.Blacksmith: return DefBlacksmithy.CraftSystem.Delay;
				case SkillName.Fletching: return DefBowFletching.CraftSystem.Delay;
				case SkillName.Peacemaking: return ChanceBasedAverage(GetOptimalSuccessChance(from, fakeMob, skill), 10, 5);
				case SkillName.Camping: return 2;
				case SkillName.Carpentry: return DefCarpentry.CraftSystem.Delay;
				case SkillName.Cartography: return DefCartography.CraftSystem.Delay;
				case SkillName.Cooking: return DefCooking.CraftSystem.Delay;
				case SkillName.DetectHidden: return 10;
				case SkillName.Discordance: return ChanceBasedAverage(GetOptimalSuccessChance(from, fakeMob, skill), 8, 5);;
				case SkillName.EvalInt: return 1;
				case SkillName.Healing: return BandageContext.GetDelay(from, fakeMob, false, skill.SkillName).TotalSeconds;
				case SkillName.Fishing: return Fishing.System.Definition.EffectDelay.TotalSeconds;
				case SkillName.Herbalism: return 0; 
				case SkillName.Herding: return 0; // no delay
				case SkillName.Hiding: return 10;
				case SkillName.Provocation: return 10;
				case SkillName.Inscribe: return DefInscription.CraftSystem.Delay;
				case SkillName.Lockpicking: return 1.5;
				case SkillName.Magery: return MagicalCastDelay(from, skill);
				case SkillName.MagicResist: return 0.5; // Lowest delay for magic resist effected spell
				case SkillName.Tactics: return 0;
				case SkillName.Snooping: return 0; // no delay
				case SkillName.Musicianship: return 1;
				case SkillName.Poisoning: return 10;
				case SkillName.Archery: return 0;
				case SkillName.SpiritSpeak: return 5;
				case SkillName.Stealing: return 10;
				case SkillName.Tailoring: return DefTailoring.CraftSystem.Delay;
				case SkillName.AnimalTaming: return 4 * 3; //3-5 ticks, 3 seconds each
				case SkillName.TasteID: return 1;
				case SkillName.Tinkering: return DefTinkering.CraftSystem.Delay;
				case SkillName.Tracking: return 10;
				case SkillName.Veterinary: return BandageContext.GetDelay(from, fakeMob, false, skill.SkillName).TotalSeconds;;
				case SkillName.Swords: return 0;
				case SkillName.Macing: return 0;
				case SkillName.Fencing: return 0;
				case SkillName.Wrestling: return 0;
				case SkillName.Lumberjacking: return Lumberjacking.System.Definition.EffectDelay.TotalSeconds;
				case SkillName.Mining: return Mining.System.OreAndStone.EffectDelay.TotalSeconds;
				case SkillName.Meditation: return 0.5; // Highest regen rate is 1/0.5s
				case SkillName.Stealth: return skill.Value * 0.4 / 5 ; //One step takes 0.4 seconds, and each 5 skill grants 1 extra step
				case SkillName.RemoveTrap: return 10;
				case SkillName.Necromancy: return MagicalCastDelay(from, skill);
				case SkillName.Focus: return 0.5; // Highest regen rate is 1/0.5s
				case SkillName.Chivalry: return MagicalCastDelay(from, skill);
				case SkillName.Bushido: return MagicalCastDelay(from, skill);
				case SkillName.Ninjitsu: return MagicalCastDelay(from, skill);
				case SkillName.Spellweaving: return MagicalCastDelay(from, skill);
				case SkillName.Mysticism: return MagicalCastDelay(from, skill);
				case SkillName.Imbuing: return 1;
				case SkillName.Throwing: return 0;
				default: throw new NotImplementedException();
			}
		}

		private static double ChanceBasedAverage(double chance, double successValue, double failValue)
		{
			return successValue * chance + failValue * (1 - chance); 
		}

		private static double MagicalCastDelay(Mobile from, Skill skill)
		{
			BestSpellChance(skill, out object best);
			switch (best)
			{
				case Spell spell:
				{
					var newSpell = (Spell) Activator.CreateInstance(spell.GetType(), from, null); //We need new spell instance for mana scaling :/
					return newSpell.ScaleMana(newSpell.GetMana()) * Mobile.GetManaRegenRate(from).TotalSeconds;
				}
				case SpecialMove move:
					return move.ScaleMana(from, move.BaseMana) * Mobile.GetManaRegenRate(from).TotalSeconds;
			}

			throw new ArgumentException("Unknown spell chance");
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
