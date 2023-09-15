using System;

namespace Server
{
	public delegate void BODCompletedEventHandler(BODCompletedEventArgs e);
	
	public class BODCompletedEventArgs : EventArgs
	{
		public Mobile User { get; }
		public IEntity Deed { get; }

		public BODCompletedEventArgs(Mobile m, IEntity bod)
		{
			User = m;
			Deed = bod;
		}
	}

	public static partial class EventSink
	{
		public static event BODCompletedEventHandler BODCompleted;
		
		public static void InvokeBODCompleted(BODCompletedEventArgs e)
		{
			BODCompleted?.Invoke(e);
		}
	}
}
