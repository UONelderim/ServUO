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
				m_Callback.Invoke(from, targeted);
			}
			catch (Exception e)
			{
				from.SendMessage("Target Error!");
				Console.WriteLine(e);
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
