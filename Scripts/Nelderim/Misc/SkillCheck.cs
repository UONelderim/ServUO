#region References

using System;
using System.Collections.Generic;
using Nelderim;
using Nelderim.Configuration;
using Nelderim.Gains;
using Server.Engines.ArenaSystem;
using Server.Mobiles;
using Server.Multis;
using Server.Regions;

#endregion

namespace Server.Misc
{
	public partial class SkillCheck
	{
	private static readonly double m_BoatGain = 0.1;
		private static readonly double m_HouseGain = 0.2;
		private static readonly double m_MineGain = 0.25;
		private static readonly double m_InnGain = 0.5;
		private static readonly double m_CityGain = 0.7;
		private static readonly double m_ArenaGain = 0.8;
		private static readonly double m_VillageGain = 0.9;
		private static readonly double m_DungeonGain = 1.25;

		private static readonly List<SkillName> m_BoatAllowedSkills = new() { SkillName.Fishing };
		private static readonly List<SkillName> m_MineAllowedSkills = new() { SkillName.Mining };

		private static readonly List<SkillName> m_CraftingSkills = new()
		{
			SkillName.Alchemy,
			SkillName.Blacksmith,
			SkillName.Carpentry,
			SkillName.Cartography,
			SkillName.Cooking,
			SkillName.Fletching,
			SkillName.Imbuing,
			SkillName.Inscribe,
			SkillName.Tailoring,
			SkillName.Tinkering
		};

		public static double NRegionalModifier(Mobile m, Skill skill)
		{
			if (m == null || m.Map == null || m.Map == Map.Internal)
				return 1.0;

			if (!m_BoatAllowedSkills.Contains(skill.SkillName) && BaseBoat.FindBoatAt(m) != null)
				return m_BoatGain;

			Region region = m.Region;

			if (region is HouseRegion)
				return m_HouseGain;
			if (region is MiningRegion && !m_MineAllowedSkills.Contains(skill.SkillName))
				return m_MineGain;
			if (region is TavernRegion && !m_CraftingSkills.Contains(skill.SkillName))
				return m_InnGain;
			if (region is CityRegion  && !m_CraftingSkills.Contains(skill.SkillName))
				return m_CityGain;
			if (region is ArenaRegion)
				return m_ArenaGain;
			if (region is VillageRegion  && !m_CraftingSkills.Contains(skill.SkillName))
				return m_VillageGain;
			if (region is DungeonRegion)
				return m_DungeonGain;

			return 1.0;
		}

		public static double NGetGainChance(Mobile from, Skill skill, double chance, bool success)
		{
			double gc = (skill.Cap - skill.Base) / skill.Cap;

			gc += (1.0 - chance) * (success ? 0.5 : 0.0);
			gc *= 0.5f;

			gc *= skill.Info.GainFactor;
			
			if (gc < 0.01)
				gc = 0.01;

			var regionalModifier = NRegionalModifier(from, skill);

			gc *= regionalModifier;

			gc *= NConfig.BaseGainFactor;
			
			gc *= Gains.calculateGainFactor(from);

			gc *= EaseInOutSine(skill.Base, skill.Cap);
			
			// Pets get a 100% bonus
			if (from is BaseCreature && ((BaseCreature)from).Controlled)
				gc += gc;

			if (gc > 1.00)
				gc = 1.00;

			if (from is PlayerMobile { GainDebug: true } && skill.Lock == SkillLock.Up)
				if (skill.SkillName != SkillName.Meditation && skill.SkillName != SkillName.Focus)
					from.SendMessage(success ? 0x40 : 0x20, 
						$"[{skill.Name}: {skill.Value}%] " +
						$"GainChance = {Math.Round(gc * 100, 2)}% " +
						$"SuccessChance = {Math.Round(chance * 100, 2)}% " +
						$"RegionalModifier = x{regionalModifier}");

			return gc;
		}
		
		//It eases out with cosine, in range 0-120 from 5 to 1
		private static double EaseInOutSine(double value, double cap) {
			return (Math.Cos(Math.PI * value / cap) * 1.5) + 2.5;
		}
	}
}
