#region References

using System;
using System.Xml;
using Server.Gumps;

#endregion

namespace Server.Regions
{
	public enum SkillTooltipType
	{
		None,
		Tailor, 
		Smithy,
		Carpenter,
		Lumberjack,
		Tinker,
		Lockpick,
		TrapRemove,
		Peace,
		Provocation,
		Music,
		Detect,
		Hiding,
		Peak,
		Stealing,
		Fencing,
		Stealth,
		Cooking,
		Meditation,
		Magic,
		Focus,
		Scribe,
		Eval,
		Resist,
		Herbalism,
		Alchemy,
		Healing,
		SpiritSpeak,
		ArmsLore,
		MaceFightning,
		Sword,
		Disco,
		Tactics,
		Wrestling,
		Ninjitsu,
		Poisoning,
		Tracking,
		Archery,
		Chivalry,
		Bushido,
		Anatomy,
		Parry,
		Fishing,
		Fletching,
		AnimalTaming,
		Vet,
		Camping,
		AnimalLore,
		Cartography,
		Necromancy
	}

	public class SkillTooltipRegion : BaseRegion
	{ 
		private readonly SkillTooltipType m_ToolTip;

		public SkillTooltipRegion(XmlElement xml, Map map, Region parent) : base(xml, map, parent)
		{
			var tooltipTypeStr = "";
			ReadString(xml, "tooltip", ref tooltipTypeStr, true);
			if (!Enum.TryParse(tooltipTypeStr, out m_ToolTip))
				Console.WriteLine("Invalid SkillTooltipRegion type " + tooltipTypeStr);
		}

		public override bool OnCombatantChange(Mobile from, IDamageable Old, IDamageable New)
		{
			return (from.AccessLevel > AccessLevel.Player);
		}

		public override void OnEnter(Mobile m)
		{
			//Console.WriteLine($"Entering region with tooltip: {m_ToolTip}");
			if (m_ToolTip != SkillTooltipType.None)
			{
				m.SendGump(new SkillTooltipGump(m, m_ToolTip));
			}
		}

		public override void OnExit(Mobile m)
		{
		}
	}
}
