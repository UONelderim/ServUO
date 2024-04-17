using System;
using Server.Network;

namespace Server.Engines.PartySystem
{
	public partial class Party
	{
		public void NSendToAll(Mobile from, Func<string, MessageLocalizedAffix> func)
		{
			foreach (var memberInfo in m_Members)
			{
				var member = memberInfo.Mobile;
				member.Send(func(from.NGetName(member)));
			}
			foreach (var mob in m_Listeners)
			{
				if (mob.Party != this)
					mob.Send(func(from.NGetName(mob)));
			}
		}
	}
}
