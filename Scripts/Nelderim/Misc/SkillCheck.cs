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
		public static double[,] SkillGains =
		{
			//{ Gain, STR,  DEX,  INT  },
			{ 1.50, 0.00, 1.00, 1.00 }, // [0]  Alchemy      
			{ 1.75, 2.00, 0.00, 2.00 }, // [1]  Anatomy      
			{ 1.75, 2.00, 0.00, 2.40 }, // [2]  AnimalLore   
			{ 3.00, 0.00, 0.00, 2.00 }, // [3]  ItemID       
			{ 1.00, 2.00, 0.00, 1.80 }, // [4]  ArmsLore     
			{ 1.00, 1.50, 1.00, 0.00 }, // [5]  Parry        
			{ 10.0, 0.00, 1.00, 1.00 }, // [6]  Begging      
			{ 2.50, 3.00, 1.50, 0.00 }, // [7]  Blacksmith   
			{ 2.50, 1.50, 3.00, 0.00 }, // [8]  Fletching    
			{ 10.0, 0.00, 1.00, 1.50 }, // [9]  Peacemaking  
			{ 1.00, 0.10, 0.50, 0.20 }, // [10] Camping      
			{ 2.50, 0.00, 1.50, 1.00 }, // [11] Carpentry    
			{ 2.50, 0.00, 1.00, 2.00 }, // [12] Cartography  
			{ 2.00, 0.00, 6.00, 4.00 }, // [13] Cooking      
			{ 10.0, 0.00, 1.60, 1.00 }, // [14] DetectHidden 
			{ 1.50, 0.00, 0.50, 1.50 }, // [15] Discordance  
			{ 1.00, 2.00, 0.00, 6.00 }, // [16] EvalInt      
			{ 5.00, 0.00, 1.20, 2.00 }, // [17] Healing      
			{ 8.50, 4.00, 6.00, 0.00 }, // [18] Fishing      
			{ 3.50, 0.00, 1.00, 2.00 }, // [19] Herbalism   
			{ 1.00, 0.00, 0.00, 6.00 }, // [20] Herding      
			{ 5.00, 0.00, 5.00, 2.00 }, // [21] Hiding       
			{ 11.5, 0.00, 1.00, 2.00 }, // [22] Provocation  
			{ 1.50, 0.00, 1.00, 3.00 }, // [23] Inscribe     
			{ 1.00, 0.00, 2.00, 1.00 }, // [24] Lockpicking  
			{ 2.50, 2.00, 0.00, 6.00 }, // [25] Magery       
			{ 1.50, 4.00, 0.00, 4.00 }, // [26] MagicResist  
			{ 3.00, 4.00, 2.00, 0.00 }, // [27] Tactics      
			{ 1.00, 0.00, 4.00, 2.00 }, // [28] Snooping     
			{ 1.50, 0.00, 2.00, 1.00 }, // [29] Musicianship 
			{ 4.00, 0.00, 2.00, 1.50 }, // [30] Poisoning    
			{ 2.60, 2.00, 4.00, 0.00 }, // [31] Archery      
			{ 5.75, 0.50, 0.00, 4.00 }, // [32] SpiritSpeak  
			{ 10.5, 0.00, 5.00, 1.50 }, // [33] Stealing     
			{ 2.50, 0.00, 3.50, 0.50 }, // [34] Tailoring    
			{ 10.5, 4.00, 0.00, 2.00 }, // [35] AnimalTaming 
			{ 1.50, 0.00, 10.0, 0.00 }, // [36] TasteID      
			{ 1.50, 0.00, 3.00, 1.00 }, // [37] Tinkering    
			{ 10.5, 0.00, 3.50, 3.00 }, // [38] Tracking     
			{ 2.50, 0.00, 4.00, 3.00 }, // [39] Veterinary   
			{ 2.25, 6.00, 4.00, 0.00 }, // [40] Swords       
			{ 2.35, 6.00, 4.00, 0.00 }, // [41] Macing       
			{ 2.15, 6.00, 4.00, 0.00 }, // [42] Fencing      
			{ 2.50, 6.00, 4.00, 0.00 }, // [43] Wrestling    
			{ 5.00, 6.00, 2.00, 0.00 }, // [44] Lumberjacking
			{ 5.00, 6.50, 2.00, 0.00 }, // [45] Mining       
			{ 4.00, 0.50, 0.00, 6.00 }, // [46] Meditation   
			{ 6.50, 0.00, 6.00, 2.00 }, // [47] Stealth      
			{ 8.50, 0.00, 4.00, 4.00 }, // [48] RemoveTrap   
			{ 2.50, 2.00, 1.00, 6.00 }, // [49] Necromancy   
			{ 0.00, 0.00, 4.00, 1.50 }, // [50] Focus        
			{ 2.50, 2.00, 0.00, 1.50 }, // [51] Chivalry     
			{ 2.50, 2.00, 0.00, 1.50 }, // [52] Bushido      
			{ 2.50, 2.00, 0.00, 1.50 }, // [53] Ninjitsu     
			{ 2.00, 1.50, 0.00, 4.00 }, // [54] Spellweaving
			{ 2.00, 0.00, 0.00, 0.00 }, // [55] Mysticism
			{ 2.00, 0.00, 0.00, 0.00 }, // [56] Imbuing
			{ 2.00, 0.00, 0.00, 0.00 }, // [57] Throwing
		};

		private static readonly double m_BoatGain = 0.1;
		private static readonly double m_HouseGain = 0.2;
		private static readonly double m_MineGain = 0.25;
		private static readonly double m_InnGain = 0.5;
		private static readonly double m_CityGain = 0.7;
		private static readonly double m_ArenaGain = 0.8;
		private static readonly double m_VillageGain = 0.9;
		private static readonly double m_DungeonGain = 1.25;

		private static readonly List<SkillName> m_BoatAllowedSkills = new List<SkillName> { SkillName.Fishing };
		private static readonly List<SkillName> m_MineAllowedSkills = new List<SkillName> { SkillName.Mining };

		private static readonly List<SkillName> m_CraftingSkills = new List<SkillName>
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
			double gc = (double)(from.Skills.Cap - from.Skills.Total) / from.Skills.Cap;

			gc += (skill.Cap - skill.Base) / skill.Cap;
			gc /= 2;

			gc += (1.0 - chance) * (success ? 0.5 : 0.0);
			gc /= 2;

			gc *= skill.Info.GainFactor;
			
			if (gc < 0.01)
				gc = 0.01;

			var regionalModifier = NRegionalModifier(from, skill);

			gc *= regionalModifier;

			gc *= NConfig.BaseGainFactor;
			
			gc *= Gains.calculateGainFactor(from);
			
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
	}
}
