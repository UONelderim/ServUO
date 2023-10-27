#region References

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Server.Items;
using Server.Mobiles;

#endregion

namespace Server.Commands
{
	public class MobilesDifficulty
	{
		public static void Initialize()
		{
			CommandSystem.Register("diff", AccessLevel.Administrator, MobilesDifficulty_OnCommand);
		}

		public static void MobilesDifficulty_OnCommand(CommandEventArgs e)
		{
			var fileName = Path.Combine(Core.BaseDirectory, $"Difficulty_{DateTime.Now.Date.ToShortDateString()}.csv");
			var header = true;

			using (var writer = new StreamWriter(fileName))
			{
				foreach (var className in GetAllClasses())
				{
					var bc = (BaseCreature)Activator.CreateInstance(ScriptCompiler.FindTypeByName(className, false));
					if (bc.AI != AIType.AI_Animal && bc.AI != AIType.AI_Vendor)
					{
						var props = new Dictionary<string, object>();
						props.Add("className", className);
						fillCommonProps(bc, props);
						fillWeaponAbilities(bc, props);
						fillSpecialAbilities(bc, props);
						fillAreaEffects(bc, props);
						fillSkills(bc, props);

						// Zapis
						if (header)
						{
							writer.WriteLine(String.Join("\t", props.Keys));
							header = false;
						}

						writer.WriteLine(String.Join("\t", props.Values.Select(val => val?.ToString())));
					}
				}

				e.Mobile.SendMessage(0x400, "Plik zostal zapisany do: {0}", fileName);
			}
		}

		private static void fillCommonProps(BaseCreature bc, Dictionary<string, object> props)
		{
			props.Add("Name", bc.Name);
			props.Add("Karma", bc.Karma);
			props.Add("Fame", bc.Fame);
			props.Add("New Fame", bc.NelderimFame);
			props.Add("Difficulty", bc.Difficulty);
			props.Add("BaseDifficulty", bc.BaseDifficulty);
			props.Add("DifficultyScalar", bc.DifficultyScalar);
			props.Add("AI", bc.AI);
			props.Add("DPS", bc.DPS);
			props.Add("Life", bc.Life);
			props.Add("Melee DPS", bc.MeleeDPS);
			props.Add("Magic DPS", bc.MagicDPS);
			props.Add("DamageMin", bc.DamageMin);
			props.Add("DamageMax", bc.DamageMax);
			props.Add("WeaponAbilitiesBonus", bc.WeaponAbilitiesBonus);
			props.Add("HitPoisonBonus", bc.HitPoisonBonus);
			props.Add("BardDiff", BaseInstrument.GetBaseDifficulty(bc));
			props.Add("Str", bc.Str);
			props.Add("Int", bc.Int);
			props.Add("HitsMax", bc.HitsMax);
			props.Add("StamMax", bc.StamMax);
			props.Add("ManaMax", bc.ManaMax);
			props.Add("SwitchTargetChance", bc.SwitchTargetChance);
			props.Add("AttackMasterChance", bc.AttackMasterChance);
			props.Add("VirtualArmor", bc.VirtualArmor);
			props.Add("BasePhysicalResistance", bc.BasePhysicalResistance);
			props.Add("BaseFireResistance", bc.BaseFireResistance);
			props.Add("BaseColdResistance", bc.BaseColdResistance);
			props.Add("BasePoisonResistance", bc.BasePoisonResistance);
			props.Add("BaseEnergyResistance", bc.BaseEnergyResistance);
			props.Add("PhysicalDamage", bc.PhysicalDamage);
			props.Add("FireDamage", bc.FireDamage);
			props.Add("ColdDamage", bc.ColdDamage);
			props.Add("PoisonDamage", bc.PoisonDamage);
			props.Add("EnergyDamage", bc.EnergyDamage);
			props.Add("RangePerception", bc.RangePerception);
			props.Add("ActiveSpeed", bc.ActiveSpeed);
			props.Add("BleedImmune", bc.BleedImmune ? 1 : 0);
			props.Add("PoisonImmune", bc.PoisonImmune != null ? bc.PoisonImmune.Level : 0);
			props.Add("HitPoison", bc.HitPoison != null ? bc.HitPoison.Level : 0);
			props.Add("HitPoisonChance", bc.HitPoisonChance);
			props.Add("Unprovokable", bc.Unprovokable ? 1 : 0);
			props.Add("Uncalmable", bc.Uncalmable ? 1 : 0);
			props.Add("ReacquireDelay", bc.ReacquireDelay.TotalSeconds);
			props.Add("WeaponAbilityChance", bc.WeaponAbilityChance);
		}

		private static void fillWeaponAbilities(BaseCreature bc, Dictionary<string, object> props)
		{
			foreach (var wa in WeaponAbility.Abilities)
				if (wa != null)
					props.Add("has " + wa.GetType().Name, bc.HasAbility(wa));
		}

		private static void fillSpecialAbilities(BaseCreature bc, Dictionary<string, object> props)
		{
			foreach (var sa in SpecialAbility.Abilities)
				if (sa != null)
					props.Add("has " + sa.GetType().Name, bc.HasAbility(sa));
			props.Add("DragonBreathBonus",
				bc.AbilityProfile != null &&
				bc.AbilityProfile.HasAbility(SpecialAbility.DragonBreath)
					? bc.ComputeDragonBreathBonus()
					: 0);
		}

		private static void fillAreaEffects(BaseCreature bc, Dictionary<string, object> props)
		{
			foreach (var ae in AreaEffect.Effects)
				if (ae != null)
					props.Add("has " + ae.GetType().Name, bc.HasAbility(ae));
		}

		private static void fillSkills(BaseCreature bc, Dictionary<string, object> props)
		{
			SkillName[] skills =
			{
				SkillName.Anatomy, SkillName.Parry, SkillName.DetectHidden, SkillName.EvalInt, SkillName.Healing,
				SkillName.Hiding, SkillName.Inscribe, SkillName.Magery, SkillName.MagicResist, SkillName.Tactics,
				SkillName.Poisoning, SkillName.Archery, SkillName.SpiritSpeak, SkillName.Swords, SkillName.Macing,
				SkillName.Fencing, SkillName.Wrestling, SkillName.Lumberjacking, SkillName.Meditation,
				SkillName.Necromancy, SkillName.Focus, SkillName.Chivalry, SkillName.Bushido, SkillName.Ninjitsu
			};

			for (var i = 0; i < skills.Length; i++)
			{
				var s = bc.Skills[skills[i]];
				props.Add(s.Name, s.Value);
			}
		}

		static List<string> GetAllClasses()
		{
			return Assembly.GetAssembly(typeof(BaseCreature)).GetTypes()
				.Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(BaseCreature)) &&
				                 myType.GetConstructor(Type.EmptyTypes) != null).Select(type => type.Name).ToList();
		}
	}
}
