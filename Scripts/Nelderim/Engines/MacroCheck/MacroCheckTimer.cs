// 05.09.20 :: troyan :: przeniesienie do przestrzeni Engines

#region References

using System;

#endregion

namespace Server.Engines
{
	public class MacroCheckTimer : Timer
	{
		private readonly CheckPlayer m_Check;

		public MacroCheckTimer(CheckPlayer check) : base(TimeSpan.FromMinutes(1))
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
