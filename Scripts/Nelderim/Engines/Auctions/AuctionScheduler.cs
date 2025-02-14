using System;
using System.Linq;
using Server;

namespace Arya.Auction
{
	public class AuctionScheduler
	{
		private static InternalTimer m_Timer;

		public static DateTime Deadline { get; private set; } = DateTime.MaxValue;

		public static void Initialize()
		{
			m_Timer = new InternalTimer();
			ResetTimer();
		}

		public static void Stop()
		{
			if (m_Timer != null)
			{
				m_Timer.Stop();
				m_Timer = null;
			}
		}

		private static void ResetTimer()
		{
			if (AuctionSystem.Running)
			{
				CalculateDeadline();
			}

			m_Timer.Start();
		}

		private static void CalculateDeadline()
		{
			var list = AuctionSystem.Auctions.ToList();
			list.AddRange(AuctionSystem.Pending);

			Deadline = DateTime.MaxValue;

			foreach (AuctionItem auction in list)
			{
				if (auction.Deadline < Deadline)
				{
					Deadline = auction.Deadline;
				}
			}
		}

		public static void UpdateDeadline(DateTime deadline)
		{
			if (deadline < Deadline)
			{
				Deadline = deadline;
			}
		}

		private static void OnDeadlineReached()
		{
			AuctionSystem.OnDeadlineReached();
		}

		private static void OnTimer()
		{
			if (Deadline < DateTime.Now)
			{
				m_Timer.Stop();

				OnDeadlineReached();

				ResetTimer();
			}
		}

		private class InternalTimer : Timer
		{
			public InternalTimer() : base(TimeSpan.FromSeconds(5.0), TimeSpan.FromSeconds(5.0))
			{
			}

			protected override void OnTick()
			{
				OnTimer();
			}
		}
	}
}
