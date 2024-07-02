#region References

using System;
using System.Xml;
using Nelderim.Factions;
using Server.Accounting;
using Server.Gumps;
using Server.Nelderim.Gumps;

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
			var roomTypeStr = "";
			ReadString(xml, "room", ref roomTypeStr, true);
			if (!Enum.TryParse(roomTypeStr, out m_Room))
				Console.WriteLine("Invalid RaceRoomRegion type " + roomTypeStr);
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

			if (m.IsPlayer() && m.Account is Account a)
			{
				if (a.Faction == Faction.None)
				{
					m.SendGump(new FactionSelectGump(m));
				}
			}
		}

		public override void OnExit(Mobile m)
		{
		}
	}
}
