using System;
using Server;
using Server.Targeting;

namespace Arya.Auction
{
	public delegate void AuctionTargetCallback(Mobile from, object targeted);
	public class AuctionTarget : Target
	{
		private readonly AuctionTargetCallback m_Callback;

		public AuctionTarget(AuctionTargetCallback callback, int range, bool allowground) : base(range, allowground,
			TargetFlags.None)
		{
			m_Callback = callback;
		}

		protected override void OnTarget(Mobile from, object targeted)
		{
			try
			{
				m_Callback.DynamicInvoke(@from, targeted);
			}
			catch
			{
				Console.WriteLine(
					"The auction system cannot access the cliloc.enu file. Please review the system instructions for proper installation");
			}
		}

		protected override void OnTargetCancel(Mobile from, TargetCancelType cancelType)
		{
			if (AuctionSystem.Running)
			{
				from.SendGump(new AuctionGump(from));
			}
		}
	}
}
