#region References

using System;
using System.Xml;
using Server.Gumps;

#endregion

namespace Server.Regions
{
	public enum RaceRoomType
	{
		None,
		Czlowiek,
		Elf,
		Krasnolud,
		Drow,
		Teleport
	}

	public class RaceRoomRegion : BaseRegion
	{
		private readonly RaceRoomType m_Room;

		public RaceRoomRegion(XmlElement xml, Map map, Region parent) : base(xml, map, parent)
		{
			if (Name.StartsWith("RaceRoom"))
			{
				string raceRoomType = Name.Substring("RaceRoom".Length);
				if (!Enum.TryParse(raceRoomType, out m_Room))
					Console.WriteLine("Invalid RaceRoomRegion type " + raceRoomType);
			}
			else
			{
				Console.WriteLine("Invlaid RaceRoom name for " + Name);
			}
		}

		public override bool OnCombatantChange(Mobile from, IDamageable Old, IDamageable New)
		{
			return (from.AccessLevel > AccessLevel.Player);
		}

		public override void OnEnter(Mobile m)
		{
			if (m_Room != RaceRoomType.None && m_Room != RaceRoomType.Teleport)
			{
				m.SendGump(new RaceRoomGump(m, m_Room));
			}
		}

		public override void OnExit(Mobile m)
		{
		}
	}
}
