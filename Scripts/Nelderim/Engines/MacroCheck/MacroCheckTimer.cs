// 05.09.20 :: troyan :: przeniesienie do przestrzeni Engines

using System;
using Server;
using Server.Gumps;

namespace Server.Engines
{
	public class MacroCheckTimer : Timer
	{			
		private CheckPlayer m_Check;
	
		public MacroCheckTimer( CheckPlayer check ) : base( TimeSpan.FromMinutes( 1 ) )
		{
			Priority = TimerPriority.FiveSeconds;
			m_Check = check;
		}

		protected override void OnTick()
		{
			m_Check.TimeOut();
		}
	}
					
}
