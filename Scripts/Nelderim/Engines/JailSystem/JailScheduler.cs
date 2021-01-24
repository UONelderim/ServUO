using System;

using Server;

namespace Arya.Jail
{
	/// <summary>
	/// Holds the timer that manages the auto release of jailings
	/// </summary>
	public class JailScheduler : Server.Timer
	{
		private static JailScheduler m_Timer;

		private JailScheduler() : base( TimeSpan.FromMinutes( 1 ), TimeSpan.FromMinutes( 1 ) )
		{
			Priority = TimerPriority.OneMinute;
		}

		public static void Begin()
		{
			m_Timer = new JailScheduler();
			m_Timer.Start();
		}

		protected override void OnTick()
		{
			JailSystem.Verify();
		}
	}
}